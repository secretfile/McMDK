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

using McMDK.Plugin;
using McMDK.Models;
using McMDK.Utils;
using McMDK.Data;

using Microsoft.WindowsAPICodePack.Dialogs;

namespace McMDK.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        public Project CurrentProject { set; get; }
        public IntPtr WindowHandle;

        public MainWindowViewModel()
        {
            this.Title = "Minecraft Mod Development Kit v" + Define.GetVersion();

            this.ProgressWindowViewModel = new ProgressWindowViewModel();
            this.NewProjectWindowViewModel = new NewProjectWindowViewModel(this, this.ProgressWindowViewModel);
            this.OpenProjectWindowViewModel = new OpenProjectWindowViewModel(this);
        }

        public void Initialize()
        {
            this.ProgressWindowViewModel.IsShow = true;
            this.ProgressWindowViewModel.IsImmediate = true;
            this.ProgressWindowViewModel.ProgressText = "プラグインの更新を確認しています...";
            foreach(IPlugin p in PluginLoader.GetPlugins())
            {
                p.Update();
            }
            this.ProgressWindowViewModel.IsShow = false;
        }


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

    }
}
