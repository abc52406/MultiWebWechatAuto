using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Common.Logic.Domain;
using Common.Logic.Service;
using Formula;
using Formula.Helper;
using Formula.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Senparc.Weixin.MP;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.Media;
using System.Data.Entity.Validation;

namespace AutoForm
{
    public partial class AutoForm : Form
    {
        #region 私有变量
        /// <summary>
        /// 配置文件
        /// </summary>
        private XmlHelper docoper;
        /// <summary>
        /// 检查是否回复粉丝的时间间隔
        /// </summary>
        private int CheckReplyTime;
        /// <summary>
        /// 检查粉丝登录状态的时间间隔
        /// </summary>
        private int CheckLoginTime;
        /// <summary>
        /// 检查好友是否拉黑的时间间隔
        /// </summary>
        private int CheckFriendsTime;
        /// <summary>
        /// 检查好友是否拉黑的并发数量
        /// </summary>
        private int CheckFriendsCount;
        /// <summary>
        /// 检查好友是否拉黑的讨论组人数
        /// </summary>
        private int CheckFriendsMax;
        /// <summary>
        /// 检查好友是否拉黑的讨论组时间间隔
        /// </summary>
        private int CheckFriendsSingleTime;
        /// <summary>
        /// 更新winform信息时间间隔
        /// </summary>
        private int UpdateStatusTime;
        /// <summary>
        /// 配置文件更新时间间隔
        /// </summary>
        private int ConfigUpdateTime = 60000;
        /// <summary>
        /// 微信交互的accesstoken
        /// </summary>
        private string accesstoken;
        /// <summary>
        /// 接口错误次数上限
        /// </summary>
        private int InterfaceMaxError;
        private WebClient wc = new WebClient();
        /// <summary>
        /// 粉丝缓存数据
        /// </summary>
        private List<FansData> fansQueue = new List<FansData>();
        /// <summary>
        /// 等待队列粉丝缓存数据
        /// </summary>
        private Queue<WaitFansData> waitFansQueue = new Queue<WaitFansData>();
        /// <summary>
        /// 等待提示语
        /// </summary>
        private string WartMessage = "您前方还有{0}人排队使用该功能，请稍等一会儿再";
        /// <summary>
        /// 登录图片发送提示语
        /// </summary>
        private string LoginMessage = "";
        /// <summary>
        /// 登录超时提示语
        /// </summary>
        private string LoginOutTimeMessage = "";
        /// <summary>
        /// 开始检查好友提示语
        /// </summary>
        private string CheckStartMessage = "";
        /// <summary>
        /// 0删除检查结果提示语
        /// </summary>
        private string CheckGoodMessage = "";
        /// <summary>
        /// 有删除检查结果提示语
        /// </summary>
        private string CheckBadMessage = "";
        /// <summary>
        /// 网页版微信接口错误上限提示语
        /// </summary>
        private string WebMaxErrorMsg = "";
        /// <summary>
        /// 二维码上传本地路径
        /// </summary>
        private string QrPhysicsPath = string.Format("{0}\\QrImg\\", Application.StartupPath);
        /// <summary>
        /// 登录异常微信返回错误编码
        /// </summary>
        private string LogoutErrorCode = System.Configuration.ConfigurationManager.AppSettings["LogoutErrorCode"];
        /// <summary>
        /// 登录异常微信返回错误编码
        /// </summary>
        private string LoginErrorTimeMessage = System.Configuration.ConfigurationManager.AppSettings["LoginErrorTimeMessage"];
        /// <summary>
        /// 登录异常微信返回错误js
        /// </summary>
        private string LogoutErrorJS = System.Configuration.ConfigurationManager.AppSettings["LogoutErrorJS"];
        private string[] SpecialUsers = new string[]{"newsapp", "fmessage", "filehelper", "weibo", "qqmail", "tmessage", "qmessage", "qqsync", "floatbottle", "lbsapp", "shakeapp", "medianote", "qqfriend", "readerapp", "blogapp", "facebookapp", "masssendapp",
                    "meishiapp", "feedsapp", "voip", "blogappweixin", "weixin", "brandsessionholder", "weixinreminder", "wxid_novlwrv3lqwv11", "gh_22b87fa7cb3c", "officialaccounts", "notification_messages", "wxitil", "userexperience_alarm"};
        private object queuelock = new object();
        #endregion

        #region 事件
        public AutoForm()
        {
            InitializeComponent();
        }

        private void AutoForm_Load(object sender, EventArgs e)
        {
            Log4NetConfig.Configure();
            UpdateAccessToken();
            int.TryParse(System.Configuration.ConfigurationManager.AppSettings["ConfigUpdateTime"], out ConfigUpdateTime);
            Thread th1 = new Thread(CheckConfig);
            th1.Start();
            Thread th2 = new Thread(CheckReply);
            th2.Start();
            Thread th3 = new Thread(CheckLogin);
            th3.Start();
            Thread th4 = new Thread(CheckFriends);
            th4.Start();
            Thread th5 = new Thread(UpdateShowStatus);
            th5.Start();
        }
        #endregion

        #region 私有函数
        /// <summary>
        /// 更新配置信息
        /// </summary>
        private void CheckConfig()
        {
            while (true)
            {
                try
                {
                    docoper = new XmlHelper(string.Format("{0}\\config.xml", Application.StartupPath), XmlType.Path);
                    int.TryParse(docoper.QueryEle("/Base/InitConfig/CheckLoginTime").InnerText, out CheckLoginTime);
                    int.TryParse(docoper.QueryEle("/Base/InitConfig/CheckFriendsTime").InnerText, out CheckFriendsTime);
                    int.TryParse(docoper.QueryEle("/Base/InitConfig/CheckFriendsCount").InnerText, out CheckFriendsCount);
                    int.TryParse(docoper.QueryEle("/Base/InitConfig/CheckFriendsMax").InnerText, out CheckFriendsMax);
                    int.TryParse(docoper.QueryEle("/Base/InitConfig/CheckReplyTime").InnerText, out CheckReplyTime);
                    int.TryParse(docoper.QueryEle("/Base/InitConfig/UpdateStatusTime").InnerText, out UpdateStatusTime);
                    int.TryParse(docoper.QueryEle("/Base/InitConfig/CheckFriendsSingleTime").InnerText, out CheckFriendsSingleTime);
                    int.TryParse(docoper.QueryEle("/Base/InitConfig/InterfaceMaxError").InnerText, out InterfaceMaxError);
                    WartMessage = docoper.QueryEle("/Base/InitConfig/WartMessage").InnerText;
                    LoginMessage = docoper.QueryEle("/Base/InitConfig/LoginMessage").InnerText;
                    LoginOutTimeMessage = docoper.QueryEle("/Base/InitConfig/LoginOutTimeMessage").InnerText; 
                    CheckStartMessage = docoper.QueryEle("/Base/InitConfig/CheckStartMessage").InnerText;
                    CheckGoodMessage = docoper.QueryEle("/Base/InitConfig/CheckGoodMessage").InnerText;
                    CheckBadMessage = docoper.QueryEle("/Base/InitConfig/CheckBadMessage").InnerText;
                    Thread.Sleep(ConfigUpdateTime);
                }
                catch (Exception ex)
                {
                    LogWriter.Error(ex);
                }
                finally
                {
                    Thread.Sleep(60000);
                }
            }
        }

        /// <summary>
        /// 检查是否回复用户
        /// </summary>
        private void CheckReply()
        {
            var entities = FormulaHelper.GetEntities<CommonEntities>();
            while (true)
            {
                try
                {
                    var ety = entities.Set<FansLogin>().Where(c => c.IsReply == false).OrderBy(c => c.ApplyDate).FirstOrDefault();
                    if (ety != null)
                    {
                        if (fansQueue.Count > CheckFriendsCount)
                        {
                            var waitfans = waitFansQueue.Where(c => c.OpenID == ety.OpenID).FirstOrDefault();
                            if (waitfans == null)
                            {
                                waitfans = new WaitFansData()
                                {
                                    OpenID = ety.OpenID,
                                    LoginID = ety.ID,
                                    ApplyTime = ety.ApplyDate.Value,
                                    IsReply = false,
                                };
                                waitFansQueue.Enqueue(waitfans);
                            }
                            if (waitfans.IsReply == false)
                            {
                                #region 发送微信消息告知用户等待
                                try
                                {
                                    CustomApi.SendText(accesstoken, ety.OpenID, string.Format(WartMessage, fansQueue.Count - CheckFriendsCount));
                                    waitfans.IsReply = true;
                                }
                                catch (Exception ex)
                                {
                                    LogWriter.Error(ex, string.Format("openid为{0}的用户第一次发送等待回复信息出错", ety.OpenID));
                                    UpdateAccessToken();
                                    try
                                    {
                                        CustomApi.SendText(accesstoken, ety.OpenID, string.Format(WartMessage, fansQueue.Count - CheckFriendsCount));
                                    }
                                    catch (Exception ex2)
                                    {
                                        LogWriter.Error(ex2, string.Format("openid为{0}的用户第二次发送等待回复信息出错", ety.OpenID));
                                        UpdateNewAccessToken();
                                        try
                                        {
                                            CustomApi.SendText(accesstoken, ety.OpenID, string.Format(WartMessage, fansQueue.Count - CheckFriendsCount));
                                        }
                                        catch (Exception ex3)
                                        {
                                            LogWriter.Error(ex3, string.Format("openid为{0}的用户第三次发送等待回复信息出错", ety.OpenID));
                                        }
                                    }
                                }
                                #endregion
                            }
                        }
                        else
                        {
                            lock (queuelock)
                            {
                                if (!fansQueue.Any(c => c.OpenID == ety.OpenID))
                                {
                                    fansQueue.Add(new FansData()
                                    {
                                        LoginID = ety.ID,
                                        OpenID = ety.OpenID,
                                        ApplyTime = ety.ApplyDate.Value,
                                        State = 0,
                                        Cookies = new CookieContainer(),
                                    });

                                    LogWriter.Info(string.Format("sqladd {0}", JsonConvert.SerializeObject(new
                                    {
                                        LoginID = ety.ID,
                                        OpenID = ety.OpenID,
                                        ApplyTime = ety.ApplyDate.Value,
                                        State = 0,
                                    })));
                                }
                            }
                        }
                        #region 更新数据库状态
                        try
                        {
                            ety.IsReply = true;
                            ety.ReplyTime = DateTime.Now;
                            entities.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            LogWriter.Error(ex, string.Format("openid为{0}的用户更新回复状态出错"));
                        }
                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    LogWriter.Error(ex);
                }
                finally
                {
                    Thread.Sleep(CheckReplyTime);
                }
            }
        }

        /// <summary>
        /// 检查用户登录
        /// </summary>
        private void CheckLogin()
        {
            var entities = FormulaHelper.GetEntities<CommonEntities>();
            while (true)
            {
                try
                {
                    #region 发送登录二维码给粉丝
                    foreach (var item in fansQueue.Where(c => c.State == 0))
                    {
                        try
                        {
                            #region 获取图片并保存到本地
                            CookieContainer cc = item.Cookies;
                            string sessionid = item.SessionID;
                            var img = WxService.GetQRCode(ref cc, ref sessionid);
                            item.Cookies = cc;
                            item.SessionID = sessionid;
                            string filename = string.Format("{0}.jpg", DateTime.Now.ToString("yyyMMddHHmmss"));
                            img.Save(string.Format("{0}{1}", QrPhysicsPath, filename), ImageFormat.Jpeg);
                            #endregion

                            #region 上传到微信临时素材库
                            UploadTemporaryMediaResult wximgresult = null;
                            try
                            {
                                wximgresult = MediaApi.UploadTemporaryMedia(accesstoken, UploadMediaFileType.image, string.Format("{0}{1}", QrPhysicsPath, filename));
                            }
                            catch (Exception ex)
                            {
                                LogWriter.Error(ex, string.Format("openid为{0}的登录图片第一次上传失败", item.OpenID));
                                UpdateAccessToken();
                                try
                                {
                                    wximgresult = MediaApi.UploadTemporaryMedia(accesstoken, UploadMediaFileType.image, string.Format("{0}{1}", QrPhysicsPath, filename));
                                }
                                catch (Exception ex2)
                                {
                                    LogWriter.Error(ex2, string.Format("openid为{0}的登录图片第二次上传失败", item.OpenID));
                                    UpdateNewAccessToken();
                                    try
                                    {
                                        wximgresult = MediaApi.UploadTemporaryMedia(accesstoken, UploadMediaFileType.image, string.Format("{0}{1}", QrPhysicsPath, filename));
                                    }
                                    catch (Exception ex3)
                                    {
                                        LogWriter.Error(ex3, string.Format("openid为{0}的登录图片第三次上传失败", item.OpenID));
                                        item.WxErrorCount++;
                                        continue;
                                    }
                                }
                            }
                            #endregion

                            #region 发送消息给用户
                            if (!string.IsNullOrEmpty(wximgresult.media_id))
                            {
                                try
                                {
                                    SendWxImg(item.OpenID, wximgresult.media_id);
                                    SendWxMsg(item.OpenID, LoginMessage);
                                }
                                catch
                                {
                                    item.WxErrorCount++;
                                    throw;
                                }
                            }
                            #endregion

                            #region 更新用户状态
                            item.State = 1;
                            item.SendQRTime = DateTime.Now;
                            var ety = entities.Set<FansLogin>().Find(item.LoginID);
                            if (ety != null)
                            {
                                ety.SessionID = sessionid;
                                ety.IsSendQR = true;
                                ety.SendQRTime = DateTime.Now;
                                entities.SaveChanges();
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            LogWriter.Error(ex);
                        }
                    }
                    #endregion

                    #region 检查超时登录登录状态
                    foreach (var item in fansQueue.Where(c => (c.State == 1 || c.State == 2) && c.SendQRTime.AddMinutes(5) <= DateTime.Now))
                    {
                        FinishFans(item, LoginOutTimeMessage);
                    }
                    lock (queuelock)
                    {
                        var count = fansQueue.RemoveAll(c => c.State == 5);
                        while (fansQueue.Count < CheckFriendsMax && waitFansQueue.Any())
                        {
                            var item = waitFansQueue.Peek();
                            fansQueue.Add(new FansData()
                            {
                                LoginID = item.LoginID,
                                OpenID = item.OpenID,
                                ApplyTime = item.ApplyTime,
                                State = 0,
                                Cookies = new CookieContainer(),
                            });

                            LogWriter.Info(string.Format("waitadd {0}", JsonConvert.SerializeObject(new
                            {
                                LoginID = item.LoginID,
                                OpenID = item.OpenID,
                                ApplyTime = item.ApplyTime,
                                State = 0,
                            })));
                            waitFansQueue.Dequeue();
                        }
                    }
                    #endregion

                    #region 检查登录状态
                    foreach (var item in fansQueue.Where(c => c.State == 1 || c.State == 2))
                    {
                        CookieContainer cc = item.Cookies;
                        string passticket = item.PassTicket;
                        string skey = item.SKey;
                        var login_result = WxService.LoginCheck(ref cc, item.SessionID);
                        if (login_result is Image)
                        {
                            item.State = 2;
                        }
                        else if (login_result is string)
                        {
                            WxService.GetSidUid(ref cc, login_result as string, ref passticket, ref skey);
                            item.State = 3;
                            item.LoginTime = DateTime.Now;
                            #region 更新用户状态
                            var ety = entities.Set<FansLogin>().Find(item.LoginID);
                            if (ety != null)
                            {
                                ety.PassTicket = passticket;
                                ety.Skey = skey;
                                ety.IsLogin = true;
                                ety.LoginTime = DateTime.Now;
                                entities.SaveChanges();
                            }
                            #endregion
                        }
                        item.Cookies = cc;
                        item.PassTicket = passticket;
                        item.SKey = skey;
                    }
                    foreach (var item in fansQueue.Where(c => new int[] { 1, 2, 3 }.Contains(c.State) && c.WxErrorCount > InterfaceMaxError))
                    {
                        FinishFans(item, "");
                    }
                    #endregion
                }
                catch (Exception ex)
                {
                    LogWriter.Error(ex);
                }
                finally
                {
                    Thread.Sleep(CheckLoginTime);
                }
            }
        }

        /// <summary>
        /// 检查好友
        /// </summary>
        private void CheckFriends()
        {
            var entities = FormulaHelper.GetEntities<CommonEntities>();
            while (true)
            {
                try
                {
                    if (fansQueue.Count > 0)
                    {
                        #region 加载联系人信息并发送开始提示信息
                        foreach (var item in fansQueue.Where(c => c.State == 3))
                        {
                            try
                            {
                                #region 加载并保存用户信息
                                CookieContainer cc = item.Cookies;
                                IDictionary<string, string> synckey = new Dictionary<string, string>();
                                JObject init_result = WxService.WxInit(ref cc, item.PassTicket, ref synckey);
                                item.SyncKey = synckey;
                                if (init_result == null)
                                    continue;
                                else if (init_result["BaseResponse"]["Ret"].ToString() == LogoutErrorCode)
                                {
                                    FinishFans(item, LoginErrorTimeMessage);
                                }
                                else
                                {
                                    var fansinfo = entities.Set<FansInfo>().Where(c => c.OpenID == item.OpenID).FirstOrDefault();
                                    if (fansinfo == null)
                                    {
                                        fansinfo = new FansInfo();
                                        fansinfo.ID = FormulaHelper.CreateGuid();
                                        fansinfo.OpenID = item.OpenID;
                                        fansinfo.NickName = init_result["User"]["NickName"].ToString();
                                        fansinfo.HeadImgUrl = init_result["User"]["HeadImgUrl"].ToString();
                                        fansinfo.RemarkName = init_result["User"]["RemarkName"].ToString();
                                        fansinfo.Sex = int.Parse(init_result["User"]["Sex"].ToString());
                                        fansinfo.Signature = init_result["User"]["Signature"].ToString();
                                        fansinfo.VerifyFlag = int.Parse(init_result["User"]["VerifyFlag"].ToString());
                                        fansinfo.SnsFlag = int.Parse(init_result["User"]["SnsFlag"].ToString());
                                        entities.Set<FansInfo>().Add(fansinfo);
                                    }
                                    else
                                    {
                                        fansinfo.NickName = init_result["User"]["NickName"].ToString();
                                        fansinfo.HeadImgUrl = init_result["User"]["HeadImgUrl"].ToString();
                                        fansinfo.RemarkName = init_result["User"]["RemarkName"].ToString();
                                        fansinfo.Sex = int.Parse(init_result["User"]["Sex"].ToString());
                                        fansinfo.Signature = init_result["User"]["Signature"].ToString();
                                        fansinfo.VerifyFlag = int.Parse(init_result["User"]["VerifyFlag"].ToString());
                                        fansinfo.SnsFlag = int.Parse(init_result["User"]["SnsFlag"].ToString());
                                    }
                                }
                                JObject contact_result = WxService.GetContact(ref cc);
                                if (init_result == null)
                                    continue;
                                else if (init_result["BaseResponse"]["Ret"].ToString() == LogoutErrorCode)
                                {
                                    FinishFans(item, LoginErrorTimeMessage);
                                }
                                else
                                {
                                    entities.Set<FansFriends>().Delete(c => c.OpenID == item.OpenID);
                                    item.Friends = new List<FansFriend>();
                                    foreach (JObject contact in contact_result["MemberList"])  //完整好友名单
                                    {
                                        var user = new FansFriends();
                                        user.ID = FormulaHelper.CreateGuid();
                                        user.OpenID = item.OpenID;
                                        user.NickName = contact["NickName"].ToString();
                                        user.HeadImgUrl = contact["HeadImgUrl"].ToString();
                                        user.ContactFlag = int.Parse(contact["ContactFlag"].ToString());
                                        user.RemarkName = contact["RemarkName"].ToString();
                                        user.Sex = int.Parse(contact["Sex"].ToString());
                                        user.Signature = contact["Signature"].ToString();
                                        user.VerifyFlag = int.Parse(contact["VerifyFlag"].ToString());
                                        user.StarFriend = int.Parse(contact["StarFriend"].ToString());
                                        user.Province = contact["Province"].ToString();
                                        user.City = contact["City"].ToString();
                                        user.Alias = contact["Alias"].ToString();
                                        user.SnsFlag = int.Parse(contact["SnsFlag"].ToString());
                                        user.KeyWord = contact["KeyWord"].ToString();
                                        entities.Set<FansFriends>().Add(user);
                                        var username = contact["UserName"].ToString();
                                        if (user.VerifyFlag == 0 && !SpecialUsers.Contains(username) && !username.Contains("@@") && username != init_result["User"]["UserName"].ToString())
                                        {
                                            FansFriend ff = new FansFriend();
                                            ff.UserName = username;
                                            ff.NickName = string.IsNullOrEmpty(user.RemarkName) ? user.NickName : user.RemarkName;
                                            ff.State = 0;
                                            item.Friends.Add(ff);
                                        }
                                    }
                                }
                                item.Cookies = cc;
                                item.SyncKey = synckey;
                                try
                                {
                                    entities.SaveChanges();
                                }
                                catch (DbEntityValidationException dbEx)
                                {
                                    LogWriter.Error(dbEx);
                                    throw dbEx;
                                }
                                #endregion

                                #region 发送微信登录提示信息
                                if (!string.IsNullOrEmpty(CheckStartMessage))
                                {
                                    try
                                    {
                                        SendWxMsg(item.OpenID, string.Format(CheckStartMessage, item.Friends.Count, (item.Friends.Count / CheckFriendsCount) + 1));
                                    }
                                    catch
                                    {
                                        item.WxErrorCount++;
                                        throw;
                                    }
                                }
                                #endregion

                                #region 更新用户状态
                                item.State = 4;
                                var ety = entities.Set<FansLogin>().Find(item.LoginID);
                                if (ety != null)
                                {
                                    ety.IsLogin = true;
                                    ety.LoginTime = DateTime.Now;
                                    entities.SaveChanges();
                                }
                                #endregion
                            }
                            catch (Exception ex)
                            {
                                LogWriter.Error(ex);
                            }
                        }
                        #endregion

                        #region 检查好友状态
                        foreach (var item in fansQueue.Where(c => c.State == 4))
                        {
                            //每次拉群的时间间隔都要超过30秒，不然微信不让调用网页版接口
                            if (item.LastCheckTime.AddSeconds(CheckFriendsSingleTime) > DateTime.Now)
                                continue;
                            CookieContainer cc = item.Cookies;
                            JObject temp = null;
                            #region 添加好友
                            if (item.ChartRoomUsers.Count() == 0)
                            {
                                var checkfriends = item.Friends.Where(c => c.State == 0).Take(CheckFriendsCount);

                                if (string.IsNullOrEmpty(item.ChartRoomName))
                                {
                                    temp = WxService.CreateRoom(checkfriends.Select(c => c.UserName).ToList(), ref cc, item.SKey, item.PassTicket);
                                    item.LastCheckTime = DateTime.Now;
                                    item.Cookies = cc;
                                    if (temp["BaseResponse"]["Ret"].ToString() == "0")
                                    {
                                        item.ChartRoomName = temp["ChatRoomName"].ToString();
                                        item.ChartRoomUsers = checkfriends.Select(c => c.UserName).ToList();
                                    }
                                    //操作太频繁
                                    else if (temp["BaseResponse"]["Ret"].ToString() == "1" || temp["BaseResponse"]["ErrMsg"].ToString().Contains("频"))
                                    {
                                        FinishFans(item, WebMaxErrorMsg);
                                    }
                                    else
                                    {
                                        item.WebErrorCount++;
                                        continue;
                                    }
                                }
                                else
                                {
                                    temp = WxService.AddMember(item.ChartRoomName, checkfriends.Select(c => c.UserName).ToList(), ref cc, item.SKey, item.PassTicket);
                                    item.LastCheckTime = DateTime.Now;
                                    if (temp["BaseResponse"]["Ret"].ToString() == "0")
                                    {
                                        item.ChartRoomName = temp["ChatRoomName"].ToString();
                                        item.ChartRoomUsers = checkfriends.Select(c => c.UserName).ToList();
                                    }
                                    //操作太频繁
                                    else if (temp["BaseResponse"]["Ret"].ToString() == "1" || temp["BaseResponse"]["ErrMsg"].ToString().Contains("频"))
                                    {
                                        FinishFans(item, WebMaxErrorMsg);
                                    }
                                    else
                                    {
                                        item.WebErrorCount++;
                                        continue;
                                    }
                                }

                                item.Cookies = cc;
                                foreach (JObject obj in temp["MemberList"])
                                {
                                    if (obj["MemberStatus"].ToString() == "4")
                                        item.Friends.Where(c => c.UserName == obj["UserName"].ToString()).AsQueryable().Update(c => c.State = 3);
                                    else if (obj["MemberStatus"].ToString() == "3")
                                        item.Friends.Where(c => c.UserName == obj["UserName"].ToString()).AsQueryable().Update(c => c.State = 2);
                                    else
                                        item.Friends.Where(c => c.UserName == obj["UserName"].ToString()).AsQueryable().Update(c => c.State = 1);
                                }
                            }
                            #endregion
                            #region 移除好友
                            else
                            {
                                temp = WxService.DeleteMember(item.ChartRoomName, item.ChartRoomUsers, ref cc, item.SKey, item.PassTicket);
                                item.LastCheckTime = DateTime.Now;
                                if (temp["BaseResponse"]["Ret"].ToString() == "0")
                                {
                                    item.ChartRoomUsers.Clear();
                                }
                                //操作太频繁
                                else if (temp["BaseResponse"]["Ret"].ToString() == "1" || temp["BaseResponse"]["ErrMsg"].ToString().Contains("频"))
                                {
                                    FinishFans(item, WebMaxErrorMsg);
                                }
                                else
                                {
                                    item.WebErrorCount++;
                                }
                                if (item.Friends.All(c => c.State != 0))
                                {
                                    WxService.Logout(ref cc, item.SKey);
                                    var delete = item.Friends.Count(c => c.State == 3);
                                    var block = item.Friends.Count(c => c.State == 2);
                                    if (delete == 0 && block == 0)
                                        FinishFans(item, CheckGoodMessage);
                                    else
                                        FinishFans(item, string.Format(CheckGoodMessage
                                        , delete == 0 ? "没有" : string.Join(",", item.Friends.Where(c => c.State == 3).Select(c => c.NickName))
                                        , block == 0 ? "没有" : string.Join(",", item.Friends.Where(c => c.State == 2).Select(c => c.NickName))));
                                }
                            }
                            #endregion
                            item.Cookies = cc;
                        }
                        #endregion

                        #region 检查登录状态
                        foreach (var item in fansQueue.Where(c => c.State == 4))
                        {
                            CookieContainer cc = item.Cookies;
                            IDictionary<string, string> synckey = item.SyncKey;
                            var sync_flag = WxService.WxSyncCheck(ref cc, ref synckey, item.SKey);
                            if (sync_flag == LogoutErrorJS)
                            {
                                FinishFans(item, LoginErrorTimeMessage);
                            }
                            else
                            {
                                item.Cookies = cc;
                                item.SyncKey = synckey;
                            }
                            if (item.WxErrorCount > InterfaceMaxError)
                            {
                                FinishFans(item, "");
                            }
                            if (item.WebErrorCount > InterfaceMaxError)
                            {
                                FinishFans(item, WebMaxErrorMsg);
                            }
                        }
                        foreach (var item in fansQueue.Where(c => new int[] { 1, 2, 3 }.Contains(c.State) && c.WxErrorCount > InterfaceMaxError))
                        {
                            FinishFans(item, "");
                        }
                        #endregion

                    }
                }
                catch (Exception ex)
                {
                    LogWriter.Error(ex);
                }
                finally
                {
                    Thread.Sleep(CheckFriendsTime);
                }
            }
        }

        /// <summary>
        /// 更新winform显示信息
        /// </summary>
        private void UpdateShowStatus()
        {
            while (true)
            {
                try
                {
                    labelWait.Text = string.Format("等待中粉丝：{0}", waitFansQueue.Count);
                    labelQR.Text = string.Format("带扫码粉丝：{0}", fansQueue.Where(c => c.State == 1).Count());
                    labelLogin.Text = string.Format("带登录粉丝：{0}", fansQueue.Where(c => c.State == 2).Count());
                    labelCheck.Text = string.Format("带检测中粉丝：{0}", fansQueue.Where(c => c.State == 3 || c.State == 4).Count());
                }
                catch (Exception ex)
                {
                    //LogWriter.Error(ex);
                }
                finally
                {
                    Thread.Sleep(UpdateStatusTime);
                }
            }
        }

        /// <summary>
        /// 获取公众号AccessToken
        /// </summary>
        /// <returns></returns>
        private bool UpdateAccessToken()
        {
            var result = (JsonConvert.DeserializeObject(wc.DownloadString(System.Configuration.ConfigurationManager.AppSettings["AccessTokenPath"])) as JObject)["access_token"];
            if (result != null && !string.IsNullOrEmpty(result.ToString()))
            {
                accesstoken = result.ToString();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 强制获取公众号最新AccessToken
        /// </summary>
        /// <returns></returns>
        private bool UpdateNewAccessToken()
        {
            var result = (JsonConvert.DeserializeObject(wc.DownloadString(System.Configuration.ConfigurationManager.AppSettings["NewAccessTokenPath"])) as JObject)["access_token"];
            if (result != null && !string.IsNullOrEmpty(result.ToString()))
            {
                accesstoken = result.ToString();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 发送微信消息
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="msg"></param>
        private void SendWxMsg(string openid, string msg)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                try
                {
                    CustomApi.SendText(accesstoken, openid, msg);
                }
                catch (Exception ex)
                {
                    LogWriter.Error(ex, string.Format("openid为{0}的用户第一次发送信息出错：{1}", openid, msg));
                    UpdateAccessToken();
                    try
                    {
                        CustomApi.SendText(accesstoken, openid, msg);
                    }
                    catch (Exception ex2)
                    {
                        LogWriter.Error(ex2, string.Format("openid为{0}的用户第二次发送信息出错：{1}", openid, msg));
                        UpdateNewAccessToken();
                        try
                        {
                            CustomApi.SendText(accesstoken, openid, msg);
                        }
                        catch (Exception ex3)
                        {
                            LogWriter.Error(ex3, string.Format("openid为{0}的用户第三次发送信息出错：{1}", openid, msg));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 发送微信消息
        /// </summary>
        /// <param name="openid"></param>
        /// <param name="mediaid"></param>
        private void SendWxImg(string openid, string mediaid)
        {
            if (!string.IsNullOrEmpty(mediaid))
            {
                try
                {
                    CustomApi.SendImage(accesstoken, openid, mediaid);
                }
                catch (Exception ex)
                {
                    LogWriter.Error(ex, string.Format("openid为{0}的用户第一次发送登录图片出错", openid));
                    UpdateAccessToken();
                    try
                    {
                        CustomApi.SendText(accesstoken, openid, mediaid);
                    }
                    catch (Exception ex2)
                    {
                        LogWriter.Error(ex2, string.Format("openid为{0}的用户第二次发送登录图片出错", openid));
                        UpdateNewAccessToken();
                        try
                        {
                            CustomApi.SendText(accesstoken, openid, mediaid);
                        }
                        catch (Exception ex3)
                        {
                            LogWriter.Error(ex3, string.Format("openid为{0}的用户第三次发送登录图片出错", openid));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 停止粉丝检测
        /// </summary>
        /// <param name="item"></param>
        /// <param name="msg"></param>
        private void FinishFans(FansData item,string msg)
        {
            #region 发送微信登录超时信息
            SendWxMsg(item.OpenID, msg);
            #endregion

            #region 更新用户状态
            item.State = 5;
            var entities = FormulaHelper.GetEntities<CommonEntities>();
            var ety = entities.Set<FansLogin>().Find(item.LoginID);
            if (ety != null)
            {
                ety.IsFinish = true;
                ety.LogoutTime = DateTime.Now;
                entities.SaveChanges();
            }
            #endregion
        }
        #endregion
    }

    /// <summary>
    /// 检测中粉丝信息
    /// </summary>
    public class FansData
    {
        public string OpenID { get; set; }
        public string LoginID { get; set; }
        /// <summary>
        /// 0是未处理，1是已发送二维码未扫码，2是已扫码未登录，3是已登录未检查，4是已登录正在检查，5是检查完毕
        /// </summary>
        public int State { get; set; }
        public DateTime LastCheckTime { get; set; }
        public DateTime ApplyTime { get; set; }
        public DateTime SendQRTime { get; set; }
        public DateTime LoginTime { get; set; }
        public CookieContainer Cookies { get; set; }
        public string SessionID { get; set; }
        public string SKey { get; set; }
        public string PassTicket { get; set; }
        public string ChartRoomName { get; set; }
        private List<string> chartRoomUsers = new List<string>();
        public List<string> ChartRoomUsers {
            get
            {
                return chartRoomUsers;
            }
            set
            {
                chartRoomUsers = value;
            }
        }
        public IDictionary<string, string> SyncKey { get; set; }
        public List<FansFriend> Friends { get; set; }
        public int WxErrorCount { get; set; }
        public int WebErrorCount { get; set; }
    }

    /// <summary>
    /// 等待中粉丝信息
    /// </summary>
    public class WaitFansData
    {
        public string OpenID { get; set; }
        public string LoginID { get; set; }
        public bool IsReply { get; set; }
        public DateTime ApplyTime { get; set; }
    }

    /// <summary>
    /// 粉丝好友信息
    /// </summary>
    public class FansFriend
    {
        public string UserName { get; set; }
        /// <summary>
        /// 0是未检查，1是好友，2是拉黑，3是删除
        /// </summary>
        public int State { get; set; }
        public string NickName { get; set; }
    }
}
