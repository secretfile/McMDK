using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace McMDK.Data.Texture
{
    public class TextureItem
    {
        public string File;

        /// <summary>
        /// RenderTypes <para />
        /// 0  : Top Texture <para />
        /// 1  : Side(Left) Texture <para />
        /// 2  : Side(Right) Texture <para />
        /// -2 : Item Rendering(TextureType.Block only) 
        /// </summary>
        public int RenderingSide;

        public RenderingType RenderingType;
    }
}
