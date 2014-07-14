using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CloneRPG
{
    class CLoadGame
    {
        CModuleManager moduleManager;

        public CLoadGame(CModuleManager moduleManagerArg)
        {
            moduleManager = moduleManagerArg;
        }

        public void draw()
        {
            // TODO: Load files from directory
            // Clear console
            Console.Clear();

            // Write information
            Console.WriteLine(CModuleManager.gameName + " v" + CModuleManager.versionMajor + "." + CModuleManager.versionMinor);
            Console.WriteLine("Load Character");
            Console.WriteLine("");

            // TODO: Fix hardcoded Players directory
            string[] fileEntries = Directory.GetFiles("Players");
            List<string> players = new List<string>();

            if( fileEntries.Count() <= 0)
            {
                Console.SetCursorPosition(0, 2);
                Console.WriteLine("No characters are currently stored.");
                Utility.Interaction.pressAnyKeyToContinue(0, 4);

                moduleManager.switchModule(CModuleManager.ModuleType.MainMenu);

                return;
            }

            foreach (string file in fileEntries)
            {
                if (file.Substring(file.Length - 4, 4) == ".txt")
                {
                    // Remove directory
                    string playerName = file.Substring(file.IndexOf('\\')+1,file.Length-file.IndexOf('\\')-1);
                    
                    // Remove .txt
                    playerName = playerName.Substring(0,playerName.IndexOf(".txt"));

                    // Add name to list
                    players.Add(playerName);
                }
            }

            int y = 4;
            for (int i = 0; i < players.Count(); i++)
            {
                Console.SetCursorPosition(0, y);
                Console.WriteLine((i + 1) + ": " + players[i]);
                y += 1;
            }
            Console.SetCursorPosition(0, y);
            Console.Write("Input: ");
            ConsoleKeyInfo keyInfo = Console.ReadKey(false);

            // Offset key down
            int playerIndex = keyInfo.Key - ConsoleKey.D1;
            
            if( Utility.FileIO.loadGame(ref moduleManager.player,players[playerIndex]))
            {
                moduleManager.Log("Successfully loaded character: " + players[playerIndex]);
                moduleManager.switchModule(CModuleManager.ModuleType.Map);
            }
            else
            {
                moduleManager.Log("Successfully loaded character: " + players[playerIndex]);
                moduleManager.switchModule(CModuleManager.ModuleType.MainMenu);
            }
        }
    }
}
