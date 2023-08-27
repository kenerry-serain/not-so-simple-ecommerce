#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/workers/NotSoSimpleEcommerce.Notificator/NotSoSimpleEcommerce.Notificator.csproj", "src/workers/NotSoSimpleEcommerce.Notificator/"]
COPY ["src/infrastructure/NotSoSimpleEcommerce.SesHandler/NotSoSimpleEcommerce.SesHandler.csproj", "src/infrastructure/NotSoSimpleEcommerce.SesHandler/"]
COPY ["src/infrastructure/NotSoSimpleEcommerce.SqsHandler/NotSoSimpleEcommerce.SqsHandler.csproj", "src/infrastructure/NotSoSimpleEcommerce.SqsHandler/"]
COPY ["src/infrastructure/NotSoSimpleEcommerce.Utils/NotSoSimpleEcommerce.Utils.csproj", "src/infrastructure/NotSoSimpleEcommerce.Utils/"]
RUN dotnet restore "src/workers/NotSoSimpleEcommerce.Notificator/NotSoSimpleEcommerce.Notificator.csproj"
COPY . .
WORKDIR "/src/src/workers/NotSoSimpleEcommerce.Notificator"
RUN dotnet build "NotSoSimpleEcommerce.Notificator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "NotSoSimpleEcommerce.Notificator.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NotSoSimpleEcommerce.Notificator.dll"]