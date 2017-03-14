using Common.Config;
using Common.Logic.Domain;
using Formula;
using Formula.Search;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Web.Manage
{
    public partial class BusinessUnitEdit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string id = Request.Params["id"];
            string data = Request.Form["data"];
            string state = Request.Params["state"];

            string method = Request.Params["Method"];
            switch (method)
            {
                case "GetInfo":
                    GetInfo(Microsoft.JScript.GlobalObject.decodeURI(id));
                    break;
                case "ModifyInfo":
                    ModifyInfo(data, state);
                    break;
                case "AddInfo":
                    AddInfo(data, state);
                    break;
                case "DeleteInfo":
                    DeleteInfo(id);
                    break;
                case "CancelPublish":
                    CancelPublish(id);
                    break;
            }
        }

        private void GetInfo(string ItemID)
        {
            var entities = FormulaHelper.GetEntities<CommonEntities>();
            try
            {
                //var ety = entities.Set<BusinessUnit>().Where(c => c.IsDelete == false && c.ID == ItemID).FirstOrDefault();
                //Response.Write(PluSoft.Utils.JSON.Encode(ety));
            }
            catch (Exception ex)
            {
                Response.Write(Microsoft.JScript.GlobalObject.encodeURI(ex.Message));
            }
            finally
            {
                Response.End();
            }
        }

        private void CancelPublish(string ItemIDs)
        {
            var entities = FormulaHelper.GetEntities<CommonEntities>();
            try
            {
                //var ids = ItemIDs.Split(',');//删除这些商品的所有历史版本
                //var items = entities.Set<BusinessUnit>().Where(c => ids.Contains(c.ID)).ToList();
                //foreach (var item in items)
                //{
                //    item.IsPublish = false;
                //    item.PublishTime = null;
                //}
                //entities.SaveChanges();
            }
            catch (Exception ex)
            {
                Response.Write(Microsoft.JScript.GlobalObject.encodeURI(ex.Message));
            }
            finally
            {
                Response.End();
            }
        }

        private void DeleteInfo(string ItemIDs)
        {
            var entities = FormulaHelper.GetEntities<CommonEntities>();
            try
            {
                //var ids = ItemIDs.Split(',');//删除这些商品的所有历史版本
                //var items = entities.Set<BusinessUnit>().Where(c => ids.Contains(c.ID)).ToList();
                //foreach (var item in items)
                //{
                //    item.IsDelete = true;
                //    item.DeleteTime = DateTime.Now;
                //}
                //entities.SaveChanges();
            }
            catch (Exception ex)
            {
                Response.Write(Microsoft.JScript.GlobalObject.encodeURI(ex.Message));
            }
            finally
            {
                Response.End();
            }
        }

        private void ModifyInfo(string data, string state)
        {
            var entities = FormulaHelper.GetEntities<CommonEntities>();
            try
            {
                //var infos = PluSoft.Utils.JSON.Decode(data, typeof(BusinessUnit[])) as BusinessUnit[];
                //var info = infos[0];
                //var iteminfo = entities.Set<BusinessUnit>().Where(c => c.ID == info.ID).FirstOrDefault();
                //if (iteminfo == null)
                //    throw new Exception(string.Format("找不到ID为{0}的信息", info.ID));
                ////商品基本信息
                //FormulaHelper.UpdateModel(iteminfo, info);
                //iteminfo.UpdateTime = DateTime.Now;
                //if (state == "publish")
                //{
                //    iteminfo.PublishTime = DateTime.Now;
                //    iteminfo.IsPublish = true;
                //}
                //entities.SaveChanges();
            }
            catch (Exception ex)
            {
                Response.Write(Microsoft.JScript.GlobalObject.encodeURI(ex.Message));
            }
            finally
            {
                Response.End();
            }
        }

        private void AddInfo(string data, string state)
        {
            var entities = FormulaHelper.GetEntities<CommonEntities>();
            try
            {
                //var infos = PluSoft.Utils.JSON.Decode(data, typeof(BusinessUnit[])) as BusinessUnit[];
                //var info = infos[0];
                //var iteminfo = new BusinessUnit();
                ////商品基本信息
                //FormulaHelper.UpdateModel(iteminfo, info);
                //iteminfo.ID = FormulaHelper.CreateGuid();
                //iteminfo.CreateTime = DateTime.Now;
                //iteminfo.IsDelete = false;
                //if (state == "publish")
                //{
                //    iteminfo.PublishTime = DateTime.Now;
                //    iteminfo.IsPublish = true;
                //}
                //if (iteminfo.IsPublish == null)
                //    iteminfo.IsPublish = false;
                //entities.Set<BusinessUnit>().Add(iteminfo);
                //entities.SaveChanges();
            }
            catch (Exception ex)
            {
                Response.Write(Microsoft.JScript.GlobalObject.encodeURI(ex.Message));
            }
            finally
            {
                Response.End();
            }
        }

        private string ReplaceImgWidth(string content)
        {
            content = content.ToLower();
            string valEx = "<img (.*) /> ";
            string styleEx = "style='(.*)'|style=\"(.*)\"";
            string widthEx = "";
            var imgs = Regex.Matches(content, valEx);
            foreach (Match img in imgs)
            {
                var imgstr = img.Value;
                if (!Regex.Match(imgstr, styleEx).Success)
                {
                    var resultStr = imgstr.Replace("<img", "<img style='width:100%;'");
                    content.Replace(imgstr, resultStr);
                }
                else
                {
                }
            }
            return content;
        }
    }
}