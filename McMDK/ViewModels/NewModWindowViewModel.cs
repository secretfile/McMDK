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
using McMDK.Plugin;

namespace McMDK.ViewModels
{
    public class NewModWindowViewModel : ViewModel, IDataErrorInfo
    {
        private MainWindowViewModel MainWindowViewModel;

        public NewModWindowViewModel(MainWindowViewModel main)
        {
            this.MainWindowViewModel = main;
            this.IsShow = false;
            this.Categories = PluginLoader.GetPlugins();
        }

        public void Initialize()
        {

        }

        public void Show()
        {
            this.ItemName = null;
            this.ItemName = "";
            this.Category = new DummyPlugin();
            this.Category = null;
            this.CopyFrom = null;
            this.CopyFrom = "";
            this.IsExistCopy = false;
            this.Message = "";
            this.ProjectItems = this.MainWindowViewModel.CurrentProject.Mods;

            this.IsShow = true;
        }

        public void Dismiss()
        {
            this.IsShow = false;
        }


        #region AddCommand
        private ViewModelCommand _AddCommand;

        public ViewModelCommand AddCommand
        {
            get
            {
                if (_AddCommand == null)
                {
                    _AddCommand = new ViewModelCommand(Add, CanAdd);
                }
                return _AddCommand;
            }
        }

        public bool CanAdd()
        {
            if(this._errors.Values.Where(item => item != null).ToArray().Length == 0)
            {
                return true;
            }
            return false;
        }

        public void Add()
        {
            IPlugin plugin = this.Category;
            this.MainWindowViewModel.View.ModdingControl.GenerateAndRenderUIs(plugin, this.ItemName);
            this.Dismiss();
            this.MainWindowViewModel.View.ModdingControl.Show();
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


        #region ItemName変更通知プロパティ
        private string _ItemName;

        public string ItemName
        {
            get
            { return _ItemName; }
            set
            { 
                if (_ItemName == value)
                    return;
                _ItemName = value;
                if(String.IsNullOrEmpty(_ItemName))
                {
                    _errors["ItemName"] = "識別用文字列が設定されていません。";
                }
                else if(this.MainWindowViewModel.CurrentProject.Mods.Exists(item => _ItemName.Equals(item.UniqueId)))
                {
                    _errors["ItemName"] = "プロジェクト内に同じ識別用文字列をもったアイテムが既に存在します。";
                }
                else
                {
                    _errors["ItemName"] = null;
                }
                RaisePropertyChanged();
                this.AddCommand.RaiseCanExecuteChanged();
            }
        }
        #endregion


        #region Categories変更通知プロパティ
        private List<IPlugin> _Categories;

        public List<IPlugin> Categories
        {
            get
            { return _Categories; }
            set
            { 
                if (_Categories == value)
                    return;
                _Categories = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region Category変更通知プロパティ
        private IPlugin _Category;

        public IPlugin Category
        {
            get
            { return _Category; }
            set
            { 
                if (_Category == value)
                    return;
                _Category = value;
                if(_Category == null)
                {
                    _errors["Category"] = "アイテムのカテゴリーが選択されていません。";
                }
                else
                {
                    _errors["Category"] = null;
                }
                RaisePropertyChanged();
                this.AddCommand.RaiseCanExecuteChanged();
            }
        }
        #endregion


        #region IsExistCopy変更通知プロパティ
        private bool _IsExistCopy;

        public bool IsExistCopy
        {
            get
            { return _IsExistCopy; }
            set
            { 
                if (_IsExistCopy == value)
                    return;
                _IsExistCopy = value;
                if(_IsExistCopy)
                {
                    _errors["CopyFrom"] = "コピー元のアイテムが選択されていません。";
                }
                else
                {
                    _errors["CopyFrom"] = null;
                }
                RaisePropertyChanged();
                this.RaisePropertyChanged("CopyFrom");
                this.AddCommand.RaiseCanExecuteChanged();
            }
        }
        #endregion


        #region ProjectItems変更通知プロパティ
        private List<McMDK.Data.Mod> _ProjectItems;

        public List<McMDK.Data.Mod> ProjectItems
        {
            get
            { return _ProjectItems; }
            set
            { 
                if (_ProjectItems == value)
                    return;
                _ProjectItems = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region CopyFrom変更通知プロパティ
        private string _CopyFrom;

        public string CopyFrom
        {
            get
            { return _CopyFrom; }
            set
            { 
                if (_CopyFrom == value)
                    return;
                _CopyFrom = value;
                if (_IsExistCopy && String.IsNullOrEmpty(_CopyFrom))
                {
                    _errors["CopyFrom"] = "コピー元のアイテムが選択されていません。";
                }
                else
                {
                    _errors["CopyFrom"] = null;
                }
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
                if(_errors.ContainsKey(name))
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
