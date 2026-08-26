# Backend
Start-Process powershell -ArgumentList "-NoExit", "-Command", `
    "cd '$PSScriptRoot\backend'; dotnet clean .\WMS.csproj; dotnet restore .\WMS.csproj; dotnet build .\WMS.csproj; dotnet run --project .\WMS.csproj"

# Frontend
Start-Process powershell -ArgumentList "-NoExit", "-Command", `
    "cd '$PSScriptRoot\frontend'; npm run dev"