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
    public class UtilsPlugin : IPlugin
    {
        public string Name
        {
            get { return "McMDK Utils"; }
        }

        public string Version
        {
            get { return FileVersionInfo.GetVersionInfo("McMDK.Utils.dll").FileVersion; }
        }

        public string Author
        {
            get { return "tuyapin"; }
        }

        public string Id
        {
            get { return "McMDK.Utils"; }
        }

        public string Dependents
        {
            get { return ""; }
        }

        public string IconPath
        {
            get { return ""; }
        }

        public string Description
        {
            get
            {
                return "McMDK.Utilsは、McMDKの共通の処理を提供します。";
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
