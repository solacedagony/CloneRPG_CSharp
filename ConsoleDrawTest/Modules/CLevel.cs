using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloneRPG
{
    class CLevel : CModule
    {
        CModuleManager moduleManager;

        public CLevel( CModuleManager moduleManagerArg)
        {
            moduleManager = moduleManagerArg;
        }

        public void draw()
        {
            // Clear screen
            Console.Clear();
            Console.ResetColor();

            Console.SetCursorPosition(0,0);
            Console.WriteLine("You gained a level!");

            List<string> descriptionList = new List<string>();
            List<string> beforeValues = new List<string>();
            List<string> afterValues = new List<string>();

            // Description column
            descriptionList.Add("Level: ");
            descriptionList.Add("HP: ");
            descriptionList.Add("MP: ");
            descriptionList.Add("Strength: ");
            descriptionList.Add("Dexterity: ");
            descriptionList.Add("Intelligence: ");
            descriptionList.Add("");
            descriptionList.Add("XP to Next Level: ");

            beforeValues.Add(((int)moduleManager.player.level).ToString());
            beforeValues.Add(((int)moduleManager.player.hpMax).ToString());
            beforeValues.Add(((int)moduleManager.player.mpMax).ToString());
            beforeValues.Add(((int)moduleManager.player.strength).ToString());
            beforeValues.Add(((int)moduleManager.player.dexterity).ToString());
            beforeValues.Add(((int)moduleManager.player.intelligence).ToString());
            beforeValues.Add("");
            
            // Gain level before checking XP to next level
            moduleManager.player.level += 1;
            moduleManager.player.hpMax += 20;
            moduleManager.player.mpMax += 20;
            moduleManager.player.strength += 5;
            moduleManager.player.dexterity += 5;
            moduleManager.player.intelligence += 5;

            beforeValues.Add(((int)moduleManager.player.xpUntilNextLevel()).ToString());

            afterValues.Add(((int)moduleManager.player.level).ToString());
            afterValues.Add(((int)moduleManager.player.hpMax).ToString());
            afterValues.Add(((int)moduleManager.player.mpMax).ToString());
            afterValues.Add(((int)moduleManager.player.strength).ToString());
            afterValues.Add(((int)moduleManager.player.dexterity).ToString());
            afterValues.Add(((int)moduleManager.player.intelligence).ToString());
            afterValues.Add("");
            afterValues.Add("");

            int x = 2;
            int boxStartY = 2;
            int y = boxStartY + 1;

            // Print descriptions
            for( int i = 0 ; i < descriptionList.Count() ;i++ )
            {
                Console.SetCursorPosition(x, y+i);
                Console.Write(descriptionList[i]);
            }

            int valuesOffsetX = 20;
            y = boxStartY+1;
            // Print values
            for( int i = 0 ; i <beforeValues.Count();i++)
            {
                // Check if anything should be written
                // Accounts for spacing
                if (beforeValues[i] != "")
                {
                    Console.SetCursorPosition(valuesOffsetX, y + i);
                    Console.Write(beforeValues[i].PadRight(5));
                    
                    // Check if parameter has changed
                    if( afterValues[i] != "")
                    {
                        Console.WriteLine( " -> " + afterValues[i].PadRight(5));
                    }
                }
            }
    
            // Print box around stats
            int boxEndY = boxStartY+descriptionList.Count()+1;
            Utility.Drawing.drawBox_DoubleLine(0, boxStartY, 35, boxEndY);
            y +=boxEndY+1;

            Utility.Interaction.pressAnyKeyToContinue(0, y);

            // Reset player's HP/MP
            moduleManager.player.hp = moduleManager.player.hpMax;
            moduleManager.player.mp = moduleManager.player.mpMax;

            // Account for multiple levels from single XP gain
            if (!moduleManager.player.checkXPForLevel())
            {
                moduleManager.switchModule(CModuleManager.ModuleType.Map);
            }
        }
    }
}
