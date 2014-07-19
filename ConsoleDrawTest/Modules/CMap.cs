using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ExtensionMethods;
using Utility;

namespace CloneRPG
{
    class CMap : CModule
    {
        CModuleManager moduleManager;

        // Map data
        List<string> map;

        // Constant positions for printing map
        const int mapWidth = 61;
        const int mapHeight = 21;

        const int heroPosX = 31;
        const int heroPosY = 11;

        const int inputLinePosY = 25;

        const int statsPosX = 70;
        const int statsPosY = 1;

        public CMap(CModuleManager moduleManagerArg)
        {
            this.moduleManager = moduleManagerArg;

            loadMap();
        }

        public void draw()
        {
            // Clear Screen
            Console.Clear();

            // Draw static map elements
            drawMap();
            drawStats();
            drawStaticText();
            drawHero();

            processInput();
        }

        private void drawMap()
        {
            const int printStartX = 1;
            const int printStartY = 1;

            // Calculate map to print coordinate transform
            int offsetX = -(moduleManager.player.mapPosX - heroPosX) - printStartX;
            int offsetY = -(moduleManager.player.mapPosY - heroPosY) - printStartY;

            // Reset color before printing
            Console.ResetColor();

            // Print map
            for (int printY = printStartY; printY < (printStartY + mapHeight); printY++)
            {
                for (int printX = printStartX; printX < (printStartX + mapWidth); printX++)
                {
                    // Calculate map position due to printing offset
                    int mapX = printX - offsetX - 1;
                    int mapY = printY - offsetY - 1;

                    // Protect against out of bounds index
                    if (mapY >= 0 &&
                        mapY < map.Count &&
                        mapX >= 0 &&
                        mapX < map[0].Length)
                    {
                        // Check if character is blank
                        if (map[mapY][mapX].ToString() != " ")
                        {
                            Console.SetCursorPosition(printX, printY);
                            Console.Write(map[mapY][mapX]);
                        }
                    }
                }
            }
        }

        private void drawStats()
        {
            int y = 0;
            Console.SetCursorPosition(statsPosX, statsPosY);
            Console.Write(moduleManager.player.name);
            y += 1;

            Console.SetCursorPosition(statsPosX, statsPosY + y);
            Console.Write("HP: " + (int)moduleManager.player.hp + "/" + (int)moduleManager.player.hpMax);
            y += 1;

            Console.SetCursorPosition(statsPosX, statsPosY + y);
            Console.Write("MP: " + (int)moduleManager.player.mp + "/" + (int)moduleManager.player.mpMax);
            y += 1;

            Console.SetCursorPosition(statsPosX, statsPosY + y);
            Console.Write("Position: " + (int)moduleManager.player.mapPosX + "/" + (int)moduleManager.player.mapPosY);
            y += 1;

            Console.SetCursorPosition(statsPosX, statsPosY + y);
            Console.Write("Strength: " + (int)moduleManager.player.strength);
            y += 1;

            Console.SetCursorPosition(statsPosX, statsPosY + y);
            Console.Write("Dexterity: " + (int)moduleManager.player.dexterity);
            y += 1;

            Console.SetCursorPosition(statsPosX, statsPosY + y);
            Console.Write("Intelligence: " + (int)moduleManager.player.intelligence);
            y += 2;

            Console.SetCursorPosition(statsPosX, statsPosY + y);
            Console.Write("Level: " + (int)moduleManager.player.level);
            y += 1;

            Console.SetCursorPosition(statsPosX, statsPosY + y);
            Console.Write("XP to Level: " + (int)moduleManager.player.xpUntilNextLevel());
            y += 1;

            /////////////////////////////////
            // Draw keypress menu
            /////////////////////////////////
            string keypresses = "CIS";

            List<string> keyDescriptions = new List<string>();
            keyDescriptions.Add("[C]haracter");
            keyDescriptions.Add("[I]nventory");
            keyDescriptions.Add("[S]ave");

            // Add extra blank line
            y += 1;

            for (int i = 0; i < keypresses.Length; i++)
            {
                Console.SetCursorPosition(statsPosX, statsPosY + y);
                Console.Write(keypresses.Substring(i, 1) + " - " + keyDescriptions[i]);
                y += 1;
            }
        }

        private void drawStaticText()
        {
            Console.ForegroundColor = ConsoleColor.White;
            Console.BackgroundColor = ConsoleColor.Black;

            /////////////////////////////////
            // Draw border around map
            /////////////////////////////////
            Utility.Drawing.drawBox_DoubleLine(0, 0, mapWidth + 1, mapHeight + 1);
        }

        private void drawHero()
        {
            Console.SetCursorPosition(CMap.heroPosX, CMap.heroPosY);
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("H");
        }

        private void processInput()
        {
            Console.ResetColor();

            Console.SetCursorPosition(0, inputLinePosY);
            Console.Write("Input: ");

            ConsoleKeyInfo keyInfo = Console.ReadKey(false);

            // In the case of a move event with no collision, check for an encounter
            bool encounterOccurred = false;

            switch (keyInfo.Key)
            {
                case ConsoleKey.UpArrow:
                    {
                        if (didCollisionOccur(moduleManager.player.mapPosX, moduleManager.player.mapPosY - 1))
                        {
                            moduleManager.player.mapPosY--;

                            encounterOccurred = didEncounterOccur();
                        }
                    }
                    break;

                case ConsoleKey.DownArrow:
                    {
                        if (didCollisionOccur(moduleManager.player.mapPosX, moduleManager.player.mapPosY + 1))
                        {
                            moduleManager.player.mapPosY++;

                            encounterOccurred = didEncounterOccur();
                        }
                    }
                    break;

                case ConsoleKey.LeftArrow:
                    {
                        if (didCollisionOccur(moduleManager.player.mapPosX - 1, moduleManager.player.mapPosY))
                        {
                            moduleManager.player.mapPosX--;

                            encounterOccurred = didEncounterOccur();
                        }
                    }
                    break;

                case ConsoleKey.RightArrow:
                    {
                        if (didCollisionOccur(moduleManager.player.mapPosX + 1, moduleManager.player.mapPosY))
                        {
                            moduleManager.player.mapPosX++;

                            encounterOccurred = didEncounterOccur();
                        }
                    }
                    break;

                case ConsoleKey.Escape:
                    {
                        moduleManager.switchModule(CModuleManager.ModuleType.Exit);
                    }
                    break;

                case ConsoleKey.S:
                    {
                        if( Utility.FileIO.saveGame( moduleManager.player) )
                        {
                            // Game saved successfully
                            moduleManager.Log("Game saved successfully: " + moduleManager.player.name + ".txt");
                        }
                        else
                        {
                            // Game could not be saved
                            moduleManager.Log("Game could not be saved: " + moduleManager.player.name + ".txt");
                        }
                    }
                    break;

                case ConsoleKey.I:
                    {

                    }
                    break;
            }

            if (encounterOccurred)
            {
                // Switch to fight mode 
                setupEnemy();
                moduleManager.switchModule(CModuleManager.ModuleType.Fight);
            }
        }

        private bool didCollisionOccur(int x, int y)
        {
            string collisionCharacters = "#*!@$%^&()_+-=,./<>?1234567890abcdefghijklmnopqrstuvwyxzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            for (int i = 0; i < collisionCharacters.Length; i++)
            {
                if (x >= 0 &&
                    x < map[0].Length &&
                    y >= 0 &&
                    y < map.Count())
                    if (map[y][x].ToString() == collisionCharacters.Substring(i, 1))
                    {
                        return false;
                    }
            }

            return true;
        }

        private bool didEncounterOccur()
        {
            int randomNumber = RandomNumberGenerator.generateRandomNumber(1, 100);

            moduleManager.Log("Random number in CMap: " + randomNumber);

            if (randomNumber > 80)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void setupEnemy()
        {
            CPlayer tempEnemy = new CPlayer();
            tempEnemy.name = "BadGuy";
            tempEnemy.hp = 20;
            tempEnemy.hpMax = 20;
            tempEnemy.mp = 20;
            tempEnemy.mpMax = 20;
            tempEnemy.strength = 3;
            tempEnemy.dexterity = 3;
            tempEnemy.intelligence = 3;
            tempEnemy.xp = 20;
            tempEnemy.level = 1;

            moduleManager.fight.setEnemy(tempEnemy);

            tempEnemy = null;
        }

        private void loadMap()
        {
            map = new List<string>();

            map.Add("##################################################################################################################################################");
            map.Add("#         ########################################################################################################################################");
            map.Add("##################################################################################################################################################");
            map.Add("#######################################################################################################################        ###################");
            map.Add("##################################################################################################################################################");
            map.Add("##################################################################################################################################################");
            map.Add("##################################################################################################################################################");
            map.Add("##################################################################################################################################################");
            map.Add("##################################################################################################################################################");
            map.Add("##################################################################################################################################################");
            map.Add("##################################################################################################################################################");
            map.Add("##################################################################################################################################################");
            map.Add("##################################################################################################################################################");
            map.Add("##################################################################################################################################################");
            map.Add("##################################################################################################################################################");
            map.Add("##################################################################################################################################################");
            map.Add("##################################################################################################################################################");
            map.Add("##################################################################################################################################################");
            map.Add("##################################################################################################################################################");
            map.Add("##################################################################################################################################################");
            map.Add("##################################################################################################################################################");
            map.Add("##################################################################################################################################################");
            map.Add("##################################################################################################################################################");
            map.Add("##################################################################################################################################################");
            map.Add("##################################################################################################################################################");
            map.Add("##################################################################################################################################################");
            map.Add("##################################################################################################################################################");
            map.Add("##################################################################################################################################################");
            map.Add("##################################################################################################################################################");
            map.Add("##################################################################################################################################################");
            map.Add("##################################################################################################################################################");
            map.Add("##################################################################################################################################################");
            map.Add("#    ########################################################################################################################       ##############");
            map.Add("##################################################################################################################################################");
        }
    }
}
