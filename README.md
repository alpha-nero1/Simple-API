# Simple-API
A simple .Net API using the CQRS + Mediator pattern

# Usefull VSCode Extensions.
- C# - Microsoft.
- C# Extensions -jchannon.
- NuGet Gallery - pcislo.
- SQLite - alexcvzz.

# Infrastructure...
## Domain Layer
Everything starts with the domain.
- We first must create our domain entity classes.

## Application Layer
Has a dependency on the domain and sits above the Domain
- Responsbile for business logic.
- Do validation here.

## Api Layer
Has dependency on Application layer.
- Responsible for apis/controllers (interfacing).

## Persistence Layer
Has a dependency on domain so it has access to entities that scaffold our database.
Application has dependeny on persistence so that it can make db changes.

- Everything is centered around the domain.

# Usefull commands
`dotnet --info` - get installation info.
`dotnet -h` - get list of commands we can use.

# Start up the projects
`dotnet new -l` - get list of types of projects we can make.
`dotnet new sln` - create a new solution project (contains all our projects), invokes the current folder you are in to be an sln.
`dotnet new webapi -n API` - creates a web api project called API
`dotnet new classlib -n Application` - creates a new class library (standard project) with name.
`dotnet new classlib -n Domain`
`dotnet new classlib -n Persistence`
`dotnet sln add API Application Domain Persistence` - add the projects to the solution (looks for the .csproj files in those folders).
`dotnet sln list` - see what projects are in the sln.

## Add dependencies.
cd into the folders to add dependencies to other projects and execute the respective commands.
(API) `dotnet add reference ../Application`
(Application) `dotnet add reference ../Domain` and `dotnet add reference ../Persistence`
(Persistence) `dotnet add reference ../Domain`

## Run commands.
(API) `dotnet run` - will run the application.
(API) `dotnet watch run` - will run the application in watch mode.

## Code style with VSCode
To setup code style where initialise field from parameter gives a result like `_config = config` in constructors, search the settings for keywords `this` (to remove the this) and `private` to change the private field prefix.

# Migrations.
Must install `dotent-ef` dotnet entity framework to create migrations.

- `dotnet tool list --global` will show what global tools you have installed. if you do not have dotnet ef listed there you will need to install it: https://www.nuget.org/packages/dotnet-ef/6.0.0-preview.5.21301.9
- Install the package globally: `dotnet tool install --global dotnet-ef --version 6.0.0-preview.5.21301.9`
- `dotnet ef -h` list executeable commands.
To migrate you can execute this at the sln level.

(You will need to install Microsoft.EntityFrameworkCore.Design on Persistence proj from nuget for this to work)
- `dotnet ef migrations add InitialCreate -p Persistence -s API`
- `dotnet ef migrations add <MigrationName> -p <PersistenceProj>/ <APIProj>/`

This will create Persistence.Migrations.

- `dotnet ef database -h` to see what commands we can use for databases.

## Infrastructure project.
Has a dependency on the Application layer to which the API layer has a dependecy on it.
This allows us to have an interface in the Application layer and the actual implementation in the
infrastructure project, this keeps the Application layer clear of needing to know about http or the user
or any auth context but still access that logic from another project. In this case we want the user from auth
context `GetUsername()`
- The goal here is that if we change how auth works in the layer above, then because the Application layer was never
concerned with this, no code change is required in the App layer, only API and infrastructure.

## Many to many relationships.
dotnet has not by convention supported this. It is supported now but not recommended because the join tables
created by ef do not accomodate extra fields on the tables.

To migrate down in dotnet-ef: `dotnet ef migrations remove -p Persistence -s API`

- For things that are cross-cutting concerns we can use an Infrastructure project.

## Loading Related Entities.

### Eager loading
The related model is included with the model when queried using the Include() syntax.

### Lazy loading
When a related model is included with the model the first time when the property is accessed.
- You turn it on/off at a very high level. Bit of a deal breaker.

### Explicit loading.
We can call the Load() method on the model to load the related entities. We explicitly use lazy laoding.

### Projection
Allow us to get our related models directly.
- Allows us to create more efficient queries.