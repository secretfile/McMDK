using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using McMDK.Utils;

namespace McMDK.Data
{
    public class Minecraft
    {
        public static List<string> MinecraftVersions = new List<string>();

        public static Dictionary<string, List<string>> ForgeVersions = new Dictionary<string, List<string>>();

        public static Dictionary<string, string> MCPVersions = new Dictionary<string, string>();

        public async static void Load()
        {
            var client = new WebClient();
            var vs = await client.DownloadStringTaskAsync(Define.MinecraftVersionUrl);

            var element = XElement.Parse(vs);
// ReSharper disable once PossibleNullReferenceException
            var q = element.Element("Minecraft").Elements("Version").Select(p => new
            {
                Version = p.Value,
                MCPVersion = (string) p.Attribute("mcp")
            });
            foreach (var item in q)
            {
                MinecraftVersions.Add(item.Version);
                MCPVersions.Add(item.Version, item.MCPVersion);

                //
                vs = await client.DownloadStringTaskAsync(String.Format(Define.ForgeVersionUrl, item.Version).Replace(" ", "_"));
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
    }
}
