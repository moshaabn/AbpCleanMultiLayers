#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
# Create uploads directory and set permissions as root
RUN mkdir -p /app/wwwroot/uploads && \
    chown -R app:app /app/wwwroot/uploads && \
    chmod -R 775 /app/wwwroot/uploads  # Allow app user to write
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/MoShaabn.CleanArch.HttpApi.Host/MoShaabn.CleanArch.HttpApi.Host.csproj", "src/MoShaabn.CleanArch.HttpApi.Host/"]
COPY ["src/MoShaabn.CleanArch.Application/MoShaabn.CleanArch.Application.csproj", "src/MoShaabn.CleanArch.Application/"]
COPY ["src/MoShaabn.CleanArch.Domain/MoShaabn.CleanArch.Domain.csproj", "src/MoShaabn.CleanArch.Domain/"]
COPY ["src/MoShaabn.CleanArch.Domain.Shared/MoShaabn.CleanArch.Domain.Shared.csproj", "src/MoShaabn.CleanArch.Domain.Shared/"]
COPY ["src/MoShaabn.CleanArch.Application.Contracts/MoShaabn.CleanArch.Application.Contracts.csproj", "src/MoShaabn.CleanArch.Application.Contracts/"]
COPY ["src/MoShaabn.CleanArch.EntityFrameworkCore/MoShaabn.CleanArch.EntityFrameworkCore.csproj", "src/MoShaabn.CleanArch.EntityFrameworkCore/"]
COPY ["src/MoShaabn.CleanArch.HttpApi/MoShaabn.CleanArch.HttpApi.csproj", "src/MoShaabn.CleanArch.HttpApi/"]
RUN dotnet restore "./src/MoShaabn.CleanArch.HttpApi.Host/MoShaabn.CleanArch.HttpApi.Host.csproj"
COPY . .
WORKDIR "/src/src/MoShaabn.CleanArch.HttpApi.Host"
RUN dotnet build "./MoShaabn.CleanArch.HttpApi.Host.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MoShaabn.CleanArch.HttpApi.Host.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MoShaabn.CleanArch.HttpApi.Host.dll"]
