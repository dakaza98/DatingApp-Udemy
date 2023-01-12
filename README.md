# Dating app

## Installaiton Instructions

### Backend
1. [Install .NET 7 SDK](https://dotnet.microsoft.com/en-us/download)
2. Setup a dev https certificate ([instructions for wsl](https://docs.microsoft.com/en-us/aspnet/core/security/enforcing-ssl?view=aspnetcore-6.0&tabs=visual-studio#trust-https-certificate-from-windows-subsystem-for-linux))
3. Install .NET entity framework CLI `dotnet tool install --global dotnet-ef`
4. Add the dotnet tools folder to PATH `export PATH=$PATH:~/.dotnet/tools`
5. Run all migrations `dotnet ef database update`

### Frontend
1. [Install nodejs (> 16.10)](https://nodejs.org/en/)
3. Run `npm i`
4. Install the https certificate `StudentAssets/generateTrustedSSL/server.crt`. On WSL copy the file to windows

## Starting
1. Run `dotnet watch run` in the `API` folder
2. Run `ng serve` in the `client` folder
