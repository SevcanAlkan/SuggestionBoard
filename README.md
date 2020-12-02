## Introduction
[![Docker](https://img.shields.io/docker/cloud/build/sevcanalkan/suggestion-board?label=Docker&style=flat)](https://hub.docker.com/r/sevcanalkan/suggestion-board/builds)

The SuggestionBoard project is a basic N-Tier Layered ASP.NET Core MVC application. I used Dependency Injection, UnitOfWork pattern, EntityFramework(Code first approach), AutoMapper on the project. You can check "ProjectDocumentation.pdf" file for details.

## Requirements

- Dotnet Core 3.1 SDK [Download](https://dotnet.microsoft.com/download/dotnet-core/3.1)
- Entity Framework Core Tools, use `dotnet tool install --global dotnetef` command to install [More Information](https://docs.microsoft.com/en-us/ef/core/cli/dotnet)
- Docker [Download](https://docs.docker.com/desktop/)

**I used Windows 10 version 20H2 as host machine operating system.*

## Preparing to start

 1. Install Dotnet Core 3.1 SDK
 2. Firstly, open the command-line and then navigate to directory of the project files. The
    `docker-compose.yml` file must be located into this directory, be sure about that. Execute `docker-compose up` command into command-line.
    That process can take a while. After that `MSSQL`, `Elasticsearch`, and `Kibana` will be started running on Docker.
 3. Navigate to `Suggestionboard.Data` folder. Then execute ``dotnet ef database update –
    startup-project ..\SuggestionBoard.Web\SuggestionBoard.Web.csproj`` to create database on the
    MS-SQL Server.
 4. Lastly, navigate to the `SuggestionBoard.Web` directory, then execute `dotnet run`
    command. The ASPNET project will be started. Open the web browser and enter
    https://localhost:5001 URL into the address bar. 
