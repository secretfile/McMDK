using System.Collections.Generic;
using McMDK.Plugin.UI.Controls;
using McMDK.Utils.Log;

namespace McMDK.Plugin
{
    public class XmlPluginBase : IPlugin
    {

        public string Name { set; get; }

        public string Version { set; get; }

        public string Author { set; get; }

        public string Id { set; get; }

        public string Dependents { set; get; }

        public Logger Logger { set; get; }

        public List<UIControl> Controls { set; get; }

        public void Loaded()
        {
            this.Controls = new List<UIControl>();
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
