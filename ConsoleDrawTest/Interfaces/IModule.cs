﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloneRPG
{
    interface IModule
    {
        void draw();
        void initialize();
        void destroy();
    }
}
