#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/services/NotSoSimpleEcommerce.Main/NotSoSimpleEcommerce.Main.csproj", "src/services/NotSoSimpleEcommerce.Main/"]
COPY ["src/domains/NotSoSimpleEcommerce.Main.Domain/NotSoSimpleEcommerce.Main.Domain.csproj", "src/domains/NotSoSimpleEcommerce.Main.Domain/"]
COPY ["src/infrastructure/NotSoSimpleEcommerce.S3Handler/NotSoSimpleEcommerce.S3Handler.csproj", "src/infrastructure/NotSoSimpleEcommerce.S3Handler/"]
COPY ["src/infrastructure/NotSoSimpleEcommerce.Utils/NotSoSimpleEcommerce.Utils.csproj", "src/infrastructure/NotSoSimpleEcommerce.Utils/"]
COPY ["src/domains/NotSoSimpleEcommerce.Order.Domain/NotSoSimpleEcommerce.Order.Domain.csproj", "src/domains/NotSoSimpleEcommerce.Order.Domain/"]
COPY ["src/infrastructure/NotSoSimpleEcommerce.Repositories/NotSoSimpleEcommerce.Repositories.csproj", "src/infrastructure/NotSoSimpleEcommerce.Repositories/"]
COPY ["src/infrastructure/NotSoSimpleEcommerce.SesHandler/NotSoSimpleEcommerce.SesHandler.csproj", "src/infrastructure/NotSoSimpleEcommerce.SesHandler/"]
COPY ["src/infrastructure/NotSoSimpleEcommerce.SqsHandler/NotSoSimpleEcommerce.SqsHandler.csproj", "src/infrastructure/NotSoSimpleEcommerce.SqsHandler/"]
COPY ["src/domains/NotSoSimpleEcommerce.Shared/NotSoSimpleEcommerce.Shared.csproj", "src/domains/NotSoSimpleEcommerce.Shared/"]
RUN dotnet restore "src/services/NotSoSimpleEcommerce.Main/NotSoSimpleEcommerce.Main.csproj"
COPY . .
WORKDIR "/src/src/services/NotSoSimpleEcommerce.Main"
RUN dotnet build "NotSoSimpleEcommerce.Main.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NotSoSimpleEcommerce.Main.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NotSoSimpleEcommerce.Main.dll"]