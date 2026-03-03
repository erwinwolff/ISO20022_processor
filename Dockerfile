FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
ARG NODE_OPTIONS=--max-old-space-size=4096

WORKDIR /src
COPY . .
RUN apt-get update && apt-get upgrade -y
RUN apt-get install curl -y
RUN curl -sL https://deb.nodesource.com/setup_20.x | bash -
RUN apt-get install nodejs -y
RUN npm install -g npm
RUN cd "./ISO20022_processor_net10/ISO20022" && npm install --legacy-peer-deps
RUN cd "./ISO20022_processor_net10/ISO20022" && npm run build
RUN dotnet restore "./ISO20022_processor_net10/ISO20022_processor_net10.csproj"
RUN dotnet build "./ISO20022_processor_net10/ISO20022_processor_net10.csproj" -c $BUILD_CONFIGURATION -o /app

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ISO20022_processor_net10/ISO20022_processor_net10.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ISO20022_processor_net10.dll"]