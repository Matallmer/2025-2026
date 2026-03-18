using System;
using Bruch;

string usage = "Usage: dotnet run -- \"<ganz zaehler/nenner>\" ...\nExample: \"1 1/2\" or \"0 3/2\"";

if (args.Length == 0 || args.Length == 1 && (args[0] == "-h" || args[0] == "--help"))
{
	Console.WriteLine(usage);
	return;
}

foreach (var arg in args)
{
	try
	{
		var b = new Bruch.Bruch(arg);
		Console.WriteLine($"{arg} -> {b.ToString()}");
	}
	catch (Exception ex)
	{
		Console.Error.WriteLine($"Error parsing '{arg}': {ex.Message}");
		Console.WriteLine(usage);
	}
}
