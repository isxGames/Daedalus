﻿using Daedalus.Functions;
using Daedalus.Modules;
using EVE.ISXEVE;
using LavishVMAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Daedalus.Behaviours
{
    static class b_Mining
    {
        public static bool InitComplete = false;

        static b_Mining()
        {
            // Init
        }

        public static void Pulse()
        {
            if (!InitComplete)
            {
                InitComplete = true;
            }
            // TODO: Mining stuff
        }
    }
}