using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Windows;
using System.Windows.Controls;

using McMDK.Utils;
using McMDK.Utils.Log;

using McMDK.Plugin.UI;
using McMDK.Plugin.UI.Controls;

namespace McMDK.Plugin
{
    public class PluginLoader
    {
        private static List<IPlugin> Plugins = new List<IPlugin>();

        public static void Load()
        {
            Plugins.Clear();
            //Load plugins
            string[] plugins = FileController.LoadDirectory(Define.PluginDirectory, true);
            foreach (string plugin in plugins)
            {
                Define.GetLogger().Info("Loading Plugin from " + plugin);
                if (plugin.EndsWith("template"))
                {
                    Define.GetLogger().Fine("Skip loading plugin of " + plugin);
                    continue;
                }
                if (!FileController.Exists(plugin + "\\plugin.xml"))
                {
                    //Not exist
                    continue;
                }
                //Load root
                Define.GetLogger().Info("Loading plugin settings...");
                var a = from b in XElement.Load(plugin + "\\plugin.xml").Elements()
                        select new XmlPluginBase
                        {
                            Name = b.Element("Name").Value,
                            Id = b.Element("PluginID").Value,
                            Author = b.Element("Author").Value,
                            Version = b.Element("Version").Value,
                            Dependents = b.Element("Dependents").Value,
                            IconPath = b.Element("IconPath").Value,
                            Description = b.Element("Description").Value
                        };
                XmlPluginBase p = null;

                foreach (var item in a)
                {
                    p = item;
                }
                Define.GetLogger().Info("Loaded plugin settings.");

                p.Logger = new Logger(p.Name);
                p.Logger.SetParent(Define.GetLogger());
                p.Logger.Info("Initializing...");
                p.Loaded();
                p.Logger.Info("Initialized.");

                p.Logger.Info("Loading UI Settings...");
                SerializeXML(p, plugin);
                p.Logger.Info("Loaded UI Settings.");

                //Load Builder 
                p.Logger.Info("Loading Builder Settings...");
                //
                p.Logger.Info("Loaded Builder Settings.");

                Define.GetLogger().Info(p.Name + " is loaded.");

                Plugins.Add(p);
            }
        }

        public static List<IPlugin> GetPlugins()
        {
            return Plugins;
        }

        private static void SerializeXML(IPlugin plugin, string dir)
        {
            if (!FileController.Exists(dir + "\\ui.xml"))
            {
                return;
            }

            try
            {
                XmlDocument document = new XmlDocument();
                document.Load(dir + "\\ui.xml");

                XmlNode root = document.DocumentElement;
                XmlNode node = root.ChildNodes[0];

                UIControl control = new UIControl();
                control.Component = (GuiComponents)Enum.Parse(typeof(GuiComponents), ((XmlElement)node).Name);

                //再帰的処理
                RecursiveSerializeXML(node, control, dir);

                plugin.Controls.Add(control);
            }
            catch (Exception e)
            {
                plugin.Logger.Error("Load failed.", e);
            }
        }

        private static void RecursiveSerializeXML(XmlNode parentNode, UIControl parentControl, string dir)
        {
            foreach (XmlNode node in parentNode.ChildNodes)
            {
                if (node is XmlComment)
                {
                    continue;
                }
                UIControl control = new UIControl();

                switch (node.Name)
                {
                    case "TextBlock":
                    case "TextBox":
                    case "CheckBox":
                    case "ComboBox":
                    case "GroupBox":
                        var textcontrol = new TextControl();
                        textcontrol.FontSize = StringToObjectConverter.StringTo<double>(((XmlElement)node).GetAttribute("FontSize"));
                        textcontrol.FontStretch = (FontStretch)StringToObjectConverter.StringToProperty(((XmlElement)node).GetAttribute("FontStretch"), typeof(FontStretches), FontStretches.Normal);
                        textcontrol.FontStyle = (FontStyle)StringToObjectConverter.StringToProperty(((XmlElement)node).GetAttribute("FontStyle"), typeof(FontStyles), FontStyles.Normal);
                        textcontrol.FontWeight = (FontWeight)StringToObjectConverter.StringToProperty(((XmlElement)node).GetAttribute("FontWeight"), typeof(FontWeights), FontWeights.Normal);
                        textcontrol.Text = (((XmlElement)node).GetAttribute("Text"));
                        textcontrol.TextAlignment = (TextAlignment?)StringToObjectConverter.StringToEnum(((XmlElement)node).GetAttribute("TextAlignment"), typeof(TextAlignment));
                        textcontrol.TextDecoration = (TextDecoration)StringToObjectConverter.StringToProperty(((XmlElement)node).GetAttribute("TextDecoration"), typeof(TextDecorations), null);
                        textcontrol.TextWrapping = (TextWrapping?)StringToObjectConverter.StringToEnum(((XmlElement)node).GetAttribute("TextWrapping"), typeof(TextWrapping));
                        control = textcontrol;
                        break;

                    case "Image":
                        var imagecontrol = new ImageControl();
                        imagecontrol.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(dir + ((XmlElement)node).GetAttribute("ImageSource")));
                        control = imagecontrol;
                        break;

                    default:
                        control = new UIControl();
                        break;
                }
                control.Background = StringToObjectConverter.StringToBrush(((XmlElement)node).GetAttribute("Background"), new System.Windows.Media.SolidColorBrush(new System.Windows.Media.Color() { A = (byte)0xFF, R = (byte)0xFF, G = (byte)0xFF, B = (byte)0xFF }));
                control.Component = StringToObjectConverter.StringToComponents(((XmlElement)node).Name);
                control.Foreground = StringToObjectConverter.StringToBrush(((XmlElement)node).GetAttribute("Foreground"), new System.Windows.Media.SolidColorBrush(new System.Windows.Media.Color() { A = (byte)0xFF, R = (byte)0x00, G = (byte)0x00, B = (byte)0x00 }));
                control.Height = StringToObjectConverter.StringTo<double>(((XmlElement)node).GetAttribute("Height"));
                control.HorizontalAlignment = (HorizontalAlignment?)StringToObjectConverter.StringToEnum(((XmlElement)node).GetAttribute("HorizontalAlignment"), typeof(HorizontalAlignment));
                control.IsEnabled = StringToObjectConverter.StringTo<bool>(((XmlElement)node).GetAttribute("IsEnabled"), true);
                control.IsVisible = StringToObjectConverter.StringTo<bool>(((XmlElement)node).GetAttribute("IsVisible"));
                control.Margin = ParseMargin(((XmlElement)node).GetAttribute("Margin"));
                control.Name = (((XmlElement)node).GetAttribute("Name"));
                control.Opacity = StringToObjectConverter.StringTo<double>(((XmlElement)node).GetAttribute("Opacity"));
                control.ToolTip = (((XmlElement)node).GetAttribute("ToolTip"));
                control.VerticalAlignment = (VerticalAlignment?)StringToObjectConverter.StringToEnum(((XmlElement)node).GetAttribute("VerticalAlignment"), typeof(VerticalAlignment));
                control.Visibility = (Visibility?)StringToObjectConverter.StringToEnum(((XmlElement)node).GetAttribute("Visibility"), typeof(Visibility));
                control.Width = StringToObjectConverter.StringTo<double>(((XmlElement)node).GetAttribute("Width"));
                parentControl.Children.Add(control);
                RecursiveSerializeXML(node, control, dir);
            }
        }

        private static Thickness? ParseMargin(string margin)
        {
            if (String.IsNullOrEmpty(margin))
            {
                return null;
            }
            string[] s = margin.Split(',');
            if (s.Length == 1)
            {
                return new Thickness(int.Parse(s[0]));
            }
            else if (s.Length == 4)
            {
                return new Thickness(int.Parse(s[0]), int.Parse(s[1]), int.Parse(s[2]), int.Parse(s[3]));
            }
            return null;
        }
    }
}