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
        Person person = new Person("sander", "sanderdemiddelaer@hotmail.com");
        return new View(Path.Combine("View", "index.scl"), person);
    }

    [HttpGet("/listing")]
    public IActionResult Listing()
    {
        List<Person> list = new();
        list.Add(new Person("sander", "dm@com"));
        list.Add(new Person("tom", "debroer@email.com"));
        list.Add(new Person("michiel", "debroer@email.com"));
        list.Add(new Person("bart", "debroer@email.com"));
        list.Add(new Person("pieter", "debroer@email.com"));
        return new View(Path.Combine("View", "listing.scl"), list);
    }

    [HttpGet("/blockTest")]
    public IActionResult BlockTest()
    {
        return new View(Path.Combine("View", "blockTest.scl"), null);
    }

    [HttpGet("/users/{id}")]
    public IActionResult UserDetails([PathVariable] int id)
    {
        return new View(Path.Combine("View", "pathVariable.scl"), id);
    }

    [HttpGet("/users")]
    public IActionResult FilterUsers([QueryParam] int id)
    {
        return new View(Path.Combine("View", "queryParam.scl"), id);
    }
}