using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace McMDK.Plugin.UI.Controls
{
// ReSharper disable once RedundantExtendsListEntry
    public partial class ModdingControl : UserControl
    {
        public ModdingControl()
        {
            InitializeComponent();
            this.Dismiss();
        }

        public void Show()
        {
            this.Visibility = Visibility.Visible;
        }

        public void Dismiss()
        {
            this.Visibility = Visibility.Collapsed;
        }

        public void Clear()
        {
            this.MainGrid.Children.Clear();
        }

        public void GenerateAndRenderUIs(IPlugin plugin)
        {
            this.Clear();
            //root
            foreach (UIControl control in plugin.Controls)
            {
                UIElement element = this.CreateInstance(control.Component);

                this.DecorateElement(element, control);

                this.RecursiveGenerate(element, control);

                this.MainGrid.Children.Add(element);
            }
            //Add Buttons
            var button = new Button();
            button.Content = "クリアー";
            button.Height = 25;
            button.Width = 80;
            button.FontSize = 14;
            button.Margin = new Thickness(0, 0, 190, 10);
            button.HorizontalAlignment = HorizontalAlignment.Right;
            button.VerticalAlignment = VerticalAlignment.Bottom;
            button.Click += Button_Clear;

            this.MainGrid.Children.Add(button);

            button = new Button();
            button.Content = "追加";
            button.Height = 25;
            button.Width = 80;
            button.FontSize = 14;
            button.Margin = new Thickness(0, 0, 100, 10);
            button.HorizontalAlignment = HorizontalAlignment.Right;
            button.VerticalAlignment = VerticalAlignment.Bottom;
            button.Click += Button_Add;
            this.MainGrid.Children.Add(button);

            button = new Button();
            button.Content = "キャンセル";
            button.Height = 25;
            button.Width = 80;
            button.FontSize = 14;
            button.Margin = new Thickness(0, 0, 10, 10);
            button.HorizontalAlignment = HorizontalAlignment.Right;
            button.VerticalAlignment = VerticalAlignment.Bottom;
            button.Click += Button_Cancel;
            this.MainGrid.Children.Add(button);
        }

        private void Button_Add(object sender, RoutedEventArgs e)
        {
            //Name が設定されているコントロールの値のみ取得
            var dic = new Dictionary<string, string>();
            this.RecursiveGet(this.MainGrid.Children, dic);
        }

        //メソッド名思いつかないわ
        private void RecursiveGet(UIElementCollection elementCollection, Dictionary<string, string> dic)
        {
            foreach (UIElement element in elementCollection)
            {
                if (element is TextBox)
                {
                    var textBox = (TextBox)element;
                    if (!String.IsNullOrEmpty(textBox.Name))
                    {
                        dic.Add(textBox.Name, textBox.Text);
                    }
                }
                else if (element is ComboBox)
                {
                    var comboBox = (ComboBox)element;
                    if (!String.IsNullOrEmpty(comboBox.Name))
                    {
                        dic.Add(comboBox.Name, comboBox.SelectedItem.ToString());
                    }
                }
                else if (element is CheckBox)
                {
                    var checkBox = (CheckBox)element;
                    if (!String.IsNullOrEmpty(checkBox.Name))
                    {
                        dic.Add(checkBox.Name, checkBox.IsChecked.ToString());
                    }
                }
                else if (element is Panel)
                {
                    this.RecursiveGet(((Panel)element).Children, dic);
                }
            }
        }

        private void Button_Cancel(object sender, RoutedEventArgs e)
        {
            this.Clear();
        }

        #region [Clear]Button

        private void Button_Clear(object sender, RoutedEventArgs e)
        {
            this.RecursiveClear(this.MainGrid.Children);
        }

        private void RecursiveClear(UIElementCollection elementCollection)
        {
            foreach (UIElement element in elementCollection)
            {
                if (element is TextBox)
                {
                    var textBox = (TextBox)element;
                    textBox.Text = "";
                }
                else if (element is ComboBox)
                {
                    var comboBox = (ComboBox)element;
                    comboBox.SelectedIndex = -1;
                }
                else if (element is CheckBox)
                {
                    var checkBox = (CheckBox)element;
                    checkBox.IsChecked = false;
                }
                else if (element is Panel)
                {
                    this.RecursiveClear(((Panel)element).Children);
                }
            }
        }

        #endregion

        private void RecursiveGenerate(UIElement parentElement, UIControl parentControl)
        {
            foreach (UIControl control in parentControl.Children)
            {
                UIElement element = this.CreateInstance(control.Component);
                this.DecorateElement(element, control);
                this.RecursiveGenerate(element, control);
                if (parentElement is Panel)
                {
                    var panel = (Panel)parentElement;
                    panel.Children.Add(element);
                }
                if (parentElement is GroupBox)
                {
                    var groupbox = (GroupBox)parentElement;
                    groupbox.Content = element;
                }
            }
        }

        private UIElement CreateInstance(GuiComponents component)
        {
            UIElement element;
            Console.WriteLine(component);
            switch (component)
            {
                case GuiComponents.Canvas:
                    element = new Canvas();
                    break;

                case GuiComponents.CheckBox:
                    element = new CheckBox();
                    break;

                case GuiComponents.ComboBox:
                    element = new ComboBox();
                    break;

                case GuiComponents.DockPanel:
                    element = new DockPanel();
                    break;

                case GuiComponents.Grid:
                    element = new Grid();
                    break;

                case GuiComponents.GroupBox:
                    element = new GroupBox();
                    break;

                case GuiComponents.Image:
                    element = new Image();
                    break;

                case GuiComponents.Null:
                    element = new UIElement();
                    break;

                case GuiComponents.StackPanel:
                    element = new StackPanel();
                    break;

                case GuiComponents.TextBlock:
                    element = new TextBlock();
                    break;

                case GuiComponents.TextBox:
                    element = new TextBox();
                    break;

                case GuiComponents.UniformGrid:
                    element = new UniformGrid();
                    break;

                case GuiComponents.WrapPanel:
                    element = new WrapPanel();
                    break;

                case GuiComponents.Separator:
                    element = new Separator();
                    break;

                default:
                    element = new UIElement();
                    break;
            }
            return element;
        }

// ReSharper disable PossibleInvalidOperationException
        private void DecorateElement(UIElement element, UIControl control)
        {
            if (element is TextBlock)
            {
                var uielement = (TextBlock)element;
                var textControl = (TextControl)control;

                if (Math.Abs(textControl.FontSize) > 0.0D) uielement.FontSize = textControl.FontSize;
                if (textControl.FontStretch != FontStretches.Normal) uielement.FontStretch = (FontStretch)textControl.FontStretch;
                if (textControl.FontStyle != FontStyles.Normal) uielement.FontStyle = (FontStyle)textControl.FontStyle;
                if (textControl.FontWeight != FontWeights.Normal) uielement.FontWeight = (FontWeight)textControl.FontWeight;
                if (!String.IsNullOrEmpty(textControl.Text)) uielement.Text = textControl.Text;
                if (textControl.TextAlignment != null) uielement.TextAlignment = (TextAlignment)textControl.TextAlignment;
                if (textControl.TextDecoration != null) uielement.TextDecorations.Add(textControl.TextDecoration);
                if (textControl.TextWrapping != null) uielement.TextWrapping = (TextWrapping)textControl.TextWrapping;

                uielement.Background = textControl.Background;
                uielement.Foreground = textControl.Foreground;
                if (Math.Abs(textControl.Height) > 0.0D) uielement.Height = textControl.Height;
                if (textControl.HorizontalAlignment != null) uielement.HorizontalAlignment = (HorizontalAlignment)textControl.HorizontalAlignment;
                uielement.IsEnabled = (bool)control.IsEnabled;
                //if (control.IsVisible == false) uielement.IsVisible = false; ;; Read-Only
                if (!String.IsNullOrEmpty(textControl.Name)) uielement.Name = textControl.Name;
                if (Math.Abs(textControl.Opacity) > 0.0D) uielement.Opacity = textControl.Opacity;
                if (!String.IsNullOrEmpty(textControl.ToolTip)) uielement.ToolTip = textControl.ToolTip;
                if (textControl.VerticalAlignment != null) uielement.VerticalAlignment = (VerticalAlignment)textControl.VerticalAlignment;
                if (textControl.Visibility != null) uielement.Visibility = (Visibility)textControl.Visibility;
                if (Math.Abs(textControl.Width) > 0.0D) uielement.Width = textControl.Width;
                if (textControl.Margin != null) uielement.Margin = (Thickness)textControl.Margin;

            }
            else if (element is TextBox)
            {
                var uielement = (TextBox)element;
                var textControl = (TextControl)control;

                if (Math.Abs(textControl.FontSize) > 0.0D) uielement.FontSize = textControl.FontSize;
                if (textControl.FontStretch != FontStretches.Normal) uielement.FontStretch = (FontStretch)textControl.FontStretch;
                if (textControl.FontStyle != FontStyles.Normal) uielement.FontStyle = (FontStyle)textControl.FontStyle;
                if (textControl.FontWeight != FontWeights.Normal) uielement.FontWeight = (FontWeight)textControl.FontWeight;
                if (!String.IsNullOrEmpty(textControl.Text)) uielement.Text = textControl.Text;
                if (textControl.TextAlignment != null) uielement.TextAlignment = (TextAlignment)textControl.TextAlignment;
                if (textControl.TextDecoration != null) uielement.TextDecorations.Add(textControl.TextDecoration);
                if (textControl.TextWrapping != null) uielement.TextWrapping = (TextWrapping)textControl.TextWrapping;

                this.Decorate(uielement, textControl);

            }
            else if (element is CheckBox)
            {
                var uielement = (CheckBox)element;
                var textControl = (TextControl)control;

                if (Math.Abs(textControl.FontSize) > 0.0D) uielement.FontSize = textControl.FontSize;
                if (textControl.FontStretch != FontStretches.Normal) uielement.FontStretch = (FontStretch)textControl.FontStretch;
                if (textControl.FontStyle != FontStyles.Normal) uielement.FontStyle = (FontStyle)textControl.FontStyle;
                if (textControl.FontWeight != FontWeights.Normal) uielement.FontWeight = (FontWeight)textControl.FontWeight;
                if (!String.IsNullOrEmpty(textControl.Text)) uielement.Content = textControl.Text;

                this.Decorate(uielement, textControl);

            }
            else if (element is ComboBox)
            {
                var uielement = (ComboBox)element;
                var textControl = (TextControl)control;

                if (Math.Abs(textControl.FontSize) > 0.0D) uielement.FontSize = textControl.FontSize;
                if (textControl.FontStretch != FontStretches.Normal) uielement.FontStretch = (FontStretch)textControl.FontStretch;
                if (textControl.FontStyle != FontStyles.Normal) uielement.FontStyle = (FontStyle)textControl.FontStyle;
                if (textControl.FontWeight != FontWeights.Normal) uielement.FontWeight = (FontWeight)textControl.FontWeight;

                this.Decorate(uielement, textControl);

            }
            else if (element is Image)
            {
                var uielement = (Image)element;
                var imagecontrol = (ImageControl)control;

                if (imagecontrol.Source != null) uielement.Source = imagecontrol.Source;
                if (Math.Abs(imagecontrol.Height) > 0.0D) uielement.Height = imagecontrol.Height;
                if (Math.Abs(imagecontrol.Width) > 0.0D) uielement.Width = imagecontrol.Width;

                if (Math.Abs(imagecontrol.Height) > 0.0D) uielement.Height = imagecontrol.Height;
                if (imagecontrol.HorizontalAlignment != null) uielement.HorizontalAlignment = (HorizontalAlignment)imagecontrol.HorizontalAlignment;
                uielement.IsEnabled = (bool)control.IsEnabled;
                if (!String.IsNullOrEmpty(imagecontrol.Name)) uielement.Name = imagecontrol.Name;
                if (Math.Abs(imagecontrol.Opacity) > 0.0D) uielement.Opacity = imagecontrol.Opacity;
                if (!String.IsNullOrEmpty(imagecontrol.ToolTip)) uielement.ToolTip = imagecontrol.ToolTip;
                if (imagecontrol.VerticalAlignment != null) uielement.VerticalAlignment = (VerticalAlignment)imagecontrol.VerticalAlignment;
                if (imagecontrol.Visibility != null) uielement.Visibility = (Visibility)imagecontrol.Visibility;
                if (Math.Abs(imagecontrol.Width) > 0.0D) uielement.Width = imagecontrol.Width;
                if (imagecontrol.Margin != null) uielement.Margin = (Thickness)imagecontrol.Margin;
            }
            else if (element is GroupBox)
            {
                var uielement = (GroupBox)element;
                var textControl = (TextControl)control;
                if (!String.IsNullOrEmpty(textControl.Text)) uielement.Header = textControl.Text;

                this.Decorate(uielement, textControl);
            }
            else if (element is Control)
            {
                this.Decorate((Control)element, control);
            }
        }

        private void Decorate(Control uielement, UIControl control)
        {
            uielement.Background = control.Background;
            uielement.Foreground = control.Foreground;
            if (Math.Abs(control.Height) > 0.0D) uielement.Height = control.Height;
            if (control.HorizontalAlignment != null) uielement.HorizontalAlignment = (HorizontalAlignment)control.HorizontalAlignment;
            uielement.IsEnabled = (bool) control.IsEnabled;
            //if (control.IsVisible == false) uielement.IsVisible = false; ;; Read-Only
            if (!String.IsNullOrEmpty(control.Name)) uielement.Name = control.Name;
            if (Math.Abs(control.Opacity) > 0.0D) uielement.Opacity = control.Opacity;
            if (!String.IsNullOrEmpty(control.ToolTip)) uielement.ToolTip = control.ToolTip;
            if (control.VerticalAlignment != null) uielement.VerticalAlignment = (VerticalAlignment)control.VerticalAlignment;
            if (control.Visibility != null) uielement.Visibility = (Visibility)control.Visibility;
            if (Math.Abs(control.Width) > 0.0D) uielement.Width = control.Width;
            if (control.Margin != null) uielement.Margin = (Thickness)control.Margin;
        }
// ReSharper restore PossibleInvalidOperationException
    }
}
