FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["NotSoSimpleEcommerce.InvoiceGenerator/NotSoSimpleEcommerce.InvoiceGenerator.csproj", "NotSoSimpleEcommerce.InvoiceGenerator/"]
RUN dotnet restore "NotSoSimpleEcommerce.InvoiceGenerator/NotSoSimpleEcommerce.InvoiceGenerator.csproj"
COPY . .
WORKDIR "/src/NotSoSimpleEcommerce.InvoiceGenerator"
RUN dotnet build "NotSoSimpleEcommerce.InvoiceGenerator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NotSoSimpleEcommerce.InvoiceGenerator.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NotSoSimpleEcommerce.InvoiceGenerator.dll"]
