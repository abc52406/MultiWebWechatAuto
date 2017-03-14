<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Shake.Portal.Login" %>

<!doctype html>
<html>
<head id="Head1" runat="server">
    <title>众引官网后台管理系统</title>
    <meta http-equiv="content-type" content="text/html; charset=UTF-8" />
    <style type="text/css">
        body {
            padding-left: 10px;
            font-size: 13px;
        }

        h1 {
            font-size: 20px;
            font-family: Verdana;
        }

        h4 {
            font-size: 16px;
            margin-top: 25px;
            margin-bottom: 10px;
        }

        .description {
            padding-bottom: 30px;
            font-family: Verdana;
        }

            .description h3 {
                color: #CC0000;
                font-size: 16px;
                margin: 0 30px 10px 0px;
                padding: 45px 0 8px;
                /*background:url(titleback.png) no-repeat scroll left bottom transparent*/
                border-bottom: solid 1px #888;
            }

        body {
            width: 100%;
            height: 100%;
            margin: 0;
            overflow: hidden;
        }
    </style>
    <script src="../css/MiniCssInc.js" type="text/javascript"></script>
    <script src="../js/combine/MiniPageInc.js" type="text/javascript"></script>
</head>
<body onkeydown='onEnterDown();' style="text-align: center;">
    <div id="loginWindow" class="mini-window" title="用户登录" style="width: 300px; height: 165px;"
        showmodal="true" showclosebutton="false">
        <div id="loginForm" style="padding: 15px; padding-top: 10px;">
            <table>
                <tr>
                    <td style="width: 60px;">
                        <label for="username$text">帐号：</label></td>
                    <td>
                        <input id="username" name="username" onvalidation="onUserNameValidation" class="mini-textbox" requirederrortext="帐号不能为空" required="true" style="width: 150px;" />
                    </td>
                </tr>
                <tr>
                    <td style="width: 60px;">
                        <label for="pwd$text">密码：</label></td>
                    <td>
                        <input id="pwd" name="pwd" onvalidation="onPwdValidation" class="mini-password" requirederrortext="密码不能为空" required="true" style="width: 150px;" onenter="onLoginClick" />
                        &nbsp;&nbsp;<!--<a href="#">忘记密码?</a>-->
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td style="padding-top: 5px;">
                        <a onclick="onLoginClick" class="mini-button" style="width: 60px;">登录</a>
                        <a onclick="onResetClick" class="mini-button" style="width: 60px;">重置</a>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    
    <script language="JScript" type="text/javascript">
        mini.parse();
        var loginWindow = mini.get("loginWindow");
        loginWindow.show();
        function SetCookie(sName, sValue) {
            date = new Date();
            document.cookie = sName + "=" + escape(sValue) + "; expires=Fri, 31 Dec 2099 23:59:59 GMT;";
        }
        function GetCookie(sName) {
            var aCookie = document.cookie.split("; ");
            for (var i = 0; i < aCookie.length; i++) {
                var aCrumb = aCookie[i].split("=");
                if (sName == aCrumb[0]) {
                    if (aCrumb[1])
                        return unescape(aCrumb[1]);
                    else
                        return "";
                }
            }
            return "";
        }
        function Init() {
            if (window.parent != window) {
                //防止重新登陆时外框消失
                window.parent.location.href = "/Portal/Login.aspx?Reason=NoLogin&FromUrl=%2fportal%2fPortal.aspx";
                return false;
            }
            else {
                document.all("LoginName").value = GetCookie("LoginName");
                //document.all("Password").value = GetCookie("Password");
                if (GetCookie("SaveInfo") == "T") {
                    document.all("SaveInfo").checked = true;
                    //document.all("Action").focus();
                }
                else {
                    document.all("SaveInfo").checked = false;
                    document.all('LoginName').focus();
                }

            }
        }
        function GotoChangePwd() {
            var strLogin = document.all("LoginName").value;
            //window.navigate("UpdatePwd.aspx?LoginName=" + strLogin);
        }
        function onLoginClick(e) {
            //if (document.all("SaveInfo").checked) {
                //SetCookie("LoginName", mini.get("username").value);
                //SetCookie("Password",mini.get("pwd").value);
                //SetCookie("SaveInfo", "T");
            //}
            //else {
                //SetCookie("LoginName", "");
                //SetCookie("Password","");
                //SetCookie("SaveInfo", "F");
            //}
            var form = new mini.Form("#loginWindow");

            form.validate();
            if (form.isValid() == false) return;

            loginWindow.hide();
            var messageId = mini.loading("正在登陆，请等待...", "正在登陆");
            $.ajax({
                type: "post",
                url: "./Login.aspx?Method=Login",
                dataType: 'text',
                //data: { SysLoginName: document.all("LoginName").value, SysPassword: document.all("Password").value },
                data: "SysLoginName=" + mini.get("username").value + "&SysPassword=" + mini.get("pwd").value,
                success: function (result) {
                    if (result.substring(0, 7) == "Success") {
                        mini.hideMessageBox(messageId);
                        messageId = mini.loading("登录成功，马上转到系统...", "登录成功");
                        setTimeout(function () {
                            window.location.href = "Portal.aspx";
                        }, 1500);
                    }
                    else {
                        mini.hideMessageBox(messageId);
                        messageId = mini.loading(result, "登录失败");
                        setTimeout(function () {
                            mini.hideMessageBox(messageId);
                            loginWindow.show();
                        }, 1500);
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    if (jqXHR.responseText != "") {
                        mini.hideMessageBox(messageId);
                        messageId = mini.loading(jqXHR.responseText, "登录失败");
                        setTimeout(function () {
                            mini.hideMessageBox(messageId);
                            loginWindow.show();
                        }, 1500);
                    }
                }
            });
        }
        function DestroySelf() {
            var oMe = window.self;
            oMe.opener = window.self;
            //oMe.close();
        }

        function LoginSucc(url) {
            window.open(url, "BE_MAIN" + Math.round(Math.random() * 1000), "height=" + (window.screen.availHeight - 60) + ", width=" + (window.screen.availWidth - 14) + ", top=0, left=0, menubar=0, location=0, resizable=1, status=1");
            DestroySelf();
        }

        function onEnterDown() {
            if (window.event.keyCode == 13) {
                onLoginClick();
            }
        }

        function onUserNameValidation(e) {
            if (e.isValid) {
                if (e.value.length < 5) {
                    e.errorText = "账号不能少于5个字符";
                    e.isValid = false;
                }
            }
        }
        function onPwdValidation(e) {
            if (e.isValid) {
                if (e.value.length < 5) {
                    e.errorText = "密码不能少于5个字符";
                    e.isValid = false;
                }
            }
        }
    </script>
</body>
</html>
