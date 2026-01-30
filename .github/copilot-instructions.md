# CRM DarkSlateBlue - AI Coding Instructions

## ⚠️ DIRECTIVE LINGUISTIQUE PRIORITAIRE

**IMPORTANT**: Cette application est entièrement en français. Tous les messages, commentaires, noms de variables, textes d'interface utilisateur, et communications doivent être rédigés en français. Utilisez toujours le français comme langue principale dans votre code et vos réponses.

## Vue d'ensemble de l'architecture

Il s'agit d'une application CRM ASP.NET Core 10.0 utilisant l'**Architecture par Tranches Verticales** organisée par fonctionnalités métier plutôt que par couches techniques. Chaque fonctionnalité est autonome dans le répertoire `Features/` avec son propre contrôleur.

### Modèles architecturaux clés

- **Tranches verticales** : Les contrôleurs se trouvent dans les répertoires `Features/{NomEntité}/`, pas dans un dossier `Controllers/` partagé
- **Vues partagées** : Toutes les vues Razor restent dans la structure traditionnelle `Views/` pour la cohérence  
- **Hub de communication** : L'entité `Communication` agit comme un hub relationnel polymorphe connectant toutes les autres entités (Contacts, Clients, Prospects, Fournisseurs) via des clés étrangères optionnelles
- **Intégration Identity** : Utilise ASP.NET Identity avec SQLite, authentification requise pour toutes les fonctionnalités CRM (`[Authorize]` sur les contrôleurs)

## Schéma de base de données

Les entités principales suivent ce modèle relationnel :
```csharp
// Communication peut être liée à N'IMPORTE QUELLE entité via des FK nullables
public class Communication {
    public int? ContactId { get; set; }      // Lien vers Contact
    public int? ClientId { get; set; }       // Lien vers Client  
    public int? ProspectId { get; set; }     // Lien vers Prospect
    public int? SupplierId { get; set; }     // Lien vers Fournisseur
}
```

Toutes les entités ont des propriétés de navigation `ICollection<Communication>` avec suppression en cascade configurée dans `ApplicationDbContext.OnModelCreating()`.

## Workflows de développement

### Exécution de l'application
```bash
dotnet run                    # Démarre sur http://localhost:5269
dotnet ef database update     # Applique les migrations en attente
dotnet ef migrations add {Nom}  # Crée une nouvelle migration
```

### Structure du projet
```
Features/
├── Clients/ClientsController.cs      # Logique métier pour les clients
├── Contacts/ContactsController.cs    # Logique métier pour les contacts
└── Communications/CommunicationsController.cs

Views/
├── Clients/                          # Vues liées aux clients
├── Communications/                   # Vues des communications
└── Shared/_Layout.cshtml            # Mise en page principale avec navigation
```

## Conventions de codage

### Modèles de contrôleurs
- Placer les contrôleurs dans `Features/{NomEntité}/{NomEntité}Controller.cs`
- Tous les contrôleurs CRM héritent de `Controller` et utilisent `[Authorize]` 
- Modèle CRUD standard : `Index()`, `Details(int? id)`, `Create()`, `Edit(int? id)`, `Delete(int? id)`
- Utiliser `OrderByDescending(x => x.CreatedAt)` pour le listage par défaut des entités
- Modèle async/await : `async Task<IActionResult>` avec `ToListAsync()`, `FirstOrDefaultAsync()`

### Conventions des modèles
- Toutes les entités ont `CreatedAt = DateTime.UtcNow` et un `UpdatedAt` optionnel 
- Utiliser les annotations de données : `[Required]`, `[StringLength(n)]`, `[EmailAddress]`, `[Phone]`
- Propriétés de navigation initialisées avec `new List<T>()` pour éviter les références nulles
- Énumérations pour les valeurs contraintes (ex. `CommunicationType`, `ProspectStatus`)

### Gestion spéciale de l'entité Communication
Lors de la création de communications, vérifier quel type d'entité est lié :
```csharp
// Dans CommunicationsController.Create()
if (contactId.HasValue) communication.ContactId = contactId;
if (clientId.HasValue) communication.ClientId = clientId;  
// etc. pour Prospects, Fournisseurs
```

### Vues et navigation
- Navigation principale dans `_Layout.cshtml` inclut toutes les sections d'entités
- Texte d'interface en français partout (ex. "Accueil", "Clients", "Contacts")
- Style Bootstrap 5 avec design responsive
- Validation de formulaires utilisant la validation de modèle ASP.NET Core avec support côté client

## Dépendances externes

- **Base de données SQLite** : Fichier `app.db` à la racine du projet (copié vers la sortie)
- **Entity Framework Core** : Migrations code-first dans `Data/Migrations/`
- **ASP.NET Identity** : Authentification avec interface par défaut, pas de confirmation d'email requise
- **Bootstrap 5** : Via `wwwroot/lib/bootstrap/` pour le style d'interface

## Points d'intégration clés

- `ApplicationDbContext` dans le dossier `Data/` contient tous les DbSets et la configuration des relations
- `Program.cs` configure les services : connexion SQLite, Identity, et Contrôleurs avec Vues
- Flux d'authentification : les pages Identity gèrent la connexion/inscription, `[Authorize]` protège les fonctionnalités CRM
- Toutes les entités se connectent via Communications pour le suivi d'activité et l'historique des relations

## French Language Context

**OBLIGATOIRE** : L'application est entièrement en français. Lors de l'ajout de nouvelles fonctionnalités :
- **Messages d'erreur** : en français ("Email requis", "Format invalide", etc.)
- **Commentaires de code** : en français  
- **Noms de variables** : préférer le français quand approprié (`nomUtilisateur`, `dateCreation`)
- **Interface utilisateur** : maintenir la cohérence avec la terminologie existante : "Clients", "Fournisseurs", "Prospects", "Communications"
- **Validations** : messages en français (`[Required(ErrorMessage = "Ce champ est requis")]`)
- **Logs et debugging** : messages en français pour la cohérence

### Exemples de terminologie française standard :
- `CreatedAt` → `DateCreation` ou garder `CreatedAt` (convention technique)
- `UpdatedAt` → `DateMiseAJour` ou garder `UpdatedAt`  
- Error messages → "Ce champ est obligatoire", "Format d'email invalide"
- Success messages → "Client ajouté avec succès", "Modification enregistrée"
