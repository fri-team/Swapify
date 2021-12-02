FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ./WebAPI/WebAPI.csproj .
#RUN dotnet restore "WebAPI/WebAPI.csproj"
COPY . .
WORKDIR "/src/WebAPI"
RUN dotnet build "WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "WebAPI.csproj" -c Release -o /app/publish

# Build React bundle
FROM node AS react-build
WORKDIR /react
COPY ./WebApp ./
RUN npm install -g npm@6.14.11
RUN npm install
RUN npm run build:CI

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=react-build /react/dist /app/wwwroot
ENTRYPOINT ["dotnet", "WebAPI.dll"]
