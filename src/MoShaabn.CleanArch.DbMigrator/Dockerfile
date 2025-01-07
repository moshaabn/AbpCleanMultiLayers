#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER app
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/MoShaabn.CleanArch.DbMigrator/MoShaabn.CleanArch.DbMigrator.csproj", "src/MoShaabn.CleanArch.DbMigrator/"]
COPY ["src/MoShaabn.CleanArch.Application.Contracts/MoShaabn.CleanArch.Application.Contracts.csproj", "src/MoShaabn.CleanArch.Application.Contracts/"]
COPY ["src/MoShaabn.CleanArch.Domain.Shared/MoShaabn.CleanArch.Domain.Shared.csproj", "src/MoShaabn.CleanArch.Domain.Shared/"]
COPY ["src/MoShaabn.CleanArch.EntityFrameworkCore/MoShaabn.CleanArch.EntityFrameworkCore.csproj", "src/MoShaabn.CleanArch.EntityFrameworkCore/"]
COPY ["src/MoShaabn.CleanArch.Domain/MoShaabn.CleanArch.Domain.csproj", "src/MoShaabn.CleanArch.Domain/"]
RUN dotnet restore "./src/MoShaabn.CleanArch.DbMigrator/MoShaabn.CleanArch.DbMigrator.csproj"
COPY . .
WORKDIR "/src/src/MoShaabn.CleanArch.DbMigrator"
RUN dotnet build "./MoShaabn.CleanArch.DbMigrator.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./MoShaabn.CleanArch.DbMigrator.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MoShaabn.CleanArch.DbMigrator.dll"]