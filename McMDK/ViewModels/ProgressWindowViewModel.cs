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

namespace McMDK.ViewModels
{
    public class ProgressWindowViewModel : ViewModel
    {

        public ProgressWindowViewModel()
        {
            this.IsShow = false;
            this.IsImmediate = false;
        }

        public void Initialize()
        {
        }

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


        #region ProgressText変更通知プロパティ
        private string _ProgressText;

        public string ProgressText
        {
            get
            { return _ProgressText; }
            set
            { 
                if (_ProgressText == value)
                    return;
                _ProgressText = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region ProgressValue変更通知プロパティ
        private int _ProgressValue;

        public int ProgressValue
        {
            get
            { return _ProgressValue; }
            set
            { 
                if (_ProgressValue == value)
                    return;
                _ProgressValue = value;
                RaisePropertyChanged();
            }
        }
        #endregion


        #region IsImmediate変更通知プロパティ
        private bool _IsImmediate;

        public bool IsImmediate
        {
            get
            { return _IsImmediate; }
            set
            { 
                if (_IsImmediate == value)
                    return;
                _IsImmediate = value;
                RaisePropertyChanged();
            }
        }
        #endregion

    }
}
