#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["SimpleEcommerceV2.Main/SimpleEcommerceV2.Main.csproj", "SimpleEcommerceV2.Main/"]
RUN dotnet restore "SimpleEcommerceV2.Main/SimpleEcommerceV2.Main.csproj"
COPY . .
WORKDIR "/src/SimpleEcommerceV2.Main"
RUN dotnet build "SimpleEcommerceV2.Main.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SimpleEcommerceV2.Main.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SimpleEcommerceV2.Main.dll"]