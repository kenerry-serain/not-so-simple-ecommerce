FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SimpleEcommerceV2.IdentityServer/SimpleEcommerceV2.IdentityServer.csproj", "SimpleEcommerceV2.IdentityServer/"]
RUN dotnet restore "SimpleEcommerceV2.IdentityServer/SimpleEcommerceV2.IdentityServer.csproj"
COPY . .
WORKDIR "/src/SimpleEcommerceV2.IdentityServer"
RUN dotnet build "SimpleEcommerceV2.IdentityServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SimpleEcommerceV2.IdentityServer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SimpleEcommerceV2.IdentityServer.dll"]
