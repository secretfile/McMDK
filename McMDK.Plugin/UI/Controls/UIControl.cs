using System.Collections.Generic;
using System.Windows;
using System.Windows.Media;

namespace McMDK.Plugin.UI.Controls
{
    public class UIControl
    {
        public GuiComponents Component { set; get; }

        public string Name { set; get; }

        public double Height { set; get; }

        public double Width { set; get; }

        public bool? IsEnabled { set; get; }

        public bool? IsVisible { set; get; }

        public Visibility? Visibility { set; get; }

        public HorizontalAlignment? HorizontalAlignment { set; get; }

        public VerticalAlignment? VerticalAlignment { set; get; }

        public Thickness? Margin { set; get; }

        public Brush Background { set; get; }

        public Brush Foreground { set; get; }

        public string ToolTip { set; get; }

        public double Opacity { set; get; }

// ReSharper disable once MemberCanBePrivate.Global
// ReSharper disable once UnusedAutoPropertyAccessor.Global
        public List<UIControl> Children { set; get; }

        public UIControl Parent { set; get; }

        public object Tag { set; get; }

        public UIControl()
        {
            this.Children = new List<UIControl>();
        }
    }
}
