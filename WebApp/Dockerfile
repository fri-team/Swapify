FROM node AS react-build
WORKDIR /react
COPY ./WebApp ./
RUN npm install -g npm@6.14.11
RUN npm install
RUN npm run build:CI