FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SimpleEcommerce.Notificator/SimpleEcommerce.Notificator.csproj", "SimpleEcommerce.Notificator/"]
RUN dotnet restore "SimpleEcommerce.Notificator/SimpleEcommerce.Notificator.csproj"
COPY . .
WORKDIR "/src/SimpleEcommerce.Notificator"
RUN dotnet build "SimpleEcommerce.Notificator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SimpleEcommerce.Notificator.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SimpleEcommerce.Notificator.dll"]
