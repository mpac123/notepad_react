# Notepad
Project for NTR class in Warsaw Univeristy of Technology.

## Technology stack
1. ReactJS + some components from Semantic-UI-React and Metarial Design as frontend
2. Dotnet Web API with EF Core in the backend
3. MSSQL database

## Requirements
Project uses a MSSQL database server. The simplest way to run a server locally is via docker:
```
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourPassword" -p 1433:1433 --name sql1 -d mcr.microsoft.com/mssql/server:2019-GA-ubuntu-16.04
```
Make sure to set up an appriopriate connection string in `appsettings.Developmnent.json` file.

## Usage
Run `dotnet run` command and go to the `localhost:5000` in your favourite browser.