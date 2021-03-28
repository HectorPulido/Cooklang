using System;
using CookLang;

class MainClass
{
    public static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Write -h to get help about the program");
            return;
        }

		if (args.Length == 1)
		{
			Console.WriteLine("You must add a path");
			return;
		}

		string code = System.IO.File.ReadAllText(args[1]);
		var compiler = new CookCompiler(code);
		compiler.Run();

    }
}

