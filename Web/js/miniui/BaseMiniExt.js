/*
针对JQuery MiniUI 的扩展JS方法
Create by Eric Yang 2012.12.18
*/

/*-----------------------------------基础方法扩展---------------------------------------------------*/
String.format = function () {
    var i = 1, args = arguments;
    var str = args[0];
    var re = /\{(\d+)\}/g;
    return str.replace(re, function () { return args[i++] });
}
String.prototype.trim = function () {
    return this.replace(/(^\s*)|(\s*$)/g, "");
}
String.prototype.ltrim = function () {
    return this.replace(/(^\s*)/g, "");
}
String.prototype.rtrim = function () {
    return this.replace(/(\s*$)/g, "");
}
//去掉最后一个指定字符
String.prototype.rtrim = function (char) {
    return this.substr(0, this.lastIndexOf(char));
}
/*--------------------------------------------------------------------------------------*/
/*-----------------------------------------------------HTTP请求扩展方法----------------------------------------------------------------------------------------------------*/
/*同步请求方法（POST 请求）
actionUrl:请求的HTTPURL地址
method: 请求的方法名称
data:请求数据
onsucess(text):请求成功后的回调方法,参数为请求后返回的JSON数据集
onfailed:请求失败后的回调方法(参数未失败信息数据JSON格式)*/
function execute(actionUrl, method, data, onsucess, onfailed) {
    if (actionUrl == null || actionUrl == "" || actionUrl == undefined)
    { mini.alert("未指定actionUrl请求链接地址"); return; }
    if (actionUrl.indexOf("?") >= 0)
        actionUrl += "&method=" + method;
    else
        actionUrl += "?method=" + method;
    var json = mini.encode(data);
    $.ajax({
        async: false,
        cache: false,
        type: "post",
        url: actionUrl,
        data: { data: json },
        success: function (text) {
            if (text != null && text != "" && text != undefined) {
                var responseData = mini.decode(text);
                if (responseData.error == -1) {
                    mini.alert(responseData.message);
                    if (onfailed)
                        onfailed(responseData);
                    return;
                }
                if (onsucess) {
                    onsucess(text);
                }
            }
            else {
                if (onsucess) {
                    onsucess(text);
                }
            }
        },
        error: function (jqxhr, textStatus, errorThrown) {
            if (onfailed) {
                onfailed(jqXHR.responseText);
            } else
                mini.alert("POST请求失败:" + jqXHR.responseText);
        }
    });
}


/*异步请求方法（POST 请求）
actionUrl:请求的HTTPURL地址
method: 请求的方法名称
data:请求数据
onsucess(text):请求成功后的回调方法,参数为请求后返回的JSON数据集
onfailed:请求失败后的回调方法(参数未失败信息数据JSON格式)*/
function executeAsync(actionUrl, method, data, onsucess, onfailed) {
    if (actionUrl == null || actionUrl == "" || actionUrl == undefined)
    { mini.alert("未指定actionUrl请求链接地址"); return; }
    if (actionUrl.indexOf("?") >= 0)
        actionUrl += "&method=" + method;
    else
        actionUrl += "?method=" + method;
    var json = mini.encode(data);
    $.ajax({
        url: actionUrl,
        cache: false,
        type: "post",
        datatype: "json",
        contentType: 'application/json; charset=utf-8',
        data: json,

        success: function (text) {
            if (text != null && text != "" && text != undefined) {
                var responseData = mini.decode(text);
                if (responseData.error == -1) {
                    mini.alert(responseData.message);
                    if (onfailed)
                        onfailed(responseData);
                    return;
                }
                if (onsucess) {
                    onsucess(responseData);
                }
            }
            else {
                if (onsucess) {
                    onsucess(text);
                }
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            if (onfailed) {
                onfailed(jqXHR.responseText);
            }
            else
                mini.alert("POST请求失败:" + jqXHR.responseText);
        }
    });
}

/*同步请求方法(Get请求)
actionUrl:请求的HTTPURL地址
method: 请求的方法名称
data:请求数据
onsucess(text):请求成功后的回调方法,参数为请求后返回的JSON数据集
onfailed:请求失败后的回调方法(参数未失败信息数据JSON格式)*/
function getSync(actionUrl, method, data, onsucess, onfailed) {
    if (actionUrl == null || actionUrl == "" || actionUrl == undefined)
    { mini.alert("未指定actionUrl请求链接地址"); return; }
    if (actionUrl.indexOf("?") >= 0)
        actionUrl += "&method=" + method;
    else
        actionUrl += "?method=" + method;
    var json = mini.encode(data);
    $.ajax({
        async: false,
        url: actionUrl,
        data: json,
        cache: false,
        success: function (text) {
            if (text != null && text != "" && text != "undefined") {
                var responseData = mini.decode(text);
                if (responseData.error == -1) {
                    mini.alert(responseData.message);
                    if (onfailed)
                        onfailed(responseData);
                    return;
                }
                if (onsucess)
                    onsucess(responseData)
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            if (onfailed)
                onfailed(jqXHR.responseText);
            else
                mini.alert("Get请求失败");
        }
    });
}

/*异步请求方法(Get请求)
actionUrl:请求的HTTPURL地址
method: 请求的方法名称
data:请求数据
onsucess(text):请求成功后的回调方法,参数为请求后返回的JSON数据集
onfailed:请求失败后的回调方法(参数未失败信息数据JSON格式)*/
function getAsync(actionUrl, method, data, onsucess, onfailed) {
    if (actionUrl == null || actionUrl == "" || actionUrl == undefined)
    { mini.alert("未指定actionUrl请求链接地址"); return; }
    if (actionUrl.indexOf("?") >= 0)
        actionUrl += "&method=" + method;
    else
        actionUrl += "?method=" + method;
    var json = mini.encode(data);
    $.ajax({
        url: actionUrl,
        data: data,
        cache: false,
        success: function (text) {
            if (text != null && text != "" && text != "undefined") {
                var responseData = mini.decode(text);
                if (responseData.error == -1) {
                    mini.alert(responseData.message);
                    if (onfailed)
                        onfailed(responseData);
                    return;
                }
                if (onsucess)
                    onsucess(responseData)
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            if (onfailed)
                onfailed(jqXHR.responseText);
            else
                mini.alert("Get请求失败");
        }
    });
}


/*---------------------------------------------------------------------------------------------------------------------------------------------------------------------*/


/*-------------------------------------------------------验证扩展方法-------------------------------------------------------------------------------------------------------*/

/*扩展验证输入内容是否是英文*/
function onEnglishValidation(e) {
    if (e.isValid) {
        if (isEnglish(e.value) == false) {
            e.errorText = "必须输入英文";
            e.isValid = false;
        }
    }
}

/*扩展验证输入内容是否是英文+数字*/
function onEnglishAndNumberValidation(e) {
    if (e.isValid) {
        if (isEnglishAndNumber(e.value) == false) {
            e.errorText = "必须输入英文+数字";
            e.isValid = false;
        }
    }
}

/* 是否英文 */
function isEnglish(v) {
    var re = new RegExp("^[a-zA-Z\_]+$");
    if (re.test(v)) return true;
    return false;
}

/* 是否英文+数字 */
function isEnglishAndNumber(v) {
    var re = new RegExp("^[0-9a-zA-Z\_]+$");
    if (re.test(v)) return true;
    return false;
}

/*--------------------------------------------------------------------------------------------------------------------------------*/

/*-------------------------默认配置------------------------------------------------------------------------*/
//Grid默认配置
var gridDefConfig = {
    id: "dataGrid", //默认gridid
    idField: "ID", //唯一标示行记录字段名
    loadingMsg: "正在努力为您加载...",
    showEmptyText: true, //数据为空时显示提示文本
    emptyText: "未找到相关数据", //数据为空时的提示文本

    quickSearchKey: "key", //快速查询录入框Id
    quickSearchField: "", //快速查询字段
    queryForm: "queryForm", //复杂查询formId

    columnWidth: 120, //Grid默认列宽
    sizeList: [10, 30, 50, 100], //页尺寸集合
    pageSize: 30, //每页条数
    allowResize: false, //允许拖拽调节表格尺寸
    multiSelect: false, //允许多选行
    showColumnsMenu: true, //显示列菜单
    allowAlternating: true, //显示斑马纹

    openHeight: 580, //默认打开Edit页面高度
    openWidth: 720, //默认打开Edit页面宽度

    actionUrl: "", //该Grid面请求数据url
    openUrl: "", //打开编辑页面url
    openTitle: "编辑页面"//Edit页面标题
}

//默认页面配置
var pageDefConfig = {
    loadingMsg: "正在努力为您加载...",
    viewAsLable: true//当action='view'时屏蔽按钮，只读.
}
/*-------------------------------------------------------------------------------------------------------------*/

/*---------------------------------------------------------页面通用扩展方法--------------------------------------------------------*/

/**
*<summary>初始化页面，当页面载入完毕后调用.</summary>
**/
function pageInit() {
    mini.parse();
    if (pageConfig == null || pageConfig == "undefined") {
        mini.alert("未配置pageConfig"); return;
    }
    if (pageConfig.grid != null && pageConfig.grid.length >= 1) {
        for (var index = 0, len = pageConfig.grid.length; index < len; index++) {
            var gridConfig = pageConfig.grid[index];
            var grid = mini.get(gridConfig.id);
            if (grid) {//初始化grid
                grid.url = gridConfig.actionUrl + "/GetList";
                grid.loadingMsg = gridConfig.loadingMsg;
                grid.showEmptyText = gridConfig.showEmptyText;
                grid.emptyText = gridConfig.emptyText;
                grid.showColumnsMenu = gridConfig.showColumnsMenu;
                grid.allowAlternating = gridConfig.allowAlternating;
                grid.columnWidth = gridConfig.columnWidth;
                grid.sizeList = gridConfig.sizeList;
                grid.pageSize = gridConfig.pageSize;
                grid.allowResize = gridConfig.allowResize;
                grid.multiSelect = gridConfig.multiSelect;

                grid.load();
            }
        }
    }
    //支持扩展方法pageLoad
    if (typeof (pageLoad) != "undefined") {
        pageLoad();
    }
}


/* 以POPUP方式打开指定连接的窗体 (需指定getpopupdata方法来返回选择的数据)
openUrl: 指定链接的URL地址
title:窗体名称
openWidth:窗体宽度(px像素)
openHeight:窗体高度(px像素)
onclose: 关闭窗体时的回调方法
data:打开链接后的请求参数数据（JSON）*/
function openpopup(openUrl, title, openWidth, openHeight, onclose, data) {
    mini.open({
        url: openUrl,
        title: title,
        width: openWidth,
        height: openHeight,
        allowResize: true,
        onload: function () {
            var iframe = this.getIFrameEl();
            if (iframe.contentWindow.pageload && data)
                iframe.contentWindow.pageload(data);
        },
        ondestroy: function (action) {
            var iframe = this.getIFrameEl();
            if (!iframe.contentWindow.getpopupdata) {
                mini.alert("没有指定getpopupdata方法返回数据"); return;
            }
            if (action.toLowerCase() == "ok") {
                var returnData = iframe.contentWindow.getpopupdata();
                returnData = mini.clone(returnData);
                if (onclose) {
                    onclose(returnData);
                }
            }
        }
    });
}

/*打开在同一个页面上的form定义窗体(窗体ID必须是window)
actionurl:请求的HTTPURL地址
action:行为参数 (add,edit,view,delete)
method:请求方法名(默认为getdata)
aftershowwin:显示窗体后调用的方法*/
function openformwindow(actionUrl, action, data, method, aftershowwin) {
    var window = mini.get("#window");
    if (!window) { mini.alert("未找到指定的window窗体"); return; }
    var forms = $("#window form");
    if (forms.length == 0)
        return;
    var form = new mini.Form(forms[0].id);
    form.clear();
    form.loading();
    var meth = "getdata";
    if (method != null && method != "undefined" && method != "")
        meth = method;
    if (actionUrl.indexOf("?") >= 0)
        actionUrl += "&method=" + meth;
    else
        actionUrl += "?method=" + meth;

    if (action != view) {
        $("#window form a").show();
        form.setEnabled(false);
    }
    else {
        $("#window form a").hide();
        form.setEnabled(true);
    }
    window.show();
    getAsync(actionUrl, meth, data, function (text) {
        var responseData = mini.decode(text);
        if (responseData.error == -1) {
            mini.alert(responseData.message);
            window.hide();
            return;
        }
        form.unmask();
        form.setData(responseData.recdata);
        if (typeof (aftershowwin) != "undefined") {
            aftershowwin(action, responseData.recdata);
        }
    },
    function () {
        mini.alert("数据加载错误"); form.unmask();
    });
}

/*关闭窗体，跨页面调用的窗体关闭方法
action:操作类型,供返回页面调用
data:操作数据,供返回页面调用
*/
function closeWindow(data) {
    if (window.CloseOwnerWindow) return window.CloseOwnerWindow(data, true);
    else window.close();
}

/*关闭窗体，同一页面内的窗体关闭方法*/
function hidewindow() {
    var window = mini.get("#window");
    if (window)
        window.hide();
    var grid = mini.get("#mygrid");
    if (grid)
        grid.reload();
}

/*将数据放入cookie缓存中
sName:名称
sValue:值*/
function setcookie(sName, sValue) {
    date = new Date();
    document.cookie = sName + "=" + escape(sValue) + "; expires=Fri, 31 Dec 2099 23:59:59 GMT;";
}

/*将数据从cookie缓存中取出
sName:名称*/
function getcookie(sName) {
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

/**<doc type="function" name="DestroyIframeCache">
<desc>销毁驻留内存的iframe</desc>
<input>
<param name="ifrm" type="object">iframe对象</param>
</input>        
</doc>**/
function DestroyIframeCache(ifrm) {
    if (typeof (ifrm) != "undefined") {
        if (ifrm) {
            ifrm = ifrm.contentWindow;
            ifrm.src = "about:blank";
            ifrm.document.write("");
            ifrm.document.clear();
            if ($.browser.msie)
                CollectGarbage();
        }
    }
}

/**<doc type="function" name="Global.CenterWin">
<desc>得到居中样式字符串</desc>
<input>
<param name="linkStyle"  type="string">连接样式</param>
<param name="isDialog"  type="bool">是否为对话框</param>
</input>
</doc>**/
function CenterWin(linkStyle, isDialog) {
    if (!linkStyle) return "";
    if (isDialog)
        var sp = new StringParam(linkStyle.toLowerCase());
    else
        var sp = new StringParam(linkStyle.toLowerCase(), ",", "=");
    if (sp.Get("left") || sp.Get("top")) {
        return linkStyle;
    }
    else {
        var width = sp.Get("width");
        if (width == null)
            width = window.screen.width / 3 * 2;
        else
            sp.Remove("width");
        var height = sp.Get("height");
        if (height == null)
            height = window.screen.height / 3 * 2;
        else
            sp.Remove("height");
        var left = (window.screen.width - width) / 2;
        var top = (window.screen.height - height) / 2;
        return "left=" + left + ",top=" + top + ",width=" + width + ",height=" + height + "," + sp.ToString();
    }
}

/*--------------------------------------------------------------------------------------------------------------------------------*/

/*---------------------------------------------------------grid扩展方法--------------------------------------------------------*/

/*绘制链接单元格*/
function onLinkRender(e) {
    var grid = e.sender;
    var record = e.record;
    var uid = record._uid;
    var rowIndex = e.rowIndex;
    var field = e.field;
    var s = '<a href="javascript:onLink(\'' + field + '\',\'' + uid + '\',\'' + grid.id + '\')">' + e.value.trim() + '</a>';
    return s;
}

function onLink(field, uid, gridId) {
    
    var gridConfig = null;
    for (var i = 0, len = pageConfig.grid.length; i < len; i++) {
        if (pageConfig.grid[i].id == gridId) {
            gridConfig = pageConfig.grid[i];
            break;
        }
    }

    if (!gridConfig) {
        alert("未定义页面配置参数gridConfig"); return;
    }
    var grid = mini.get(gridId);
    if (grid) {
        var row = grid.getRowByUID(uid);
        var action = "view";
        if (gridConfig.openUrl != null && gridConfig.openUrl != "" && gridConfig.openUrl != "undefined")
            action = gridConfig.openUrl;

        openLink(gridConfig.openUrl, { action: "view", ID: row[gridConfig.idField], actionUrl: gridConfig.actionUrl }, gridConfig.openTitle,
                gridConfig.openWidth, gridConfig.openHeight, function () { grid.reload(); });
    }
}

/*绘制功能单元格*/
function functionRender(e) {
    var grid = e.sender;
    var record = e.record;
    var uid = record._uid;
    var rowIndex = e.rowIndex;
    var field = e.field;
    var value = e.column.header;
    var s = '<a href="javascript:onfunction(\'' + field + '\',\'' + uid + '\',\'' + grid.id + '\')">' + value + '</a>';
    return s;
}

/**
*<summary>绘制枚举型单元格</summary>
*<param name="e">cell</param>
**/
function onEnumRender(e) {
    if (e.value == null || e.value == "") {
        return "";
    }

    var enumDef;
    for (var i = 0, len = enumMap.length; i < len; i++) {
        if (enumMap[i].column == e.field) {
            enumDef = enumMap[i]["enum"];
            break;
        }
    }
    if (enumDef != null) {
        var value = e.value.split(',');
        var text = "";
        for (var j = 0; j < value.length; j++) {//多枚举
            for (var i = 0, len = enumDef.length; i < len; i++) {
                var def = enumDef[i];
                if (def.value == value[j])
                    text += def.text + "、";
            }
        }
        return text.rtrim('、');
    }
    else
        return e.value;

}
/*-------------------------------------------------------------------------------------------------------------------------------------*/
/*--------------------------------------------------------------list页扩展方法Begin-----------------------------------------------------------*/
/*默认的LIST页面新增方法(指定的列表名称必须为mygrid)
openUrl:打开窗体的URL链接地址,当showformWin未true时，该参数失效
openTitle:打开窗体的名称
actionUrl:HTTP请求的URL地址
openWidth:打开窗体的宽度 
openHeight: 打开窗体的高度*/
function listAdd(gridConfig, data) {
    //默认第一个grid
    if (gridConfig == null && pageConfig.grid[0] != null) {
        gridConfig = pageConfig.grid[0];
    }
    else {
        mini.alert("页面上未配置gridConfig");
        return;
    }

    var grid = mini.get(gridConfig.id);
    if (!grid) return;

    if (data)
        openLink(gridConfig.openUrl, { action: "add" }, gridConfig.openTitle,
            gridConfig.openWidth, gridConfig.openHeight, function () { grid.reload(); }, data);
    else
        openLink(gridConfig.openUrl, { action: "add", actionUrl: gridConfig.actionUrl }, gridConfig.openTitle,
            gridConfig.openWidth, gridConfig.openHeight, function () { grid.reload(); });

}



/*默认的LIST页面删除方法
pagedata.openUrl:打开窗体的URL链接地址
pagedata.title:打开窗体的名称
pagedata.actionUrl:HTTP请求的URL地址
pagedata.openWidth:打开窗体的宽度 当showformWin未true时，该参数失效
pagedata.openHeight: 打开窗体的高度 当showformWin未true时，该参数失效*/
function listEdit(gridConfig) {
    //默认第一个grid
    if (gridConfig == null && pageConfig.grid[0] != null) {
        gridConfig = pageConfig.grid[0];
    }
    else {
        mini.alert("页面上未配置gridConfig");
        return;
    }
    var grid = mini.get(gridConfig.id);
    if (grid) {
        var row = grid.getSelected();
        if (!row) {
            mini.alert("请选中一条记录"); return;
        }
        openLink(gridConfig.openUrl, { action: "edit", id: row[gridConfig.idField] },
            gridConfig.openTitle, gridConfig.openWidth, gridConfig.openHeight, function () { grid.reload(); });
    }
}

function listView(gridConfig) {
    //默认第一个grid
    if (gridConfig == null && pageConfig.grid[0] != null) {
        gridConfig = pageConfig.grid[0];
    }
    else {
        mini.alert("页面上未配置gridConfig");
        return;
    }
    var grid = mini.get(gridConfig.id);
    if (grid) {
        var row = grid.getSelected();
        if (!row) {
            mini.alert("请选中一条记录"); return;
        }
        openLink(gridConfig.openUrl, { action: "view", id: row[gridConfig.idField], method: "view" },
            gridConfig.openTitle, gridConfig.openWidth, gridConfig.openHeight, function () { grid.reload(); });
    }
}

/*默认的LIST页面删除方法(指定的列表名称必须为mygrid)
actionUrl:HTTP请求的URL地址
onsucess:请求成功后的调用方法
onfailed: 请求失败后的调用方法*/
function listDelete(gridConfig, onsucess, onfailed) {
    //默认第一个grid
    if (gridConfig == null && pageConfig.grid[0] != null) {
        gridConfig = pageConfig.grid[0];
    }
    else {
        mini.alert("页面上未配置gridConfig", "提示");
        return;
    }
    var grid = mini.get(gridConfig.id);
    if (!grid) return;

    var rows = grid.getSelecteds();
    if (rows.length == 0)
    { mini.alert("请选择需要删除的记录", "提示"); return; }
    if (!mini.confirm("确定删除选中记录？", "提示"))
        return;
    grid.loading("操作中，请稍后......");
    if (onsucess) {
        if (onfailed)
            executeAsync(gridConfig.actionUrl, "delete", rows, onsucess, onfailed);
        else
            executeAsync(gridConfig.actionUrl, "delete", rows, onsucess);
    }
    else {
        if (onfailed)
            executeAsync(gridConfig.actionUrl, "delete", rows, function () { grid.reload(); }, onfailed);
        else
            executeAsync(gridConfig.actionUrl, "delete", rows, function () { grid.reload(); });
    }
}


/* 打开指定连接的窗体 (指定链接需要有setdata方法以供页面打开时候初次请求数据)
openUrl: 指定链接的URL地址
data:打开链接后的请求数据（JSON）
openWidth:窗体宽度(px像素)
openHeight:窗体高度(px像素)
onclose: 关闭窗体时的回调方法
loaddata:打开链接窗体后，载入数据的方法名称 默认为setdata*/
function openLink(openUrl, data, title, openWidth, openHeight, onclose, loaddata) {
    mini.open({
        url: openUrl,
        title: title,
        width: openWidth,
        height: openHeight,
        allowResize: false,
        onload: function () {
            var iframe = this.getIFrameEl();
            if (loaddata == null || loaddata == "" || loaddata == undefined) {
                loaddata = "setData";
            }
            if (iframe.contentWindow[loaddata])
                iframe.contentWindow[loaddata](data, true);
        },
        onunload: function () {
            var iframe = this.getIFrameEl();
            DestroyIframeCache(iframe);
        },
        ondestroy: function (data) {
            if (onclose) {
                onclose(data, true);
            }
        }
    });
}

/*--------------------------------------------------------------list页扩展方法End--------------------------------------------------------*/


/*--------------------------------------------------------------list页扩展方法-----------------------------------------------------------*/

/*清空查询条件
指定的查询框必须以queryform命名*/
function clearForm() {
    if ($("#queryform").length > 0) {
        var queryForm = new mini.Form("queryform");
        if (queryForm)
            queryForm.clear();
    }
    var key = mini.get("#key");
    if (key)
        mini.get("#key").setValue("");
}

/**
*<summary>默认的查询方法</summary>
*<param name="type">enum:quick(快速查询),multi(复杂查询)</param>
*<param name="gridConfig">grid配置</param>
**/
function search(type, gridConfig) {
    if (type == null || type == "undefined")
        type = "quick";

    if (gridConfig == null && pageConfig.grid[0] != null)
        gridConfig = pageConfig.grid[0]; //默认
    else
        return;

    var grid = mini.get(gridConfig.id);
    if (!grid) {
        mini.alert("未定义mygrid，请检查代码.");
        return;
    }

    var data = {};
    //简单查询
    if (type == "quick") {
        var keyCo = mini.get(gridConfig.quickSearchKey);
        if (keyCo) {
            if (gridConfig == null || gridConfig == "") {
                mini.alert("您尚未配置pageConfig中key属性，\n\r如多个字段请用逗号分隔."); return;
            }

            var keys = gridConfig.quickSearchField.split(',');
            for (i = 0, len = keys.length; i < len; i++) {
                data["$LK$" + keys[i]] = keyCo.getValue();
            }

            if ($("#" + gridConfig.queryForm).length > 0)
                new mini.Form(gridConfig.queryForm).clear();
        }
    }
    //复杂查询
    if (type == "multi") {
        if ($("#" + gridConfig.queryForm).length > 0) {
            var form = new mini.Form(gridConfig.queryForm);
            form.validate();
            if (form.isValid() == false) return;
            data = form.getData();

            if (mini.get(gridConfig.quickSearchKey))
                mini.get(gridConfig.quickSearchKey).setValue("");
        }
    }
    grid.load(data);
}

/*显示详细查询界面*/
function showQueryForm() {
    var window = mini.get("#querywindow");
    if (!window) { mini.alert("未找到指定的window窗体"); return; }
    var queryforms = $("#querywindow form");
    if (queryforms.length == 0)
        return;
    var queryform = new mini.Form(queryforms[0].id);
    window.show();
}

/*为所有的列表页增加回车查询事件*/
$(function () {
    var $inp = $('#key');
    $inp.bind('keydown', function (e) {
        var key = e.which;
        if (key == 13) {
            if (typeof (search) != "undefined")
                search("quick");
        }
    });

    var $queryInput = $('#queryform input');
    $queryInput.bind('keydown', function (e) {
        var key = e.which;
        if (key == 13) {
            if (typeof (search) != "undefined")
                search("multi");
        }
    });
});


/*----------------------------------------------------------------------------------------------------------------------------------------*/


/*--------------------------------------------------------------form页扩展方法------------------------------------------------------------*/

/**
*<summary>请求并获得form页上的数据信息.</summary>
*<param name="pageConfig">请求的数据参数，必须url、ction，非add时id也必须. </param>
**/
function setData(data, iscross) {
    if (!data) return;
    if (iscross)
        data = mini.clone(data);
    if (data.action)
        action = data.action.toLowerCase();
    if (!pageConfig) {
        mini.alert("Edit页面未配置pageConfig.");
    }
    var url = pageConfig.url + "/GetModel";
    if (data.id != null)
        url += "?id=" + data.id;

    var method = (action == "view") ? "view" : "";

    getAsync(url, method, data, function (text) {
        var formId = pageConfig.pageForm;
        if (formId == null || formId == "undefined" || formId == "")
            formId = "dataForm";

        if ($("#" + formId).length == 0) {
            mini.alert("未找到对应" + formId); return;
        }
        var form = new mini.Form(formId);
        form.setData(text);
        if (typeof (beforeSetData) != "undefined") {
            beforeSetData(action, text);
        }
        if (action == "view") {
            setFormDisabled(form, pageConfig.viewAsLable);
        }

        if (typeof (afterSetData) != "undefined") {
            afterSetData(action, text);
        }

    }, function () { mini.alert("表单加载错误"); });
}

/**
*<summary>保存表单数据</summary>
*<param name="pageConfig">表单form对象</param>
*<param name="closeWin">是否已关闭窗体来结束保存动作</param>
*<param name="afterAction">HTTP请求的URL地址</param>
**/
function saveData(pageConfig, closeWin, afterAction) {
    var formId = pageConfig.pageForm;
    if (formId == null || formId == "undefined" || formId == "")
        formId = "dataForm";
    if ($("#" + formId).length == 0)
        return;
    var form = new mini.Form(formId);
    var data = form.getData();
    form.validate();
    if (form.isValid() == false) return;

    var method = "save";
    var url = pageConfig.url + "/save";

    if (closeWin)
        executeAsync(url, method, data, function (data) {
            //            if (!data) return;
            //            var action = data.action;
            closeWindow(data);
        });
    else {
        executeAsync(url, method, data, function (data) {
            hidewindow();
            if (afterAction) {
                afterAction(data.action, data, false);
            }
        });
    }
}

/**
*<summary>设置表单只读</summary>
*<param name="form">表单form对象</param>
*<param name="aslable">是否以lable方式显示</param>
**/
function setFormDisabled(form, aslable) {
    if (!form)
        return;
    var fields = form.getFields();
    if (aslable) {
        for (var i = 0, l = fields.length; i < l; i++) {
            var c = fields[i];
            if (c.setReadOnly) c.setReadOnly(true);
            if (c.setIsValid) c.setIsValid(true);      //去除错误提示
            if (c.addCls) c.addCls("asLabel");          //增加asLabel外观
        }
    }
    else
        form.setEnabled(false);
    $(".mini-button").hide();
}

/*----------------------------------------------------------------------------------------------------------------------------------------*/

/*----------------------------------------------------------------tree页扩展方法------------------------------------------------------------*/

/*基于树集合增加子节点(指定的树控件名称必须为mytree,如果没有选中任何节点，默认在根节点下增加)
showformWin:boolean 值 是否为以同页面窗体打开
openUrl:打开窗体的URL链接地址,当showformWin未true时，该参数失效
title:打开窗体的名称
actionUrl:HTTP请求的URL地址
openWidth:打开窗体的宽度 当showformWin为true时，该参数失效
openHeight: 打开窗体的高度 当showformWin为true时，该参数失效*/
function treeadd(pageconfigdata) {
    var tree = mini.get("mytree");
    if (!tree) return;
    var node = tree.getSelectedNode();
    if (!node)
        node = tree.getRootNode();
    if (!node)
    { mini.alert("请选择一个节点"); return; }
    if (pageconfigdata.showformWin) {
        openformwindow(pageconfigdata.actionUrl, "add", { action: "add", ParentID: node.ID });
    }
    else {
        openlink(pageconfigdata.openUrl, { action: "add", actionUrl: pageconfigdata.actionUrl, ParentID: node.ID },
        pageconfigdata.title, pageconfigdata.openWidth, pageconfigdata.openHeight, aftersavenode);
    }
}

/*基于树集合修改节点(指定的树控件名称必须为mytree)
pageconfigdata.showformWin:boolean 值 是否为以同页面窗体打开
pageconfigdata.openUrl:打开窗体的URL链接地址,当showformWin未true时，该参数失效
pageconfigdata.title:打开窗体的名称
pageconfigdata.actionUrl:HTTP请求的URL地址
pageconfigdata.openWidth:打开窗体的宽度 当showformWin为true时，该参数失效
pageconfigdata.openHeight: 打开窗体的高度 当showformWin为true时，该参数失效*/
function treeedit(pageconfigdata) {
    var tree = mini.get("mytree");
    if (!tree) return;
    var node = tree.getSelectedNode();
    if (!node)
    { mini.alert("请选择一个节点"); return; }
    if (pageconfigdata.showformWin) {
        openformwindow(pageconfigdata.actionUrl, "edit", { ID: node.ID });
    }
    else {
        openlink(pageconfigdata.openUrl, { action: "edit", actionUrl: pageconfigdata.actionUrl, ID: node.ID },
        pageconfigdata.title, pageconfigdata.openWidth, pageconfigdata.openHeight, aftersavenode);
    }
}

/*默认的Tree页面删除方法(指定的树控件名称必须为mytree)
pageconfigdata.actionUrl:HTTP请求的URL地址
pageconfigdata.onsucess:请求成功后的调用方法
pageconfigdata.onfailed: 请求失败后的调用方法*/
function treedelete(pageconfigdata, onsucess, onfailed) {
    var tree = mini.get("mytree");
    var node = tree.getSelectedNode();
    if (!node)
    { mini.alert("请选择一个节点"); return; }
    if (!confirm("您确定需要删除吗？"))
        return;

    if (onsucess) {
        if (onfailed)
            executeAsync(pageconfigdata.actionUrl, "delete", [node], onsucess, onfailed);
        else
            executeAsync(pageconfigdata.actionUrl, "delete", [node], onsucess);
    }
    else {
        if (onfailed)
            executeAsync(pageconfigdata.actionUrl, "delete", [node], function (data) {
                tree.removeNode(node);
            }, onfailed);
        else
            executeAsync(pageconfigdata.actionUrl, "delete", [node], function (data) {
                tree.removeNode(node);
            });
    }
}

/*树界面操作后方法体
data:操作后数据源
iscross:是否是跨页面操作*/
function aftersavenode(data, iscross) {
    if (!data || data == null || data == "undefined" || data == "close") return;
    if (iscross)
        data = mini.clone(data);
    var action = data.action;
    var rec = data.recdata;
    var tree = mini.get("mytree");
    if (!tree) return;
    var node = tree.getSelectedNode();
    if (typeof (updateTreeNode) != "undefined") {
        updateTreeNode(action, rec, tree, node);
    }
    else {
        if (action == "add") {
            var newNode = { "ID": rec.ID, "Name": rec.Name, "Type": rec.Type };
            tree.addNode(newNode, "add", node);
        }
        else {
            tree.updateNode(node, { Name: rec.Name });
        }
    }
}

/*自定义渲染树形节点方法（目前默认方法只支持自定义图标）*/
function onDrawNode(e) {
    var tree = e.sender;
    var node = e.node;
    if (typeof (treeconfig) != "undefined") {
        if (treeconfig) {
            var type = node[treeconfig.IconField];
            var icons = treeconfig["Icons"];
            for (var i = 0; i < icons.length; i++) {
                if (icons[i]["value"] == type) {
                    e.iconCls = icons[i]["iconCls"];
                    break;
                }
            }
        }
    }
}

/*----------------------------------------------------------------------------------------------------------------------------------------*/


//
//--------------------------------StringParam 对象定义开始---------------------------------------------------
//

/**<doc type="objdefine" name="StringParam" inhert="Collection">
<desc>串参数集合对象定义</desc>
<input>
<param name="spstr" type="string">初始化参数串</param>
<param name="type" type="enum">参数串类型</param>
</input>
<enum name="type">
<item value="style" isdefault="true">Key:Value;Key1:Value0;Key1:Value1;</item>
<item value="query" >Key=Value&Key1=Value0&Key1=Value1</item>
</enum>
</doc>**/
function StringParam(spstr, divchar, gapchar) {
    this.Keys = new Array();
    this.Values = new Array();
    this.DivChar = ";";
    this.GapChar = ":";
    if (divchar && gapchar) {
        this.DivChar = divchar;
        this.GapChar = gapchar;
    }
    if (spstr) {
        var ary = spstr.split(this.DivChar);
        for (var i = 0; i < ary.length; i++) {
            var subary = ary[i].split(this.GapChar);
            if (subary[0] != "") {
                this.Values.push(subary[1]);
                this.Keys.push(subary[0]);
            }
        }
    }
}
/**<doc type="protofunc" name="StringParam.GetCount">
<desc>获得元素个数</desc>
<output type="number">元素的个数</output>
</doc>**/
StringParam.prototype.GetCount = function () {
    return this.Keys.length;
}


/**<doc type="protofunc" name="StringParam.Add">
<desc>添加元素</desc>
<input>
<param name="key" type="string">元素key</param>
<param name="value" type="string">元素value</param>
<param name="notescape" type="bool">是否不对对value进行编码</param>
</input>
</doc>**/
StringParam.prototype.Add = function (key, value, notescape) {
    var pos = ArrayFind(this.Keys, key);
    if (pos >= 0) {
        var ary = this.Values[pos].split(",");
        if (ArrayFind(ary, escape(value)) < 0 && !notescape) {
            if (notescape) {
                this.Values[pos] = this.Values[pos] + "," + escape(value);
            } else {
                this.Values[pos] = this.Values[pos] + "," + value;
            }
        }
        else
            throw new Error(0, "(" + key + " , " + value + ") has exist!");
    } else {
        if (notescape) {
            this.Values.push(value);
        } else {
            this.Values.push(escape(value));
        }
        this.Keys.push(key);
    }
}

/**<doc type="protofunc" name="StringParam.Set">
<desc>设置元素值</desc>
<input>
<param name="key" type="string">元素key</param>
<param name="value" type="string">元素value</param>
<param name="notclear" type="bool">为true时查询原Values中是否包含指定值，不包含则加入</param>
</input>
</doc>**/
StringParam.prototype.Set = function (key, value, notclear) {
    if (typeof (key) == "string") {
        var pos = ArrayFind(this.Keys, key);
        if (pos >= 0) {
            if (notclear) {
                var ary = this.Values[pos].split(",");
                if (ArrayFind(ary, escape(value)) < 0)
                    this.Values[pos] = this.Values[pos] + "," + escape(value);
            } else
                this.Values[pos] = escape(value);
        } else {
            this.Values.push(escape(value));
            this.Keys.push(key);
        }
    }
    else if (typeof (key) == "number") {
        if ((key < 0 || key >= this.Keys.length))
            throw new Error(0, key + " not in scope!");
        if (notclear) {
            var ary = this.Values[key].split(",");
            if (ArrayFind(ary, escape(value)) < 0)
                this.Values[key] = this.Values[key] + "," + escape(value);
        } else
            this.Values[key] = escape(value);
    }
}

/**<doc type="protofunc" name="StringParam.Insert">
<desc>在StringParam的制定位置插入值</desc>
<input>
<param name="key" type="string">元素key</param>
<param name="value" type="string">元素value</param>
<param name="before" type="number/string">制定位置或值得位置,默认为末位</param>
</input>
</doc>**/
StringParam.prototype.Insert = function (key, value, before) {
    if (ArrayFind(this.Keys, key) >= 0) {
        throw new Error(0, key + " has exist!");
        return;
    }
    if (typeof (before) == "undefined") {
        this.Add(key, escape(value));
        return;
    }
    if (typeof (before) == "number") {
        if ((before < 0 || before >= this.Keys.length))
            this.Add(key, value);
        else {
            this.Keys.splice(before, 0, key);
            this.Values.splice(before, 0, escape(value));
        }
    }
    if (typeof (before) == "string") {
        var bpos = ArrayFind(this.Keys, before);
        if (bpos < 0)
            this.Add(key, escape(value)); //这里有错误?
        else {
            this.Keys.splice(bpos, 0, key);
            this.Values.splice(bpos, 0, escape(value));
        }
    }

}

/**<doc type="protofunc" name="StringParam.Get">
<desc>得到指定位置的值</desc>
<input>
<param name="key" type="string/number">元素key或位置</param>
</input>
</doc>**/
StringParam.prototype.Get = function (key) {
    if (typeof (key) == "string") {
        var pos = ArrayFind(this.Keys, key);
        if (pos < 0) return null;
        return unescape(this.Values[pos]);
    }
    if (typeof (key) == "number") {
        if ((key < 0 || key >= this.Keys.length)) return null;
        return unescape(this.Values[key]);
    }
}

/**<doc type="protofunc" name="StringParam.GetArray">
<desc>返回指定Key对应的Value并转化为数组</desc>
<output type="object">由指定Key对应的Value转化的数组</output>
</doc>**/
StringParam.prototype.GetArray = function (key) {
    var value;
    if (typeof (key) == "string") {
        var pos = ArrayFind(this.Keys, key);
        if (pos < 0) return null;
        value = this.Values[pos];
    }
    if (typeof (key) == "number") {
        if ((key < 0 || key >= this.Keys.length)) return null;
        value = this.Values[key];
    }
    var ary = value.split(",");
    for (var i = 0; i < ary.length; i++)
        ary[i] = unescape(ary[i]);
    return ary;
}
/**<doc type="protofunc" name="StringParam.Remove">
<desc>删除一个元素</desc>
<input>
<param name="keyindex" type="string/number">键值或索引</param>
</input>
</doc>**/
StringParam.prototype.Remove = function (key) {
    if (typeof (key) == "number") {
        if (key < 0 || key >= this.Keys.length)
            return false;
        this.Keys.splice(key, 1);
        this.Values.splice(key, 1);
        return true;
    } else if (typeof (key) == "string") {
        var pos = ArrayFind(this.Keys, key);
        if (pos < 0)
            return false;
        this.Keys.splice(pos, 1);
        this.Values.splice(pos, 1);
        return true;
    }
}


/**<doc type="protofunc" name="StringParam.Clear">
<desc>清除所有元素</desc>
</doc>**/
StringParam.prototype.Clear = function () {
    this.Keys.splice(0, this.Keys.length);
    this.Values.splice(0, this.Values.length);
}

/**<doc type="protofunc" name="StringParam.Clone">
<desc>复制本集合</desc>
<output type="object">StringParam对象</output>
</doc>**/
StringParam.prototype.Clone = function () {
    var newsp = new StringParam();
    var count = this.Keys.length;
    for (var i = 0; i < count; i++) {
        newsp.Keys[i] = this.Keys[i];
        newsp.Values[i] = this.Values[i];
    }
    return newsp;
}

/**<doc type="protofunc" name="StringParam.ToString">
<desc>返回StringParam的字符串形式格式为 "Key+GapChar+Value"</desc>
<input>
<param name="noescape" type="bool">是否用unescape编码Value</param>
</input>
</doc>**/
StringParam.prototype.ToString = function (noescape) {
    var str = "", value;
    var count = this.Keys.length;
    for (var i = 0; i < count; i++) {
        if (noescape)
            value = unescape(this.Values[i]);
        else
            value = this.Values[i];
        str += this.Keys[i] + this.GapChar + value + this.DivChar;
    }
    if (str.length > 0)
        return str.substr(0, str.length - 1);
    return "";
}

/**
</doc>**/
/**<doc type="function" name="Global.ArrayFind">
<desc>查找指定数组中值位置</desc>
<output>返回数组中指定值位置</output>
</doc>**/
ArrayFind = function (ary, value) {
    for (var i = 0; i < ary.length; i++)
        if (ary[i] == value) return i;
    return -1;
}


/**<doc type="function" name="Global.ToArray">
<desc>用于将object.all(xx)取回的值统一转换成数组</desc>
<input>
<param name="ele" type="variant">需要转换的变量或数组</param>
</input>
</doc>**/
function ToArray(ele) {
    if (ele == null) return new Array();
    if (typeof (ele.length) == "undefined") {
        var ary = new Array();
        ary[0] = ele;
        return ary;
    } else
        return ele;
}
//
//--------------------------------StringParam 对象定义结束---------------------------------------------------