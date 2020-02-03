#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["RatesWebService/RatesWebService.csproj", "RatesWebService/"]
COPY ["RatesSources/RatesSources.Common/RatesSources.Common.csproj", "RatesSources/RatesSources.Common/"]
COPY ["Rates.Common/Rates.Common.csproj", "Rates.Common/"]
COPY ["RatesSources/OpenExchangeRatesSource/OpenExchangeRatesSource.csproj", "RatesSources/OpenExchangeRatesSource/"]
COPY ["RatesStorageService/RatesStorageService.csproj", "RatesStorageService/"]
COPY ["RatesService/RatesService.csproj", "RatesService/"]
RUN dotnet restore "RatesWebService/RatesWebService.csproj"
COPY . .
WORKDIR "/src/RatesWebService"
RUN dotnet build "RatesWebService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RatesWebService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RatesWebService.dll"]