﻿# Auth.Api

The project was generated using the [Clean.Architecture.Solution.Template](https://github.com/jasontaylordev/Auth.Api)
version 8.0.0-preview.7.2.

## Build

Run `dotnet build -tl` to build the solution.

## Run

To run the web application:

```bash
cd .\src\Web\
dotnet watch run
```

Navigate to https://localhost:5001. The application will automatically reload if you change any of the source files.

## Code Styles & Formatting

The template includes [EditorConfig](https://editorconfig.org/) support to help maintain consistent coding styles for
multiple developers working on the same project across various editors and IDEs. The **.editorconfig** file defines the
coding styles applicable to this solution.

## Code Scaffolding

The template includes support to scaffold new commands and queries.

Start in the `.\src\Application\` folder.

Create a new command:

```
dotnet new ca-usecase --name CreateTodoList --feature-name TodoLists --usecase-type command --return-type int
```

Create a new query:

```
dotnet new ca-usecase -n GetTodos -fn TodoLists -ut query -rt TodosVm
```

If you encounter the error *"No templates or subcommands found matching: 'ca-usecase'."*, install the template and try
again:

```bash
dotnet new install Clean.Architecture.Solution.Template::8.0.0-preview.7.2
```

## Test

The solution contains unit, integration, and functional tests.

To run the tests:

```bash
dotnet test
```

<!--#else -->
The solution contains unit, integration, functional, and acceptance tests.

To run the unit, integration, and functional tests (excluding acceptance tests):

```bash
dotnet test --filter "FullyQualifiedName!~AcceptanceTests"
dotnet test --filter "FullyQualifiedName!~UnitTests"
```

To run the acceptance tests, first start the application:

```bash
cd .\src\Web\
dotnet run
```

Then, in a new console, run the tests:

```bash
cd .\src\Web\
dotnet test
```

## Load testing

LoadTesting is made with the k6 project.

To execute a test:

```bash
k6 run script.js
```

To execute a test with debug ( requests and responses without the body ):

```bash
k6 run --http-debug script.js
```

To execute a test with full debug ( requests and responses with the body ):

```bash
k6 run --http-debug="full" script.js
```

## Docker Compose

```sh
docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d
```

## Database

When you run the application the database will be automatically created (if necessary) and the latest migrations will be
applied.

Running database migrations is easy. Ensure you add the following flags to your command (values assume you are executing
from repository root)

* `--project src/Infrastructure` (optional if in this folder)
* `--startup-project src/Web`
* `--output-dir Data/Migrations`

For example, to add a new migration from the root folder:

```sh
dotnet ef migrations add "TodoListRemoved" --project src/Infrastructure --startup-project src/Web --output-dir Data/Migrations
```

To create a database :

```sh
dotnet ef database update --verbose --project src/Infrastructure --startup-project src/Web
```

## Help

To learn more about the template go to the [project website](https://github.com/JasonTaylorDev/Auth.Api). Here you can
find additional guidance, request new features, report a bug, and discuss the template with other users.