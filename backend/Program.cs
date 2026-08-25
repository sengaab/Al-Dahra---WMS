using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using whm.Middleware;
using whm.Models;
using whm.Repositories;
using whm.Repositories.Interfaces;
using whm.Services;
using whm.UnitOfWork;


namespace whm
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // =====================================================
            // DATABASE
            // =====================================================

            builder.Services.AddDbContext<DataBaseContext>(options =>
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString(
                        "DefaultConnection"
                    )
                ));


            // =====================================================
            // QR CODE SERVICE
            // =====================================================

            builder.Services.AddScoped<
                IQRCodeService,
                QRCodeService
            >();
            builder.Services.AddScoped<
              IBarcodeService,
               BarcodeService
            >();


            // =====================================================
            // UNIT OF WORK
            // =====================================================
            //
            // IMPORTANT:
            // We use the full class name because
            // "UnitOfWork" is also the namespace name.
            //

            builder.Services.AddScoped<
                IUnitOfWork,
                whm.UnitOfWork.UnitOfWork
            >();


            // =====================================================
            // CORS - NEXT.JS
            // =====================================================

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("NextJs", policy =>
                {
                    policy
                        .WithOrigins(
                            "http://localhost:3000"
                        )
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });


            // =====================================================
            // SUPABASE CONFIGURATION
            // =====================================================

            var supabaseUrl =
                builder.Configuration["Supabase:Url"];

            if (string.IsNullOrWhiteSpace(supabaseUrl))
            {
                throw new InvalidOperationException(
                    "Supabase:Url is not configured."
                );
            }

            supabaseUrl = supabaseUrl.TrimEnd('/');


            // =====================================================
            // SUPABASE JWT
            // =====================================================

            var issuer =
                $"{supabaseUrl}/auth/v1";

            var metadataAddress =
                $"{issuer}/.well-known/openid-configuration";


            // =====================================================
            // OPENID CONFIGURATION
            // =====================================================

            var httpDocumentRetriever =
                new HttpDocumentRetriever
                {
                    RequireHttps = true
                };

            var configurationManager =
                new ConfigurationManager<OpenIdConnectConfiguration>(
                    metadataAddress,
                    new OpenIdConnectConfigurationRetriever(),
                    httpDocumentRetriever
                );


            // =====================================================
            // AUTHENTICATION
            // =====================================================

            builder.Services
                .AddAuthentication(
                    JwtBearerDefaults.AuthenticationScheme
                )
                .AddJwtBearer(options =>
                {
                    options.ConfigurationManager =
                        configurationManager;

                    options.RequireHttpsMetadata = true;

                    options.SaveToken = true;


                    // =================================================
                    // TOKEN VALIDATION
                    // =================================================

                    options.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            // -----------------------------
                            // ISSUER
                            // -----------------------------

                            ValidateIssuer = true,

                            ValidIssuer = issuer,


                            // -----------------------------
                            // AUDIENCE
                            // -----------------------------

                            ValidateAudience = true,

                            ValidAudience = "authenticated",


                            // -----------------------------
                            // LIFETIME
                            // -----------------------------

                            ValidateLifetime = true,


                            // -----------------------------
                            // SIGNING KEY
                            // -----------------------------

                            ValidateIssuerSigningKey = true,


                            // -----------------------------
                            // CLAIMS
                            // -----------------------------

                            NameClaimType = "email",

                            RoleClaimType = "role",


                            // -----------------------------
                            // CLOCK SKEW
                            // -----------------------------

                            ClockSkew =
                                TimeSpan.FromMinutes(1)
                        };


                    // =================================================
                    // JWT EVENTS
                    // =================================================

                    options.Events =
                        new JwtBearerEvents
                        {
                            OnAuthenticationFailed =
                                context =>
                                {
                                    Console.WriteLine(
                                        "================================"
                                    );

                                    Console.WriteLine(
                                        "JWT Authentication Failed:"
                                    );

                                    Console.WriteLine(
                                        context.Exception.Message
                                    );

                                    Console.WriteLine(
                                        "================================"
                                    );

                                    return Task.CompletedTask;
                                },


                            OnTokenValidated =
                                context =>
                                {
                                    Console.WriteLine(
                                        "================================"
                                    );

                                    Console.WriteLine(
                                        "Supabase JWT successfully validated."
                                    );

                                    Console.WriteLine(
                                        $"User: {context.Principal?.Identity?.Name}"
                                    );

                                    Console.WriteLine(
                                        "================================"
                                    );

                                    return Task.CompletedTask;
                                },


                            OnMessageReceived =
                                context =>
                                {
                                    Console.WriteLine(
                                        "Authorization header received."
                                    );

                                    return Task.CompletedTask;
                                }
                        };
                });


            // =====================================================
            // AUTHORIZATION
            // =====================================================

            builder.Services.AddAuthorization();


            // =====================================================
            // CONTROLLERS
            // =====================================================

            builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
    });


            // =====================================================
            // SWAGGER
            // =====================================================

            builder.Services.AddEndpointsApiExplorer();

            builder.Services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition(
                    "Bearer",
                    new OpenApiSecurityScheme
                    {
                        Name = "Authorization",

                        Type = SecuritySchemeType.Http,

                        Scheme = "Bearer",

                        BearerFormat = "JWT",

                        In = ParameterLocation.Header,

                        Description =
                            "Enter: Bearer {Supabase access token}"
                    }
                );


                options.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                        {
                            new OpenApiSecurityScheme
                            {
                                Reference =
                                    new OpenApiReference
                                    {
                                        Type =
                                            ReferenceType.SecurityScheme,

                                        Id = "Bearer"
                                    }
                            },

                            Array.Empty<string>()
                        }
                    }
                );
            });

            // =====================================================
            // Repositories
            // =====================================================

            builder.Services.AddScoped<IStockRepository, StockRepository>();
            builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();

            // =====================================================
            // BUILD
            // =====================================================

            var app = builder.Build();


            // =====================================================
            // GLOBAL EXCEPTION MIDDLEWARE
            // =====================================================

            //app.UseMiddleware<ExceptionMiddleware>();


            // =====================================================
            // SWAGGER
            // =====================================================

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();

                app.UseSwaggerUI();
            }


            // =====================================================
            // HTTPS
            // =====================================================

            // Disabled for local development
            // app.UseHttpsRedirection();


            // =====================================================
            // CORS
            // =====================================================

            app.UseCors("NextJs");


            // =====================================================
            // AUTHENTICATION
            // =====================================================

            app.UseAuthentication();


            // =====================================================
            // AUTHORIZATION
            // =====================================================

            app.UseAuthorization();


            // =====================================================
            // CONTROLLERS
            // =====================================================

            app.MapControllers();


            // =====================================================
            // RUN
            // =====================================================

            app.Run();
        }
    }
}