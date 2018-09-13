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
- Remove `app.UseHttpsRedirection();` from Startup.cs
- `Ctrl + Shift + B` to build (`Shift + ⌘ + B` on a Mac)
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

- Add the content of the `Game` folder to your `wwwroot` folder.
- Verify that the game runs on [http://localhost:5000/Game.html](http://localhost:5000/Game.html).

![](doc/game.png)

#### Link game from example site

- Add `<li><a href="Game.html">Game</a></li>` to `views/Shared/_Layout.cshtml`.
- Verify that the game can be opened from the menu 

#### Create High Score service

- Create a directory called `Services`
- In it, create the interface of a simple high score service, `IHighScoreService.cs`.
- Implement it in a new file called `HighScoreService.cs` in the same directory.
- Add it as a singleton to the service collection in `Startup.cs`
- Create `HighScoreController`. Check that it works by visiting [http://localhost:5000/api/highscore](http://localhost:5001/api/highscore) (You might need to restart the server).
  - Verify the value is 0, posting a high score of 1 using _Postman_ and content type JSON, and verifying that the new high score is indeed one.

![](doc/PostmanGet.PNG)
![](doc/PostmanPost.png)

#### Integrate High Score service

- Create a new JS class in `high_score_manager.js` in `wwwroot/js`
  - The class should have a `setHighScore` and a `getHighScore` method which should use your highscore service to get and update the highscore
- Add this file to `Game.html`'s script imports.
- Add `HighScoreManager` to `application.js`.
- Use the `HighScoreManager` in `game_manager.js`.
- Verify that `scores/highScore.json` is written to disk in the _bin-folder_.

## Part 2: Local machine ain't good enough

Now we'll host the webapp in a Docker container

### First Docker try

- Change the `highScoreEndpoint` in `HighScoreManager` to [http://localhost/api/highscore](http://localhost/api/highscore).
- Create a `Dockerfile` to build and then contain the application
- Create a `.dockerignore` file to copy the minimum needed files to the build context
- Run `docker build -t dips/workshop .` to build the docker image
- Run `docker run -p 80:80 dips/workshop` to run the app through in container
- Verify that it works in the browser

### Adding mounting to preserve high score

- Run `docker run -p 80:80 -v <Path-To-Project>/scores:/app/scores dips/workshop`
  - For windows you need to enable drive sharing in Docker settings

## Part 3: Divide and conquer

We don't want the website and API in the same app. Let's fix this.

### Create a new folder for the web project

- Create a new folder called `web` in the root folder.
- Move all files and folders to `web`, except `LICENSE` and `readme.md`.
- Recreate `.dockerignore` if it got lost in the moving process

### Move high score to own app

- Create a new folder called `api` in the root folder.
- Navigate into the `api` folder and create a new app `dotnet new webapi`
- Open the project i VS Code `code .`
- Move `IHighScoreService`, `HighScoreService` and `HighScoreController` to the new app.
- Move `services.AddSingleton<IHighScoreService, HighScoreService>();` to the new `Startup`.
- Remove `using Workshop.Services;` from the original `Startup`.
- Verify that the new service works using Postman.

### Create another Docker container

All commands here are called from the `api`-folder.

- Create a `Dockerfile` to build and then contain the application
- Create a `.dockerignore` file to copy the minimum needed files to the build context
- Run `docker build -t dips/api .` to build the docker image
- Run `docker run -p 80:80 -v <Path-To-Project>/api/scores:/app/scores dips/api` to run the app through the container
- Verify it works using Postman.

### Make them work together

- Create a new file, `docker-compose.yml`, in the root folder.
- In `high_score_manager` in the web-project, change `highScoreEndpoint` to [http://api/highscore](http://api/highscore).
- Navigate to the `web` folder and run `docker build -t dips/workshop .`
- Navigate back to the root folder and run `docker-compose up`.
- Enjoy your apps working together.