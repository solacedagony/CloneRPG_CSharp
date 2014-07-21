using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloneRPG
{
    class CItemManager
    {
        CModuleManager moduleManager;
        List<CItem> items = null;

        public CItemManager( CModuleManager moduleManagerArg)
        {
            moduleManager = moduleManagerArg;
            items = new List<CItem>();
        }

        public bool loadItems( string itemDirectory)
        {
            return Utility.FileIO.loadItems(ref items, itemDirectory);
        }

        public CItem getItemByName(string itemName)
        {
            for (int i = 0; i < items.Count(); i++)
            {
                if (items[i].name.ToString().ToUpperInvariant() == itemName.ToUpperInvariant())
                {
                    return items[i];
                }
            }

            return null;
        }
    }
}
