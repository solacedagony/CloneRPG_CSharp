using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloneRPG
{
    abstract class CModule
    {
        abstract public void draw();

        abstract public void initialize();

        abstract public void destroy();
    }
}
