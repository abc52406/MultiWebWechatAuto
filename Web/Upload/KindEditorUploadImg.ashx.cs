using Common.Config;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;

namespace Web.Upload
{
    /// <summary>
    /// KindEditorUploadImg 的摘要说明
    /// </summary>
    public class KindEditorUploadImg : IHttpHandler
    {
        private HttpContext context;

        public void ProcessRequest(HttpContext context)
        {
            //最大文件大小
            int maxSize = 10 * 1024 * 1024;
            this.context = context;

            HttpPostedFile uploadFile = context.Request.Files["imgFile"];
            if (uploadFile == null)
            {
                showError("请选择文件。");
            }

            String fileName = uploadFile.FileName;
            String fileExt = Path.GetExtension(fileName).ToLower();

            if (uploadFile.InputStream == null || uploadFile.InputStream.Length > maxSize)
            {
                showError("上传文件大小不能超过10M。");
            }

            if (String.IsNullOrEmpty(fileExt) || !"gif,jpg,jpeg,png,bmp".Split(',').Contains(fileExt.Substring(1)))
            {
                showError("上传文件扩展名是不允许的扩展名。\n只允许gif,jpg,jpeg,png,bmp格式。");
            }

            string visualpath = SysConfig.FileVisualPath;
            string localpath = context.Server.MapPath(visualpath);
            var imgid = Guid.NewGuid();
            var subdir = DateTime.Now.ToString("yyyyMMdd");
            DirectoryInfo localbigdi = new DirectoryInfo(string.Format("{0}Pic\\{1}\\", localpath, subdir));
            if (!localbigdi.Exists)
                localbigdi.Create();
            string fullName = string.Format("{0}Pic\\{1}\\{2}{3}", localpath, subdir, imgid, fileExt);
            string fullwebpath = string.Format("{0}Pic/{1}/{2}{3}", visualpath, subdir, imgid, fileExt);
            try
            {
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
                Hashtable hash = new Hashtable();
                hash["error"] = 0;
                hash["url"] = fullwebpath;
                hash["width"] = "100%";
                context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
                context.Response.Write(PluSoft.Utils.JSON.Encode(hash));
            }
            catch (Exception ex)
            {
                showError(ex.Message);
            }
            finally
            {
                context.Response.End();
            }
        }

        private void showError(string message)
        {
            Hashtable hash = new Hashtable();
            hash["error"] = 1;
            hash["message"] = message;
            context.Response.AddHeader("Content-Type", "text/html; charset=UTF-8");
            context.Response.Write(PluSoft.Utils.JSON.Encode(hash));
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