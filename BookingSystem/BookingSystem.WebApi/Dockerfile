#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["BookingSystem.WebApi/BookingSystem.WebApi.csproj", "BookingSystem.WebApi/"]
COPY ["BookingSystem.Application/BookingSystem.Application.csproj", "BookingSystem.Application/"]
COPY ["BookingSystem.Domain/BookingSystem.Domain.csproj", "BookingSystem.Domain/"]
COPY ["BookingSystem.Core/BookingSystem.Core.csproj", "BookingSystem.Core/"]
COPY ["BookingSystem.Persistence/BookingSystem.Persistence.csproj", "BookingSystem.Persistence/"]
RUN dotnet restore "BookingSystem.WebApi/BookingSystem.WebApi.csproj"
COPY . .
WORKDIR "/src/BookingSystem.WebApi"
RUN dotnet build "BookingSystem.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BookingSystem.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BookingSystem.WebApi.dll"]
