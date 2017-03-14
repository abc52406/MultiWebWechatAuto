using System.Web;
using System.Text;
using System.IO;
using System.Net;
using System;
using System.Collections.Generic;

namespace Formula.Alipay
{
    /// <summary>
    /// 类名：Config
    /// 功能：基础配置类
    /// 详细：设置帐户有关信息及返回路径
    /// 版本：3.3
    /// 日期：2012-07-05
    /// 说明：
    /// 以下代码只是为了方便商户测试而提供的样例代码，商户可以根据自己网站的需要，按照技术文档编写,并非一定要使用该代码。
    /// 该代码仅供学习和研究支付宝接口使用，只是提供一个参考。
    /// 
    /// 如何获取安全校验码和合作身份者ID
    /// 1.用您的签约支付宝账号登录支付宝网站(www.alipay.com)
    /// 2.点击“商家服务”(https://b.alipay.com/order/myOrder.htm)
    /// 3.点击“查询合作者身份(PID)”、“查询安全校验码(Key)”
    /// </summary>
    public class Config
    {
        #region 字段
        private static string partner = "";
        private static string key = "";
        private static string private_key = "";
        private static string public_key = "";
        private static string input_charset = "";
        private static string sign_type = "";
        private static string email = "";
        #endregion

        static Config()
        {
            //↓↓↓↓↓↓↓↓↓↓请在这里配置您的基本信息↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓

            //合作身份者ID，以2088开头由16位纯数字组成的字符串
            //partner = "2088411556294234";//dean_yang
            partner = "2088511310277623";//tom

            //交易安全检验码，由数字和字母组成的32位字符串
            //如果签名方式设置为“MD5”时，请设置该参数
            //key = "nbdk7lx4i26ra17ia2un79x3pgztrf7t"; //dean_yang
            key = "dgtzaaxw77j55h3sw0jjhscwjp8khzil"; //tom

            //商户的私钥
            //如果签名方式设置为“0001”时，请设置该参数
            
            //private_key = @"MIICXAIBAAKBgQC7WmfflJcyS4IiTGYgWlLIUN7iyg33tLDqxB+PQdd90+ic5VG5EkieVxzd/qrxDxu1eOZ8MAksiETIQkLmsaISkyG5Q8GyL1w9qoWh0TZbjj9YTqywPOWglQyMR7ceA+Tvj0ZR2v2EFjgZAilSJkgs8/oqMBr3EiWa4snJN6KSuwIDAQABAoGAY+MecGCrf+AsIJc099jQPAaJ3sY1TjSjAnfQD7Pd7TMW2NeWi6KI3wq41E7b2qvgbQ9paxq9OWXprg9N1ess6eqB7MB5Vr6MasHmLBvfVBcSEXTrIJ2CYoVC1IZ/+kkWL116ZFzQV8mKDsA6oSIB7krjDQstbVK4n66VuUEBNokCQQDmHDX5ti+5G3k3GzSsIPXQ9Ml7jWZ7fx4mdLHWVAfYnICtO2Uh0hi/AaR4mQng6EO8dHELE9lGarNDUTmZ6xm9AkEA0G6tb7uF68GkA3U5yslzJOzj7HX1U3MYsHMiPdVJ1+cB7liLVC5w2AYAhnnu9GKzQ8596/jKBaAlG0YCJeaZ1wJAEkPW/PU7IIRPwNIGAkuzd1yWyZnVsqPKbt8AZrTQ0p7Jj/aQ9nhIwpCMz43GPPXyuni7qFdw/afmhsdhvRfuOQJBAMhbe9DJ/AQSUi0YxIMQfuTh6n7lLPwYyYTkR+gUXTVzVEHfT5+OPN8LdfnOwEqfjh8CIb1xnBEoTkMXWv0/3GUCQEXeGGzRV9s15ZELIUO50s/hU5z8LUXGdzjj6Ut2eIPXM+tPzp2kyA1k9ic0eiubl0NcywUrWAsl9v2x9nlqdAY=";
            private_key = @"MIICdgIBADANBgkqhkiG9w0BAQEFAASCAmAwggJcAgEAAoGBALtaZ9+UlzJLgiJMZiBaUshQ3uLKDfe0sOrEH49B133T6JzlUbkSSJ5XHN3+qvEPG7V45nwwCSyIRMhCQuaxohKTIblDwbIvXD2qhaHRNluOP1hOrLA85aCVDIxHtx4D5O+PRlHa/YQWOBkCKVImSCzz+iowGvcSJZriyck3opK7AgMBAAECgYBj4x5wYKt/4CwglzT32NA8BonexjVONKMCd9APs93tMxbY15aLoojfCrjUTtvaq+BtD2lrGr05ZemuD03V6yzp6oHswHlWvoxqweYsG99UFxIRdOsgnYJihULUhn/6SRYvXXpkXNBXyYoOwDqhIgHuSuMNCy1tUrifrpW5QQE2iQJBAOYcNfm2L7kbeTcbNKwg9dD0yXuNZnt/HiZ0sdZUB9icgK07ZSHSGL8BpHiZCeDoQ7x0cQsT2UZqs0NROZnrGb0CQQDQbq1vu4XrwaQDdTnKyXMk7OPsdfVTcxiwcyI91UnX5wHuWItULnDYBgCGee70YrNDzn3r+MoFoCUbRgIl5pnXAkASQ9b89TsghE/A0gYCS7N3XJbJmdWyo8pu3wBmtNDSnsmP9pD2eEjCkIzPjcY89fK6eLuoV3D9p+aGx2G9F+45AkEAyFt70Mn8BBJSLRjEgxB+5OHqfuUs/BjJhORH6BRdNXNUQd9Pn4483wt1+c7ASp+OHwIhvXGcEShOQxda/T/cZQJARd4YbNFX2zXlkQshQ7nSz+FTnPwtRcZ3OOPpS3Z4g9cz60/OnaTIDWT2JzR6K5uXQ1zLBStYCyX2/bH2eWp0Bg==";

            //支付宝的公钥
            //如果签名方式设置为“0001”时，请设置该参数
            public_key = @"MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQCnxj/9qwVfgoUh/y2W89L6BkRAFljhNhgPdyPuBV64bfQNN1PjbCzkIM6qRdKBoLPXmKKMiFYnkd6rAoprih3/PrQEB/VsW8OoM8fxn67UDYuyBTqA23MML9q1+ilIZwBC2AQ2UBVOrFXfFl75p6/B5KsiNG9zpgmLCUYuLkxpLQIDAQAB";

            //↑↑↑↑↑↑↑↑↑↑请在这里配置您的基本信息↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑



            //字符编码格式 目前支持 utf-8
            input_charset = "utf-8";

            //签名方式，选择项：0001(RSA)、MD5
            sign_type = "MD5";
            //无线的产品中，签名方式为rsa时，sign_type需赋值为0001而不是RSA

            //email = "payment@hyatek.com"; //dean_yang
            email = "payment@chinabusinessunion.com";//tom
        }

        #region 属性
        /// <summary>
        /// 获取或设置卖家支付宝账号
        /// </summary>
        public static string Email
        {
            get { return email; }
            set { email = value; }
        }

        /// <summary>
        /// 获取或设置合作者身份ID
        /// </summary>
        public static string Partner
        {
            get { return partner; }
            set { partner = value; }
        }

        /// <summary>
        /// 获取或设交易安全校验码
        /// </summary>
        public static string Key
        {
            get { return key; }
            set { key = value; }
        }

        /// <summary>
        /// 获取或设置商户的私钥
        /// </summary>
        public static string Private_key
        {
            get { return private_key; }
            set { private_key = value; }
        }

        /// <summary>
        /// 获取或设置支付宝的公钥
        /// </summary>
        public static string Public_key
        {
            get { return public_key; }
            set { public_key = value; }
        }

        /// <summary>
        /// 获取字符编码格式
        /// </summary>
        public static string Input_charset
        {
            get { return input_charset; }
        }

        /// <summary>
        /// 获取签名方式
        /// </summary>
        public static string Sign_type
        {
            get { return sign_type; }
        }
        #endregion
    }
}