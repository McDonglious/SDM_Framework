# SDM Framework

## author
Sander de Middelaer - IAO03


# Custom MVC Templating Language

Welcome to the documentation for our custom MVC templating language! This language is designed to provide a simple and expressive syntax for building dynamic views in our MVC framework.

## Table of Contents

- [Model Declaration](#model-declaration)
- [Displaying Model Data](#displaying-model-data)
- [HTML Elements](#html-elements)
- [Razor Blocks](#razor-blocks)
- [Control Flow Structures](#control-flow-structures)
    - [If Statements](#if-statement)
    - [foreach Loops](#foreach-statement)
- [Expressions](#expressions)
- [Antler4](#antlr4-integration)
- [Framework Usage](#using-the-sdmframework-in-csharp)
- [testing](#testing-the-sdmframework)

## Model Declaration

```razor
@model MyModel
```
Currently "MyModel" is still required but it can be anything as it is not used to bind the model
## Displaying Model Data

```razor
<h1>@Model.Title</h1>
```
This assumes that the model is class with a Title as property
```razor
<h1>@Model.Value</h1>
```
Value is used when the model is a primitive object.
## HTML Elements
```html
<div>
    <p>Hello World</p>
</div>
```
Currently special text is not supported inside html elements as plain text.
## Razor Blocks
```razor
@{
    var greeting = "Hello, ";
    var name = "John";
}
```

# Control Flow Structures
## If Statement
```razor
@if (condition) {
    // Code to execute when the condition is true
}
```

## foreach Statement
```razor
@foreach(Model)
    {
        <h1>@item.Name</h1>
        <p>@item.Email</p>
    }  
```
this assumes the model itself is a iterable object.

## Expressions
```razor
@{
    result = 2 + 3 * (4 - 1);
}
```

## Global Variables
```razor
@{
    result = 2 + 3 * (4 - 1);
}
```
Result is stored as a global variable and will be accesible like so

```razor
<p>@Global.result</p>
```

# ANTLR4 Integration

To provide support for our custom MVC templating language, we use the ANTLR4 tool to generate a lexer and parser for our language. This allows us to parse and interpret the templates efficiently.

## ANTLR4 Commands

To generate the lexer and parser from our grammar file (`BasicRazor.g4`), we use the following ANTLR4 commands:

we are using Absolute Paths

```bash
antlr4 -visitor -Dlanguage=CSharp -o C:\School\programmeren3\GIT\sander.demiddelaer\SdmFramework\SdmFramework\Service\ViewEngine\Compiler\Visitor\Generated BasicRazor.g4
```

# Using the SdmFramework in CSharp

## Framework initialization
```csharp
var sdmFramework = new SdmFrameWorkApplication("http://localhost:8080/");
sdmFramework.Run(typeof(Program));
```
simply add the library make an instance of the framework with the base url you wish it to listen to and then Run it with the Type of your application.

## Controller and Method Annotations


To use the SdmFramework effectively, controllers must be annotated with the `Controller` attribute. The `Controller` attribute requires a path that will be used as the base path for all actions within the controller.
Methods too must be annotated correctly with a child of the attribute HttpRequest wich in this example is HttpGet with also a path , both will be used to create a route key.
### Example:

```csharp
using SdmFramework.Annotations;

[Controller("/home")]
public class HomeController
{
    // Controller actions will be relative to "/home"
    
    [HttpGet("/about")]
    public IActionResult About()
    {

        Person person = new Person("sander", "sanderdemiddelaer@hotmail.com");
        
        return new View("C:\\School\\programmeren3\\GIT\\sander.demiddelaer\\SdmFramework\\DemoApplication\\View\\index.scl", person);
    }
}
```
## return type
the return type should be IActionResult. currently only implimentation supported is the View wich takes the Absolute Path of the .scl file, wich holds the custom razor code, and a model  
# URL usage

1. **URL Example:**
  - URL: `http://base-url/controller/action`
  - Example: `http://localhost:8080/myapp/home/index`

base-url = the url you've provided the framework when initializing it.


# Testing the SdmFramework

## Unit Tests

We have provided a set of unit tests that are designed to work out of the box. You can run these tests to ensure the correctness and functionality of the SdmFramework.

## Browser Testing

If you wish to test the framework from the browser, you can use the following sample URLs:
URL: `http://your-server/base-url/controller/action`
1. **Display a View with Model Data:**
  - URL: `http://localhost:8080/Testcontroller/about`

2. **Display a View with a collection as Model Data:**
- URL: `http://localhost:8080/Testcontroller/listing`


3. **Display a View with a primitive Model Data using path variables:**
- URL: `http://localhost:8080/Testcontroller/users/2`

4. **Display a View with a primitive Model Data using query parameters:**
- URL: `http://localhost:8080/Testcontroller/users?id=5`

These URLs provide a quick way to test different aspects of the SdmFramework from the browser. this assumes that we have our Test application named "DemoApplication"

### Additional info
When pulling the sdmframework with it's demo application the absolute path on the view that should point to the .scl file will need to be reset to the correct path for the provided URL's to work.