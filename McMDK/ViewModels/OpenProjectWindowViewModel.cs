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

namespace McMDK.ViewModels
{
    public class OpenProjectWindowViewModel : ViewModel
    {
        /* コマンド、プロパティの定義にはそれぞれ 
         * 
         *  lvcom   : ViewModelCommand
         *  lvcomn  : ViewModelCommand(CanExecute無)
         *  llcom   : ListenerCommand(パラメータ有のコマンド)
         *  llcomn  : ListenerCommand(パラメータ有のコマンド・CanExecute無)
         *  lprop   : 変更通知プロパティ(.NET4.5ではlpropn)
         *  
         * を使用してください。
         * 
         * Modelが十分にリッチであるならコマンドにこだわる必要はありません。
         * View側のコードビハインドを使用しないMVVMパターンの実装を行う場合でも、ViewModelにメソッドを定義し、
         * LivetCallMethodActionなどから直接メソッドを呼び出してください。
         * 
         * ViewModelのコマンドを呼び出せるLivetのすべてのビヘイビア・トリガー・アクションは
         * 同様に直接ViewModelのメソッドを呼び出し可能です。
         */

        /* ViewModelからViewを操作したい場合は、View側のコードビハインド無で処理を行いたい場合は
         * Messengerプロパティからメッセージ(各種InteractionMessage)を発信する事を検討してください。
         */

        /* Modelからの変更通知などの各種イベントを受け取る場合は、PropertyChangedEventListenerや
         * CollectionChangedEventListenerを使うと便利です。各種ListenerはViewModelに定義されている
         * CompositeDisposableプロパティ(LivetCompositeDisposable型)に格納しておく事でイベント解放を容易に行えます。
         * 
         * ReactiveExtensionsなどを併用する場合は、ReactiveExtensionsのCompositeDisposableを
         * ViewModelのCompositeDisposableプロパティに格納しておくのを推奨します。
         * 
         * LivetのWindowテンプレートではViewのウィンドウが閉じる際にDataContextDisposeActionが動作するようになっており、
         * ViewModelのDisposeが呼ばれCompositeDisposableプロパティに格納されたすべてのIDisposable型のインスタンスが解放されます。
         * 
         * ViewModelを使いまわしたい時などは、ViewからDataContextDisposeActionを取り除くか、発動のタイミングをずらす事で対応可能です。
         */

        /* UIDispatcherを操作する場合は、DispatcherHelperのメソッドを操作してください。
         * UIDispatcher自体はApp.xaml.csでインスタンスを確保してあります。
         * 
         * LivetのViewModelではプロパティ変更通知(RaisePropertyChanged)やDispatcherCollectionを使ったコレクション変更通知は
         * 自動的にUIDispatcher上での通知に変換されます。変更通知に際してUIDispatcherを操作する必要はありません。
         */

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
