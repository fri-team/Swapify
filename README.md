# Swapify


[![Build Status](https://github.com/fri-team/Swapify/workflows/GitHub%20Actions/badge.svg?branch=develop)](https://github.com/fri-team/Swapify/actions?query=branch%3Adevelop)
<!--outdated travis[![Build Status](https://travis-ci.org/fri-team/Swapify.svg?branch=master)](https://travis-ci.org/fri-team/Swapify)-->
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=Swapify&metric=alert_status)](https://sonarcloud.io/dashboard?id=Swapify)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=Swapify&metric=security_rating)](https://sonarcloud.io/dashboard?id=Swapify)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=Swapify&metric=sqale_rating)](https://sonarcloud.io/dashboard?id=Swapify)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=Swapify&metric=coverage)](https://sonarcloud.io/dashboard?id=Swapify)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=Swapify&metric=code_smells)](https://sonarcloud.io/dashboard?id=Swapify)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=Swapify&metric=bugs)](https://sonarcloud.io/dashboard?id=Swapify)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=Swapify&metric=vulnerabilities)](https://sonarcloud.io/dashboard?id=Swapify)
[![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=Swapify&metric=duplicated_lines_density)](https://sonarcloud.io/dashboard?id=Swapify)

Web based application enabling students of [University of Zilina](http://www.uniza.sk/) to exchange course time blocks.

### Prerequirements

* Visual Studio 2017
* ASP .NET Core 2.0
* Visual Studio Code
* Node.js
* NPM

### Install Visual Studio Code Extensions

* [EditorConfig](https://marketplace.visualstudio.com/items?itemName=chrisdias.vscodeEditorConfig)
* [Eslint](https://marketplace.visualstudio.com/items?itemName=dbaeumer.vscode-eslint)

## Built With

* [ASP .NET Core](https://github.com/aspnet/home) - Cross-platform framework for building modern web apps
* [Docker](https://www.docker.com) - Container technology for packaging application together with it's dependencies
* [React](https://github.com/facebook/react) - JavaScript library for building user interfaces
* [React Slingshot](https://github.com/coryhouse/react-slingshot) - Boilerplate for React + Redux development
* [SonarQube](https://www.sonarqube.org/) - Continuous inspection of code quality to perform automatic reviews with static analysis of code to detect bugs, code smells, and security vulnerabilities.

## How to run application

### backend
1. Open your Visual Studio and clone this repository 
2. Check in which folder your project will be saved
3. Run project (Backend should launch)
### frontend
1. Open command line (Preferably in Admin mode to make sure everything works right)
2. Go to the folder where your swapify project is stored
3. Go to the WebApp folder
4. write: npm install
5. write: npm start 

After these steps, the application should launch in your browser.

If you get error that npm start can't launch then try this solution: https://stackoverflow.com/questions/45499656/error-occured-when-executing-command-npm-run-serve