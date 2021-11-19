#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BookingSystem.Identity/BookingSystem.Identity.csproj", "BookingSystem.Identity/"]
COPY ["BookingSystem.Domain/BookingSystem.Domain.csproj", "BookingSystem.Domain/"]
COPY ["BookingSystem.Core/BookingSystem.Core.csproj", "BookingSystem.Core/"]
COPY ["BookingSystem.Persistence/BookingSystem.Persistence.csproj", "BookingSystem.Persistence/"]
RUN dotnet restore "BookingSystem.Identity/BookingSystem.Identity.csproj"
COPY . .
WORKDIR "/src/BookingSystem.Identity"
RUN dotnet build "BookingSystem.Identity.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BookingSystem.Identity.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BookingSystem.Identity.dll"]