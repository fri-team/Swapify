# https://docs.docker.com/develop/develop-images/multistage-build/
# Build React bundle
FROM node AS react-build
WORKDIR /app/WebApp
COPY ./WebApp ./
RUN npm install
RUN npm run build:CI

# Build .NET Code app
FROM microsoft/aspnetcore-build:2.0 AS dotnet-build
WORKDIR /src
COPY ./ ./
RUN dotnet restore
RUN mkdir -p /src/WebAPI/wwwroot/
COPY --from=react-build /app/WebApp/dist/ /src/WebAPI/wwwroot/
WORKDIR /src/WebAPI
RUN dotnet build -c Release -o /app
RUN dotnet publish -c Release -o /app

# Assemble final container
FROM microsoft/aspnetcore:2.0 AS final
WORKDIR /app
COPY --from=dotnet-build /app .
RUN mkdir -p /app/logs
EXPOSE 80
ENTRYPOINT ["dotnet", "WebAPI.dll"]
