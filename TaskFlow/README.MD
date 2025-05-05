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

Si votre serveur SQL n'est pas hebérgé sur localhost, vous devez également modifier la chaîne de connexion dans le fichier ```appsettings.json```.
```JSON
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TaskFlow;User Id=sa;Password=Your_password123;"
  }
}
```

## Execution

Pour lancer le serveur éxecuter le fichier ```TaskFlow.exe```.


## Swagger UI

Le lien pour accéder à la documentation de l'API est le suivant : ```https://[URL]:[PORT]/swagger/```