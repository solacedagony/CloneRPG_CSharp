using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloneRPG
{
    class CMainMenu
    {
        CModuleManager moduleManager;

        public CMainMenu(CModuleManager moduleManagerArg)
        {
            moduleManager = moduleManagerArg;
        }

        public void draw()
        {
            // Clear console
            Console.Clear();

            // Write information
            Console.WriteLine(CModuleManager.gameName + " v" + CModuleManager.versionMajor + "." + CModuleManager.versionMinor);
            Console.WriteLine("");
            Console.WriteLine("1. New Game");
            Console.WriteLine("2. Load Game");
            Console.WriteLine("3. Exit");
            Console.WriteLine();
            Console.Write("Input: ");

            // Read key
            ConsoleKeyInfo keyInfo = Console.ReadKey();

            // Parse input
            if (keyInfo.Key.Equals(ConsoleKey.D1))
            {
                // Create new base player
                CPlayer tempPlayer = new CPlayer();
                tempPlayer.name = "";
                tempPlayer.playerClass = PlayerClass.WARRIOR;
                tempPlayer.hp = -1;
                tempPlayer.hpMax = -1;
                tempPlayer.mp = -1;
                tempPlayer.mpMax = -1;
                tempPlayer.strength = -1;
                tempPlayer.dexterity = -1;
                tempPlayer.intelligence = -1;

                moduleManager.player = tempPlayer;
                tempPlayer = null;

                moduleManager.switchModule(CModuleManager.ModuleType.NewGame);
            }
            else if (keyInfo.Key.Equals(ConsoleKey.D2))
            {
                moduleManager.switchModule(CModuleManager.ModuleType.LoadGame);
            }
            else if (keyInfo.Key.Equals(ConsoleKey.D3) ||
                    keyInfo.Key.Equals(ConsoleKey.Escape) )
            {
                moduleManager.switchModule(CModuleManager.ModuleType.Exit);
            }
        }
    }
}
