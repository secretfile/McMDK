using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using McMDK.Plugin;
using McMDK.Plugin.UI.Controls;
using McMDK.Utils.Log;

namespace McMDK.Assets
{
    public class DataPlugin : IPlugin
    {
        public string Name
        {
            get { return "McMDK Data"; }
        }

        public string Version
        {
            get { return FileVersionInfo.GetVersionInfo("McMDK.Data.dll").FileVersion; }
        }

        public string Author
        {
            get { return "tuyapin"; }
        }

        public string Id
        {
            get { return "McMDK.Data"; }
        }

        public string Dependents
        {
            get { return "McMDK.Plugin;McMDK.Utils"; }
        }

        public string IconPath
        {
            get { return ""; }
        }

        public string Description
        {
            get
            {
                return "McMDK.DataはMcMDKのデータ構造、処理を提供します。";
            }
        }

        public Logger Logger
        {
            get { return null; }
        }

        public List<UIControl> Controls
        {
            get { return null; }
        }

        public void Loaded()
        {

        }

        public void Update()
        {

        }

        public override string ToString()
        {
            return this.Name + " (" + this.Version + ")";
        }
    }
}
