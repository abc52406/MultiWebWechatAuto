<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BusinessUnitList.aspx.cs" Inherits="Web.Manage.BusinessUnitList" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>业务单元管理</title>
    <meta http-equiv="content-type" content="text/html; charset=UTF-8" />
    <script src="../js/combine/MiniPageInc.js" type="text/javascript"></script>
    <script src="../css/MiniCssInc.js" type="text/javascript"></script>
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
    <div class="mini-layout" style="width:100%;height:100%">
    <div>
        <div style="width:100%;">
            <div class="mini-toolbar" style="border-bottom:0;padding:0px;">
                <table style="width:100%;">
                    <tr>
                        <td style="width:100%;">  
                            <a class="mini-button" iconcls="icon-add" onclick="add()">增加</a>
                            <%--<a class="mini-button" iconcls="icon-edit" onclick="edit()">编辑</a>
                            <a class="mini-button" iconcls="icon-remove" onclick="remove()">删除</a> 
                            <input type="button" value="导出excel" onclick="doExport()"/>--%>        
                        </td>
                        <td style="white-space:nowrap;">
                            <input id="key" class="mini-textbox" emptyText="请输入关键字" style="width:150px;" onenter="onKeyEnter"/>   
                            <a class="mini-button" onclick="search()">查询</a>
                        </td>
                    </tr>
                </table>           
            </div>
        </div>
        <div class="mini-fit">
            <div id="datagrid1" class="mini-datagrid" style="width:100%;height:100%" 
            url="./BusinessUnitList.aspx?Method=GetData" idField="id" 
            allowResize="true" pageSize="20" 
            allowCellEdit="false" allowCellSelect="true" multiSelect="true" 
            editNextOnEnterKey="true"  editNextRowCell="true"
        >
            <div property="columns">
                <div type="indexcolumn" width="40"></div>
                <div name="action" width="150" headerAlign="center">操作</div>
                <div  field="Type" width="80" headerAlign="center" align="center" allowSort="true" >类别
                </div>    
                <div  field="Title" width="200" headerAlign="center" allowSort="true" >标题
                </div>    
                <div  field="Description" width="*" headerAlign="center" allowSort="true" >副标题
                </div>    
                <div  field="CreateTime" width="130" headerAlign="center" align="center" allowSort="true" dateFormat="yyyy-MM-dd HH:mm:ss" >创建时间
                </div>
                <div  field="UpdateTime" width="130" headerAlign="center" align="center" allowSort="true" dateFormat="yyyy-MM-dd HH:mm:ss" >修改时间
                </div>   
                <div  field="PublishTime" width="130" headerAlign="center" align="center" allowSort="true" dateFormat="yyyy-MM-dd HH:mm:ss" >发布时间
                </div>
                <div  field="IsPublish" width="60" headerAlign="center" align="center" allowSort="true" >是否发布
                </div>     
            </div>
          
        </div>
        </div>
        <script type="text/javascript">
            var InfoType = [{ "value": "00", "text": "整合营销" }, { "value": "01", "text": "数字营销" }
            , { "value": "02", "text": "技术营销" }, { "value": "03", "text": "内容营销" }
            , { "value": "04", "text": "娱乐营销" }, { "value": "05", "text": "体育营销" }
            , { "value": "06", "text": "事件营销" }, { "value": "07", "text": "网络口碑营销" }
            , { "value": "08", "text": "社会化媒体营销" }, { "value": "09", "text": "促销和关系营销" }
            , { "value": "10", "text": "品牌解决方案" }, { "value": "11", "text": "媒介及购买计划" }
            , { "value": "12", "text": "电商渠道促销" }, { "value": "13", "text": "线上渠道促销" }
            , { "value": "14", "text": "线下渠道促销" }];
            var BoolState = [{ "value": false, "text": "否" }, { "value": true, "text": "是" }];
            mini.parse();
            var grid = mini.get("datagrid1");
            search();

            grid.on("drawcell", function (e) {
                var record = e.record,
                column = e.column,
                field = e.field,
                value = e.value;

                //action列，超连接操作按钮
                if (column.name == "action") {
                    e.cellStyle = "text-align:center";
                    if (record.IsPublish)
                        e.cellHtml = '<a href="javascript:view(\'' + record.ID + '\')">预览</a>&nbsp;<a href="javascript:edit(\'' + record.ID + '\')">编辑</a>&nbsp;<a href="javascript:cancel(\'' + record.ID + '\')">取消发布</a>&nbsp;<a href="javascript:remove(\'' + record.ID + '\')">删除</a>';
                    else
                        e.cellHtml = '<a href="javascript:view(\'' + record.ID + '\')">预览</a>&nbsp;<a href="javascript:edit(\'' + record.ID + '\')">编辑</a>&nbsp;<a href="javascript:remove(\'' + record.ID + '\')">删除</a>';
                }
                else if (field == 'Type') {
                    e.cellHtml = getEnumText(value, InfoType);
                }
                else if (field == 'IsPublish') {
                    e.cellHtml = getEnumText(value, BoolState);
                }
            });

            function getEnumText(value, enuminfo) {
                if (value == null || value.length <= 0) {
                    return '';
                } else {
                    var text = '';
                    if (typeof (value) == "String") {
                        var val = value.split(',');
                        for (var j = 0, len = val.length; j < len; j++) {
                            for (var i = 0, l = enuminfo.length; i < l; i++) {
                                var enumObj = enuminfo[i];
                                if (enumObj.value == val[j]) {
                                    text += enumObj.text + ',';
                                    break;
                                }
                            }
                        }
                        if (text.length > 0)
                            text = text.substr(0, text.length - 1);
                        return text;
                    }
                    else {
                        for (var i = 0, l = enuminfo.length; i < l; i++) {
                            var enumObj = enuminfo[i];
                            if (enumObj.value == value) {
                                return enumObj.text;
                            }
                        }
                        return "";
                    }
                }
            }

            function search() {
                var key = mini.get("key").getValue();

                grid.load({ key: key });
            }
            function onKeyEnter(e) {
                search();
            }
            function add() {
                mini.open({
                    url: "../Manage/BusinessUnitEdit.aspx",
                    title: "新增", width: 880, height: 560,
                    onload: function () {
                        var iframe = this.getIFrameEl();
                        var data = { action: "new", PicType: "1" };
                        iframe.contentWindow.SetData(data);
                    },
                    ondestroy: function (action) {

                        grid.reload();
                    }
                });
            }
            function edit(id) {
                mini.open({
                    url: "../Manage/BusinessUnitEdit.aspx",
                    title: "编辑", width: 880, height: 560,
                    onload: function () {
                        var iframe = this.getIFrameEl();
                        iframe.scrolling = "yes";
                        var data = { action: "edit", id: encodeURI(id) };
                        iframe.contentWindow.SetData(data);
                    },
                    ondestroy: function (action) {
                        grid.reload();
                    }
                });
            }
            function remove(id) {
                if (confirm("确定删除该记录？")) {
                    grid.loading("操作中，请稍后......");
                    $.ajax({
                        url: "../Manage/BusinessUnitEdit.aspx?method=DeleteInfo&id=" + id,
                        success: function (text) {
                            if (text != "") {
                                alert(decodeURI(text));
                                grid.unmask();
                            }
                            else
                                grid.reload();
                        },
                        error: function () {
                        }
                    });
                }
            }
            function cancel(id) {
                if (confirm("确定取消发布该记录？")) {
                    grid.loading("操作中，请稍后......");
                    $.ajax({
                        url: "../Manage/BusinessUnitEdit.aspx?method=CancelPublish&id=" + id,
                        success: function (text) {
                            if (text != "") {
                                alert(decodeURI(text));
                                grid.unmask();
                            }
                            else
                                grid.reload();
                        },
                        error: function () {
                        }
                    });
                }
            }
            function view(id) {
                window.open("../businessunititem.html?ID=" + id);
            }
            function doExport() {
                var key = mini.get("key").getValue();
                window.open("../Manage/BusinessUnitList.aspx?Method=Export&key=" + encodeURI(key));
            }
    </script>
    </div>
    </div>
</body>
</html>


