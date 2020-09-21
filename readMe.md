# App web System de suivi ..

## Le projet à éte developpé par les technologies suivantes : 
[ASP.net Core 3.1 SDK](https://dotnet.microsoft.com/download)

[Node js et NPM](https://nodejs.org/en/download/) & [Angular 9](https://cli.angular.io/)

[Git for Windows](https://git-scm.com/downloads)

## Etape d'instalation : 

### clone le repo
```
git clone https://github.com/djm2x/mem
```

### Pour installer les dependences éxécuter la commande suivante : 
```
npm run install
```

### pour le lancer le back-end asp.net core restful api :
```
npm run b
```

### pour lancer le projet front-end angular : 
```
npm run f 
```

### configurer heroku pour deploye l'application

- Crée un compte [Heroku](https://www.heroku.com/)
- Dans le Dashboard clique sur new => `create a new app`
- Donner un nom à l'application et choisie un la région proche a vous => `create app`
- Dans l'ongle Settings -> `add Buildpacks` : https://github.com/anuraj/dotnetcore-buildpack (pour qui Heroku support ASP.NET Core)
- Dans l'ongle `Deploy`, choisie comme `Deployment method` GitHub, connectez-vous après choisie votre repo est cliqué sur `connect` -> `Enable Automatic Deploy`


### pour deployer le projet sur Heroku : 
Cette commande au dessous (plus de detail dans package.json -> scripts) va pushe les changement dans votre repo, puis Heroku ecoute les action faite sur votre repo alors ila copy et builder votre app , et le site sera actualiser
```
npm run ci 
```
### pour deployer le projet sur IIS : 
- 1. Dans le fichier Startup.cs , change la ligne 133 , utiliser SqlServer au lieu de Sqlite
- 2. Dans le fichie appsettings.json, modifier la chaine de connexion de base de donnee dans la ligne 10
- 3. Pour genere la base de donne install ce package de entity framwork core
```
dotnet tool install --global dotnet-ef
```
- apres lance la commande de generation:
```
npm run m
```
- 4. Apres genere la distribusion d'Angular avec
```
npm run f-p
```
- 5. Finalement genere le deploy d'ASP .net core avec
```
dotnet publish -o D:/chemin souhaite
```

### Useful Angular cli command:
ng g m controle --route controle --module admin

