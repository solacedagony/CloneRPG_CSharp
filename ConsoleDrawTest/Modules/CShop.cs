using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Defines;

namespace CloneRPG
{
    class CShop : IModule
    {
        CModuleManager moduleManager;
        int numberOfPages;
        int currentPage = 0;
        const int maxItemsPerPage = 15;

        const int numberX = 2;
        const int quantityX = 6;
        const int itemX = 12;
        const int descriptionX = 35;
        const int headerY = 2;

        const int inputX = 1;
        const int inputY = 21;

        const int helpX = 64;
        const int helpY = 5;
        
        const int column2 = 4;
        const int column3 = 10;
        const int column4 = 33;

        CPlayer npc;

        public CShop(CModuleManager moduleManagerArg)
        {
            moduleManager = moduleManagerArg;
        }

        public void draw()
        {
            Console.Clear();
            drawBox();
            drawHeader();
            drawHelp();
            drawItems();
            processInput();
        }

        void drawBox()
        {
            // Main 
            Utility.Drawing.drawBox_DoubleLine(0, 1, 60, 3);
            Utility.Drawing.drawBox_DoubleLine(0, 3, 60, 19);

            // Columns
            Utility.Drawing.drawBox_DoubleLine(column2, 1, column3, 19);
            Utility.Drawing.drawBox_DoubleLine(column3, 1, column4, 19);

            // 1st row
            Console.SetCursorPosition(4, 1);
            Console.Write(defines.DOUBLELINE_3WAYDOWN);

            Console.SetCursorPosition(10, 1);
            Console.Write(defines.DOUBLELINE_3WAYDOWN);

            Console.SetCursorPosition(33, 1);
            Console.Write(defines.DOUBLELINE_3WAYDOWN);

            // 2nd row
            Console.SetCursorPosition(0, 3);
            Console.Write(defines.DOUBLELINE_3WAYRIGHT);

            Console.SetCursorPosition(4, 3);
            Console.Write(defines.DOUBLELINE_4WAY);

            Console.SetCursorPosition(10, 3);
            Console.Write(defines.DOUBLELINE_4WAY);

            Console.SetCursorPosition(33, 3);
            Console.Write(defines.DOUBLELINE_4WAY);

            Console.SetCursorPosition(60, 3);
            Console.Write(defines.DOUBLELINE_3WAYLEFT);

            // 3rd row
            Console.SetCursorPosition(4, 19);
            Console.Write(defines.DOUBLELINE_3WAYUP);

            Console.SetCursorPosition(10, 19);
            Console.Write(defines.DOUBLELINE_3WAYUP);

            Console.SetCursorPosition(33, 19);
            Console.Write(defines.DOUBLELINE_3WAYUP);
        }

        void drawHeader()
        {
            Console.ResetColor();
            Console.SetCursorPosition(0, 0);
            Console.Write(moduleManager.player.name + "'s Inventory - Page " + currentPage + "/" + numberOfPages);

            Console.SetCursorPosition(numberX, headerY);
            Console.Write("#");

            Console.SetCursorPosition(quantityX, headerY);
            Console.Write("Cost");

            Console.SetCursorPosition(itemX, headerY);
            Console.Write("Item");

            Console.SetCursorPosition(descriptionX, headerY);
            Console.Write("Description");
        }

        void drawItems()
        {
            int y = headerY + 2;
            int offset = (currentPage - 1) * maxItemsPerPage;
            for (int i = 0; i < maxItemsPerPage; i++)
            {
                int index = i + offset;

                if (index >= moduleManager.player.inventory.Count())
                {
                    break;
                }

                int numberField = i + 1;
                string numberFieldStr = "";
                if (numberField <= 9)
                {
                    numberFieldStr = numberField.ToString();
                }
                else if (numberField == 10)
                {
                    numberFieldStr = "0";
                }
                else if (numberField >= 11 && numberField <= maxItemsPerPage)
                {
                    char tempChar = 'A';
                    tempChar += (char)(numberField - 11);
                    numberFieldStr = tempChar.ToString();
                }
                else
                {
                    numberFieldStr = "G";
                }

                Console.SetCursorPosition(numberX, y);
                Console.Write(numberFieldStr);

                Console.SetCursorPosition(quantityX, y);
                Console.Write(moduleManager.player.inventory[i].quantity.ToString());

                Console.SetCursorPosition(itemX, y);
                Console.Write(moduleManager.player.inventory[i].name);

                // Write dependent on item type
                Console.SetCursorPosition(descriptionX, y);
                //Console.Write(moduleManager.player.inventory[i].decription());

                y++;
            }
        }

        void drawHelp()
        {
            List<string> descriptions = new List<string>();

            descriptions.Add("Right Arrow - Next Page");
            descriptions.Add("Left Arrow - Previous Page");
            descriptions.Add("Q/Escape - Exit");
            descriptions.Add("Number/Letter - Use Item");

            int y = helpY;

            for (int i = 0; i < descriptions.Count(); i++)
            {
                Console.SetCursorPosition(helpX, y);
                Console.Write(descriptions[i]);
                y++;
            }
        }

        void setNPC( ref CPlayer npc)
        {

        }

        void processInput()
        {
            Console.SetCursorPosition(inputX, inputY);
            Console.Write("Input: ");

            ConsoleKeyInfo keyInfo = Console.ReadKey(false);

            // Check for number keys/letter keys
            if (keyInfo.Key >= ConsoleKey.D0 && keyInfo.Key <= ConsoleKey.D9)
            {

            }
            else if (keyInfo.Key >= ConsoleKey.A && keyInfo.Key <= ConsoleKey.A)
            {

            }
            else if (keyInfo.Key.Equals(ConsoleKey.RightArrow))
            {
                if (currentPage == numberOfPages)
                {
                    currentPage = 1;
                }
                else
                {
                    currentPage++;
                }
            }
            else if (keyInfo.Key.Equals(ConsoleKey.LeftArrow))
            {
                if (currentPage == 1)
                {
                    currentPage = numberOfPages;
                }
                else
                {
                    currentPage--;
                }

            }
            else if (keyInfo.Key.Equals(ConsoleKey.Escape) || keyInfo.Key.Equals(ConsoleKey.Q))
            {
                moduleManager.switchModule(CModuleManager.ModuleType.Map);
            }
        }

        public void initialize()
        {
            numberOfPages = (int)((double)moduleManager.player.inventory.Count() / (double)maxItemsPerPage) + 1;
            currentPage = 1;
            //calculateList();
        }

        public void destroy()
        {
        }
    }
}
