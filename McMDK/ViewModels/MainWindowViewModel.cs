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
using McMDK.Utils;

namespace McMDK.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {

        public MainWindowViewModel()
        {
            this.Title = "Minecraft Mod Development Kit v" + Define.GetVersion();

            this.NewProjectWindowViewModel = new NewProjectWindowViewModel();
        }

        public void Initialize()
        {

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
            this.NewProjectWindowViewModel.Show();
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

    }
}
