using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using McMDK.Plugin;
using McMDK.Plugin.UI.Controls;
using McMDK.Utils.Log;

namespace McMDK.Assets
{
    public class CorePlugin : IPlugin
    {
        public string Name
        {
            get { return "McMDK"; }
        }

        public string Version
        {
            get { return Utils.Define.GetVersion(); }
        }

        public string Author
        {
            get { return "tuyapin"; }
        }

        public string Id
        {
            get { return "McMDK.Core"; }
        }

        public string Dependents
        {
            get { return "McMDK.Data;McMDK.Plugin;McMDK.Utils"; }
        }

        public string IconPath
        {
            get { return "assembly;/Resources/favicon.png"; }
        }

        public string Description
        {
            get
            {
                return "McMDK Home:http://tuyapin.net/mcmdk/" + Environment.NewLine + 
                    "Build with  Livet, Json.Net, Windows API Code Pack" + Environment.NewLine + 
                    "Copyright (c) 2013, 2014 tuyapin" + Environment.NewLine + 
                    "McMDK is OSS released under The Apache License version 2.";
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
