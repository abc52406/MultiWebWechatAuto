using Common.Config;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace Web.Upload
{
    public struct ImgStruct
    {
        public string imgpath, thumbspath;
    }
    /// <summary>
    /// UploadImg 的摘要说明
    /// </summary>
    public class UploadImg : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            // 获得程序路径
            //TempFilePath
            string visualpath = SysConfig.FileVisualPath;
            string localpath = context.Server.MapPath(visualpath);
            try
            {
                var subdir = DateTime.Now.ToString("yyyyMMdd");
                DirectoryInfo localbigdi = new DirectoryInfo(string.Format("{0}Pic\\{1}\\", localpath, subdir));
                if (!localbigdi.Exists)
                    localbigdi.Create();

                //找到目标文件对象
                HttpPostedFile uploadFile = context.Request.Files[context.Request.Params["MiniName"]];
                var imgid = Guid.NewGuid();
                string fullName = string.Format("{0}Pic\\{1}\\{2}{3}", localpath, subdir, imgid, Path.GetExtension(uploadFile.FileName));
                string fullwebpath = string.Format("{0}Pic/{1}/{2}{3}", visualpath, subdir, imgid, Path.GetExtension(uploadFile.FileName));
                
                // 如果有文件, 则保存到一个地址
                if (uploadFile.ContentLength > 0)
                {
                    uploadFile.SaveAs(fullName);

                    //转为jpg格式的文件
                    if (Path.GetExtension(fullName).ToLower() != ".jpg")
                    {
                        imgid = Guid.NewGuid();
                        Bitmap bit = new Bitmap(fullName);
                        string oldname = fullName;
                        fullName = string.Format("{0}Pic\\{1}\\{2}.jpg", localpath, subdir, imgid);
                        fullwebpath = string.Format("{0}Pic/{1}/{2}.jpg", visualpath, subdir, imgid);
                        bit.Save(fullName, ImageFormat.Jpeg);
                        bit.Dispose();
                        File.Delete(oldname);
                    }
                }
                context.Response.Write(new JavaScriptSerializer().Serialize(new ImgStruct()
                {
                    imgpath = fullwebpath,
                }));
            }
            catch
            {
                context.Response.Write("");
            }
            finally
            {
                context.Response.End();
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}