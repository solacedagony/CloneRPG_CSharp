using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloneRPG
{
    class CInventoryMap : IModule
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

        public CInventoryMap(CModuleManager moduleManagerArg)
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
            Utility.Drawing.drawBox_DoubleLine(0, 1, 60, 3);
            Utility.Drawing.drawBox_DoubleLine(0, 3, 60, 19);
        }

        void drawHeader()
        {
            Console.ResetColor();
            Console.SetCursorPosition(0, 0);
            Console.Write(moduleManager.player.name + "'s Inventory - Page " + currentPage + "/" + numberOfPages );

            Console.SetCursorPosition(numberX, headerY);
            Console.Write("#");

            Console.SetCursorPosition(quantityX, headerY);
            Console.Write("Qty");

            Console.SetCursorPosition(itemX, headerY);
            Console.Write("Item");

            Console.SetCursorPosition(descriptionX, headerY);
            Console.Write("Description");
        }

        void drawItems()
        {
            int y = headerY + 2;
            int offset = (currentPage-1) * maxItemsPerPage;
            for (int i = 0; i < maxItemsPerPage; i++)
            {
                int index = i + offset;

                if( index >= moduleManager.player.inventory.Count() )
                {
                    break;
                }

                int numberField = i + 1;
                string numberFieldStr = "";
                if( numberField <= 9)
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
            else if( keyInfo.Key >= ConsoleKey.A && keyInfo.Key <= ConsoleKey.A )
            {

            }
            else if( keyInfo.Key.Equals(ConsoleKey.RightArrow))
            {
                if( currentPage == numberOfPages )
                {
                    currentPage = 1;
                }
                else
                {
                    currentPage++;
                }
            }
            else if( keyInfo.Key.Equals(ConsoleKey.LeftArrow))
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
            else if( keyInfo.Key.Equals(ConsoleKey.Escape) || keyInfo.Key.Equals(ConsoleKey.Q) )
            {
                moduleManager.switchModule(CModuleManager.ModuleType.Map);
            }
        }

        public void initialize()
        {
            numberOfPages = (int)((double)moduleManager.player.inventory.Count() / (double)maxItemsPerPage)+1;
            currentPage = 1;
            //calculateList();
        }

        public void destroy()
        {
        }
    }
}
