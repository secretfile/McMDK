using System.Collections.Generic;
using McMDK.Plugin.UI.Controls;
using McMDK.Utils.Log;

namespace McMDK.Plugin
{
    public interface IPlugin
    {
        string Name { get; }

        string Version { get; }

        string Author { get; }

        string Id { get; }

        string Dependents { get; }

        Logger Logger { get; }

        List<UIControl> Controls { get; } 

        void Loaded();
    }
}
