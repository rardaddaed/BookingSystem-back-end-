# Create Database Migrations

```powershell
$Env:ASPNETCORE_ENVIRONMENT = "Local"
```

## Booking System Database

```
cd BookingSystem.WebApi
dotnet ef migrations add <MigrationName> --project=../BookingSystem.Persistence --context BSDbContext
```

## Identity Database

It is unlikely going to 

```
cd BookingSystem.Identity
dotnet ef migrations add <MigrationName> --project=../BookingSystem.Persistence --context AppIdentityDbContext
```
# Apply Database Migrations

Run BookingSystem.Deploy separately.

# Local Https Docker Settings

```powershell
dotnet dev-certs https --clean
dotnet dev-certs https -ep $env:USERPROFILE\.aspnet\https\aspnetapp.pfx -p 123456
dotnet dev-certs https --trust
```
