# CRM DarkSlateBlue

Un système de gestion de la relation client (CRM) développé en C# avec ASP.NET Core.

## Caractéristiques

- **Architecture MVC** : Application structurée selon le modèle Modèle-Vue-Contrôleur
- **Vertical Slice Architecture** : Organisation par fonctionnalités dans le dossier `Features/`
- **Base de données SQLite** : Base de données légère et autonome
- **Authentification sécurisée** : Système d'authentification avec ASP.NET Identity
- **Gestion complète des entités** :
  - **Contacts** : Gestion des contacts individuels
  - **Clients** : Gestion des entreprises clientes
  - **Fournisseurs** : Gestion des fournisseurs
  - **Prospects** : Suivi des prospects avec statuts
  - **Communications** : Historique des communications avec toutes les entités

## Prérequis

- .NET 10.0 SDK ou supérieur
- Un navigateur web moderne

## Installation

1. Clonez le repository :
```bash
git clone https://github.com/gladir/CRM-DarkSlateBlue.git
cd CRM-DarkSlateBlue
```

2. Restaurez les dépendances :
```bash
dotnet restore
```

3. Appliquez les migrations de base de données :
```bash
dotnet ef database update
```

4. Lancez l'application :
```bash
dotnet run
```

5. Ouvrez votre navigateur à l'adresse : `https://localhost:5269` ou `http://localhost:5268`

## Utilisation

### Première connexion

1. Cliquez sur **Register** pour créer un compte utilisateur
2. Remplissez le formulaire d'inscription
3. Connectez-vous avec vos identifiants

### Gestion des entités

Une fois connecté, vous pouvez :
- **Contacts** : Ajouter, modifier, supprimer et consulter les contacts
- **Clients** : Gérer votre portefeuille clients
- **Fournisseurs** : Maintenir la liste de vos fournisseurs
- **Prospects** : Suivre vos opportunités commerciales avec différents statuts (New, Contacted, Qualified, etc.)
- **Communications** : Enregistrer toutes vos interactions (emails, appels, réunions, notes)

### Communications

Les communications peuvent être associées à :
- Un contact
- Un client
- Un fournisseur
- Un prospect

Chaque communication comprend :
- Un sujet
- Un contenu
- Un type (Email, Phone, Meeting, Note, Other)
- Une date

## Structure du projet

```
CRM-DarkSlateBlue/
├── Features/              # Vertical Slice Architecture
│   ├── Contacts/         # Fonctionnalité Contacts
│   ├── Clients/          # Fonctionnalité Clients
│   ├── Suppliers/        # Fonctionnalité Fournisseurs
│   ├── Prospects/        # Fonctionnalité Prospects
│   └── Communications/   # Fonctionnalité Communications
├── Models/               # Modèles de données
├── Views/                # Vues Razor
├── Data/                 # Contexte de base de données et migrations
├── Controllers/          # Contrôleurs MVC de base
└── wwwroot/             # Fichiers statiques (CSS, JS, images)
```

## Technologies utilisées

- **ASP.NET Core 10.0** : Framework web
- **Entity Framework Core** : ORM pour l'accès aux données
- **SQLite** : Base de données
- **ASP.NET Identity** : Authentification et autorisation
- **Bootstrap 5** : Framework CSS pour l'interface utilisateur
- **Razor Pages** : Moteur de templates

## Sécurité

- Authentification obligatoire pour accéder aux fonctionnalités CRM
- Mots de passe hashés avec ASP.NET Identity
- Protection CSRF sur tous les formulaires
- Validation des données côté serveur

## Licence

Voir le fichier LICENSE pour plus de détails.
