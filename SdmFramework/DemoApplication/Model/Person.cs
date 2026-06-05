namespace DemoApplication.Model;

public class Person
{
    public string Name { get; set; }
    public string Email { get; set; }
    public int Age { get; set; }
    public string PhoneNumber { get; set; }
    public string JobTitle { get; set; }
    public string Department { get; set; }

    public Person(string name, string email, int age, string phoneNumber, string jobTitle, string department)
    {
        Name = name;
        Email = email;
        Age = age;
        PhoneNumber = phoneNumber;
        JobTitle = jobTitle;
        Department = department;
    }
}