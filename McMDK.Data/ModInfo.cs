namespace McMDK.Data
{
    public class ModInfo
    {
        public string ModId { set; get; }

        public string Name { set; get; }

        public string Description { set; get; }

        public string Version { get; set; }

        public string McVersion { get; set; }

        public string Url { get; set; }

        public string UpdateUrl { get; set; }

        public string[] Authors { get; set; }

        public string Credits { get; set; }

        public string LogoFile { get; set; }

        public string[] ScreenShots { get; set; }

        public string Parent { get; set; }

        public object[] Dependencies { get; set; }
    }
}
