using DemoApplication.Model;
using SdmFramework.Annotations;
using SdmFramework.Model;
using SdmFramework.Service.ViewEngine.ViewObjects;

namespace SdmFrameworkTest.Controller;
[SdmFramework.Annotations.Controller("/TestController")]
public class TestController
{
    [HttpGet("/about")]
    public IActionResult About()
    {

        Person person = new Person("sander", "sanderdemiddelaer@hotmail.com");
        
        return new View("C:\\School\\programmeren3\\GIT\\sander.demiddelaer\\SdmFramework\\DemoApplication\\View\\index.scl", person);
    }
    
    [HttpGet("/listing")]
    public IActionResult Listing()
    {
        List<Person> list = new();
        list.Add(new Person("sander" , "dm@com"));
        list.Add(new Person("tom", "debroer@email.com"));
        list.Add(new Person("michiel", "debroer@email.com"));
        list.Add(new Person("bart", "debroer@email.com"));
        list.Add(new Person("pieter", "debroer@email.com"));    
        return new View("C:\\School\\programmeren3\\GIT\\sander.demiddelaer\\SdmFramework\\DemoApplication\\View\\listing.scl", list);
    }
    
    
    [HttpGet("/blockTest")]
    public IActionResult BlockTest()
    {
        return new View("C:\\School\\programmeren3\\GIT\\sander.demiddelaer\\SdmFramework\\DemoApplication\\View\\blockTest.scl", null);
    }
    
    [HttpGet("/users/{id}")]
    public IActionResult UserDetails([PathVariable]int id)
    {
        Int64 pathVariable = id;
        return new View("C:\\School\\programmeren3\\GIT\\sander.demiddelaer\\SdmFramework\\DemoApplication\\View\\pathVariable.scl", id);
    }
    
    [HttpGet("/users/{age}")]
    public IActionResult UserDetails([PathVariable]double age)
    {
        return null;
    }
    
    [HttpGet("/users")]
    public IActionResult FilterUsers([QueryParam]int id)
    {
        return new View("C:\\School\\programmeren3\\GIT\\sander.demiddelaer\\SdmFramework\\DemoApplication\\View\\queryParam.scl", id);
    }
}