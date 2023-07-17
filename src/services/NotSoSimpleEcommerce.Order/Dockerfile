FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["NotSoSimpleEcommerce.Order/NotSoSimpleEcommerce.Order.csproj", "NotSoSimpleEcommerce.Order/"]
RUN dotnet restore "NotSoSimpleEcommerce.Order/NotSoSimpleEcommerce.Order.csproj"
COPY . .
WORKDIR "/src/NotSoSimpleEcommerce.Order"
RUN dotnet build "NotSoSimpleEcommerce.Order.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NotSoSimpleEcommerce.Order.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NotSoSimpleEcommerce.Order.dll"]
