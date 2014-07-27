using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloneRPG
{
    class CNewGame : IModule
    {
        CModuleManager moduleManager;
        NewGameState newGameState;

        public enum NewGameState
        {
            NAME,
            CLASS
        }

        public CNewGame(CModuleManager moduleManagerArg)
        {
            this.moduleManager = moduleManagerArg;
            this.newGameState = NewGameState.NAME;
        }

        public void draw()
        {
            // Clear console
            Console.Clear();

            // Write information
            Console.WriteLine(CModuleManager.gameName + " v" + CModuleManager.versionMajor + "." + CModuleManager.versionMinor);
            Console.WriteLine("New Character Creation");
            Console.WriteLine("");

            // Choose state
            if (newGameState.Equals(NewGameState.NAME))
            {
                Console.Write("Character name: ");

                // Read character name
                string characterName = Console.ReadLine();

                // Clean string of non alphanumeric characters
                string cleanString = new string(characterName.Where(Char.IsLetterOrDigit).ToArray());

                // Check that input is greater than 0 characters
                if (cleanString.Length > 0)
                {
                    // Capitalize
                    cleanString = cleanString[0].ToString().ToUpper() + cleanString.Substring(1);

                    moduleManager.player.name = cleanString;
                    newGameState = NewGameState.CLASS;
                }
            }
            else if (newGameState.Equals(NewGameState.CLASS))
            {
                // Write information
                Console.WriteLine("Choose your class, " + moduleManager.player.name + ":");
                Console.WriteLine("1. Warrior");
                Console.WriteLine("2. Thief");
                Console.WriteLine("3. Mage");
                Console.Write("Input: ");

                // Read key
                ConsoleKeyInfo keyInfo = Console.ReadKey(false);

                bool goodKeyPress = false;

                // Set default stats depending on class chosen
                switch (keyInfo.Key)
                {
                    case ConsoleKey.D1:
                        {
                            moduleManager.player.id = CIDManager.getId();
                            moduleManager.player.playerClass = PlayerClass.WARRIOR;
                            moduleManager.player.hp = 100;
                            moduleManager.player.hpMax = 100;
                            moduleManager.player.mp = 0;
                            moduleManager.player.mpMax = 0;
                            moduleManager.player.strength = 10;
                            moduleManager.player.dexterity = 5;
                            moduleManager.player.intelligence = 1;
                            moduleManager.player.xp = 0;
                            moduleManager.player.level = 1;
                            moduleManager.player.isNPC = false;

                            // Add sword and chest armor
                            for (int i = 0; i < 20; i++)
                            {
                                moduleManager.player.addItem(moduleManager.itemManager.getItemByName("Wooden Sword"));
                            }
                            moduleManager.player.addItem(moduleManager.itemManager.getItemByName("Bamboo Chest Piece"));

                            goodKeyPress = true;
                            moduleManager.switchModule(CModuleManager.ModuleType.Map);
                            break;
                        }
                    case ConsoleKey.D2:
                        {
                            moduleManager.player.id = CIDManager.getId();
                            moduleManager.player.playerClass = PlayerClass.THIEF;
                            moduleManager.player.hp = 70;
                            moduleManager.player.hpMax = 70;
                            moduleManager.player.mp = 20;
                            moduleManager.player.mpMax = 20;
                            moduleManager.player.strength = 6;
                            moduleManager.player.dexterity = 10;
                            moduleManager.player.intelligence = 2;
                            moduleManager.player.xp = 0;
                            moduleManager.player.level = 1;
                            moduleManager.player.isNPC = false;

                            // Add sword and chest armor
                            moduleManager.player.addItem(moduleManager.itemManager.getItemByName("Wooden Dagger"));
                            moduleManager.player.addItem(moduleManager.itemManager.getItemByName("Bamboo Chest Piece"));

                            goodKeyPress = true;
                            moduleManager.switchModule(CModuleManager.ModuleType.Map);
                            break;
                        }
                    case ConsoleKey.D3:
                        {
                            moduleManager.player.id = CIDManager.getId();
                            moduleManager.player.playerClass = PlayerClass.MAGE;
                            moduleManager.player.hp = 40;
                            moduleManager.player.hpMax = 40;
                            moduleManager.player.mp = 100;
                            moduleManager.player.mpMax = 100;
                            moduleManager.player.strength = 1;
                            moduleManager.player.dexterity = 5;
                            moduleManager.player.intelligence = 15;
                            moduleManager.player.xp = 0;
                            moduleManager.player.level = 1;
                            moduleManager.player.isNPC = false;

                            // Add sword and chest armor
                            moduleManager.player.addItem(moduleManager.itemManager.getItemByName("Wooden Staff"));
                            moduleManager.player.addItem(moduleManager.itemManager.getItemByName("Cloth Robe"));

                            goodKeyPress = true;
                            moduleManager.switchModule(CModuleManager.ModuleType.Map);
                            break;
                        }
                }

                // Reset
                if (goodKeyPress)
                {
                    newGameState = NewGameState.NAME;
                }
            }
        }

        public void initialize()
        {

        }

        public void destroy()
        {

        }
    }
}
