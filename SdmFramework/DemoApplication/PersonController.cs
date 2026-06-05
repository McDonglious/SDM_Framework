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
        return new View("index", person);
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