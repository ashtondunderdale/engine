using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace engine;

internal class Help
{
    public static void CommandHelp()
    {
        Console.Clear();
        Helpers.OutputGreen("Help\n\n");
        Console.WriteLine("Commands:\n");

        Helpers.OutputYellow("\t\'CR OBJ\'\n");
        Helpers.OutputYellow("\tExample: ");
        Helpers.OutputGreen("cr obj");
        Helpers.OutputYellow(" player ");
        Helpers.OutputRed("myPlayer ");
        Console.Write("0 0 ");

        Helpers.OutputYellow("\n\n\t\'DL OBJ\'\n");
        Helpers.OutputYellow("\tExample: ");
        Helpers.OutputGreen("dl obj");
        Helpers.OutputRed(" myPlayer ");

        Helpers.OutputYellow("\n\n\t\'LD OBJ\'\n");
        Helpers.OutputYellow("\tExample: ");
        Helpers.OutputGreen("cr obj\n");

        Helpers.OutputYellow("\n\n\t\'CR LVL\'\n");
        Helpers.OutputYellow("\tExample: ");
        Helpers.OutputGreen("cr lvl ");
        Helpers.OutputRed(" myLevel ");

        Helpers.OutputYellow("\n\n\t\'DL LVL\'\n");
        Helpers.OutputYellow("\tExample: ");
        Helpers.OutputGreen("dl lvl ");
        Helpers.OutputRed(" myLevel ");

        Helpers.OutputYellow("\n\n\t\'LD LVL\'\n");
        Helpers.OutputYellow("\tExample: ");
        Helpers.OutputGreen("ld lvl ");

        Helpers.OutputYellow("\n\n\t\'SL LVL\'\n");
        Helpers.OutputYellow("\tExample: ");
        Helpers.OutputGreen("sl lvl ");
        Helpers.OutputRed(" 1 ");
    }

    public static void ErrorHelp()
    {
        Console.Clear();

        Helpers.OutputGreen("Help\n\n");
        Console.WriteLine("Error Messages:\n");

        Helpers.OutputRed("\t\'Player object has already been added to space\'\n");
        Helpers.OutputYellow("\tYou can only have 1 player object per level, it already exists in your Active Level.");

        Helpers.OutputRed("\n\n\t\'Not a valid object type\'\n");
        Helpers.OutputYellow("\tRefer to Help Guide for a complete list of vali objects you can add to your space.");

        Helpers.OutputRed("\n\n\t\'Select an active level to run this game space\'\n");
        Helpers.OutputYellow("\tRun the ");
        Helpers.OutputGreen("\'sl lvl\' ");
        Helpers.OutputYellow("command to select an active level. Refer to above command list for the correct format.");
    }
}