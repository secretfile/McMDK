using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;

namespace McMDK.Data
{
    public class Minecraft
    {
        public static List<string> MinecraftVersions = new List<string>();

        public static Dictionary<string, List<string>> ForgeVersions = new Dictionary<string, List<string>>();

        public static Dictionary<string, string> MCPVersions = new Dictionary<string, string>();

        private const string Domain = "http://tuyapin.net/application/mcmdk/";
        public static readonly string NewVersionUrl = Domain + "version.xml";
        public static readonly string ForgeVersionUrl = Domain + "forge/{0}.xml";
        public static readonly string MinecraftVersionUrl = Domain + "minecraft.xml";

        public async static void Load()
        {
            try
            {
                var client = new WebClient();
                var vs = await client.DownloadStringTaskAsync(MinecraftVersionUrl);

                var element = XElement.Parse(vs);
                // ReSharper disable once PossibleNullReferenceException
                var q = element.Element("Minecraft").Elements("Version").Select(p => new
                {
                    Version = p.Value,
                    MCPVersion = (string)p.Attribute("mcp")
                });
                foreach (var item in q)
                {
                    MinecraftVersions.Add(item.Version);
                    MCPVersions.Add(item.Version, item.MCPVersion);

                    //
                    vs = await client.DownloadStringTaskAsync(String.Format(ForgeVersionUrl, item.Version).Replace(" ", "_"));
                    var list = new List<string>();
                    XElement element1 = XElement.Parse(vs);
                    // ReSharper disable once PossibleNullReferenceException
                    var s = element1.Element("MinecraftForge").Elements("Version").Select(r => new
                    {
                        Version = r.Value
                    });
                    foreach (var item1 in s)
                    {
                        list.Add(item1.Version);
                    }
                    ForgeVersions.Add(item.Version, list);
                }
            }
            catch (WebException)
            {

            }
        }
    }
}
