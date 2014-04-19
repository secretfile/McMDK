using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using McMDK.Data.Texture;
using McMDK.Utils;

namespace ExperimentFunction
{
    abstract class TextureManager
    {
        public static List<string> CreateTexture(List<TextureItem> items)
        {
            Bitmap bitmap = new Bitmap(256, 256);
            Graphics g = Graphics.FromImage(bitmap);
            List<string> textures = new List<string>();

            int x, y, j, k;
            x = y = j = k = 0;

            for (int i = 0; i < items.Count; i++)
            {
                if (x > 240)
                {
                    x = 0; y += 16;
                }
                if(y > 240)
                {
                    y = 0;
                }
                if(i > 255 && i % 256 == 0)
                {
                    if(FileController.Exists(""))
                    {
                        FileController.Delete("");
                    }
                    bitmap.Save("", ImageFormat.Png);
                    textures.Add("");

                    bitmap = new Bitmap(256, 256);
                    g = Graphics.FromImage(bitmap);
                    j++;
                    y = 0;
                }

                if(FileController.Exists(""))
                {
                    Image img = Image.FromFile("");
                    g.DrawImage(img, x, y);
                    img.Dispose();
                }
                else
                {
                    Image img = Image.FromFile(Define.AssetsDirectory + "\\mcmdk\\dummy.png");
                    g.DrawImage(img, x, y);
                    img.Dispose();
                }
                x += 16;
                k++;
            }
            if (k != 0)
            {
                if(FileController.Exists(""))
                {
                    FileController.Delete("");
                }
                bitmap.Save("", ImageFormat.Png);
                textures.Add("");
            }

            return textures;
        }

        public abstract static void CreateTexture(Texture texture);
    }
}
