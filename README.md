# Swapify

[![Build Status](https://github.com/fri-team/Swapify/workflows/GitHub%20Actions/badge.svg?branch=develop)](https://github.com/fri-team/Swapify/actions?query=branch%3Adevelop)

Web based application enabling students of [University of Zilina](http://www.uniza.sk/) to exchange course time blocks.

### Prerequirements
* Docker
* Visual Studio
* ASP.NET Core 3.1
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
2. Open command line, go to repository folder
3. Run containers with command: docker compose -f docker-compose-local.yml up -d (may not start if port 80 is used)
4. Use browser to access the app using url "localhost"
5. Stop containers with command: docker compose -f docker-compose-local.yml down

### Old approach
-- Backend --
1. Clone repository
2. Open solution using Visual studio (Swapify.sln)
3. Run project (Backend should launch)
-- Frontend --
4. Open command line (Preferably in Admin mode to make sure everything works right)
5. Go to the folder where your swapify project is stored
6. Go to the WebApp folder
7. write: npm install
8. write: npm start

After these steps, the application should launch in your browser.

If you get error that npm start can't launch then try this solution: https://stackoverflow.com/questions/45499656/error-occured-when-executing-command-npm-run-serve
