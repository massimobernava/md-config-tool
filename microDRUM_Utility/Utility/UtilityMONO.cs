using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace microDrum
{
    public class UtilityMONO
    {
        public static bool RunningOnMONO()
        {
            Type t = Type.GetType ("Mono.Runtime");
            if (t != null) return true;

            return false;
        }

        public static bool RunningOnUnix()
        {
            int p = (int)Environment.OSVersion.Platform;
            if ((p == 4) || (p == 6) || (p == 128))
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }
}
