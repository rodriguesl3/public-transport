FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["src/N2L.PublicTransport.API/N2L.PublicTransport.API.csproj", "src/N2L.PublicTransport.API/"]
RUN dotnet restore "src/N2L.PublicTransport.API/N2L.PublicTransport.API.csproj"
COPY . .
WORKDIR "/src/src/N2L.PublicTransport.API"
RUN dotnet build "N2L.PublicTransport.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "N2L.PublicTransport.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "N2L.PublicTransport.API.dll"]