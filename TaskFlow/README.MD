# TaskFlow API

## Configuration

Pour configurer les urls/ports de l'api modifier la valeur de <u>applicationUrl</u> dans le fichier ```launchSettings.json```.

```JSON
{
  "$schema": "https://json.schemastore.org/launchsettings.json",
  "profiles": {
    "http": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": false,
      "applicationUrl": "http://localhost:5072",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    },
    "https": {
      "commandName": "Project",
      "dotnetRunMessages": true,
      "launchBrowser": false,
      "applicationUrl": "https://localhost:7228;http://localhost:5072",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```

Si votre serveur SQL n'est pas heb�rg� sur localhost, vous devez �galement modifier la cha�ne de connexion dans le fichier ```appsettings.json```.
```JSON
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TaskFlow;User Id=sa;Password=Your_password123;"
  }
}
```

## Execution

Pour lancer le serveur �xecuter le fichier ```TaskFlow.exe```.


## Swagger UI

Le lien pour acc�der � la documentation de l'API est le suivant : ```https://[URL]:[PORT]/swagger/```