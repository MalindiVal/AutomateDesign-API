# AutomateDesign
AutomateDesign est un projet qui consiste en la création d'un schéma d'automate avec ses états et ses transitions

# Objectif du projet
Le but d’AutomateDesign est de fournir un outil intuitif permettant de :
- Concevoir visuellement des automates à états finis
- Définir leurs transitions et propriétés
- Sauvegarder et charger des automates depuis une base de données
- Exporter les automates pour réutilisation dans d’autres applications.

Ce projet met en œuvre plusieurs technologies modernes de développement .NET (C#, WPF, ASP.NET, SQLite, Docker) afin de proposer une solution robuste et maintenable.

# Fonctionnalités
- Gestion centralisée des automates via une base SQLite
- Endpoints RESTful pour interagir avec les données
- Sérialisation/désérialisation des modèles
- Hébergement compatible Docker
### Contrôleur principal : AutomateController
- /GetAllAutomates [GET] : Récupèration des automates présents dans la base de données
- /GetAllAutomatesByUser [Post] : Récupèration des automates crées par un utilisateur présents dans la base de données
- /ExportAutomate [POST] : Exportation d'un Automate
- /UpdateAutomate [PUT] : Mise à jour d'un Automate
- /GetAutomateById?id=[Insérer identifiant Automate] [GET] : Récuperartion d'un automate en fonction d'un id
### Contrôleur des utilisateurs : UtilisateurController
- /Login [POST] : Récupèration des automates présents dans la base de données
- /Register [POST] : Exportation d'un Automates

# Installation & exécution
## Prérequis
- .NET 8 SDK ou supérieur
- Docker (optionnel pour déploiement)
- Visual Studio 2022.

## Cloner le dépôt
- Ouvrez Powershell sur le dossier de votre choix
- Cloner le projet
```
git clone https://github.com/dept-info-iut-dijon/S5_ModernTech_AutomateDesign.git
```

## Lancer l’API
- Cliquez sur le dossier Client
  
<img width="196" height="259" alt="image" src="https://github.com/user-attachments/assets/5d851ae1-3668-4075-97e8-f9fb870d020b" />

- Cliquez sur la solution AutomateDesign.sln

<img width="252" height="429" alt="image" src="https://github.com/user-attachments/assets/ce6f62aa-d0bb-426e-a946-7d1de3d5895c" />

- Choisisez Visual Studio 2022 pour ouvrir la solution
- Cliquez sur la flèche à coté de l'engrenage et sélectionner API

<img width="253" height="49" alt="image" src="https://github.com/user-attachments/assets/cd300ee9-0145-43e9-8f2d-fd82524ddaf5" />

<img width="190" height="189" alt="image" src="https://github.com/user-attachments/assets/c56a63bd-23d1-4230-a094-61279260baed" />

- Vous pouvez choisir le type d'API (https,http,Docker Container) en cliquant sur la fleche à coté du bouton démarrer

<img width="350" height="333" alt="image" src="https://github.com/user-attachments/assets/833e98bc-c8d8-42fd-af39-40651620df08" />

- Vous pouvez également choisir le navigateur pour le swagger (Chrome,Edge,Brave,FireFox(Recommandé)) en cliquant sur la fleche à coté du bouton démarrer

<img width="620" height="99" alt="image" src="https://github.com/user-attachments/assets/7854a413-970f-49a2-891a-8a7292c330a5" />

## Lancer le client WPF
- Cliquez sur le dossier Client
  
<img width="196" height="259" alt="image" src="https://github.com/user-attachments/assets/5d851ae1-3668-4075-97e8-f9fb870d020b" />

- Cliquez sur la solution AutomateDesign.sln

<img width="252" height="429" alt="image" src="https://github.com/user-attachments/assets/ce6f62aa-d0bb-426e-a946-7d1de3d5895c" />

- Choisisez Visual Studio 2022 pour ouvrir la solution
- Cliquez sur la flèche à coté de l'engrenage et sélectionner IHM

<img width="189" height="191" alt="image" src="https://github.com/user-attachments/assets/5d7a0368-212f-4b04-8eef-afafa28bcb7a" />

- Cliquez sur le bouton Démarrer

<img width="66" height="63" alt="image" src="https://github.com/user-attachments/assets/5d433954-0ea7-478d-a4d5-3a285cbd9310" />

## Lancer le client et l'API
- Cliquez sur le dossier Client
  
<img width="196" height="259" alt="image" src="https://github.com/user-attachments/assets/5d851ae1-3668-4075-97e8-f9fb870d020b" />

- Cliquez sur la solution AutomateDesign.sln

<img width="252" height="429" alt="image" src="https://github.com/user-attachments/assets/ce6f62aa-d0bb-426e-a946-7d1de3d5895c" />

- Choisisez Visual Studio 2022 pour ouvrir la solution
- Cliquez sur la fleche à coté de démarrer et choissiez "Configurez des projets de start-up"

<img width="280" height="98" alt="image" src="https://github.com/user-attachments/assets/419b8270-83d9-4cec-9c51-9b0ce5a02e8e" />

- Cocher plusieurs projets de démarrages

<img width="223" height="381" alt="image" src="https://github.com/user-attachments/assets/c5857362-353d-450a-ba91-8cfe73c878fc" />

- Activer IHM et API (avec le type de démarrage de l'api (https,http,Docker))

<img width="309" height="242" alt="image" src="https://github.com/user-attachments/assets/40c464fe-cb47-4a2f-9030-99f9b71e50eb" />

- Cliquez sur Appliquer

<img width="587" height="384" alt="image" src="https://github.com/user-attachments/assets/349c3c05-68b2-40b7-98be-3ea2c8735107" />


### Configuration de la connexion Client-API
La connexion entre le client et l'API est gérée dans un projet nommé ClientData et les adresses sont stockées dans le fichier appsettings.json (il se trouve également dans le dossier contenant l'executable dans les releases)

<img width="669" height="148" alt="Capture d’écran 2025-11-13 142229" src="https://github.com/user-attachments/assets/53c19d25-5203-4c93-bae9-8f9fdb6aff8a" />

- LocalMode permet à la classe APIConfig de savoir si il doit utiliser l'adresse se trouvant dans LocalUrl ou BaseUrl (true (le client utilisera l'api en localhost) ou false (le client utilisera l'api qui est actuellement déployé))
- BaseUrl coresspond à l'adresse de l'API déployé
- APICertificate correspond aux certificat de sécurité de l'API (SHA-1)
- LocalUrl correspond à l'adresse utilisé pour l'hébergement local de l'API

## Hébergement de l'API
L'hébergement est réalisé sur Docker sur un serveur uniquement accesible via le VPN de l'IUT de Dijon

### Installer Docker Desktop
Allez sur le site officiel de Docker : https://www.docker.com/
- Cliquez sur "Download Docker Desktop" et selectionnner le systeme d'exploitation de votre choix

<img width="419" height="420" alt="image" src="https://github.com/user-attachments/assets/ebb79ede-a3f1-4b98-8f50-9b74ef25ba1d" />

- Cliquez sur l'executable et suivez ensuite les consignes 
Quand vous lancez Docker Desktop pour la première fois

- Cliquez sur accepter

<img width="798" height="499" alt="image" src="https://github.com/user-attachments/assets/3bbdcc22-38fd-4f32-b272-407920dcba24" />

- Cliquez sur "Continue without signing in"

<img width="463" height="379" alt="image" src="https://github.com/user-attachments/assets/1645d8dd-42a1-436e-9cbd-d405b0618255" />

- Cliquez sur "Skip survey"

<img width="511" height="609" alt="image" src="https://github.com/user-attachments/assets/806b7891-47c6-42ef-bbe4-40f6087d2c19" />

Vous voila sur le tableau de port de docker desktop


### Installer un sous-systeme WSL

- Cliquez sur Affichage puis Terminal pour afficher le terminal powershell

<img width="341" height="479" alt="image" src="https://github.com/user-attachments/assets/c1b645d0-ded1-4a39-9d02-758ee610322e" />

![Enregistrement 2025-12-08 180029](https://github.com/user-attachments/assets/a2280d18-24e4-4a60-942e-4af4a8b5b9bf)

- Faites la commande sur le terminal pour installer un wsl :
```
wsl --install -d [inserer la distribution linux de votre choix (ex : Debian)]
```
<img width="427" height="22" alt="image" src="https://github.com/user-attachments/assets/792e6fd2-aa26-42ba-b62b-37fbd8b8443f" />

- Patientez le temps que la distribution soit téléchargée

<img width="426" height="39" alt="image" src="https://github.com/user-attachments/assets/583f540e-1567-49bd-add6-2c113b2d7c5d" />

- Après l'installation, il vous sera demander de créer un compte utilisateur

<img width="716" height="46" alt="image" src="https://github.com/user-attachments/assets/7900ea9c-57d4-4a9b-9599-91bbbd0bfe96" />

Une fois que vous aurez crée votre mot de passe, vous aurez acces au terminal du wsl

### Lier Docker Desktop à la distribution Linux
Allez dans le menu paramètre en cliquant sur les engrenages :
<img width="173" height="40" alt="image" src="https://github.com/user-attachments/assets/71119dcd-b38a-4ed1-8396-1de676be77fa" />

- Cliquez sur Ressources 

<img width="248" height="398" alt="image" src="https://github.com/user-attachments/assets/271549ce-4519-41c5-b52b-b38c7e16f570" />

- Cliquez sur WSL integration

<img width="245" height="165" alt="image" src="https://github.com/user-attachments/assets/58ce6ac4-ea18-496c-ab21-59f2479e78d6" />

- Cliquez sur le toggle avec le nom de votre distribution

<img width="435" height="196" alt="image" src="https://github.com/user-attachments/assets/34c7ef90-966c-4b94-9163-2a659acb8d02" />

- Et cliquez sur "Apply & restart"

### Lancer l'API EN Mode Release
- Sélectionnez le projet API : 

<img width="172" height="190" alt="image" src="https://github.com/user-attachments/assets/c6da7973-9aa8-4a9d-9a4e-fc167a4d9fa3" />

- Selectionner le mode Release :

<img width="186" height="81" alt="image" src="https://github.com/user-attachments/assets/5638aacc-9bb1-459f-ad2e-db108b0efa25" />

Cliquez-droit sur la solution puis cliquer sur "Nettoyer la solution"

<img width="436" height="72" alt="image" src="https://github.com/user-attachments/assets/cf943c51-9e9e-4850-96a4-e7bee30d6866" />

- Cliquez-droit à nouveau sur la solution puis cliquer sur "Régénérer la solution"

- Vérifier que l'option de lancement de l'api soit "Container (Dockerfile)"

<img width="221" height="45" alt="image" src="https://github.com/user-attachments/assets/1768726f-94d7-41ad-bbf2-860c26630d79" />

- Sinon, Cliquez sur la flèche à coté du bouton démarer et sélectionnez l'option

<img width="350" height="166" alt="image" src="https://github.com/user-attachments/assets/52114ad4-c3ad-4ec3-9662-fc722addd4f2" />

- Enfin cliquer sur le bouton de démarrage

<img width="407" height="40" alt="image" src="https://github.com/user-attachments/assets/65303750-f2ad-42fa-9326-c65834184baf" />

Cela déclenche la compillation du fichier Dockerfile pour configurer le container local

- Répondez oui au differents fenetres et cliquez sur continuer le débougage

<img width="500" height="191" alt="image" src="https://github.com/user-attachments/assets/f4066f33-38b4-4792-b8b3-39e5d53fed25" />

<img width="411" height="390" alt="image" src="https://github.com/user-attachments/assets/0629bc45-dc5b-4b98-81f5-a4f8abe07c72" />

<img width="596" height="306" alt="image" src="https://github.com/user-attachments/assets/8b58deaa-2a36-4403-91d6-ca294ef794f1" />


L'api sera alors affiché dans le tableau de bord de Docker Desktop

<img width="992" height="445" alt="image" src="https://github.com/user-attachments/assets/9bbe6fda-2ae8-4186-8a52-5112514825a6" />

### Récupération de l'image de l'API
- Ouvrez le terminal et ouvrez votre wsl

- Faites la commande suivante  
```
sudo docker save api:latest | gzip > [Nom d'archive de votre choix].tgz
```
<img width="589" height="37" alt="image" src="https://github.com/user-attachments/assets/758ebde5-ddb5-4b5a-b902-e440cb12ff91" />

Cette commande enregistrera l'image de l'api dans une archive tgz

- Cela devrait etre dans votre compte utilisateur ubuntu

- Allez ,dans l'explorateur de fichiers, dans le dossier Linux

<img width="138" height="421" alt="image" src="https://github.com/user-attachments/assets/f194a975-8fd1-4054-a439-b50d923345f1" />

- Ensuite, cliquez sur le dossier correspondant à votre distribution

<img width="724" height="97" alt="image" src="https://github.com/user-attachments/assets/2e90a229-6709-499a-8bbf-7b0d5a2522a6" />

- Cliquez sur le dossier home

<img width="639" height="177" alt="image" src="https://github.com/user-attachments/assets/d9d60c8a-a390-4cfd-95a6-24bc4f9d68a4" />

- Enfin ouvrez sur le dossier en votre nom : (ici , c'est vm009962)

<img width="598" height="100" alt="image" src="https://github.com/user-attachments/assets/8c67f0cb-e10a-46f7-8ddc-644bd44d97f7" />

l'archive devra se trouvait dans ce dossier

<img width="645" height="242" alt="image" src="https://github.com/user-attachments/assets/d57a06ce-0529-47de-b5fb-0350239e557c" />

- Copiez l'archive dans un endroit plus accessible
### Connexion sur Portainer
- Connectez-vous  à votre Portainer

<img width="471" height="501" alt="image" src="https://github.com/user-attachments/assets/1e552621-cac9-47bc-b3cf-309b0d497feb" />

- Selectionner votre environement 

<img width="1597" height="397" alt="image" src="https://github.com/user-attachments/assets/dfacfb72-3b03-4f83-b8d9-29f7d07d1cf5" />

### Importation d'une Image
- Allez dans la page Image

<img width="254" height="272" alt="image" src="https://github.com/user-attachments/assets/cf45a8ed-25f5-4418-9ecd-35cf2a7c569d" />

- Cliquez sur Import

<img width="1571" height="80" alt="image" src="https://github.com/user-attachments/assets/f1101ba0-8552-4edb-93de-c63ab87d7192" />

- Cliquez sur "Select File"

<img width="516" height="515" alt="image" src="https://github.com/user-attachments/assets/5a663e8c-aaf9-4934-beb7-cb8213d172a6" />

- Selectionnez l'archive contenant l'image de l'api

<img width="810" height="493" alt="image" src="https://github.com/user-attachments/assets/3f9e518e-a62d-4c01-9318-b390128ff2d7" />

- Cliquez sur Upload

<img width="333" height="531" alt="image" src="https://github.com/user-attachments/assets/e41016c4-79f4-4fd5-87ed-c6b246665f34" />


### Création d'un container
- Cliquez sur l'onglet Containers

<img width="264" height="280" alt="image" src="https://github.com/user-attachments/assets/7b155ecd-964f-4cb9-9434-d72e42471135" />

- Cliquez sur "Add a container"

<img width="1581" height="68" alt="image" src="https://github.com/user-attachments/assets/b143448c-805f-4db1-9b81-a8adce988918" />

- Indiquez le nom de votre Container

<img width="401" height="60" alt="image" src="https://github.com/user-attachments/assets/afbf5a14-13fb-4f28-a40e-12bfaeff56e1" />

- Cliquez sur advanced mode

<img width="176" height="194" alt="image" src="https://github.com/user-attachments/assets/2f590fb3-53c5-45fa-8936-6286f16ef8cf" />

- Inserer le nom de l'image

<img width="485" height="106" alt="image" src="https://github.com/user-attachments/assets/7a790a20-b69a-4952-89cf-c43cf76fafa7" />

- Indiquer les ports pour la version http(8080) et https(8081)

<img width="723" height="144" alt="image" src="https://github.com/user-attachments/assets/379d7bd3-5ede-4040-aa42-f8c112e75a2f" />

- Pour le contrôle d'accès , sélectionnez "restricted" 

<img width="1546" height="187" alt="image" src="https://github.com/user-attachments/assets/b74af828-15c1-4274-984f-561253cddc63" />

- Enfin, Cliquez sur "deploy the container"

<img width="213" height="66" alt="image" src="https://github.com/user-attachments/assets/bd71bd3c-67a7-4895-b9ac-e5cf17d38ba8" />

### Récupèration du certificat
- Utilisez firefox pour récuperer plus facilement le certificat
- Utilisez la racine du lien de votre portainer et faites la requete pour récupèrer l'intégralité des automates
Dans l'exemple, ce sera :
```
https://10.128.207.62:8081/Automate/GetAllAutomates
```

- Cliquez sur le verrou

<img width="373" height="43" alt="image" src="https://github.com/user-attachments/assets/ee3f2d72-ee9b-4a0f-8ea7-a4ae827e18c4" />

- Cliquez sur "Connexion non sécurisée" 

<img width="372" height="225" alt="image" src="https://github.com/user-attachments/assets/0f0ef7ce-ed51-43b3-9ff2-8575a564155a" />

- Cliquez sur puis "plus d'informations"
<img width="374" height="227" alt="image" src="https://github.com/user-attachments/assets/6276f072-f672-455f-8f5c-6e75e54779f5" />

- Cliquez sur "afficher le certificat"

<img width="693" height="165" alt="image" src="https://github.com/user-attachments/assets/6545677b-c7f0-431c-809f-028f7f091006" />

- Descendez le certificat jusqu'à la section "Empreintes numériques"

<img width="794" height="137" alt="image" src="https://github.com/user-attachments/assets/f89be394-be33-4299-8fbd-231b029e5360" />

- Copier l'empreinte SHA-1

<img width="794" height="137" alt="image" src="https://github.com/user-attachments/assets/deb8b69e-de23-400c-8d76-153e804e37c2" />

- Allez dans le fichier appsettings.json du projet ClientData ou dans le dossier avec l'executable et modifié la section APICertificate en remplaçant l'ancien par le certificat que vous avez copiez

<img width="632" height="113" alt="image" src="https://github.com/user-attachments/assets/9471872e-4522-4502-bf79-36ffa21ff527" />


# Structure du Projet
Le projet est composé de plusieurs projets :
- Un Client IHM en WPF
- Une API ASP.NET connecté à une base de données en SQLite
- Un LogicLayer commun au Client et à l'API
- Une bibliothèque dédiée à la gestion des données coté client
- Une bibliothèque dédié aux ViewModels qui font le lien entre le logiclayer et l'IHM
- Des tests unitaires pour les differents projets

## Structure des données
### MCD (Modele conceptuelle des données)
<img width="1217" height="540" alt="image" src="https://github.com/user-attachments/assets/64e87579-b3c3-4947-9b65-2d52b4f85f59" />

Nos données sont stockées dans une base de données SQLite structurées comme le MCD ci-dessus





