using System;
using System.Collections.Generic;
using McMDK.Plugin;

namespace McMDK.Data
{
    [Serializable]
    public class Mod
    {
        public Dictionary<string, string> Properties { set; get; }

        public IPlugin Plugin { set; get; }

        public string UniqueId { set; get; }

    }
}
