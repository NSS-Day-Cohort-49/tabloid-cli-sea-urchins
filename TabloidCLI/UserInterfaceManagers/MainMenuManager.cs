using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TabloidCLI.UserInterfaceManagers
{
    public class MainMenuManager : IUserInterfaceManager
    {
        private const string CONNECTION_STRING = 
            @"Data Source=localhost\SQLEXPRESS;Database=TabloidCLI;Integrated Security=True";

        public IUserInterfaceManager Execute()
        {
            Console.WriteLine("Main Menu");

            Console.WriteLine(" 1) Journal Management");
            Console.WriteLine(" 2) Blog Management");
            Console.WriteLine(" 3) Author Management");
            Console.WriteLine(" 4) Post Management");
            Console.WriteLine(" 5) Tag Management");
            Console.WriteLine(" 6) Search by Tag");
            Console.WriteLine(" 7) Change Background Color");
            Console.WriteLine(" 0) Exit");

            Console.Write("> ");
            string choice = Console.ReadLine();
            switch (choice)
            {
                case "1": throw new NotImplementedException();
                case "2": return new BlogManager(this, CONNECTION_STRING);
                case "3": return new AuthorManager(this, CONNECTION_STRING);
                case "4": throw new NotImplementedException();
                case "5": return new TagManager(this, CONNECTION_STRING);
                case "6": return new SearchManager(this, CONNECTION_STRING);
                case "7":
                    
                    ConsoleColor[] consoleColors
            = (ConsoleColor[])ConsoleColor
                  .GetValues(typeof(ConsoleColor));

                    // Display the list
                    // of available console colors
                    Console.WriteLine("List of available "
                                      + "Console Colors:");
                    foreach (var color in consoleColors)
                       Console.WriteLine(color);
                    Console.Write("Background Color: ");
                    string backgroundColorString = Convert.ToString(Console.ReadLine()).Trim();

                    if (Enum.TryParse(backgroundColorString, ignoreCase: true, out ConsoleColor _backgroundColor))
                        Console.BackgroundColor = _backgroundColor;
                    

                    // output to the console using the requested colors (assuming they were valid)

                    Console.WriteLine("Hello World!");
                    Console.WriteLine("Press any key to return to the menu...");
                    Console.ReadKey();
                    Console.Clear();
                    return this;
                case "0":
                    Console.WriteLine("Good bye");
                    return null;
                default:
                    Console.WriteLine("Invalid Selection");
                    return this;
            }
        }
    }
}
