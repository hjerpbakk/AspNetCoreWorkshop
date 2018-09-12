# Workshop Tromsø 20.09.2018

An ASP.net Web app running in containers orchestrated by Docker Compose. In this workshop, all commands are run from the root folder of project unless otherwise noted.

## Prerequisites

- Install `git`
- Install the latest version of `.Net Core`.
- Install the latest version of `Docker`, using the edge channel.
- Install `Visual Studio Code`.
- Run `docker run --rm microsoft/dotnet:2.1-sdk-alpine`.
- Run `docker run --rm microsoft/dotnet:2.1-aspnetcore-runtime-alpine`.
- Install postman.

## Part 1: Anatomy of an ASP.Net Web App

In this part you will create a simple ASP.Net web app and web API, familiarising yourself with their architectures.

### Create app from template

- `mkdir Workshop`
- `cd Workshop`
- `dotnet new mvc`

### Use Visual Studio Code to develop and debug

- `code .`

![](doc/vscode.png)

- Press *yes*.
- `Shift + CMD + B` to build
- `F5` to debug
- You should see the default ASP.Net website template running in your browser.

### Get to know ASP.Net

- Static resources
- Model View Controller
- Dependency Injection
- Configuration
- Routes
- Request pipeline

### Implement an actual app

Now you'll add a simple 2048 game and high score services. 

#### Create HTML5 game

- Add the content of the `Game` folder to the `wwwroot` folder.
- Verify that the game runs on [https://localhost:5001/Game.html](https://localhost:5001/Game.html).

TODO: Bilde av spillet

#### Link game from example site

- Add `<li><a href="Game.html">Game</a></li>` to `_Layout.cshtml`.
- Verify that the game can be opened from the menu 

#### Create High Score service

- Create the interface of a simple high score service, `IHighScoreService`.
- Implement it in `HighScoreService`.
- Add it as a singleton to the service collection in `Startup`.
- Create `HighScoreController`. Check that it works by visiting [https://localhost:5001/api/highscore](https://localhost:5001/api/highscore), verify the value is 0,posting a high score of 1 using _Postman_, and verifying that the new high score is indeed one.

TODO: two screens fra postman

#### Integrate High Score service

- Create a new JS class in `high_score_manager.js`.
- Add this file to `Game.html`.
- Add `HighScoreManager` to `application.js`.
- Use the `HighScoreManager` in `game_manager.js`.
- Verify that `scores/highScore.json` is written to disk in the _bin-folder_.

## Part 2: Local machine ain't good enough

Now we'll host the webapp in a Docker container

### First Docker try

- Change the `highScoreEndpoint` in `HighScoreManager` to [http://localhost/api/highscore](http://localhost/api/highscore).
- Create a `Dockerfile` to build and then contain the application
- Create a `.dockerignore` file to copy the minimum needed files to the build context
- Run `docker build -t dips/workshop .` to build the docker imager
- Run `docker run -p 80:80 dips/workshop` to run the app through in container

### Adding mounting to prevserve high score

- Run `docker run -p 80:80 -v /Users/sankra/projects/AspNetCoreWorkshop/scores:/app/scores dips/workshop`

## Part 3: Divide and conquer

We don't want the website and API in the same app. Let's fix this.

### Move high score to own app

- Create a new folder called `api` 
- Navigate to the folder and create a new app `dotnet new webapi`
- Open the project i VS Code `code .`

### Create another Docker container

### Make them work together
