using System.Reflection;
using DemoApplication.Model;
using SdmFramework.Annotations;
using SdmFramework.Registry;
using SdmFramework.Service.HttpHandler;
using SdmFramework.Service.ViewEngine.ViewObjects;
using SdmFramework.Utils;
using SdmFrameworkTest.Controller;

namespace SdmFrameworkTest;

public class Tests
{
    [TestFixture]
    public class RouterTests
    {
        private Router _router;
        private RouteRegistry _routeRegistry;
        

        [SetUp]
        public void Setup()
        {
            RoutePathProcessor routePathProcessor = new RoutePathProcessor();
            _routeRegistry = new RouteRegistry(routePathProcessor);
            Resolver resolver = new Resolver();
            _router = new Router(_routeRegistry,resolver);
        }
        
        [Test]
        public void ACorrectPathShouldResolveAndReturnAnActionInfo()
        {
            // Arrange
            string path = "/Testcontroller/about";
            string route = "/Testcontroller/about";
            string query = "";

            HttpRequest request = new HttpGet("/about");
            TestController testController = new TestController();
            MethodInfo method = testController.GetType().GetMethod("About");

            // Creating an expected View result
            Person expectedPerson = new Person("sander", "sanderdemiddelaer@hotmail.com");
            View expectedView = new View("C:\\School\\programmeren3\\GIT\\sander.demiddelaer\\SdmFramework\\DemoApplication\\View\\index.scl", expectedPerson); // Adjust the path and model as needed

            _routeRegistry.RegisterRoute(request, route, new ActionInfo(testController, method));

            // Act
            IActionResult actualResult = _router.ResolveRoute("GET", route, query) as IActionResult;

            // Assert
            Assert.That(actualResult, Is.TypeOf<View>());

            View actualView = (View)actualResult;
            Assert.That(actualView.Model, Is.TypeOf<Person>());
            Person actualPerson = (Person)actualView.Model;
            Assert.That(actualPerson.Name, Is.EqualTo(expectedPerson.Name));
            Assert.That(actualPerson.Email, Is.EqualTo(expectedPerson.Email));
        }
        
        [Test]
        public void ACorrectPathWithQueryParametersShouldResolveAndReturnAnActionInfo()
        {
            // Arrange
            string path = "/users/2";
            string route = "/users/{id}";
            string query = "";

            HttpRequest request = new HttpGet("/about?param1=value1&param2=value2");
            TestController testController = new TestController();
            MethodInfo method = testController.GetType().GetMethod("UserDetails", new[] { typeof(int) });


            // Creating an expected View result
            int expectedId = 2;
            View expectedView = new View("C:\\School\\programmeren3\\GIT\\sander.demiddelaer\\SdmFramework\\DemoApplication\\View\\pathVariable.scl",expectedId);

            _routeRegistry.RegisterRoute(request, route, new ActionInfo(testController, method));

            // Act
            IActionResult actualResult = _router.ResolveRoute("GET", path, query) as IActionResult;

            // Assert
            Assert.That(actualResult, Is.TypeOf<View>());
            View actualView = (View)actualResult;
            Assert.That(actualView.Model, Is.TypeOf<int>());
            int actualId = (int)actualView.Model;
            Assert.That(actualId, Is.EqualTo(expectedId));
        }
        
        [Test]
        public void RegisteringAmbiguousRouteShouldThrowException()
        {
            // Arrange
            HttpRequest request = new HttpGet("/users/2");
            TestController testController = new TestController();
            MethodInfo method1 = testController.GetType().GetMethod("UserDetails", new[] { typeof(int) });
            MethodInfo method2 = testController.GetType().GetMethod("UserDetails", new[] { typeof(double) });

            string route = "/users/{id}";

            // Register the first route
            _routeRegistry.RegisterRoute(request, route, new ActionInfo(testController, method1));

            // Act and Assert
            Assert.Throws<ArgumentException>(() =>
            {
                // Attempting to register a second route with the same key should throw an exception
                _routeRegistry.RegisterRoute(request, route, new ActionInfo(testController, method2));
            });
        }
        
    }
    
    
    
    [TestFixture]
    public class RoutePathHelperTests
    {
        
        [SetUp]
        public void Setup()
        {
            
        }

        [Test]
        public void ACleanPathShouldReturnAnEmptyListOfParameters()
        {
            string path = "/Testcontroller/about";
            string route = "/Testcontroller/about";
            string query = "";
            var parameters = RoutePathHelper.ExtractAllVariables(path, route, "");
            Assert.That(parameters, Is.Empty);
        }
        
        [Test]
        public void APathWithOnePathVariableShouldReturnADictionaryOfParameters()
        {
            string path = "/Testcontroller/users/2";
            string route = "/Testcontroller/users/{user}";
            string query = "";
            var parameters = RoutePathHelper.ExtractAllVariables(path, route, "");
            
            
            Assert.That(parameters, Has.Exactly(1).Property("Key").EqualTo("user")
                .And.Exactly(1).Property("Value").EqualTo("2"));
        }
        
        [Test]
        public void APathWithMultiplePathVariablesShouldReturnADictionaryOfParameters()
        {
            string path = "/Testcontroller/users/sander/posts/42";
            string route = "/Testcontroller/users/{user}/posts/{id}";
            string query = "";
            var parameters = RoutePathHelper.ExtractAllVariables(path, route, "");

            Assert.That(parameters, Has.Exactly(1).Property("Key").EqualTo("user")
                .And.Exactly(1).Property("Value").EqualTo("sander")
                .And.Exactly(1).Property("Key").EqualTo("id")
                .And.Exactly(1).Property("Value").EqualTo("42"));
        }
        
        [Test]
        public void APathWithASingleQueryParameterShouldReturnADictionaryOfParameters()
        {
            string path = "/Testcontroller/about";
            string route = "/Testcontroller/about";
            string query = "?page=2";
            var parameters = RoutePathHelper.ExtractAllVariables(path, route, query);

            Assert.That(parameters, Has.Exactly(1).Property("Key").EqualTo("page")
                .And.Exactly(1).Property("Value").EqualTo("2"));
        }
        
        [Test]
        public void APathWithdMultipleQueryParametersShouldReturnADictionaryOfParameters()
        {
            string path = "/Testcontroller/about";
            string route = "/Testcontroller/about";
            string query = "?page=2&category=programming";
            var parameters = RoutePathHelper.ExtractAllVariables(path, route, query);

            Assert.That(parameters, Has.Exactly(1).Property("Key").EqualTo("page")
                .And.Exactly(1).Property("Value").EqualTo("2")
                .And.Exactly(1).Property("Key").EqualTo("category")
                .And.Exactly(1).Property("Value").EqualTo("programming"));
        }
    }
    
    [Test]
    public void APathWithPathVariableAndQueryParametersShouldReturnADictionaryOfParameters()
    {
        string path = "/Testcontroller/users/sander/posts/42";
        string route = "/Testcontroller/users/{user}/posts/{id}";
        string query = "?category=programming";
        var parameters = RoutePathHelper.ExtractAllVariables(path, route, query);

        Assert.That(parameters, Has.Exactly(1).Property("Key").EqualTo("user")
            .And.Exactly(1).Property("Value").EqualTo("sander")
            .And.Exactly(1).Property("Key").EqualTo("id")
            .And.Exactly(1).Property("Value").EqualTo("42")
            .And.Exactly(1).Property("Key").EqualTo("category")
            .And.Exactly(1).Property("Value").EqualTo("programming"));
    }
}