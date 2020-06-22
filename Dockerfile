# https://docs.docker.com/develop/develop-images/multistage-build/
# Build React bundle
FROM node AS react-build
WORKDIR /app/WebApp
# Copy the FE source from your machine onto the container.
COPY ./WebApp ./
RUN npm install
# Run clean FE build
RUN npm run build:CI

# Build .NET Core app
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS dotnet-build
WORKDIR /src
# COPY ./ ./
COPY ./WebAPI/WebAPI.csproj .
# RUN dotnet restore
COPY . .
RUN mkdir -p /src/WebAPI/wwwroot/
COPY --from=react-build /app/WebApp/dist/ /src/WebAPI/wwwroot/
WORKDIR /src/WebAPI
# RUN dotnet build -c Release -o /app
RUN dotnet publish -c Release -o /app

# Assemble final container
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS final
WORKDIR /app
COPY --from=dotnet-build /app .
RUN mkdir -p /app/logs
EXPOSE 80
ENTRYPOINT ["dotnet", "WebAPI.dll"]
# EXPOSE 300
