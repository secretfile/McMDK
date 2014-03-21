using System.Collections.Generic;

namespace McMDK.Data
{
    public class Project
    {
        public Project()
        {
            this.Name = "";
            this.McVersion = "";
            this.ForgeVersion = "";
            this.McpVersion = "";
            this.Mods = new List<Mod>();
            this.Path = "";
        }

        public string Name { set; get; }

        public string McVersion { set; get; }

        public string ForgeVersion { set; get; }

        public string McpVersion { set; get; }

        public List<Mod> Mods { set; get; }

        public string Path { set; get; }

        public override string ToString()
        {
            return this.Name + "(MC" + this.McVersion + ", FORGE" + this.ForgeVersion + ", MCP" + this.McpVersion + ")";
        }
    }
}
