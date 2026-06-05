// See https://aka.ms/new-console-template for more information

using SdmFramework;

Console.WriteLine("Hello, World!");


var sdmFramework = new SdmFrameWorkApplication("http://localhost:8080/");
sdmFramework.Run(typeof(Program));

