FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["NotSoSimpleEcommerce.IdentityServer/NotSoSimpleEcommerce.IdentityServer.csproj", "NotSoSimpleEcommerce.IdentityServer/"]
RUN dotnet restore "NotSoSimpleEcommerce.IdentityServer/NotSoSimpleEcommerce.IdentityServer.csproj"
COPY . .
WORKDIR "/src/NotSoSimpleEcommerce.IdentityServer"
RUN dotnet build "NotSoSimpleEcommerce.IdentityServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NotSoSimpleEcommerce.IdentityServer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NotSoSimpleEcommerce.IdentityServer.dll"]
