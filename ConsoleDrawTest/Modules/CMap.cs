using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ExtensionMethods;
using Utility;

namespace CloneRPG
{
    class CMap : IModule
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

        int encounter = 0;

        List<CPlayer> mapNPCs;

        public CMap(CModuleManager moduleManagerArg)
        {
            this.moduleManager = moduleManagerArg;

            CPlayer newplayer = new CPlayer(moduleManager);
            CItem item = new CItem();
            item.name = "newitem";
            item.description = "A really cool item";
            newplayer.name = "NewGuy";

            newplayer.inventory.Add(item);
            newplayer.isNPC = true;
            newplayer.npcType = NPCClass.BOSS;
            newplayer.mapPosX = 5;
            newplayer.mapPosY = 1;

            newplayer.hp = 200;
            newplayer.hpMax = 200;
            newplayer.mp = 0;
            newplayer.mpMax = 0;
            newplayer.strength = 6;
            newplayer.dexterity = 3;
            newplayer.intelligence = 3;
            newplayer.xp = 20;
            newplayer.level = 1;

            // TODO: Add shops at 66/2 and 70/2

            mapNPCs = new List<CPlayer>();
            mapNPCs.Add( newplayer );

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

                        // Check for NPC at this position
                        for( int i = 0 ; i < mapNPCs.Count ; i++ )
                        {
                            if( mapNPCs[i].mapPosX == mapX &&
                                mapNPCs[i].mapPosY == mapY &&
                                mapNPCs[i].isNPCVisible() )
                            {
                                Console.BackgroundColor = ConsoleColor.Black;
                                Console.ForegroundColor = ConsoleColor.DarkYellow;

                                Console.SetCursorPosition( printX, printY );

                                if( mapNPCs[i].mapSymbol == "" )
                                {
                                    Console.Write( "U" );
                                }
                                else
                                {
                                    Console.Write( mapNPCs[i].mapSymbol );
                                }

                                Console.ResetColor();
                            }
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
                        if (!didCollisionOccur(moduleManager.player.mapPosX, moduleManager.player.mapPosY - 1))
                        {
                            moduleManager.player.mapPosY--;

                            encounterOccurred = didEncounterOccur();
                        }
                    }
                    break;

                case ConsoleKey.DownArrow:
                    {
                        if (!didCollisionOccur(moduleManager.player.mapPosX, moduleManager.player.mapPosY + 1))
                        {
                            moduleManager.player.mapPosY++;

                            encounterOccurred = didEncounterOccur();
                        }
                    }
                    break;

                case ConsoleKey.LeftArrow:
                    {
                        if (!didCollisionOccur(moduleManager.player.mapPosX - 1, moduleManager.player.mapPosY))
                        {
                            moduleManager.player.mapPosX--;

                            encounterOccurred = didEncounterOccur();
                        }
                    }
                    break;

                case ConsoleKey.RightArrow:
                    {
                        if (!didCollisionOccur(moduleManager.player.mapPosX + 1, moduleManager.player.mapPosY))
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
                        CInventoryMap inventoryMap = new CInventoryMap( moduleManager );
                        inventoryMap.draw();
                    }
                    break;

                case ConsoleKey.A:
                    {
                        // Find nearest character starting north
                        for( int i = 0 ; i < mapNPCs.Count ; i++)
                        {
                            // Check north, east, south, west
                            if( mapNPCs[i].mapPosX == moduleManager.player.mapPosX &&
                                mapNPCs[i].mapPosY == (moduleManager.player.mapPosY-1) &&
                                mapNPCs[i].isNPCVisible())
                            {
                                triggerNPCType(mapNPCs[i]);
                            }
                            else if( mapNPCs[i].mapPosX == (moduleManager.player.mapPosX+1) &&
                                     mapNPCs[i].mapPosY == moduleManager.player.mapPosY &&
                                     mapNPCs[i].isNPCVisible() )
                            {
                                triggerNPCType( mapNPCs[i] );
                            }
                            else if( mapNPCs[i].mapPosX == moduleManager.player.mapPosX &&
                                     mapNPCs[i].mapPosY == (moduleManager.player.mapPosY + 1) &&
                                     mapNPCs[i].isNPCVisible() )
                            {
                                triggerNPCType( mapNPCs[i] );
                            }
                            else if( mapNPCs[i].mapPosX == (moduleManager.player.mapPosX-1) &&
                                     mapNPCs[i].mapPosY == moduleManager.player.mapPosY &&
                                     mapNPCs[i].isNPCVisible() )
                            {
                                triggerNPCType( mapNPCs[i] );
                            }
                        }
                    }
                    break;
            }

            if (encounterOccurred)
            {
                // Switch to fight mode 
                List<CPlayer> enemyList = setupEnemy();
                CFight fight = new CFight( moduleManager, enemyList );
                fight.draw();
            }
        }

        private void triggerNPCType( CPlayer npc )
        {
            if( npc.npcType == NPCClass.BOSS)
            {
                List<CPlayer> enemyList = new List<CPlayer>();
                enemyList.Add( npc );

                CFight fight = new CFight( moduleManager, enemyList );
                fight.draw();
            }
            else if( npc.npcType == NPCClass.DIALOG)
            {
                // Add dialog scene here
            }
            else if( npc.npcType == NPCClass.SHOP )
            {
                CShop shop = new CShop( moduleManager, npc );
                shop.draw();
            }
        }

        private bool didCollisionOccur(int x, int y)
        {
            /////////////////////////////////
            // Check all map characters
            /////////////////////////////////
            string collisionCharacters = "#*!@$%^&()_+-=,./<>?1234567890abcdefghijklmnopqrstuvwyxzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            for (int i = 0; i < collisionCharacters.Length; i++)
            {
                // Ensure an out of bounds index doesn't occur
                if( x >= 0 &&
                    x < map[0].Length &&
                    y >= 0 &&
                    y < map.Count() )
                {
                    // Check map characters
                    if( map[y][x].ToString() == collisionCharacters.Substring( i, 1 ) )
                    {
                        return true;
                    }
                }
            }

            ////////////////////////////////
            // Check map NPC locations
            ////////////////////////////////
            for( int i = 0 ; i < mapNPCs.Count ; i++ )
            {
                if( mapNPCs[i].mapPosX == x &&
                    mapNPCs[i].mapPosY == y &&
                    mapNPCs[i].isNPCVisible() )
                {
                    return true;
                }
            }

            return false;
        }

        private bool didEncounterOccur()
        {
            return false;
            // This method allows encounters to occur more consistently
            const int lowValue = 5;
            const int highValue = 5;
            const int encounterTotal = 100;

            int randomNumber = RandomNumberGenerator.generateRandomNumber( lowValue, highValue );
            encounter += randomNumber;

            // Check for an encounter
            if( encounter > encounterTotal )
            {
                encounter = 0;
                return true;
            }
            else
            {
                return false;
            }
        }

        private List<CPlayer> setupEnemy()
        {
            List<CPlayer> enemyList = new List<CPlayer>();

            CPlayer tempEnemy = new CPlayer(moduleManager);
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

            CPlayer tempEnemy2 = new CPlayer( moduleManager );
            tempEnemy2.name = "BadGuy2";
            tempEnemy2.hp = 20;
            tempEnemy2.hpMax = 20;
            tempEnemy2.mp = 20;
            tempEnemy2.mpMax = 20;
            tempEnemy2.strength = 3;
            tempEnemy2.dexterity = 3;
            tempEnemy2.intelligence = 3;
            tempEnemy2.xp = 20;
            tempEnemy2.level = 1;

            enemyList.Add( tempEnemy );
            enemyList.Add( tempEnemy2 );

            return enemyList;
        }

        private void loadMap()
        {
            map = new List<string>();

            map.Add("##################################################################################################################################################");
            map.Add("#           ###############             ##################       /-\\/-\\            ################################                   #########");
            map.Add("##########   ##########        #####       ############          | |  | |           ################################                     #########");
            map.Add("############     ###       ############                                                      ######################                     ##########");
            map.Add("################      ######################                                                     ################              ###################");
            map.Add("######################################################                                  __-__      #############        ##########################");
            map.Add("#######################################################                              /~~     ~~\\    ##########         ###########################");
            map.Add("#########################################################                          /~~         ~~\\   ##########    ###############################");
            map.Add("##########################################################                        {               }    ######     ################################");
            map.Add("##########################################################                         \\  _-     -_  /   #######       ###############################");
            map.Add("###########################################################                          ~  \\\\ //  ~  ######      ####################################");
            map.Add("###########################################################                              | |                  ####################################");
            map.Add("###########################################################                              | |                 #####################################");
            map.Add("###########################################################                            //   \\\\    ################################################");
            map.Add("###########################################################                                        ###############################################");
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

        public void initialize()
        {

        }

        public void destroy()
        {

        }
    }
}
