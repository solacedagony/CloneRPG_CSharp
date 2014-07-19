using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloneRPG
{
    class InventoryMap : CModule
    {
        CModuleManager moduleManager;

        public InventoryMap( CModuleManager moduleManagerArg )
        {
            moduleManager = moduleManagerArg;
        }

        void draw()
        {
            drawHeader();
        }

        void drawHeader()
        {

        }

        void initialize()
        {
            //calculateList();
        }

        void destroy()
        {
        }
    }
}
