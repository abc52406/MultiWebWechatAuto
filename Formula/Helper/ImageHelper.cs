using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formula.Helper
{
    /// <summary>
    /// 图片帮助类
    /// </summary>
    public static class ImageHelper
    {
        #region 图片转字节
        /// <summary>
        /// 图片转字节
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static byte[] GetBitmapBytes(Bitmap bmp)
        {
            if (bmp == null) return null;
            try
            {
                System.IO.MemoryStream ms = new MemoryStream();
                bmp.Save(ms, ImageFormat.Bmp);
                byte[] bs = ms.ToArray();
                ms.Close();
                return bs;
            }
            catch { return null; }
        }
        #endregion
    }
}
