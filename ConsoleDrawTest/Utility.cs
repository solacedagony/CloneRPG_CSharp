using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Defines;

namespace Utility
{
    static class StringColorizer
    {
        public class ColorTag
        {
            public ConsoleColor color = ConsoleColor.Black;
            public string tag = "";
            public int index = 0;
        }

        public static void colorizeString( string data )
        {
            List<ColorTag> colorTags = new List<ColorTag>();

            colorTags = parseColorTags( ref data );
            printString( data, colorTags );
        }

        private static List<ColorTag> parseColorTags( ref string data )
        {
            char startTag = '{';
            char endTag = '}';
            int pos = 0;
            int offset = 0;
            List<ColorTag> tagList = new List<ColorTag>();

            while( (pos = data.IndexOf( startTag, offset )) >= 0 )
            {
                var tempTag = new ColorTag();
                tempTag.index = pos;
                tagList.Add( tempTag );

                offset = pos + 1;
            }

            // Validate data
            for( int i = 0 ; i < tagList.Count() ; i++ )
            {
                int endPos = data.IndexOf( endTag, tagList[i].index );
                if( endPos > 0 )
                {
                    tagList[i].tag = data.Substring( tagList[i].index, endPos - tagList[i].index + 1 );

                    // Remove tag from data
                    data = data.Remove( tagList[i].index, tagList[i].tag.Length );
                    for( int j = i + 1 ; j < tagList.Count() ; j++ )
                    {
                        tagList[j].index -= tagList[i].tag.Length;
                    }
                }
                else
                {
                    return null;
                }
            }

            return tagList;
        }

        private static void printString( string data, List<ColorTag> colorTags )
        {
            for( int i = 0 ; i < colorTags.Count() ; i++ )
            {
                if( i == 0 )
                {
                    Console.Write( data.Substring( 0, colorTags[i].index ) );
                }
                else if( i > 0 )
                {
                    Console.Write( data.Substring( colorTags[i - 1].index, colorTags[i].index - colorTags[i - 1].index ) );
                }
                ConsoleColor color = returnColor( colorTags[i].tag );
                if( colorTags[i].tag.Contains("FG" ))
                {
                    Console.ForegroundColor = color;
                }
                else
                {
                   Console.BackgroundColor = color;
                }
            }
            Console.Write( data.Substring( colorTags[colorTags.Count() - 1].index, data.Length - colorTags[colorTags.Count() - 1].index ) );
        }

        private static ConsoleColor returnColor( string tag )
        {
            if( tag == Colors.FGblack )
            {
                return ConsoleColor.Black;
            }
            else if( tag == Colors.FGdarkblue )
            {
                return ConsoleColor.DarkBlue;
            }
            else if( tag == Colors.FGdarkgreen )
            {
                return ConsoleColor.DarkGreen;
            }
            else if( tag == Colors.FGdarkcyan )
            {
                return ConsoleColor.DarkCyan;
            }
            else if( tag == Colors.FGdarkred )
            {
                return ConsoleColor.DarkRed;
            }
            else if( tag == Colors.FGdarkmagenta )
            {
                return ConsoleColor.DarkMagenta;
            }
            else if( tag == Colors.FGdarkyellow )
            {
                return ConsoleColor.DarkYellow;
            }
            else if( tag == Colors.FGgray )
            {
                return ConsoleColor.Gray;
            }
            else if( tag == Colors.FGdarkgray )
            {
                return ConsoleColor.DarkGray;
            }
            else if( tag == Colors.FGblue )
            {
                return ConsoleColor.Blue;
            }
            else if( tag == Colors.FGgreen )
            {
                return ConsoleColor.Green;
            }
            else if( tag == Colors.FGcyan )
            {
                return ConsoleColor.Cyan;
            }
            else if( tag == Colors.FGred )
            {
                return ConsoleColor.Red;
            }
            else if( tag == Colors.FGmagenta )
            {
                return ConsoleColor.Magenta;
            }
            else if( tag == Colors.FGyellow )
            {
                return ConsoleColor.Yellow;
            }
            else if( tag == Colors.FGwhite )
            {
                return ConsoleColor.White;
            }
            if( tag == Colors.BGblack )
            {
                return ConsoleColor.Black;
            }
            else if( tag == Colors.BGdarkblue )
            {
                return ConsoleColor.DarkBlue;
            }
            else if( tag == Colors.BGdarkgreen )
            {
                return ConsoleColor.DarkGreen;
            }
            else if( tag == Colors.BGdarkcyan )
            {
                return ConsoleColor.DarkCyan;
            }
            else if( tag == Colors.BGdarkred )
            {
                return ConsoleColor.DarkRed;
            }
            else if( tag == Colors.BGdarkmagenta )
            {
                return ConsoleColor.DarkMagenta;
            }
            else if( tag == Colors.BGdarkyellow )
            {
                return ConsoleColor.DarkYellow;
            }
            else if( tag == Colors.BGgray )
            {
                return ConsoleColor.Gray;
            }
            else if( tag == Colors.BGdarkgray )
            {
                return ConsoleColor.DarkGray;
            }
            else if( tag == Colors.BGblue )
            {
                return ConsoleColor.Blue;
            }
            else if( tag == Colors.BGgreen )
            {
                return ConsoleColor.Green;
            }
            else if( tag == Colors.BGcyan )
            {
                return ConsoleColor.Cyan;
            }
            else if( tag == Colors.BGred )
            {
                return ConsoleColor.Red;
            }
            else if( tag == Colors.BGmagenta )
            {
                return ConsoleColor.Magenta;
            }
            else if( tag == Colors.BGyellow )
            {
                return ConsoleColor.Yellow;
            }
            else if( tag == Colors.BGwhite )
            {
                return ConsoleColor.White;
            }
            else
            {
                return 0;
            }
        }

        private static void applyColor( string tag )
        {
            if( tag == Colors.FGblack )
            {
                Console.ForegroundColor = ConsoleColor.Black;
            }
            else if( tag == Colors.FGdarkblue)
            {
                Console.ForegroundColor = ConsoleColor.DarkBlue;
            }
            else if( tag == Colors.FGdarkgreen)
            {
                Console.ForegroundColor = ConsoleColor.DarkGreen;
            }
            else if( tag == Colors.FGdarkcyan)
            {
                Console.ForegroundColor = ConsoleColor.DarkCyan;
            }
            else if( tag == Colors.FGdarkred)
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
            }
            else if( tag == Colors.FGdarkmagenta)
            {
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
            }
            else if( tag == Colors.FGdarkyellow)
            {
                Console.ForegroundColor = ConsoleColor.DarkYellow;
            }
            else if( tag == Colors.FGgray)
            {
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            else if( tag == Colors.FGdarkgray)
            {
                Console.ForegroundColor = ConsoleColor.DarkGray;
            }
            else if( tag == Colors.FGblue)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
            }
            else if( tag == Colors.FGgreen)
            {
                Console.ForegroundColor = ConsoleColor.Green;
            }
            else if( tag == Colors.FGcyan)
            {
                Console.ForegroundColor = ConsoleColor.Cyan;
            }
            else if( tag == Colors.FGred)
            {
                Console.ForegroundColor = ConsoleColor.Red;
            }
            else if( tag == Colors.FGmagenta)
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
            }
            else if( tag == Colors.FGyellow)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
            }
            else if( tag == Colors.FGwhite)
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
            if( tag == Colors.BGblack )
            {
                Console.BackgroundColor = ConsoleColor.Black;
            }
            else if( tag == Colors.BGdarkblue )
            {
                Console.BackgroundColor = ConsoleColor.DarkBlue;
            }
            else if( tag == Colors.BGdarkgreen )
            {
                Console.BackgroundColor = ConsoleColor.DarkGreen;
            }
            else if( tag == Colors.BGdarkcyan )
            {
                Console.BackgroundColor = ConsoleColor.DarkCyan;
            }
            else if( tag == Colors.BGdarkred )
            {
                Console.BackgroundColor = ConsoleColor.DarkRed;
            }
            else if( tag == Colors.BGdarkmagenta )
            {
                Console.BackgroundColor = ConsoleColor.DarkMagenta;
            }
            else if( tag == Colors.BGdarkyellow )
            {
                Console.BackgroundColor = ConsoleColor.DarkYellow;
            }
            else if( tag == Colors.BGgray )
            {
                Console.BackgroundColor = ConsoleColor.Gray;
            }
            else if( tag == Colors.BGdarkgray )
            {
                Console.BackgroundColor = ConsoleColor.DarkGray;
            }
            else if( tag == Colors.BGblue )
            {
                Console.BackgroundColor = ConsoleColor.Blue;
            }
            else if( tag == Colors.BGgreen )
            {
                Console.BackgroundColor = ConsoleColor.Green;
            }
            else if( tag == Colors.BGcyan )
            {
                Console.BackgroundColor = ConsoleColor.Cyan;
            }
            else if( tag == Colors.BGred )
            {
                Console.BackgroundColor = ConsoleColor.Red;
            }
            else if( tag == Colors.BGmagenta )
            {
                Console.BackgroundColor = ConsoleColor.Magenta;
            }
            else if( tag == Colors.BGyellow )
            {
                Console.BackgroundColor = ConsoleColor.Yellow;
            }
            else if( tag == Colors.BGwhite )
            {
                Console.BackgroundColor = ConsoleColor.White;
            }
            else
            {
                Console.ResetColor();
            }
        }
    }

    static class RandomNumberGenerator
    {
        static Random randomNumber = new Random(DateTime.Now.Millisecond + DateTime.Now.Second + DateTime.Now.Minute + DateTime.Now.Hour);

        static public int generateRandomNumber(int lowValue, int highValue)
        {
            return randomNumber.Next(lowValue, highValue + 1);
        }
    }

    static class Interaction
    {
        static public void pressAnyKeyToContinue(int posX, int posY)
        {
            Console.SetCursorPosition(posX, posY);
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(false);
        }

        static public void throwError(string error)
        {
            Console.Clear();
            Console.ResetColor();
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("Error! " + error);

            Utility.Interaction.pressAnyKeyToContinue(0, 3);
        }
    }

    static class Drawing
    {
        static public void drawBox_DoubleLine(int left, int top, int right, int bottom)
        {
            // Draw top horizontal section
            Console.SetCursorPosition(left + 1, top);
            for (int x = left + 1; x < right; x++)
            {
                Console.Write(UnicodeChars.HORIZONTALDOUBLELINE);
            }

            // Draw bottom horizontal section
            Console.SetCursorPosition(left + 1, bottom);
            for (int x = (left + 1); x < right; x++)
            {
                Console.Write(UnicodeChars.HORIZONTALDOUBLELINE);
            }

            // Draw left side vertical section
            for (int y = top + 1; y < bottom; y++)
            {
                Console.SetCursorPosition(left, y);
                Console.Write(UnicodeChars.VERTICALDOUBLELINE);
            }

            // Draw right side vertical section
            for (int y = top + 1; y < bottom; y++)
            {
                Console.SetCursorPosition(right, y);
                Console.Write(UnicodeChars.VERTICALDOUBLELINE);
            }

            // Draw top left corner piece
            Console.SetCursorPosition(left, top);
            Console.Write(UnicodeChars.TOPLEFTCORNER);

            // Draw top right corner piece
            Console.SetCursorPosition(right, top);
            Console.Write(UnicodeChars.TOPRIGHTCORNER);

            // Draw bottom left corner piece
            Console.SetCursorPosition(left, bottom);
            Console.Write(UnicodeChars.BOTTOMLEFTCORNER);

            // Draw bottom right corner piece
            Console.SetCursorPosition(right, bottom);
            Console.Write(UnicodeChars.BOTTOMRIGHTCORNER);
        }
    }

    static class FileIO
    {
        // TODO: Fix dependent playerDirectory and pull only from CModuleManager
        const string playerDirectory = "PLAYERS";

        const string IDName = "NAME:";
        const string IDLevel = "LEVEL:";
        const string IDXP = "XP:";
        const string IDHP = "HP:";
        const string IDHPMax = "HPMAX:";
        const string IDMP = "MP:";
        const string IDMPMax = "MPMAX:";
        const string IDStrength = "STRENGTH:";
        const string IDDexterity = "DEXTERITY:";
        const string IDIntelligence = "INTELLIGENCE:";
        const string IDMapX = "MAPX:";
        const string IDMapY = "MAPY:";

        static public bool saveGame(CloneRPG.CPlayer playerData)
        {
            string dataFilename = playerDirectory + "/" + playerData.name + ".txt";

            // Make backup copy of file
            if (File.Exists(dataFilename))
            {
                if (File.Exists(dataFilename + ".bak"))
                {
                    File.Delete(dataFilename + ".bak");
                }

                File.Move(dataFilename, dataFilename + ".bak");
            }

            using (StreamWriter file = new StreamWriter(dataFilename, false))
            {
                string output = "";
                output += IDName + playerData.name + Environment.NewLine;
                output += IDLevel + playerData.level + Environment.NewLine;
                output += IDXP + playerData.xp + Environment.NewLine;
                output += IDHP + playerData.hp + Environment.NewLine;
                output += IDHPMax + playerData.hpMax + Environment.NewLine;
                output += IDMP + playerData.mp + Environment.NewLine;
                output += IDMPMax + playerData.mpMax + Environment.NewLine;
                output += IDStrength + playerData.strength + Environment.NewLine;
                output += IDDexterity + playerData.dexterity + Environment.NewLine;
                output += IDIntelligence + playerData.intelligence + Environment.NewLine;
                output += IDMapX + playerData.mapPosX + Environment.NewLine;
                output += IDMapY + playerData.mapPosY + Environment.NewLine;

                file.WriteLine(output);
            }

            return true;
        }

        static public bool loadGame(ref CloneRPG.CModuleManager moduleManager, string playerName)
        {
            // Delimiters for file loading
            char[] delimiters = { ':' };

            // Load new character with default values
            CloneRPG.CPlayer tempPlayer = new CloneRPG.CPlayer(moduleManager);
            tempPlayer.name = "";
            tempPlayer.level = -1;
            tempPlayer.xp = -1;
            tempPlayer.hp = -1;
            tempPlayer.hpMax = -1;
            tempPlayer.mp = -1;
            tempPlayer.mpMax = -1;
            tempPlayer.strength = -1;
            tempPlayer.dexterity = -1;
            tempPlayer.intelligence = -1;
            tempPlayer.mapPosX = -1;
            tempPlayer.mapPosY = -1;

            // Open file based on player's name
            string dataFilename = playerName;

            using (StreamReader sr = new StreamReader(playerDirectory + "\\" + dataFilename + ".txt"))
            {
                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();

                    if (line.Contains(IDName))
                    {
                        string[] name = line.Split(delimiters);
                        if (name.Length == 2)
                        {
                            if (name[1] == playerName)
                            {
                                tempPlayer.name = name[1];
                            }
                            else
                            {
                                Console.Clear();
                                Console.WriteLine("Player filename does not match name contained in file!");
                                Utility.Interaction.pressAnyKeyToContinue(0, 2);

                                return false;
                            }
                        }
                    }
                    else if (line.Contains(IDLevel))
                    {
                        int fileLevel = Convert.ToInt32(line.Split(delimiters)[1]);
                        tempPlayer.level = fileLevel;
                    }
                    else if (line.Contains(IDXP))
                    {
                        double fileXP = Convert.ToDouble(line.Split(delimiters)[1]);
                        tempPlayer.xp = fileXP;
                    }
                    else if (line.Contains(IDHP))
                    {
                        double fileHP = Convert.ToDouble(line.Split(delimiters)[1]);
                        tempPlayer.hp = fileHP;
                    }
                    else if (line.Contains(IDHPMax))
                    {
                        double fileHPMax = Convert.ToDouble(line.Split(delimiters)[1]);
                        tempPlayer.hpMax = fileHPMax;
                    }
                    else if (line.Contains(IDMP))
                    {
                        double fileMP = Convert.ToDouble(line.Split(delimiters)[1]);
                        tempPlayer.mp = fileMP;
                    }
                    else if (line.Contains(IDMPMax))
                    {
                        double fileMPMax = Convert.ToDouble(line.Split(delimiters)[1]);
                        tempPlayer.mpMax = fileMPMax;
                    }
                    else if (line.Contains(IDStrength))
                    {
                        double fileStrength = Convert.ToDouble(line.Split(delimiters)[1]);
                        tempPlayer.strength = fileStrength;
                    }
                    else if (line.Contains(IDDexterity))
                    {
                        double fileDexterity = Convert.ToDouble(line.Split(delimiters)[1]);
                        tempPlayer.dexterity = fileDexterity;
                    }
                    else if (line.Contains(IDIntelligence))
                    {
                        double fileIntelligence = Convert.ToDouble(line.Split(delimiters)[1]);
                        tempPlayer.intelligence = fileIntelligence;
                    }
                    else if (line.Contains(IDMapX))
                    {
                        int fileMapX = Convert.ToInt32(line.Split(delimiters)[1]);
                        tempPlayer.mapPosX = fileMapX;
                    }
                    else if (line.Contains(IDMapY))
                    {
                        int fileMapY = Convert.ToInt32(line.Split(delimiters)[1]);
                        tempPlayer.mapPosY = fileMapY;
                    }
                }
            }

            // Validate character to make sure all fields were filled
            if (tempPlayer.name != "" &&
                tempPlayer.level > -1 &&
                tempPlayer.xp > -1 &&
                tempPlayer.hp > -1 &&
                tempPlayer.hpMax > -1 &&
                tempPlayer.mp > -1 &&
                tempPlayer.mpMax > -1 &&
                tempPlayer.strength > -1 &&
                tempPlayer.dexterity > -1 &&
                tempPlayer.intelligence > -1 &&
                tempPlayer.mapPosX > -1 &&
                tempPlayer.mapPosY > -1)
            {
                moduleManager.player = tempPlayer;
                tempPlayer = null;

                Console.Clear();
                Console.WriteLine("Character loaded successfully: " + moduleManager.player.name);
                Utility.Interaction.pressAnyKeyToContinue(0, 2);

                return true;
            }
            else
            {
                tempPlayer = null;
                Console.Clear();
                Console.WriteLine("Character loaded successfully: " + moduleManager.player.name);
                Utility.Interaction.pressAnyKeyToContinue(0, 2);

                return false;
            }
        }

        // All Items
        const string ITEM_START = "[START]";
        const string ITEM_NAME = "NAME:";
        const string ITEM_COST = "COST:";
        const string ITEM_TYPE = "TYPE:";
        const string ITEM_END = "[END]";

        // Quest Items
        const string ITEM_QUESTID = "QUESTID:";

        // Weapons/Armor
        const string ITEM_EQUIPABLEPOSITION = "EQUIPABLEPOSITION:";
        const string POS_WEAPON = "WEAPON";
        const string POS_HEAD = "HEAD";
        const string POS_CHEST = "CHEST";
        const string POS_ARMS = "ARMS";
        const string POS_LEGS = "LEGS";
        const string POS_FINGER = "FINGER";
        const string POS_NECK = "NECK";

        const string ITEM_ARMOR = "ARMOR:";
        const string ITEM_STRENGTHMODIFIER = "STRENGTHMODIFIER:";
        const string ITEM_DEXTERITYMODIFIER = "DEXTERITYMODIFIER:";
        const string ITEM_INTELLIGENCEMODIFIER = "INTELLIGENCEMODIFIER:";
        const string ITEM_ABSORB = "ABSORB:";
        const string ITEM_DAMAGEMIN = "DAMAGEMIN:";
        const string ITEM_DAMAGEMAX = "DAMAGEMAX:";
        const string ITEM_DAMAGETYPE = "DAMAGETYPE:";
        const string DAMAGE_BLUNT = "BLUNT";
        const string DAMAGE_PIERCE = "PIERCE";
        const string DAMAGE_MAGIC = "MAGIC";
        const string DAMAGE_EXPLOSIVE = "EXPLOSIVE";

        // Consumables
        const string ITEM_HPMODIFIER = "HPMODIFIER:";
        const string ITEM_MPMODIFIER = "MPMODIFIER:";
        const string ITEM_DAMAGEMODIFIER = "DAMAGEMODIFIER:";
        const string ITEM_ARMORMODIFIER = "ARMORMODIFIER:";

        static public bool loadItems(ref List<CloneRPG.CItem> itemList, string itemDirectory)
        {
            string comment = "//";
            char[] delimiters = { ':' };
            List<string> itemFiles = new List<string>();
            List<CloneRPG.CItem> tempItemList = new List<CloneRPG.CItem>();

            itemFiles = getAllFilesOfTypeInDirectory(itemDirectory, ".item");

            foreach (string file in itemFiles)
            {
                using (StreamReader sr = new StreamReader(file))
                {
                    bool foundStart = false;
                    bool foundType = false;
                    CloneRPG.CItem tempItem = new CloneRPG.CItem();

                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();

                        string[] parameterList;
                        string parameter = "";

                        if (line.Contains(comment))
                        {
                            continue;
                        }

                        if (foundStart)
                        {
                            parameterList = line.Split(delimiters);

                            if (parameterList.Length > 1)
                            {
                                parameter = parameterList[1];
                            }
                        }

                        if (!foundStart)
                        {
                            if (line.Contains(ITEM_START))
                            {
                                foundStart = true;
                            }
                        }
                        else if (foundStart &&
                                !foundType)
                        {
                            if (line.Contains(ITEM_NAME))
                            {
                                tempItem.name = parameter;
                            }
                            else if (line.Contains(ITEM_COST))
                            {
                                tempItem.cost = Convert.ToInt32(parameter);
                            }
                            else if (line.Contains(ITEM_TYPE))
                            {
                                if (parameter.Equals("QUEST"))
                                {
                                    tempItem.itemType = CloneRPG.CItem.ItemType.QUEST;
                                }
                                else if (parameter.Equals("CONSUMABLE"))
                                {
                                    tempItem.itemType = CloneRPG.CItem.ItemType.CONSUMABLE;
                                }
                                else if (parameter.Equals("ARMOR"))
                                {
                                    tempItem.itemType = CloneRPG.CItem.ItemType.ARMOR;
                                }
                                else if (parameter.Equals("WEAPON"))
                                {
                                    tempItem.itemType = CloneRPG.CItem.ItemType.WEAPON;
                                }
                                else
                                {
                                    Utility.Interaction.throwError("Item type not valid: " + line);
                                    return false;
                                }

                                foundType = true;
                            }
                            else if (line.Contains(ITEM_END))
                            {
                                Utility.Interaction.throwError("Item type not found in file: " + file);
                                return false;
                            }
                        }
                        else if (foundStart &&
                                 foundType)
                        {
                            if (line.Contains(ITEM_END))
                            {
                                foundStart = false;
                                foundType = false;
                                itemList.Add(tempItem);
                                tempItem = new CloneRPG.CItem();
                            }
                            else if (tempItem.itemType.Equals(CloneRPG.CItem.ItemType.QUEST))
                            {
                                if (line.Contains(ITEM_QUESTID))
                                {
                                    tempItem.questID = Convert.ToInt32(parameter);
                                }
                            }
                            else if (tempItem.itemType.Equals(CloneRPG.CItem.ItemType.CONSUMABLE))
                            {
                                if (line.Contains(ITEM_HPMODIFIER))
                                {
                                    tempItem.hpModifier = Convert.ToDouble(parameter);
                                }
                                else if (line.Contains(ITEM_MPMODIFIER))
                                {
                                    tempItem.mpModifier = Convert.ToDouble(parameter);
                                }
                                else if (line.Contains(ITEM_DAMAGEMODIFIER))
                                {
                                    tempItem.damageModifier = Convert.ToDouble(parameter);
                                }
                                else if (line.Contains(ITEM_ARMORMODIFIER))
                                {
                                    tempItem.armorModifier = Convert.ToDouble(parameter);
                                }
                                else
                                {
                                    Utility.Interaction.throwError("Consumable parameter unknown: " + line);
                                    return false;
                                }
                            }
                            else if (tempItem.itemType.Equals(CloneRPG.CItem.ItemType.ARMOR) ||
                                     tempItem.itemType.Equals(CloneRPG.CItem.ItemType.WEAPON))
                            {
                                // Armor specific
                                if (tempItem.itemType.Equals(CloneRPG.CItem.ItemType.ARMOR))
                                {
                                    if (line.Contains(ITEM_EQUIPABLEPOSITION))
                                    {
                                        if (parameter.Equals(POS_ARMS))
                                        {
                                            tempItem.equipablePosition = CloneRPG.CItem.EquipablePosition.ARMS;
                                        }
                                        else if (parameter.Equals(POS_CHEST))
                                        {
                                            tempItem.equipablePosition = CloneRPG.CItem.EquipablePosition.CHEST;
                                        }
                                        else if (parameter.Equals(POS_FINGER))
                                        {
                                            tempItem.equipablePosition = CloneRPG.CItem.EquipablePosition.FINGER;
                                        }
                                        else if (parameter.Equals(POS_HEAD))
                                        {
                                            tempItem.equipablePosition = CloneRPG.CItem.EquipablePosition.HEAD;
                                        }
                                        else if (parameter.Equals(POS_LEGS))
                                        {
                                            tempItem.equipablePosition = CloneRPG.CItem.EquipablePosition.LEGS;
                                        }
                                        else if (parameter.Equals(POS_NECK))
                                        {
                                            tempItem.equipablePosition = CloneRPG.CItem.EquipablePosition.NECK;
                                        }
                                        else
                                        {
                                            Utility.Interaction.throwError("Armor parameter unknown: " + line);
                                            return false;
                                        }
                                    }
                                }
                                // Weapon specific
                                else if (tempItem.itemType.Equals(CloneRPG.CItem.ItemType.WEAPON))
                                {
                                    if (line.Contains(ITEM_DAMAGEMIN))
                                    {
                                        tempItem.damageMin = Convert.ToDouble(parameter);
                                    }
                                    else if (line.Contains(ITEM_DAMAGEMAX))
                                    {
                                        tempItem.damageMax = Convert.ToDouble(parameter);
                                    }
                                    else if (line.Contains(ITEM_DAMAGETYPE))
                                    {
                                        if (parameter.Equals(DAMAGE_BLUNT))
                                        {
                                            tempItem.damageType = CloneRPG.CItem.DamageType.BLUNT;
                                        }
                                        else if (parameter.Equals(DAMAGE_PIERCE))
                                        {
                                            tempItem.damageType = CloneRPG.CItem.DamageType.PIERCE;
                                        }
                                        else if (parameter.Equals(DAMAGE_MAGIC))
                                        {
                                            tempItem.damageType = CloneRPG.CItem.DamageType.MAGIC;
                                        }
                                        else if (parameter.Equals(DAMAGE_EXPLOSIVE))
                                        {
                                            tempItem.damageType = CloneRPG.CItem.DamageType.EXPLOSIVE;
                                        }
                                        else
                                        {
                                            Utility.Interaction.throwError("Damage type parameter unknown: " + line);
                                            return false;
                                        }
                                    }
                                    else
                                    {
                                        Utility.Interaction.throwError("Weapon/armor parameter unknown: " + line);
                                        return false;
                                    }
                                }
                                // Applies to both
                                else if (line.Contains(ITEM_ARMOR))
                                {
                                    tempItem.armor = Convert.ToDouble(parameter);
                                }
                                else if (line.Contains(ITEM_STRENGTHMODIFIER))
                                {
                                    tempItem.strengthModifier = Convert.ToDouble(parameter);
                                }
                                else if (line.Contains(ITEM_DEXTERITYMODIFIER))
                                {
                                    tempItem.dexterityModifier = Convert.ToDouble(parameter);
                                }
                                else if (line.Contains(ITEM_INTELLIGENCEMODIFIER))
                                {
                                    tempItem.strengthModifier = Convert.ToDouble(parameter);
                                }
                                else if (line.Contains(ITEM_DAMAGEMODIFIER))
                                {
                                    tempItem.damageModifier = Convert.ToDouble(parameter);
                                }
                                else if (line.Contains(ITEM_ABSORB))
                                {
                                    tempItem.absorb = Convert.ToDouble(parameter);
                                }
                            }
                            else
                            {
                                Utility.Interaction.throwError("Item type parameter unknown: " + line);
                                return false;
                            }
                        }
                    }
                }
            }

            return true;
        }

        static public bool loadNPCs(ref List<CloneRPG.CPlayer> npcList, string npcDirectory)
        {


            return true;
        }

        static private List<string> getAllFilesOfTypeInDirectory(string directory, string extension)
        {
            List<string> fileList = new List<string>();

            // Get all files from item directory
            string[] fileEntries = Directory.GetFiles(directory);

            // Loop through all files and check for .item extension
            foreach (string file in fileEntries)
            {
                // TODO: put .item extension into variable
                if (file.Contains(extension))
                {
                    fileList.Add(file);
                }
            }

            return fileList;
        }
    }
}

namespace DebugHelperNS
{
    static class DebugHelper
    {
        static public string getMethodName()
        {
            return System.Reflection.MethodBase.GetCurrentMethod().ToString();
        }
    }
}
