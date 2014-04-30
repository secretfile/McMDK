using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using McMDK.Plugin;
using McMDK.Plugin.UI.Controls;
using McMDK.Models;
using McMDK.Utils;
using McMDK.Data;
using McMDK.Views;
using Microsoft.WindowsAPICodePack.Dialogs;

using Newtonsoft.Json;

namespace McMDK.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        public Project CurrentProject { set; get; }
        public IntPtr WindowHandle;

        //TODO: 分離
        public MainWindow View { set; get; }
        public ModdingControl ModinggControl;

        public MainWindowViewModel()
        {
            this.Title = "Minecraft Mod Development Kit " + Define.GetVersion();

            this.AboutWindowViewModel = new AboutWindowViewModel();
            this.ProgressWindowViewModel = new ProgressWindowViewModel();
            this.NewProjectWindowViewModel = new NewProjectWindowViewModel(this, this.ProgressWindowViewModel);
            this.OpenProjectWindowViewModel = new OpenProjectWindowViewModel(this);
            this.InformationWindowViewModel = new InformationWindowViewModel(this, this.ProgressWindowViewModel);
            this.NewModWindowViewModel = new NewModWindowViewModel(this);
        }

        public void Initialize()
        {
            this.ModinggControl = new ModdingControl();
            this.ModinggControl.Margin = new System.Windows.Thickness(200, 115, 0, 0);
            this.View.MainGrid.Children.Add(this.ModinggControl);

            Define.GetLogger().Info("Cheking plugins updates...");

            this.ProgressWindowViewModel.IsShow = true;
            this.ProgressWindowViewModel.IsImmediate = true;
            this.ProgressWindowViewModel.ProgressText = "プラグインの更新を確認しています...";
            foreach(IPlugin p in PluginLoader.GetPlugins())
            {
                p.Update();
            }
            Define.GetLogger().Info("Check finished.");
            this.ProgressWindowViewModel.IsShow = false;
        }


        #region OpenAboutCommand
        private ViewModelCommand _OpenAboutCommand;

        public ViewModelCommand OpenAboutCommand
        {
            get
            {
                if (_OpenAboutCommand == null)
                {
                    _OpenAboutCommand = new ViewModelCommand(OpenAbout);
                }
                return _OpenAboutCommand;
            }
        }

        public void OpenAbout()
        {
            this.AboutWindowViewModel.Show();
        }
        #endregion


        #region NewProjectCommand
        private ViewModelCommand _NewProjectCommand;

        public ViewModelCommand NewProjectCommand
        {
            get
            {
                if (_NewProjectCommand == null)
                {
                    _NewProjectCommand = new ViewModelCommand(NewProject);
                }
                return _NewProjectCommand;
            }
        }

        public void NewProject()
        {
            if (this.CurrentProject == null)
            {
                this.NewProjectWindowViewModel.Show();
            }
            else
            {
                var taskDialog = new TaskDialog();
                taskDialog.Caption = "警告";
                taskDialog.InstructionText = "プロジェクトが既に開かれています。";
                taskDialog.Text = "プロジェクトが既に開かれています。新規にプロジェクトを開くと、現在のプロジェクトはアンロードされますがよろしいですか？\n※保存していない場合はこれまでの作業が破棄されます。";
                taskDialog.Icon = TaskDialogStandardIcon.Warning;
                taskDialog.StandardButtons = TaskDialogStandardButtons.Yes | TaskDialogStandardButtons.No;
                taskDialog.Opened += (_, __) =>
                {
                    var sender = (TaskDialog)_;
                    sender.Icon = sender.Icon;
                };
                taskDialog.StartupLocation = TaskDialogStartupLocation.CenterOwner;
                taskDialog.OwnerWindowHandle = this.WindowHandle;
                taskDialog.Cancelable = false;
                if(taskDialog.Show() == TaskDialogResult.No)
                {
                    return;
                }
                this.NewProjectWindowViewModel.Show();
            }
        }
        #endregion


        #region OpenProjectCommand
        private ViewModelCommand _OpenProjectCommand;

        public ViewModelCommand OpenProjectCommand
        {
            get
            {
                if (_OpenProjectCommand == null)
                {
                    _OpenProjectCommand = new ViewModelCommand(OpenProject);
                }
                return _OpenProjectCommand;
            }
        }

        public void OpenProject()
        {
            if(this.CurrentProject == null)
            {
                this.OpenProjectWindowViewModel.IsShow = true;
            }
            else
            {
                var taskDialog = new TaskDialog();
                taskDialog.Caption = "警告";
                taskDialog.InstructionText = "プロジェクトが既に開かれています。";
                taskDialog.Text = "プロジェクトが既に開かれています。新規にプロジェクトを開くと、現在のプロジェクトはアンロードされますがよろしいですか？\n※保存していない場合はこれまでの作業が破棄されます。";
                taskDialog.Icon = TaskDialogStandardIcon.Warning;
                taskDialog.StandardButtons = TaskDialogStandardButtons.Yes | TaskDialogStandardButtons.No;
                taskDialog.StartupLocation = TaskDialogStartupLocation.CenterOwner;
                taskDialog.OwnerWindowHandle = this.WindowHandle;
                taskDialog.Cancelable = false;
                taskDialog.Opened += (_, __) =>
                {
                    var sender = (TaskDialog)_;
                    sender.Icon = sender.Icon;
                };
                if(taskDialog.Show() == TaskDialogResult.No)
                {
                    return;
                }
                this.OpenProjectWindowViewModel.IsShow = true;
            }
        }
        #endregion


        #region SaveProjectCommand
        private ViewModelCommand _SaveProjectCommand;

        public ViewModelCommand SaveProjectCommand
        {
            get
            {
                if (_SaveProjectCommand == null)
                {
                    _SaveProjectCommand = new ViewModelCommand(SaveProject);
                }
                return _SaveProjectCommand;
            }
        }

        public void SaveProject()
        {
            var project = this.CurrentProject;
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
            var s = new StreamWriter(Define.ProjectDirectory + "\\" + project.Name + "\\project\\settings.json", false);
            s.Write(sb.ToString());
            s.Close();
            s.Dispose();

        }
        #endregion


        #region OpenInformationCommand
        private ViewModelCommand _OpenInformationCommand;

        public ViewModelCommand OpenInformationCommand
        {
            get
            {
                if (_OpenInformationCommand == null)
                {
                    _OpenInformationCommand = new ViewModelCommand(OpenInformation);
                }
                return _OpenInformationCommand;
            }
        }

        public void OpenInformation()
        {
            if(this.CurrentProject != null)
            {
                this.InformationWindowViewModel.Show();
            }
            else
            {
                var taskDialog = new TaskDialog();
                taskDialog.Caption = "エラー";
                taskDialog.InstructionText = "プロジェクトが開かれていません。";
                taskDialog.Text = "プロジェクトが開かれていないため、プロジェクト情報を取得することができませんでした。\nプロジェクトを開いてから実行してください。";
                taskDialog.Icon = TaskDialogStandardIcon.Error;
                taskDialog.StandardButtons = TaskDialogStandardButtons.Ok;
                taskDialog.StartupLocation = TaskDialogStartupLocation.CenterOwner;
                taskDialog.OwnerWindowHandle = this.WindowHandle;
                taskDialog.Cancelable = false;
                taskDialog.Opened += (_, __) =>
                {
                    var sender = (TaskDialog)_;
                    sender.Icon = sender.Icon;
                };
                taskDialog.Show();
            }
        }
        #endregion


        #region AddItemCommand
        private ViewModelCommand _AddItemCommand;

        public ViewModelCommand AddItemCommand
        {
            get
            {
                if (_AddItemCommand == null)
                {
                    _AddItemCommand = new ViewModelCommand(AddItem);
                }
                return _AddItemCommand;
            }
        }

        public void AddItem()
        {
            if (this.CurrentProject != null)
            {
                this.NewModWindowViewModel.Show();
            }
            else
            {
                var taskDialog = new TaskDialog();
                taskDialog.Caption = "エラー";
                taskDialog.InstructionText = "プロジェクトが開かれていません。";
                taskDialog.Text = "プロジェクトが開かれていないため、新規アイテムを追加することができませんでした。\nプロジェクトを開いてから実行してください。";
                taskDialog.Icon = TaskDialogStandardIcon.Error;
                taskDialog.StandardButtons = TaskDialogStandardButtons.Ok;
                taskDialog.StartupLocation = TaskDialogStartupLocation.CenterOwner;
                taskDialog.OwnerWindowHandle = this.WindowHandle;
                taskDialog.Cancelable = false;
                taskDialog.Opened += (_, __) =>
                {
                    var sender = (TaskDialog)_;
                    sender.Icon = sender.Icon;
                };
                taskDialog.Show();
            }
        }
        #endregion


        #region Title変更通知プロパティ
        private string _Title;

        public string Title
        {
            get
            { return _Title; }
            set
            { 
                if (_Title == value)
                    return;
                _Title = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region AboutWindowViewModel変更通知プロパティ
        private AboutWindowViewModel _AboutWindowViewModel;

        public AboutWindowViewModel AboutWindowViewModel
        {
            get
            { return _AboutWindowViewModel; }
            set
            { 
                if (_AboutWindowViewModel == value)
                    return;
                _AboutWindowViewModel = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region NewProjectWindowViewModel変更通知プロパティ
        private NewProjectWindowViewModel _NewProjectWindowViewModel;

        public NewProjectWindowViewModel NewProjectWindowViewModel
        {
            get
            { return _NewProjectWindowViewModel; }
            set
            { 
                if (_NewProjectWindowViewModel == value)
                    return;
                _NewProjectWindowViewModel = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region ProgressWindowViewModel変更通知プロパティ
        private ProgressWindowViewModel _ProgressWindowViewModel;

        public ProgressWindowViewModel ProgressWindowViewModel
        {
            get
            { return _ProgressWindowViewModel; }
            set
            { 
                if (_ProgressWindowViewModel == value)
                    return;
                _ProgressWindowViewModel = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region OpenProjectWindowViewModel変更通知プロパティ
        private OpenProjectWindowViewModel _OpenProjectWindowViewModel;

        public OpenProjectWindowViewModel OpenProjectWindowViewModel
        {
            get
            { return _OpenProjectWindowViewModel; }
            set
            { 
                if (_OpenProjectWindowViewModel == value)
                    return;
                _OpenProjectWindowViewModel = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region InformationWindowViewModel変更通知プロパティ
        private InformationWindowViewModel _InformationWindowViewModel;

        public InformationWindowViewModel InformationWindowViewModel
        {
            get
            { return _InformationWindowViewModel; }
            set
            { 
                if (_InformationWindowViewModel == value)
                    return;
                _InformationWindowViewModel = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region NewModWindowViewModel変更通知プロパティ
        private NewModWindowViewModel _NewModWindowViewModel;

        public NewModWindowViewModel NewModWindowViewModel
        {
            get
            { return _NewModWindowViewModel; }
            set
            { 
                if (_NewModWindowViewModel == value)
                    return;
                _NewModWindowViewModel = value;
                RaisePropertyChanged();
            }
        }
        #endregion

    }
}
