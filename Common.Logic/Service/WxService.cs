using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Common.Logic.Service
{
    public static class WxService
    {
        //获取会话ID的URL
        private static string _session_id_url = "https://login.weixin.qq.com/jslogin?appid=wx782c26e4c19acffb";
        //获取二维码的URL
        private static string _qrcode_url = "https://login.weixin.qq.com/qrcode/"; //后面增加会话id
        //判断二维码扫描情况   200表示扫描登录  201表示已扫描未登录  其它表示未扫描
        private static string _login_check_url = "https://login.weixin.qq.com/cgi-bin/mmwebwx-bin/login?loginicon=true&uuid="; //后面增加会话id
        /// <summary>
        /// 微信初始化url
        /// </summary>
        private static string _init_url = "https://wx.qq.com/cgi-bin/mmwebwx-bin/webwxinit?r=1377482058764";
        /// <summary>
        /// 获取好友头像
        /// </summary>
        private static string _geticon_url = "https://wx.qq.com/cgi-bin/mmwebwx-bin/webwxgeticon?username=";
        /// <summary>
        /// 获取群聊（组）头像
        /// </summary>
        private static string _getheadimg_url = "https://wx.qq.com/cgi-bin/mmwebwx-bin/webwxgetheadimg?username=";
        /// <summary>
        /// 获取好友列表
        /// </summary>
        private static string _getcontact_url = "https://wx.qq.com/cgi-bin/mmwebwx-bin/webwxgetcontact";
        /// <summary>
        /// 同步检查url
        /// </summary>
        private static string _synccheck_url = "https://webpush.weixin.qq.com/cgi-bin/mmwebwx-bin/synccheck?sid={0}&uin={1}&synckey={2}&r={3}&skey={4}&deviceid={5}";
        /// <summary>
        /// 同步url
        /// </summary>
        private static string _sync_url = "https://wx.qq.com/cgi-bin/mmwebwx-bin/webwxsync?sid=";
        /// <summary>
        /// 发送消息url
        /// </summary>
        private static string _sendmsg_url = "https://wx.qq.com/cgi-bin/mmwebwx-bin/webwxsendmsg?sid=";
        /// <summary>
        /// 创建群聊url
        /// </summary>
        private static string _addroom_url = "https://wx.qq.com/cgi-bin/mmwebwx-bin/webwxcreatechatroom?pass_ticket={0}&r={1}";
        /// <summary>
        /// 删除群聊成员url
        /// </summary>
        private static string _delmember_url = "https://wx.qq.com/cgi-bin/mmwebwx-bin/webwxupdatechatroom?fun=delmember&pass_ticket={0}";
        /// <summary>
        /// 添加群聊成员url
        /// </summary>
        private static string _addmember_url = "https://wx.qq.com/cgi-bin/mmwebwx-bin/webwxupdatechatroom?fun=addmember&pass_ticket={0}";
        /// <summary>
        /// 登出url
        /// </summary>
        private static string _logout_url = "https://wx.qq.com/cgi-bin/mmwebwx-bin/webwxlogout?redirect=1&type=0&skey={0}";

        /// <summary>
        /// 获取登录二维码
        /// </summary>
        /// <returns></returns>
        public static Image GetQRCode(ref CookieContainer CookiesContainer, ref string sessionid)
        {
            byte[] bytes = BaseService.SendGetRequest(ref CookiesContainer, _session_id_url);
            sessionid = Encoding.UTF8.GetString(bytes).Split(new string[] { "\"" }, StringSplitOptions.None)[1];
            bytes = BaseService.SendGetRequest(ref CookiesContainer, _qrcode_url + sessionid);

            return Image.FromStream(new MemoryStream(bytes));
        }

        /// <summary>
        /// 登录扫描检测
        /// </summary>
        /// <returns></returns>
        public static object LoginCheck(ref CookieContainer CookiesContainer, string sessionid)
        {
            if (sessionid == null)
            {
                return null;
            }
            byte[] bytes = BaseService.SendGetRequest(ref CookiesContainer,_login_check_url + sessionid);
            string login_result = Encoding.UTF8.GetString(bytes);
            if (login_result.Contains("=201")) //已扫描 未登录
            {
                string base64_image = login_result.Split(new string[] { "\'" }, StringSplitOptions.None)[1].Split(',')[1];
                byte[] base64_image_bytes = Convert.FromBase64String(base64_image);
                MemoryStream memoryStream = new MemoryStream(base64_image_bytes, 0, base64_image_bytes.Length);
                memoryStream.Write(base64_image_bytes, 0, base64_image_bytes.Length);
                //转成图片
                return Image.FromStream(memoryStream);
            }
            else if (login_result.Contains("=200"))  //已扫描 已登录
            {
                string login_redirect_url = login_result.Split(new string[] { "\"" }, StringSplitOptions.None)[1];
                return login_redirect_url;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取sid uid   结果存放在cookies中
        /// </summary>
        public static void GetSidUid(ref CookieContainer CookiesContainer, string login_redirect, ref string passticket, ref string skey)
        {
            byte[] bytes = BaseService.SendGetRequest(ref CookiesContainer, login_redirect + "&fun=new&version=v2&lang=zh_CN");
            string pass_ticket = Encoding.UTF8.GetString(bytes);
            passticket = pass_ticket.Split(new string[] { "pass_ticket" }, StringSplitOptions.None)[1].TrimStart('>').TrimEnd('<', '/');
            skey = pass_ticket.Split(new string[] { "skey" }, StringSplitOptions.None)[1].TrimStart('>').TrimEnd('<', '/');
        }

        /// <summary>
        /// 微信初始化
        /// </summary>
        /// <returns></returns>
        public static JObject WxInit(ref CookieContainer CookiesContainer, string passticket, ref IDictionary<string, string> synckey)
        {
            string init_json = "{{\"BaseRequest\":{{\"Uin\":\"{0}\",\"Sid\":\"{1}\",\"Skey\":\"\",\"DeviceID\":\"e1615250492\"}}}}";
            Cookie sid = BaseService.GetCookie(CookiesContainer, "wxsid");
            Cookie uin = BaseService.GetCookie(CookiesContainer, "wxuin");

            if (sid != null && uin != null)
            {
                init_json = string.Format(init_json, uin.Value, sid.Value);
                byte[] bytes = BaseService.SendPostRequest(ref CookiesContainer, _init_url + "&pass_ticket=" + passticket, init_json);
                string init_str = Encoding.UTF8.GetString(bytes);

                JObject init_result = JsonConvert.DeserializeObject(init_str) as JObject;

                foreach (JObject sk in init_result["SyncKey"]["List"])  //同步键值
                {
                    synckey.Add(sk["Key"].ToString(), sk["Val"].ToString());
                }
                return init_result;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 获取好友头像
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public static Image GetIcon(ref CookieContainer CookiesContainer, string username)
        {
            byte[] bytes = BaseService.SendGetRequest(ref CookiesContainer,_geticon_url + username);

            return Image.FromStream(new MemoryStream(bytes));
        }
        /// <summary>
        /// 获取微信讨论组头像
        /// </summary>
        /// <param name="usename"></param>
        /// <returns></returns>
        public static Image GetHeadImg(ref CookieContainer CookiesContainer, string usename)
        {
            byte[] bytes = BaseService.SendGetRequest(ref CookiesContainer,_getheadimg_url + usename);

            return Image.FromStream(new MemoryStream(bytes));
        }
        /// <summary>
        /// 获取好友列表
        /// </summary>
        /// <returns></returns>
        public static JObject GetContact(ref CookieContainer CookiesContainer)
        {
            byte[] bytes = BaseService.SendGetRequest(ref CookiesContainer,_getcontact_url);
            string contact_str = Encoding.UTF8.GetString(bytes);

            return JsonConvert.DeserializeObject(contact_str) as JObject;
        }
        /// <summary>
        /// 微信同步检测
        /// </summary>
        /// <returns></returns>
        public static string WxSyncCheck(ref CookieContainer CookiesContainer, ref IDictionary<string, string> synckey, string skey)
        {
            string sync_key = "";
            foreach (KeyValuePair<string, string> p in synckey)
            {
                sync_key += p.Key + "_" + p.Value + "%7C";
            }
            sync_key = sync_key.TrimEnd('%', '7', 'C');

            Cookie sid = BaseService.GetCookie(CookiesContainer,"wxsid");
            Cookie uin = BaseService.GetCookie(CookiesContainer,"wxuin");

            if (sid != null && uin != null)
            {
                _synccheck_url = string.Format(_synccheck_url, sid.Value, uin.Value, sync_key, (long)(DateTime.Now.ToUniversalTime() - new System.DateTime(1970, 1, 1)).TotalMilliseconds, skey.Replace("@", "%40"), "e1615250492");

                byte[] bytes = BaseService.SendGetRequest(ref CookiesContainer,_synccheck_url + "&_=" + DateTime.Now.Ticks);
                if (bytes != null)
                {
                    return Encoding.UTF8.GetString(bytes);
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 微信同步
        /// </summary>
        /// <returns></returns>
        public static JObject WxSync(ref CookieContainer CookiesContainer, ref IDictionary<string, string> synckey, string skey, string passticket)
        {
            string sync_json = "{{\"BaseRequest\" : {{\"DeviceID\":\"e1615250492\",\"Sid\":\"{1}\", \"Skey\":\"{5}\", \"Uin\":\"{0}\"}},\"SyncKey\" : {{\"Count\":{2},\"List\":[{3}]}},\"rr\" :{4}}}";
            Cookie sid = BaseService.GetCookie(CookiesContainer,"wxsid");
            Cookie uin = BaseService.GetCookie(CookiesContainer,"wxuin");

            string sync_keys = "";
            foreach (KeyValuePair<string, string> p in synckey)
            {
                sync_keys += "{\"Key\":" + p.Key + ",\"Val\":" + p.Value + "},";
            }
            sync_keys = sync_keys.TrimEnd(',');
            sync_json = string.Format(sync_json, uin.Value, sid.Value, synckey.Count, sync_keys, (long)(DateTime.Now.ToUniversalTime() - new System.DateTime(1970, 1, 1)).TotalMilliseconds, skey);

            if (sid != null && uin != null)
            {
                byte[] bytes = BaseService.SendPostRequest(ref CookiesContainer,_sync_url + sid.Value + "&lang=zh_CN&skey=" + skey + "&pass_ticket=" + passticket, sync_json);
                string sync_str = Encoding.UTF8.GetString(bytes);
                JObject sync_resul = JsonConvert.DeserializeObject(sync_str) as JObject;

                if (sync_resul["SyncKey"]["Count"].ToString() != "0")
                {
                    synckey.Clear();
                    foreach (JObject key in sync_resul["SyncKey"]["List"])
                    {
                        synckey.Add(key["Key"].ToString(), key["Val"].ToString());
                    }
                }
                return sync_resul;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="type"></param>
        public static void SendMsg(string msg, string from, string to, int type, ref CookieContainer CookiesContainer, string skey, string passticket)
        {
            string msg_json = "{{" +
            "\"BaseRequest\":{{" +
                "\"DeviceID\" : \"e441551176\"," +
                "\"Sid\" : \"{0}\"," +
                "\"Skey\" : \"{6}\"," +
                "\"Uin\" : \"{1}\"" +
            "}}," +
            "\"Msg\" : {{" +
                "\"ClientMsgId\" : {8}," +
                "\"Content\" : \"{2}\"," +
                "\"FromUserName\" : \"{3}\"," +
                "\"LocalID\" : {9}," +
                "\"ToUserName\" : \"{4}\"," +
                "\"Type\" : {5}" +
            "}}," +
            "\"rr\" : {7}" +
            "}}";

            Cookie sid = BaseService.GetCookie(CookiesContainer,"wxsid");
            Cookie uin = BaseService.GetCookie(CookiesContainer,"wxuin");

            if (sid != null && uin != null)
            {
                msg_json = string.Format(msg_json, sid.Value, uin.Value, msg, from, to, type, skey, DateTime.Now.Millisecond, DateTime.Now.Millisecond, DateTime.Now.Millisecond);

                byte[] bytes = BaseService.SendPostRequest(ref CookiesContainer,_sendmsg_url + sid.Value + "&lang=zh_CN&pass_ticket=" + passticket, msg_json);

                string send_result = Encoding.UTF8.GetString(bytes);
            }
        }

        /// <summary>
        /// 创建群聊
        /// </summary>
        /// <param name="userNames">群成员</param>
        /// <returns></returns>
        public static JObject CreateRoom(List<string> userNames, ref CookieContainer CookiesContainer, string skey, string passticket)
        {
            Cookie sid = BaseService.GetCookie(CookiesContainer,"wxsid");
            Cookie uin = BaseService.GetCookie(CookiesContainer,"wxuin");
            string _addroom_msg_json = "{{\"BaseRequest\": {{\"Uin\": {0},\"Sid\": \"{1}\",\"Skey\": \"{2}\",\"DeviceID\": \"e441551176\"}},\"MemberCount\": {3},\"MemberList\": {4},\"Topic\": \"\"}}";
            byte[] bytes = BaseService.SendPostRequest(ref CookiesContainer, string.Format(_addroom_url, passticket
                , (long)(DateTime.Now.ToUniversalTime() - new System.DateTime(1970, 1, 1)).TotalMilliseconds)
                , string.Format(_addroom_msg_json, uin.Value, sid.Value, skey, userNames.Count()
                , JsonConvert.SerializeObject(userNames.Select(c => new { UserName = c }).ToList(), Formatting.None)));
            string room_str = Encoding.UTF8.GetString(bytes);

            return JsonConvert.DeserializeObject(room_str) as JObject;
        }

        /// <summary>
        /// 增加群聊成员
        /// </summary>
        /// <param name="roomName">房间名</param>
        /// <param name="userNames">成员</param>
        public static JObject AddMember(string roomName, List<string> userNames, ref CookieContainer CookiesContainer, string skey, string passticket)
        {
            Cookie sid = BaseService.GetCookie(CookiesContainer,"wxsid");
            Cookie uin = BaseService.GetCookie(CookiesContainer,"wxuin");
            string _addmem_msg_json = "{{\"BaseRequest\": {{\"Uin\": \"{0}\",\"Sid\": \"{1}\",\"Skey\": \"{2}\",\"DeviceID\": \"e441551176\"}},\"ChatRoomName\": \"{3}\",\"AddMemberList\": \"{4}\"}}";
            byte[] bytes = BaseService.SendPostRequest(ref CookiesContainer,string.Format(_addmember_url, passticket)
                , string.Format(_addmem_msg_json, uin.Value, sid.Value, skey, roomName
                , string.Join(",", userNames)));
            string delmem_str = Encoding.UTF8.GetString(bytes);

            return JsonConvert.DeserializeObject(delmem_str) as JObject;
        }

        /// <summary>
        /// 移除群聊成员
        /// </summary>
        /// <param name="roomName">房间名</param>
        /// <param name="userNames">成员</param>
        /// <returns></returns>
        public static JObject DeleteMember(string roomName, List<string> userNames, ref CookieContainer CookiesContainer, string skey, string passticket)
        {
            Cookie sid = BaseService.GetCookie(CookiesContainer,"wxsid");
            Cookie uin = BaseService.GetCookie(CookiesContainer,"wxuin");
            string _delmem_msg_json = "{{\"BaseRequest\": {{\"Uin\": {0},\"Sid\": \"{1}\",\"Skey\": \"{2}\",\"DeviceID\": \"e441551176\"}},\"ChatRoomName\": \"{3}\",\"DelMemberList\": \"{4}\"}}";
            byte[] bytes = BaseService.SendPostRequest(ref CookiesContainer, string.Format(_delmember_url, passticket)
                , string.Format(_delmem_msg_json, uin.Value, sid.Value, skey, roomName
                , string.Join(",", userNames)));
            string delmem_str = Encoding.UTF8.GetString(bytes);

            return JsonConvert.DeserializeObject(delmem_str) as JObject;
        }

        public static void Logout(ref CookieContainer CookiesContainer, string skey)
        {
            string init_json = "sid={0}&uin={1}";
            Cookie sid = BaseService.GetCookie(CookiesContainer, "wxsid");
            Cookie uin = BaseService.GetCookie(CookiesContainer, "wxuin");
            init_json = string.Format(init_json, sid.Value, uin.Value);
            byte[] bytes = BaseService.SendPostRequest(ref CookiesContainer, string.Format(_logout_url, skey.Replace("@", "%40")), init_json);
        }
    }
}
