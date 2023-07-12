FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SimpleEcommerce.Order/SimpleEcommerce.Order.csproj", "SimpleEcommerce.Order/"]
RUN dotnet restore "SimpleEcommerce.Order/SimpleEcommerce.Order.csproj"
COPY . .
WORKDIR "/src/SimpleEcommerce.Order"
RUN dotnet build "SimpleEcommerce.Order.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SimpleEcommerce.Order.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SimpleEcommerce.Order.dll"]
