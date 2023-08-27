#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/services/NotSoSimpleEcommerce.IdentityServer/NotSoSimpleEcommerce.IdentityServer.csproj", "src/services/NotSoSimpleEcommerce.IdentityServer/"]
COPY ["src/domains/NotSoSimpleEcommerce.IdentityServer.Domain/NotSoSimpleEcommerce.IdentityServer.Domain.csproj", "src/domains/NotSoSimpleEcommerce.IdentityServer.Domain/"]
COPY ["src/infrastructure/NotSoSimpleEcommerce.Repositories/NotSoSimpleEcommerce.Repositories.csproj", "src/infrastructure/NotSoSimpleEcommerce.Repositories/"]
COPY ["src/domains/NotSoSimpleEcommerce.Shared/NotSoSimpleEcommerce.Shared.csproj", "src/domains/NotSoSimpleEcommerce.Shared/"]
RUN dotnet restore "src/services/NotSoSimpleEcommerce.IdentityServer/NotSoSimpleEcommerce.IdentityServer.csproj"
COPY . .
WORKDIR "/src/src/services/NotSoSimpleEcommerce.IdentityServer"
RUN dotnet build "NotSoSimpleEcommerce.IdentityServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NotSoSimpleEcommerce.IdentityServer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NotSoSimpleEcommerce.IdentityServer.dll"]