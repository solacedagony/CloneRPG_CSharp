using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloneRPG
{
    static class CIDManager
    {
        private static double currentId = 0;

        public static double getId()
        {
            currentId += 1;
            return currentId;
        }
    }

    
}
