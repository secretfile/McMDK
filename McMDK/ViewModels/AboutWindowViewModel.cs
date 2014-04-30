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

using McMDK.Assets;
using McMDK.Models;
using McMDK.Plugin;

namespace McMDK.ViewModels
{
    public class AboutWindowViewModel : ViewModel
    {

        public AboutWindowViewModel()
        {
            this.Plugins = new List<IPlugin>();
            this.Plugins.Add(new CorePlugin());
            this.Plugins.Add(new DataPlugin());
            this.Plugins.Add(new PInterfacePlugin());
            this.Plugins.Add(new UtilsPlugin());
            foreach(IPlugin plugin in PluginLoader.GetPlugins())
            {
                this.Plugins.Add(plugin);
            }
            this.Version = McMDK.Utils.Define.GetVersion();
            this.IsShow = false;
        }

        public void Initialize()
        {
        }

        public void Show()
        {
            this.IsShow = true;
        }

        public void Dismiss()
        {
            this.IsShow = false;
        }


        #region OKCommand
        private ViewModelCommand _OKCommand;

        public ViewModelCommand OKCommand
        {
            get
            {
                if (_OKCommand == null)
                {
                    _OKCommand = new ViewModelCommand(OK);
                }
                return _OKCommand;
            }
        }

        public void OK()
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


        #region Version変更通知プロパティ
        private string _Version;

        public string Version
        {
            get
            { return _Version; }
            set
            { 
                if (_Version == value)
                    return;
                _Version = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region Plugins変更通知プロパティ
        private List<IPlugin> _Plugins;

        public List<IPlugin> Plugins
        {
            get
            { return _Plugins; }
            set
            { 
                if (_Plugins == value)
                    return;
                _Plugins = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region SelectedItem変更通知プロパティ
        private IPlugin _SelectedItem;

        public IPlugin SelectedItem
        {
            get
            { return _SelectedItem; }
            set
            { 
                if (_SelectedItem == value)
                    return;
                _SelectedItem = value;
                if(_SelectedItem != null)
                {
                    this.Text = _SelectedItem.Name + " " + _SelectedItem.Version + Environment.NewLine + _SelectedItem.Description;
                }
                RaisePropertyChanged();
            }
        }
        #endregion


        #region Text変更通知プロパティ
        private string _Text;

        public string Text
        {
            get
            { return _Text; }
            set
            { 
                if (_Text == value)
                    return;
                _Text = value;
                RaisePropertyChanged();
            }
        }
        #endregion

    }
}
