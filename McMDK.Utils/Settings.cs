using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McMDK.Utils
{
    public class Settings
    {
        public static bool IsShowToolTips { set; get; }

        public static bool IsSendReport { set; get; }

        public static bool EnableAutoUpdate { set; get; }

        public static bool EnableBackgroundWork { set; get; }

        public static bool IsOffilineMode { set; get; }

        public static void Load()
        {

        }
    }
}
