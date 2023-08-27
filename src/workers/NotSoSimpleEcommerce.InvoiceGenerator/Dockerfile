#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/workers/NotSoSimpleEcommerce.InvoiceGenerator/NotSoSimpleEcommerce.InvoiceGenerator.csproj", "src/workers/NotSoSimpleEcommerce.InvoiceGenerator/"]
COPY ["src/domains/NotSoSimpleEcommerce.Shared/NotSoSimpleEcommerce.Shared.csproj", "src/domains/NotSoSimpleEcommerce.Shared/"]
COPY ["src/infrastructure/NotSoSimpleEcommerce.SqsHandler/NotSoSimpleEcommerce.SqsHandler.csproj", "src/infrastructure/NotSoSimpleEcommerce.SqsHandler/"]
COPY ["src/infrastructure/NotSoSimpleEcommerce.Utils/NotSoSimpleEcommerce.Utils.csproj", "src/infrastructure/NotSoSimpleEcommerce.Utils/"]
RUN dotnet restore "src/workers/NotSoSimpleEcommerce.InvoiceGenerator/NotSoSimpleEcommerce.InvoiceGenerator.csproj"
COPY . .
WORKDIR "/src/src/workers/NotSoSimpleEcommerce.InvoiceGenerator"
RUN dotnet build "NotSoSimpleEcommerce.InvoiceGenerator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NotSoSimpleEcommerce.InvoiceGenerator.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NotSoSimpleEcommerce.InvoiceGenerator.dll"]