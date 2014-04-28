using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using McMDK.Models;
using McMDK.Data;
using McMDK.Plugin;
using McMDK.Utils;

namespace McMDK.ViewModels
{
    public class OpenProjectWindowViewModel : ViewModel
    {
        private MainWindowViewModel MainWindowViewModel;

        public OpenProjectWindowViewModel(MainWindowViewModel main)
        {
            this.MainWindowViewModel = main;
            this.IsShow = false;

            this.Projects = ProjectLoader.GetProjects();
        }


        #region OpenButtonCommand
        private ViewModelCommand _OpenButtonCommand;

        public ViewModelCommand OpenButtonCommand
        {
            get
            {
                if (_OpenButtonCommand == null)
                {
                    _OpenButtonCommand = new ViewModelCommand(OpenButton);
                }
                return _OpenButtonCommand;
            }
        }

        public void OpenButton()
        {
            this.MainWindowViewModel.CurrentProject = (Project)this.SelectedItem;
            this.MainWindowViewModel.Title = "" + ((Project)this.SelectedItem).Name + " - Minecraft Mod Development Kit " + Define.GetVersion();
            this.IsShow = false;
        }
        #endregion



        #region CancelButtonCommand
        private ViewModelCommand _CancelButtonCommand;

        public ViewModelCommand CancelButtonCommand
        {
            get
            {
                if (_CancelButtonCommand == null)
                {
                    _CancelButtonCommand = new ViewModelCommand(CancelButton);
                }
                return _CancelButtonCommand;
            }
        }

        public void CancelButton()
        {
            this.IsShow = false;
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


        #region Project変更通知プロパティ
        private List<Project> _Projects;

        public List<Project> Projects
        {
            get
            { return _Projects; }
            set
            { 
                if (_Projects == value)
                    return;
                _Projects = value;
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
                RaisePropertyChanged();
            }
        }
        #endregion


        #region ProjectPath変更通知プロパティ
        private string _ProjectPath;

        public string ProjectPath
        {
            get
            { return _ProjectPath; }
            set
            { 
                if (_ProjectPath == value)
                    return;
                _ProjectPath = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region MinecraftVer変更通知プロパティ
        private string _MinecraftVer;

        public string MinecraftVer
        {
            get
            { return _MinecraftVer; }
            set
            { 
                if (_MinecraftVer == value)
                    return;
                _MinecraftVer = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region MCForgeVer変更通知プロパティ
        private string _MCForgeVer;

        public string MCForgeVer
        {
            get
            { return _MCForgeVer; }
            set
            { 
                if (_MCForgeVer == value)
                    return;
                _MCForgeVer = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region MCPPVer変更通知プロパティ
        private string _MCPVer;

        public string MCPVer
        {
            get
            { return _MCPVer; }
            set
            {
                if (_MCPVer == value)
                    return;
                _MCPVer = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region ModInfo変更通知プロパティ
        private string _ModInfo;

        public string ModInfo
        {
            get
            { return _ModInfo; }
            set
            { 
                if (_ModInfo == value)
                    return;
                _ModInfo = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region Mods変更通知プロパティ
        private string _Mods;

        public string Mods
        {
            get
            { return _Mods; }
            set
            { 
                if (_Mods == value)
                    return;
                _Mods = value;
                RaisePropertyChanged();
            }
        }
        #endregion




        #region SelectedItem変更通知プロパティ
        private object _SelectedItem;

        public object SelectedItem
        {
            get
            { return _SelectedItem; }
            set
            { 
                if (_SelectedItem == value)
                    return;
                _SelectedItem = value;
                RaisePropertyChanged();
                if(_SelectedItem is Project)
                {
                    var project = (Project)_SelectedItem;
                    this.ProjectName = project.Name;
                    this.ProjectPath = project.Path;
                    this.MinecraftVer = project.McVersion;
                    this.MCForgeVer = project.ForgeVersion;
                    this.MCPVer = project.McpVersion;
                    this.Mods = project.Mods.Count.ToString();
                }
            }
        }
        #endregion

    }
}
