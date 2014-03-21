using System.Diagnostics;
using System.IO;
using McMDK.Utils.Log;

namespace McMDK.Utils
{
    public class Define
    {
        public static string FilePath
        {
            get { return Process.GetCurrentProcess().MainModule.FileName; }
        }

        public static string GetVersion()
        {
            return Version + "." + Release;
        }

        private static Logger _logger;
        public static Logger GetLogger()
        {
            if (_logger == null)
            {
                _logger = new Logger("McMDK - Main");
            }
            return _logger;
        }

        private const string Version = "2.0.0";

        private const int Release = 25;

        public static readonly string CurrentDirectory = Directory.GetCurrentDirectory();

        public static readonly string PluginDirectory = CurrentDirectory + "\\plugins";

        public static readonly string ProjectDirectory = CurrentDirectory + "\\projects";

        public static readonly string LogDirectory = CurrentDirectory + "\\logs";

        public static readonly string TempDirectory = CurrentDirectory + "\\temp";

        public static readonly string SettingPath = CurrentDirectory + "\\settings.xml";

        private const string Domain = "http://tuyapin.net/application/mcmdk/";

        public static readonly string NewVersionUrl = Domain + "version.xml";

        public static readonly string ForgeVersionUrl = Domain + "forge/{0}.xml";

        public static readonly string MinecraftVersionUrl = Domain + "minecraft.xml";

        public static readonly string PatchUrl = Domain + "patch/{0}.zip";

        public static readonly string LibrariesUrl = Domain + "libs/";

        public static readonly string McBinaryUrl = "http://assets.minecraft.net/{0}/minecraft.jar";

        public static readonly string McSrvBinaryUrl = "http://assets.minecraft.net/{0}/minecraft_server.jar";

        public static readonly string McResourcesUrl = "http://s3.amazonaws.com/MinecraftDownload/";

        public static readonly string CoderPackUrl = "http://mcp.ocean-labs.de/files/archive/mcp{0}.zip";

        public static readonly string ForgeGradleUrl = "http://files.minecraftforge.net/maven/net/minecraftforge/forge/{0}-{1}/forge-{0}-{1}-src.zip";

        public static readonly string ForgeBinaryUrl = "http://files.minecraftforge.net/minecraftforge/minecraftforge-src-{0}-{1}.zip";
    }
}
