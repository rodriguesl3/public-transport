FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["src/N2L.PublicTransport.Authentication/N2L.PublicTransport.Authentication.csproj", "src/N2L.PublicTransport.Authentication/"]
RUN dotnet restore "src/N2L.PublicTransport.Authentication/N2L.PublicTransport.Authentication.csproj"
COPY . .
WORKDIR "/src/src/N2L.PublicTransport.Authentication"
RUN dotnet build "N2L.PublicTransport.Authentication.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "N2L.PublicTransport.Authentication.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "N2L.PublicTransport.Authentication.dll"]