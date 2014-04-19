using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using McMDK.Data.Texture;

namespace ExperimentFunction
{
    public class NormalBlockTexManager : TextureManager
    {
        /// <summary>
        /// Generate Normal Block Texture
        /// </summary>
        /// <param name="texture"></param>
        public static override void CreateTexture(Texture texture, string output)
        {
            if(texture.Items.Length != 3)
            {
                throw new ArgumentException("texture.Items.Length");
            }

            Bitmap bitmap = new Bitmap(32, 32);
            Graphics g = Graphics.FromImage(bitmap);

            foreach(TextureItem item in texture.Items)
            {
                float[][] matrix;
                Point[] point;

                if (item.RenderingSide == 0)
                {
                    point = new Point[] { 
                        new Point(2, 7), 
                        new Point(16, 24), 
                        new Point(2, 25) 
                    };

                    matrix = new float[][] { 
                        new float[] {1, 0, 0, 0, 0},
                        new float[] {0, 1, 0, 0, 0},
                        new float[] {0, 0, 1, 0, 0},
                        new float[] {0, 0, 0, 1, 0},
                        new float[] {-30/255, -30/255, -30/255, 0, 1}
                    };

                    ColorMatrix colorMatrix = new ColorMatrix(matrix);
                    ImageAttributes attr = new ImageAttributes();
                    attr.SetColorMatrix(colorMatrix);

                    Image img = Image.FromFile(item.File);
                    g.DrawImage(img, point, new Rectangle(0, 0, 16, 16), GraphicsUnit.Pixel, attr);
                }
                if(item.RenderingSide == 1)
                {
                    point = new Point[] {
                        new Point(16, 14),
                        new Point(30, 7),
                        new Point(16, 31)
                    };

                    matrix = new float[][] { 
                        new float[] {1, 0, 0, 0, 0},
                        new float[] {0, 1, 0, 0, 0},
                        new float[] {0, 0, 1, 0, 0},
                        new float[] {0, 0, 0, 1, 0},
                        new float[] {-50/255, -50/255, -50/255, 0, 1}
                    };

                    ColorMatrix colorMatrix = new ColorMatrix(matrix);
                    ImageAttributes attr = new ImageAttributes();
                    attr.SetColorMatrix(colorMatrix);

                    Image img = Image.FromFile(item.File);
                    g.DrawImage(img, point, new Rectangle(0, 0, 16, 16), GraphicsUnit.Pixel, attr);
                }
                if(item.RenderingSide == 2)
                {
                    point = new Point[] {
                        new Point(15, 0),
                        new Point(30, 8),
                        new Point(1, 8)
                    };

                    Image img = Image.FromFile(item.File);
                    g.DrawImage(img, point);
                }
            }
            bitmap.Save(output, ImageFormat.Png);
            g.Dispose();
            bitmap.Dispose();
        }
    }
}
