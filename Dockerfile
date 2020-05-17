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
FROM microsoft/aspnetcore-build:2.0 AS dotnet-build
WORKDIR /src
# COPY ./ ./
COPY ./WebAPI/WebAPI.csproj .
RUN dotnet restore
COPY . .
RUN mkdir -p /src/WebAPI/wwwroot/
COPY --from=react-build /app/WebApp/dist/ /src/WebAPI/wwwroot/
WORKDIR /src/WebAPI
# RUN dotnet build -c Release -o /app
RUN dotnet publish -c Release -o /app

# Assemble final container
FROM microsoft/aspnetcore:2.0 AS final
WORKDIR /app
COPY --from=dotnet-build /app .
RUN mkdir -p /app/logs
EXPOSE 80
ENTRYPOINT ["dotnet", "WebAPI.dll"]
# EXPOSE 300

# # Pull down an image from Docker Hub that includes the .NET core SDK: 
# # https://hub.docker.com/_/microsoft-dotnet-core-sdk
# FROM mcr.microsoft.com/dotnet/core/sdk:2.1-bionic AS build

# # Fetch and install Node 10. Make sure to include the --yes parameter 
# # to automatically accept prompts during install, or it'll fail.
# RUN curl --silent --location https://deb.nodesource.com/setup_10.x | bash -
# RUN apt-get install --yes nodejs

# # Copy the source from your machine onto the container.
# WORKDIR /src
# COPY . .

# # Install dependencies. 
# # https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-restore?tabs=netcore2x
# RUN dotnet restore "./WebAPI/WebAPI.csproj"

# # Compile, then pack the compiled app and dependencies into a deployable unit.
# # https://docs.microsoft.com/en-us/dotnet/core/tools/dotnet-publish?tabs=netcore21
# RUN dotnet publish "/WebAPI/WebAPI.csproj" -c Release -o /app/publish

# # Pull down an image from Docker Hub that includes the ASP.NET core runtime:
# # https://hub.docker.com/_/microsoft-dotnet-core-aspnet/
# FROM mcr.microsoft.com/dotnet/core/aspnet:2.1-bionic

# # Expose port 80 to your local machine so you can access the app.
# EXPOSE 80

# # Copy the published app to this new runtime-only container.
# COPY --from=build /app/publish .

# # To run the app, run `dotnet sample-app.dll`, which we just copied over.
# ENTRYPOINT ["dotnet", "WebAPI.dll"]


# # 2 years OLD STUFF
# # https://docs.docker.com/develop/develop-images/multistage-build/
# # Build React bundle
# FROM node AS react-build
# WORKDIR /app/WebApp
# COPY ./WebApp ./
# RUN npm install
# RUN npm run build:CI

# # Build .NET Code app
# FROM microsoft/aspnetcore-build:2.0 AS dotnet-build
# WORKDIR /src
# COPY ./ ./
# RUN dotnet restore
# RUN mkdir -p /src/WebAPI/wwwroot/
# COPY --from=react-build /app/WebApp/dist/ /src/WebAPI/wwwroot/
# WORKDIR /src/WebAPI
# RUN dotnet build -c Release -o /app
# RUN dotnet publish -c Release -o /app

# # Assemble final container
# FROM microsoft/aspnetcore:2.0 AS final
# WORKDIR /app
# COPY --from=dotnet-build /app .
# RUN mkdir -p /app/logs
# EXPOSE 80
# ENTRYPOINT ["dotnet", "WebAPI.dll"]
