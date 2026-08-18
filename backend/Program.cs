using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Protocols;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using whm.Middleware;
using whm.Models;
using whm.Repositories;
using whm.Repositories.Interfaces;
using whm.Middleware;

namespace whm
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // =========================
            // Database - Supabase PostgreSQL
            // =========================

            builder.Services.AddDbContext<DataBaseContext>(options =>
                options.UseNpgsql(
                    builder.Configuration.GetConnectionString("DefaultConnection")
                ));


            // =========================
            // Email Service
            // =========================

            


            // =========================
            // CORS - Next.js
            // =========================

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("NextJs", policy =>
                {
                    policy
                        .WithOrigins("http://localhost:3000")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });


            builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();










            // =========================
            // Supabase Configuration
            // =========================

            var supabaseUrl = builder.Configuration["Supabase:Url"];

            if (string.IsNullOrWhiteSpace(supabaseUrl))
            {
                throw new InvalidOperationException(
                    "Supabase:Url is not configured."
                );
            }

            supabaseUrl = supabaseUrl.TrimEnd('/');


            // =========================
            // Supabase JWT Endpoints
            // =========================

            var issuer = $"{supabaseUrl}/auth/v1";

            var metadataAddress =
                $"{issuer}/.well-known/openid-configuration";


            // =========================
            // OpenID Configuration Manager
            // =========================
            //
            // This retrieves:
            //
            // - issuer
            // - JWKS URI
            // - Supabase public signing keys
            //
            // The public key is what verifies
            // the ES256 Supabase access token.
            //

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


            // =========================
            // Authentication - Supabase JWT
            // =========================

            builder.Services
                .AddAuthentication(
                    JwtBearerDefaults.AuthenticationScheme
                )
                .AddJwtBearer(options =>
                {
                    // Explicitly use our Supabase
                    // OpenID configuration manager.
                    options.ConfigurationManager =
                        configurationManager;

                    options.RequireHttpsMetadata = true;

                    options.SaveToken = true;


                    // =========================
                    // Token Validation
                    // =========================

                    options.TokenValidationParameters =
                        new TokenValidationParameters
                        {
                            // -------------------------
                            // Issuer
                            // -------------------------

                            ValidateIssuer = true,

                            ValidIssuer = issuer,


                            // -------------------------
                            // Audience
                            // -------------------------

                            ValidateAudience = true,

                            ValidAudience = "authenticated",


                            // -------------------------
                            // Lifetime
                            // -------------------------

                            ValidateLifetime = true,


                            // -------------------------
                            // Signing Key
                            // -------------------------

                            ValidateIssuerSigningKey = true,


                            // -------------------------
                            // Claims
                            // -------------------------

                            NameClaimType = "email",

                            RoleClaimType = "role",


                            // -------------------------
                            // Clock tolerance
                            // -------------------------

                            ClockSkew = TimeSpan.FromMinutes(1)
                        };


                    // =========================
                    // Authentication Events
                    // =========================

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


            // =========================
            // Authorization
            // =========================

            builder.Services.AddAuthorization();


            // =========================
            // Controllers
            // =========================

            builder.Services.AddControllers();


            // =========================
            // Swagger
            // =========================

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


            // =========================
            // Build
            // =========================

            var app = builder.Build();
            app.UseMiddleware<ExceptionMiddleware>();


            // =========================
            // Swagger
            // =========================

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();

                app.UseSwaggerUI();
            }


            // =========================
            // HTTPS
            // =========================
            //
            // Local backend currently runs on
            // http://localhost:5171
            //
            // Keep this disabled for now.
            //

            // app.UseHttpsRedirection();


            // =========================
            // CORS
            // =========================

            app.UseCors("NextJs");


            // =========================
            // Authentication
            // =========================

            app.UseAuthentication();


            // =========================
            // Authorization
            // =========================

            app.UseAuthorization();


            // =========================
            // Controllers
            // =========================

            app.MapControllers();


            // =========================
            // Run
            // =========================

            app.Run();
        }
    }
}