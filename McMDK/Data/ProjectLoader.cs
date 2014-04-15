using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using McMDK.Utils;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace McMDK.Data
{
    public class ProjectLoader
    {
        private static List<Project> Projects = new List<Project>();

        public static void Load()
        {
            //Load projects
            string[] projects = FileController.LoadDirectory(Define.ProjectDirectory, true);
            foreach (string project in projects)
            {
                try
                {
                    Define.GetLogger().Fine("Loading Project from " + project);
                    if (!FileController.Exists(project + "//project//settings.json"))
                    {
                        Define.GetLogger().Fine("Skip loading project of " + project);
                        Define.GetLogger().Fine("\"settings.json\" is not exist!");
                        continue;
                    }
                    string json = FileController.LoadFile(project + "//project//settings.json");
                    JObject o = JObject.Parse(json);

                    Project p = new Project();
                    p.ForgeVersion = (string)o["Minecraft Forge Version"];
                    p.McpVersion = (string)o["MCP Version"];
                    p.McVersion = (string)o["Minecraft Version"];
                    p.Name = (string)o["Project Name"];
                    p.Path = (string)o["Project Path"];

                    //Load Mods
                    string[] files = FileController.LoadDirectory(project + "//project", true);
                    foreach (string file in files)
                    {
                        if (file == "settings.json")
                        {
                            continue;
                        }
                        //
                    }

                    Projects.Add(p);
                }
                catch (Exception)
                {

                }
            }
        }

        public static List<Project> GetProjects()
        {
            return Projects;
        }
    }
}