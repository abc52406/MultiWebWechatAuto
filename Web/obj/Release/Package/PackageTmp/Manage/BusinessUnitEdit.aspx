<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BusinessUnitEdit.aspx.cs" Inherits="Web.Manage.BusinessUnitEdit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>业务单元管理</title>
    <meta http-equiv="content-type" content="text/html; charset=UTF-8" />
    <script src="../js/combine/MiniPageInc.js" type="text/javascript"></script>
    <script src="../js/miniui/swfupload/swfupload.js" type="text/javascript"></script>
    <script src="../css/MiniCssInc.js" type="text/javascript"></script>
	<link rel="stylesheet" href="../kindeditor/themes/default/default.css" />
	<script charset="utf-8" src="../kindeditor/kindeditor-min.js"></script>
	<script charset="utf-8" src="../kindeditor/lang/zh_CN.js"></script>
    <style type="text/css">
        html, body {
            margin: 0;
            padding: 0;
            border: 0;
            width: 100%;
            height: 100%;
            overflow: hidden;
        }
    </style>
</head>
<body>
    <form id="form1" method="post">
        <input id="ID" name="ID" class="mini-hidden" />
        <input id="ImgPath" name="ImgPath" class="mini-hidden" />
        <input id="CreateTime" name="CreateTime" class="mini-hidden" />
        <input id="ModifyTime" name="ModifyTime" class="mini-hidden" />
        <input id="IsDelete" name="IsDelete" class="mini-hidden" />
        <input id="DeleteTime" name="DeleteTime" class="mini-hidden" />
        <input id="IsPublish" name="IsPublish" class="mini-hidden" />
        <input id="PublishTime" name="PublishTime" class="mini-hidden" />
        <fieldset style="border: solid 1px #aaa; padding: 3px;">
            <legend>基本信息</legend>
            <div style="padding-left: 11px; padding-bottom: 5px;">
                <table style="table-layout: fixed;">
                    <tr>
                        <td style="width: 100px;">标题：
                        </td>
                        <td style="width: 600px;">
                            <input id="Title" name="Title" width="100%" class="mini-textbox" required="true" emptytext="请输入标题" />
                        </td>
                        <td colspan="2" rowspan="4">
                            <img id="img1" border="0" src="" width="150px" height="150px" />
                        </td>
                    </tr>
                    <tr>
                        <td>副标题：
                        </td>
                        <td>
                            <textarea width="100%" height="45px" class="mini-textarea" emptyText="请输入副标题" id="Description" name="Description"></textarea>
                        </td>
                    </tr>
                    <tr>
                        <td>类别：
                        </td>
                        <td>
                            <input name="Type" class="mini-combobox" valuefield="value" style="width: 100%;" data="InfoType" />
                        </td>
                    </tr>
                    <tr>
                        <td>图片(正方形)：
                        </td>
                        <td>
                            <input id="Fdata" class="mini-fileupload" name="Fdata" limittype="*.jpg;*.jpeg;*.jpe;*.bmp;*.gif;*.tif;*.png" limitsize="3MB"
                                flashurl="../js/miniui/swfupload/swfupload.swf" width="100%"
                                uploadurl="../Upload/UploadImg.ashx?MiniName=Fdata"
                                onuploadsuccess="onUploadSuccess"
                                onuploaderror="onUploadError" onfileselect="onFileSelect" />
                        </td>
                    </tr>
                </table>
            </div>
        </fieldset>
        <fieldset style="border: solid 1px #aaa; padding: 3px;">
            <legend>详情</legend>
            <div style="padding-left: 11px; padding-bottom: 5px;">
                <table style="table-layout: fixed;width: 100%;">
                    <tr>
                        <td>
                            <textarea id="KindContent" name="KindContent" style="width: 100%; height: 260px;" rows="10" cols="10">
                            </textarea>
                        </td>
                    </tr>
                </table>
            </div>
        </fieldset>
        <div style="text-align: center; padding: 10px;">
            <a id="btnPublish" class="mini-button" onclick="onPublish" style="width: 60px; margin-right: 20px;display:none">发布</a>
            <a id="btnSave" class="mini-button" onclick="onOk" style="width: 60px; margin-right: 20px;display:none">暂存</a>
            <a class="mini-button" onclick="onCancel" style="width: 60px;">取消</a>
        </div>
    </form>
    
    <script type="text/javascript">
        var InfoType = [{ "value": "00", "text": "整合营销" }, { "value": "01", "text": "数字营销" }
            , { "value": "02", "text": "技术营销" }, { "value": "03", "text": "内容营销" }
            , { "value": "04", "text": "娱乐营销" }, { "value": "05", "text": "体育营销" }
            , { "value": "06", "text": "事件营销" }, { "value": "07", "text": "网络口碑营销" }
            , { "value": "08", "text": "社会化媒体营销" }, { "value": "09", "text": "促销和关系营销" }
            , { "value": "10", "text": "品牌解决方案" }, { "value": "11", "text": "媒介及购买计划" }
            , { "value": "12", "text": "电商渠道促销" }, { "value": "13", "text": "线上渠道促销" }
            , { "value": "14", "text": "线下渠道促销" }];
        mini.parse();
        var id = "";
        var fucType = "";
        var parentiframe = null;
        var form = new mini.Form("form1");
        form.loading();
        var imgArray = new Array();
        var currentImg = 0;

        var editor;
        KindEditor.ready(function (K) {
            editor = K.create('textarea[name="KindContent"]', {
                resizeType: 1,
                allowPreviewEmoticons: false,
                uploadJson: '../Upload/KindEditorUploadImg.ashx', // 相对于当前页面的路径
                items: [
		'source', '|', 'undo', 'redo', '|', 'preview', 'print', 'template', 'code', 'cut', 'copy', 'paste',
		'plainpaste', 'wordpaste', '|', 'justifyleft', 'justifycenter', 'justifyright',
		'justifyfull', 'insertorderedlist', 'insertunorderedlist', 'indent', 'outdent', 'subscript',
		'superscript', 'clearhtml', 'quickformat', 'selectall', '|', 'fullscreen', '/',
		'formatblock', 'fontname', 'fontsize', '|', 'forecolor', 'hilitecolor', 'bold',
		'italic', 'underline', 'strikethrough', 'lineheight', 'removeformat', '|', 'image', 'multiimage',
		 'table', 'hr', 'emoticons', 'baidumap', 'pagebreak',
		'anchor', 'link', 'unlink'
                ],
            });
        });

        function SaveData(state) {
            var o = form.getData();
            if (state == "publish") {
                if (!confirm("确定要发布该信息吗？"))
                    return;
            }
            form.validate();
            if (form.isValid() == false) return;
            var text = editor.html();
            o.Body = encodeURI(text);
            var json = mini.encode([o]);
            form.loading();
            $.ajax({
                url: "./BusinessUnitEdit.aspx?method=" + fucType + "&state=" + state,
                type: 'post',
                data: { data: json },
                cache: false,
                success: function (text) {
                    form.unmask();
                    if (text != "")
                        alert(text)
                    else
                        CloseWindow("save");
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    form.unmask();
                    alert(jqXHR.responseText);
                }
            });
        }

        ////////////////////
        //标准方法接口定义
        function SetData(data) {
            if (data.action == "edit") {
                //跨页面传递的数据对象，克隆后才可以安全使用
                data = mini.clone(data);
                id = data.id;
                $.ajax({
                    url: "./BusinessUnitEdit.aspx?method=GetInfo&id=" + data.id,
                    cache: false,
                    success: function (text) {
                        var o = mini.decode(text);
                        form.setData(o);
                        editor.html(decodeURI(o.Body));
                        $("#img1").attr("src", o.ImgPath);
                        $("#btnSave").show();
                        if (o.IsPublish == false) {
                            $("#btnPublish").show();
                        }
                        form.unmask();
                    },
                    error: function (jqXHR, textStatus, errorThrown) {
                        alert(decodeURI(jqXHR.responseText));
                        CloseWindow();
                    }
                });
                fucType = "ModifyInfo"
            }
            else {
                fucType = "AddInfo"
                $("#btnSave").show();
                $("#btnPublish").show();
                form.unmask();
            }
        }

        function GetData() {
            var o = form.getData();
            return o;
        }
        function CloseWindow(action) {
            if (action == "cancel" && form.isChanged()) {
                if (confirm("数据被修改了，是否先保存？")) {
                    return false;
                }
            }
            if (window.CloseOwnerWindow) return window.CloseOwnerWindow(action);
            else window.close();
        }
        function onOk(e) {
            SaveData("save");
        }
        function onPublish(e) {
            SaveData("publish");
        }
        function onCancel(e) {
            CloseWindow("cancel");
        }

        function onFileSelect(e) {
            startUpload();
        }
        function onUploadSuccess(e) {
            mini.get("Fdata").setText("");
            var o = mini.decode(e.serverData);
            $("#img1").attr("src", o.imgpath);
            mini.get("ImgPath").setValue(o.imgpath);
            form.unmask();
        }
        function onUploadError(e) {
            form.unmask();
            alert("上传失败");
            this.setText("");
        }

        function startUpload() {
            var fileupload = mini.get("Fdata");
            if (fileupload.text != ""){
                form.loading();
                fileupload.startUpload();
            }
        }
    </script>

</body>
</html>
