# SDM Framework

A lightweight MVC web framework for C# built from scratch, featuring a custom view engine powered by ANTLR4. The framework handles HTTP routing, controller discovery via reflection, and view rendering using a Razor-inspired templating language (`.scl` files) with its own grammar, lexer, and parser.

**Author:** Sander de Middelaer

---

## Table of Contents

- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [Framework Initialization](#framework-initialization)
- [Controllers and Routing](#controllers-and-routing)
- [Views](#views)
- [Templating Language](#templating-language)
  - [Model Declaration](#model-declaration)
  - [Displaying Model Data](#displaying-model-data)
  - [HTML Elements](#html-elements)
  - [Razor Blocks and Global Variables](#razor-blocks-and-global-variables)
  - [If Statements](#if-statements)
  - [foreach Loops](#foreach-loops)
  - [Expressions](#expressions)
- [Path Variables and Query Parameters](#path-variables-and-query-parameters)
- [ANTLR4](#antlr4)
- [Testing](#testing)

---

## Project Structure

```
SdmFramework/
├── SdmFramework/          # Core framework library
│   ├── Annotations/       # Controller, HttpGet/Post/Put/Delete, PathVariable, QueryParam
│   ├── Model/             # Model wrapping
│   ├── Registry/          # Route registry and interfaces
│   ├── Service/
│   │   ├── AssemblyProcessor/   # Reflection-based controller and route discovery
│   │   ├── HttpHandler/         # HTTP handling, routing, and path resolution
│   │   └── ViewEngine/          # ANTLR4-based view compiler and renderer
│   └── Utils/
├── DemoApplication/       # Example application using the framework
│   ├── View/              # .scl template files
│   ├── Model/
│   └── PersonController.cs
└── SdmFrameworkTest/      # Unit tests
```

---

## Getting Started

**Prerequisites:** .NET 9.0 SDK

Clone the repository and build the solution:

```bash
git clone <repo-url>
cd SdmFramework
dotnet build SdmFramework.sln
```

No additional tooling is required. The ANTLR4-generated lexer and parser are already committed to the repository. The only runtime dependency is `Antlr4.Runtime.Standard`, which is restored automatically via NuGet.

To run the demo application:

```bash
cd DemoApplication
dotnet run
```

---

## Framework Initialization

```csharp
var sdmFramework = new SdmFrameWorkApplication("http://localhost:8080/");
sdmFramework.Run(typeof(Program));
```

Pass the base URL the framework should listen on, and the type of your application's entry point. The framework uses reflection to scan the assembly for controllers and register their routes automatically.

---

## Controllers and Routing

Controllers must be annotated with `[Controller]`, which takes a base path. Action methods are annotated with an HTTP method attribute (`[HttpGet]`, `[HttpPost]`, `[HttpPut]`, `[HttpDelete]`), each taking a sub-path. The two paths are combined to form the full route.

```csharp
using SdmFramework.Annotations;

[Controller("/home")]
public class HomeController
{
    [HttpGet("/about")]
    public IActionResult About()
    {
        Person person = new Person("sander", "sander@example.com");
        return new View("index", person);
    }

    [HttpGet("/listing")]
    public IActionResult Listing()
    {
        var list = new List<Person>
        {
            new Person("sander", "sander@example.com"),
            new Person("tom", "tom@example.com")
        };
        return new View("listing", list);
    }
}
```

URL format: `http://<base-url>/<controller-path>/<action-path>`

Example: `http://localhost:8080/home/about`

---

## Views

Action methods return a `View`, which takes the name of the template file and the model object:

```csharp
return new View("index", person);
```

The framework resolves the view by convention: it looks for a file named `<name>.scl` inside a `View/` folder relative to the application's output directory. To ensure `.scl` files are included in the build output, add the following to your `.csproj`:

```xml
<ItemGroup>
    <None Update="View\*.scl">
        <CopyToOutputDirectory>Always</CopyToOutputDirectory>
    </None>
</ItemGroup>
```

---

## Templating Language

Templates are written in `.scl` files using a Razor-inspired syntax.

### Model Declaration

```razor
@model MyModel
```

This is required at the top of each template. The identifier after `@model` is not used for type binding — it is a structural declaration.

### Displaying Model Data

For a model that is a class:

```razor
<h1>@Model.Title</h1>
<p>@Model.Email</p>
```

For a model that is a primitive:

```razor
<p>@Model.Value</p>
```

### HTML Elements

Standard HTML is supported:

```html
<div>
    <p>Hello World</p>
</div>
```

> Note: Plain text directly inside HTML elements (outside of tags) is not currently supported.

### Razor Blocks and Global Variables

Variables declared in a Razor block are stored as globals and accessible anywhere in the template:

```razor
@{
    var greeting = "Hello";
    var name = "John";
}

<p>@Global.greeting</p>
```

### If Statements

```razor
@if (condition) {
    <p>Condition is true</p>
}
```

### foreach Loops

Iterates over the model when the model itself is a collection. Each element is accessed via `@item`:

```razor
@foreach(Model)
{
    <h1>@item.Name</h1>
    <p>@item.Email</p>
}
```

### Expressions

Mathematical expressions are supported inside Razor blocks:

```razor
@{
    result = 2 + 3 * (4 - 1);
}

<p>@Global.result</p>
```

---

## Path Variables and Query Parameters

**Path variable:**

```csharp
[HttpGet("/users/{id}")]
public IActionResult UserDetails([PathVariable] int id)
{
    return new View("pathVariable", id);
}
```

**Query parameter:**

```csharp
[HttpGet("/users")]
public IActionResult FilterUsers([QueryParam] int id)
{
    return new View("queryParam", id);
}
```

---

## ANTLR4

The templating language is defined in `SdmFramework/Service/ViewEngine/Compiler/Grammar/BasicRazor.g4`. ANTLR4 is used to generate the lexer and parser, which are committed to the repository under `Compiler/Visitor/Generated/`. The runtime dependency (`Antlr4.Runtime.Standard`) is pulled in via NuGet — no ANTLR CLI installation is needed to build or run the project.

If you modify the grammar and need to regenerate the lexer and parser, use the following command (adjust the output path as needed):

```bash
antlr4 -visitor -Dlanguage=CSharp -o <absolute-path-to-Generated-folder> BasicRazor.g4
```

---

## Testing

The solution includes a unit test project (`SdmFrameworkTest`) which can be run with:

```bash
dotnet test
```

The demo application also provides a set of browser-testable endpoints. With `DemoApplication` running on `http://localhost:8080/`:

| Scenario | URL |
|---|---|
| View with object model | `http://localhost:8080/Testcontroller/about` |
| View with collection model | `http://localhost:8080/Testcontroller/listing` |
| View with primitive via path variable | `http://localhost:8080/Testcontroller/users/2` |
| View with primitive via query parameter | `http://localhost:8080/Testcontroller/users?id=5` |