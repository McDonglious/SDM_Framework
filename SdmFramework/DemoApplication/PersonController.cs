using DemoApplication.Model;
using SdmFramework.Annotations;
using SdmFramework.Model;
using SdmFramework.Service.ViewEngine.ViewObjects;

namespace DemoApplication;
[Controller("/TestController")]
public class PersonController
{
    [HttpGet("/about")]
    public IActionResult About()
    {
        Person person = new Person("Sander", "sander@example.com", 22, "+32 123 45 67", "Software Engineer", "Development");
        return new View("index", person);
    }

    [HttpGet("/listing")]
    public IActionResult Listing()
    {
        List<Person> list = new();
        list.Add(new Person("Sander", "sander@example.com", 22, "+32 123 45 67", "Software Engineer", "Development"));
        list.Add(new Person("Tom", "tom@example.com", 24, "+32 234 56 78", "UX Designer", "Design"));
        list.Add(new Person("Michiel", "michiel@example.com", 27, "+32 345 67 89", "DevOps Engineer", "Infrastructure"));
        list.Add(new Person("Bart", "bart@example.com", 31, "+32 456 78 90", "Product Manager", "Management"));
        list.Add(new Person("Pieter", "pieter@example.com", 29, "+32 567 89 01", "Backend Developer", "Development"));
        return new View("listing", list);
    }

    [HttpGet("/blockTest")]
    public IActionResult BlockTest()
    {
        return new View("blockTest", null);
    }

    [HttpGet("/users/{id}")]
    public IActionResult UserDetails([PathVariable] int id)
    {
        return new View("pathVariable", id);
    }

    [HttpGet("/users")]
    public IActionResult FilterUsers([QueryParam] int id)
    {
        return new View("queryParam", id);
    }
}