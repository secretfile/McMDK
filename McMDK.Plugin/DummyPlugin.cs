using System.Collections.Generic;
using McMDK.Plugin.UI.Controls;
using McMDK.Utils.Log;

namespace McMDK.Plugin
{
    public class DummyPlugin : IPlugin
    {
        public string Name
        {
            get { return "Dummy"; }
        }

        public string Version
        {
            get { return "1.0.0"; }
        }

        public string Author
        {
            get { return "Dummy"; }
        }

        public string Id
        {
            get { return "McMDK.Plugin.Dummy"; }
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
            get { return ""; }
        }

        private Logger _Logger;
        public Logger Logger
        {
            get
            {
                if (_Logger == null)
                {
                    _Logger = new Logger("Dummy");
                }
                return _Logger;
            }
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
    }
}
