# Swapify

[![Build Status](https://github.com/fri-team/Swapify/workflows/GitHub%20Actions/badge.svg?branch=develop)](https://github.com/fri-team/Swapify/actions?query=branch%3Adevelop)

Web based application enabling students of [University of Zilina](http://www.uniza.sk/) to exchange course time blocks.

### Prerequirements
* Docker
* Visual Studio
* ASP.NET 5
* Visual Studio Code
* Node.js
* NPM

### Install Visual Studio Code Extensions

* [EditorConfig](https://marketplace.visualstudio.com/items?itemName=chrisdias.vscodeEditorConfig)
* [Eslint](https://marketplace.visualstudio.com/items?itemName=dbaeumer.vscode-eslint)

## Built With

* [Docker](https://www.docker.com) - Container technology for packaging application together with it's dependencies
* [ASP .NET Core](https://github.com/aspnet/home) - Cross-platform framework for building modern web apps
* [React](https://github.com/facebook/react) - JavaScript library for building user interfaces
* [React Slingshot](https://github.com/coryhouse/react-slingshot) - Boilerplate for React + Redux development

## How to run application

### Using Docker
1. Clone repository
2. Open solution using Visual Studio
3. Run project docker-compose
4. If you change FE, it docker image needs to be build using command "docker build -t react -f ./WebApp/Dockerfile ."

### Old approach
-- Backend --
1. Clone repository
2. Open solution using Visual studio (Swapify.sln)
3. Set startup project to WebAPI
4. Make sure you use WebAPI debug profile
5. Run project (Backend should launch)
-- Frontend --
6. Open command line (Preferably in Admin mode to make sure everything works right)
7. Go to the folder where your swapify project is stored
8. Go to the WebApp folder
9. write: npm install
10. write: npm start

After these steps, the application should launch in your browser.

If you get error that npm start can't launch then try this solution: https://stackoverflow.com/questions/45499656/error-occured-when-executing-command-npm-run-serve

## How to open Swagger documentation
Swagger UI is located on URL: <webpage>:<port>/swagger for example localhost/swagger or localhost:5020/swagger
Swagger JSON file is located on URL /swagger/v1/swagger.json
