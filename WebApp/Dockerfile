FROM node AS react-build
WORKDIR /react
COPY ./WebApp ./
RUN npm install -g npm@6.14.11
RUN npm install
RUN npm run build:CI

FROM ubuntu AS final
RUN mkdir -p /app/wwwroot
COPY --from=react-build /react/dist /app/wwwroot