using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using Livet;
using Livet.Commands;
using McMDK.Data;
using McMDK.Utils;
using McMDK.Work;
using Newtonsoft.Json;

namespace McMDK.ViewModels
{
    public class NewProjectWindowViewModel : ViewModel, IDataErrorInfo
    {
        private ProgressWindowViewModel ProgressWindowViewModel;

        public NewProjectWindowViewModel(ProgressWindowViewModel progressWindowViewModel)
        {
            this.IsShow = false;
            this.MinecraftVersionList = Minecraft.MinecraftVersions;
            this.ProgressWindowViewModel = progressWindowViewModel;
        }

        public void Initialize()
        {

        }

        public void Show()
        {
            this.ProjectName = null;
            this.ProjectName = "";
            this.MinecraftVersion = null;
            this.MinecraftVersion = "";
            this.McForgeVersion = null;
            this.McForgeVersion = "";
            this.Message = "";

            this.IsShow = true;
        }

        public void Dismiss()
        {
            this.IsShow = false;
        }


        #region MakeCommand
        private ViewModelCommand _MakeCommand;

        public ViewModelCommand MakeCommand
        {
            get
            {
                if (_MakeCommand == null)
                {
                    _MakeCommand = new ViewModelCommand(Make);
                }
                return _MakeCommand;
            }
        }

        public void Make()
        {
            this.ProgressWindowViewModel.IsShow = true;

            var project = new Project();
            project.McVersion = this.MinecraftVersion;
            project.McpVersion = Minecraft.MCPVersions[this.MinecraftVersion];
            project.ForgeVersion = this.McForgeVersion;
            project.Mods = new List<Mod>();
            project.Name = this.ProjectName;
            project.Path = Define.ProjectDirectory + "\\" + this.ProjectName;

            var setup = new Setup(project);
            setup.ProgressWindowViewModel = this.ProgressWindowViewModel;
            setup.OnFinished += SetupOnOnFinished;
            setup.Work();
        }

        private void SetupOnOnFinished(object sender)
        {
            if (sender is Setup)
            {
                var setup = (Setup) sender;
                var project = setup.Project;

                FileController.CreateDirectory(project.Path);
                var sb = new StringBuilder();
                var sw = new StringWriter(sb);
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    writer.Formatting = Formatting.Indented;
                    writer.WriteStartObject();
                    writer.WritePropertyName("Project Name");
                    writer.WriteValue(project.Name);
                    writer.WritePropertyName("Project Path");
                    writer.WriteValue(project.Path);
                    writer.WritePropertyName("Minecraft Version");
                    writer.WriteValue(project.McVersion);
                    writer.WritePropertyName("Minecraft Forge Version");
                    writer.WriteValue(project.ForgeVersion);
                    writer.WritePropertyName("MCP Version");
                    writer.WriteValue(project.McpVersion);
                    writer.WriteEndObject();
                }
                FileController.CreateDirectory(Define.ProjectDirectory + "\\" + project.Name + "\\project");
                var s = new StreamWriter(Define.ProjectDirectory + "\\" + project.Name + "\\project\\settings.json");
                s.Write(sb.ToString());
                s.Close();
                s.Dispose();
            }
            this.IsShow = false;
            this.ProgressWindowViewModel.IsShow = false;
        }

        #endregion



        #region CancelCommand
        private ViewModelCommand _CancelCommand;

        public ViewModelCommand CancelCommand
        {
            get
            {
                if (_CancelCommand == null)
                {
                    _CancelCommand = new ViewModelCommand(Cancel);
                }
                return _CancelCommand;
            }
        }

        public void Cancel()
        {
            this.Dismiss();
        }
        #endregion


        #region IsShow変更通知プロパティ
        private bool _IsShow;

        public bool IsShow
        {
            get
            { return _IsShow; }
            set
            { 
                if (_IsShow == value)
                    return;
                _IsShow = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region MinecraftVersionList変更通知プロパティ
        private List<string> _MinecraftVersionList;

        public List<string> MinecraftVersionList
        {
            get
            { return _MinecraftVersionList; }
            set
            { 
                if (_MinecraftVersionList == value)
                    return;
                _MinecraftVersionList = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region McForgeVersionList変更通知プロパティ
        private List<string> _McForgeVersionList;

        public List<string> McForgeVersionList
        {
            get
            { return _McForgeVersionList; }
            set
            {
                if (_McForgeVersionList == value)
                    return;
                _McForgeVersionList = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region ProjectName変更通知プロパティ
        private string _ProjectName;

        public string ProjectName
        {
            get
            { return _ProjectName; }
            set
            { 
                if (_ProjectName == value)
                    return;
                _ProjectName = value;
                if (String.IsNullOrEmpty(_ProjectName))
                {
                    _errors["ProjectName"] = "プロジェクト名が入力されていません。";
                }
                else if (!String.IsNullOrEmpty(_ProjectName) &&
                         FileController.Exists(Define.ProjectDirectory + "\\" + _ProjectName))
                {
                    _errors["ProjectName"] = "すでに同名のプロジェクトが作成されています。";
                }
                else
                {
                    _errors["ProjectName"] = null;
                }
                RaisePropertyChanged();
            }
        }
        #endregion


        #region MinecraftVersion変更通知プロパティ
        private string _MinecraftVersion;

        public string MinecraftVersion
        {
            get
            { return _MinecraftVersion; }
            set
            { 
                if (_MinecraftVersion == value)
                    return;
                _MinecraftVersion = value;

                if (String.IsNullOrEmpty(_MinecraftVersion))
                {
                    _errors["MinecraftVersion"] = "Minecraftのバージョンが選択されていません。";
                }
                else
                {
                    _errors["MinecraftVersion"] = null;
                    this.McForgeVersionList = Minecraft.ForgeVersions[_MinecraftVersion];
                    this.McForgeVersion = "";

                    if (_MinecraftVersion.Contains("Gradle"))
                    {
                        this.Message = "※GradleはMinecraft Forge #953以降で使用されているビルドツールです。";
                    }
                    else
                    {
                        this.Message = "";
                    }
                }

                RaisePropertyChanged();
            }
        }
        #endregion


        #region McForgeVersion変更通知プロパティ
        private string _McForgeVersion;

        public string McForgeVersion
        {
            get
            { return _McForgeVersion; }
            set
            { 
                if (_McForgeVersion == value)
                    return;
                _McForgeVersion = value;

                if (String.IsNullOrEmpty(_McForgeVersion))
                {
                    _errors["McForgeVersion"] = "Minecraft Forgeのバージョンが選択されていません。";
                }
                else
                {
                    _errors["McForgeVersion"] = null;
                }

                RaisePropertyChanged();
            }
        }
        #endregion


        #region IsGenModInfo変更通知プロパティ
        private bool _IsGenModInfo;

        public bool IsGenModInfo
        {
            get
            { return _IsGenModInfo; }
            set
            { 
                if (_IsGenModInfo == value)
                    return;
                _IsGenModInfo = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region Message変更通知プロパティ
        private string _Message;

        public string Message
        {
            get
            { return _Message; }
            set
            { 
                if (_Message == value)
                    return;
                _Message = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region IDataErrorInfo

        private Dictionary<string, string> _errors = new Dictionary<string, string>(); 
        public string this[string name]
        {
            get
            {
                if (_errors.ContainsKey(name))
                {
                    return _errors[name];
                }
                return null;
            }
        }

        public string Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}
