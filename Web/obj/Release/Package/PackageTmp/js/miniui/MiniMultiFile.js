var localFileServerUrl = getRootPath() + "/FileStore.Local";
var SlUploadUrl = getRootPath() + "/FileStore/SlUpload/Upload.aspx";


var configSrc = localFileServerUrl + '/Config.aspx';
document.write('<SCRIPT language="JScript" src="' + configSrc + '" type="text/javascript"></SCRIPT>');

function getFile(url, fileId) {
    var getFileIdUrl = localFileServerUrl + "/GetEncryptStr.ashx?str=" + fileId;
    $.get(getFileIdUrl, function (enFileId) {
        window.open(url + "?FileId=" + fileId + "&key=" + enFileId + "&url=" + window.location.host);
    });
}

function getRootPath() {
    var pathName = window.location.pathname.substring(1);
    var webName = pathName == '' ? '' : pathName.substring(0, pathName.indexOf('/'));
    return window.location.protocol + '//' + window.location.host;
}


//将文件Id中的文件名处理掉
function fixFileIds(fileIds) {
    var result = fileIds.replace(/[^_,0-9]/g, function (n) {
        return "";
    }).replace(/_+/g, "_");

    return result;
}

var delFileUrl = "";
//多个文件Id用逗号隔开
function delFileById(ctrlId, fileIds, relateId, callback) {
    if ((fileIds == null || fileIds == "") && (relateId == null || relateId == ""))
        return;
    fileIds = fixFileIds(fileIds);
    $.get(localFileServerUrl + "/GetEncryptStr.ashx?str=" + fileIds, function (enFileIds) {
        $.get(localFileServerUrl + "/GetEncryptStr.ashx?str=" + relateId, function (enRelateId) {
            var url = localFileServerUrl + "/DelFiles.ashx?callback=" + callback + "&ctrlId=" + ctrlId + "&delReason=Web Operation";
            url += "&fileIds=" + enFileIds + "&relateId=" + enRelateId;
            scriptServerCall(url);
        });
    });

}

//跨域动态加载js
function scriptServerCall(url) {
    var me = arguments.callee;
    me.Script && me.Script.parentNode.removeChild(me.Script);
    me.Script = document.createElement("script");
    me.Script.setAttribute("type", "text/javascript");
    me.Script.src = url;
    document.getElementsByTagName("head")[0].appendChild(me.Script);
//    me.Script.onload = me.Script.onreadystatechange = function () {
//        if (me.Script.readyState) {
//            if (me.Script.readyState.match(/loaded|complete/i)) return fnback();
//        } else {
//            return fnback();
//        }
//    }
}


//--------------------------------mini-MultiFile对象定义开始 从mini-Panel继承----------------------------------------
mini.ux.MultiFile = function () {
    mini.ux.MultiFile.superclass.constructor.call(this);

    this.files = [];
    this.initControls();
    this.initEvents();

}
mini.extend(mini.ux.MultiFile, mini.Panel, {
    formField: true,
    title: "上传",
    showHeader: false,
    showToolbar: false,
    showFooter: false,
    showCloseButton: false,
    showCollapseButton: false,
    width: "99.5%",
    height: 120,
    required: false, //是否必填
    readonly: false, //是否只读
    disabled: false, //是否禁用
    maximumupload: "", //上传的最大文件大小
    maxnumbertoupload: "", //上传的最大文件数量
    filter: "", //上传的文件类型
    allowthumbnail: false, //是否缩略图
    src: "system", //所属系统模块
    uiCls: "mini-multifile",
    bodyStyle: "padding:0px;margin:0px",

    initControls: function () {
        var bodyEl = this.getBodyEl();
        var bodyHtml = "<table cellspacing='0' style='padding:0px; margin:0px; border:0px;' width='100%' height='100%' id='_FileTable'>" +
					"<tr>" +
						"<td valign='top'>" +
							"<div style='width:100%;height:" + (this.getHeight() - 4) + "px;overflow-y:auto;overflow-x:hidden' name='fileBody'></div>" +
						"</td>" +
						"<td width='40px' id='_FileTable_td2'>" +
							"<table border='0' width='100%'><tr><td><a name='btnAdd' class='mini-button' style='width:60px;'>添加</a></td></tr>" +
							"<tr><td><a name='btnDel' class='mini-button' style='width:60px;'>删除</a></td></tr>" +
							"<tr><td><a name='btnClear' class='mini-button' style='width:60px;'>清空</a></td></tr>" +
                            "<tr id='_downloadbtnParent'><td><a name='btnDownload' class='mini-button' style='width:60px;'>下载</a></td></tr></table>" +
						"</td>" +
					"</tr>" +
				  "</table>";
        bodyEl.innerHTML = bodyHtml;
        mini.parse(this.el);
        var attrs = mini.ux.MultiFile.superclass.getAttrs.call(this, this.el);
        this.FileBodyEle = $(bodyEl).find("div")[0];
        this._btnAdd = mini.getbyName("btnAdd", this);
        this._btnDel = mini.getbyName("btnDel", this);
        this._btnDownload = mini.getbyName("btnDownload", this);
        this._btnClear = mini.getbyName("btnClear", this);
    },
    initEvents: function () {
        this._btnAdd.on("click", function (e) {
            //弹上传页面，并取返回值
            this.uploadFile();
        }, this);
        this._btnDel.on("click", function (e) {
            var delFileChecked = $(this.FileBodyEle).find("input:checked");
            if (delFileChecked.length > 0) {
                var ctrl = this;
                mini.confirm("确定删除文件?", "确定",
                function (action) {
                    if (action == "ok") {
                        //获取内容区选中checkbox，并移除
                        var delFiles = [];
                        $.each(delFileChecked, function (i, val) {
                            delFiles.push($(val).val());
                        });
                        delFileById(ctrl.id, delFiles.join(","), "", "delFilesCallback");
                    }
                });
            }
        }, this);
        this._btnDownload.on("click", this.downloadFiles, this);
        this._btnClear.on("click", function (e) {
            //清除内容区的内容
            var bodyEl = this.FileBodyEle;
            var fileIds = this.getValue();
            var ctrlId = this.id;
            mini.confirm("确定删除所有文件?", "确定",
                function (action) {
                    if (action == "ok") {
                        delFileById(ctrlId, fileIds, "", "clearFilesCallback");
                    }
                }
            );
        }, this);
    },
    setValue: function (files) {
        this.files = files && files != "" ? files.split(',') : [];
        this.renderFiles();
    },
    addValue: function (files) {
        if (files && files != "") {
            var oldFiles = this.files.length > 0 ? this.files.join(",") + "," : "";
            this.files = (oldFiles + files).split(",");
            this.renderFiles();
        }
    },
    removeValue: function () {
        var delFileChecked = $(this.FileBodyEle).find("input:checked");
        if (delFileChecked.length > 0) {
            var arrdelfiles = [];
            $.each(delFileChecked, function (i, val) {
                arrdelfiles.push($(val).val());
            });
            if (arrdelfiles.length > 0) {
                var newfiles = [];
                $.each(this.files, function (i, val) {
                    if ($.inArray(val, arrdelfiles) == -1) {
                        newfiles.push(val);
                    }
                });
                this.files = newfiles;
                this.renderFiles();
            }
        }
    },
    renderFiles: function () {
        var bodyHtml = "";
        var ctrlId = this.id;
        $.each(this.files, function (i, file) {
            if ($.trim(file) != "") {
                var filename = getMiniFileName(file);
                var fileId = file.substring(0, file.indexOf("_"));
                bodyHtml += "<span><table width=100%><tbody><tr>"
                    + "<td align='right' width='5'><input type='checkbox' id='_FileId' value='" + file + "'></td>"
                    + "<td width='5'></td>"
                    + "<td align='left'><span class='MultiMode_FileName' id='_FileName'><a fileid='" + fileId + "' file='" + file + "' style='text-decoration: underline;cursor:hand;color: #1122CC;' onclick='DoViewFile(\"" + fileId + "\",\"" + file + "\")'>" + filename + "</a></span></td>"
                    + "</tr></tbody></table></span>";
            }
        });
        this.FileBodyEle.innerHTML = bodyHtml;
    },
    getValue: function () {
        var value = this.files.join(",");
        return value;
    },
    setReadonly: function (isReadonly) {
        if (isReadonly) {
            this.readonly = true;
            this._btnAdd.disable();
            this._btnDel.disable();
            this._btnClear.disable();
        }
        else {
            this.readonly = false;
            this._btnAdd.enable();
            this._btnDel.enable();
            this._btnClear.enable();
        }
    },
    setDisabled: function (isDisabled) {
        if (isDisabled) {
            this.disabled = true;
            $(this.FileBodyEle).find("#_FileId").attr('disabled', 'disabled');
            $(this.FileBodyEle).find("a").each(function (i) {
                $(this).unbind();
            });
            $(this.FileBodyEle).find("#_FileName").attr('disabled', 'disabled');
            this.disable();
        }
        else {
            this.disabled = false;
            $(this.FileBodyEle).find("#_FileId").removeAttr('disabled');
            $(this.FileBodyEle).find("a").each(function (i) {
                $(this).bind("click", function () { DoViewFile(this.fileid, this.file) });
            });
            $(this.FileBodyEle).find("#_FileName").removeAttr('disabled');
            this.enable();
        }
    },
    setRequired: function (isRequired) {
        if (isRequired) {
            this.required = true;
            $(this.getBodyEl()).find("#_FileTable").css("backgroundColor", "rgb(255, 255, 230)");
        }
        else {
            this.required = false;
            $(this.getBodyEl()).find("#_FileTable").css("backgroundColor", "");
        }
    },
    validate: function () {
        var isVal = this.isValid();
        var errorId = this.id + "-errorIcson";
        if (!isVal) {
            if ($("#" + errorId).length == 0) {
                //添加error图标
                var imgError = $("<img id='" + errorId + "' src='/CommonWebResource/Theme/Default/MiniUI/images/panel/error.gif' width='14' height='14' title='不能为空' />");
                $("#" + this.id).after(imgError);
            }
            return false;
        }
        else {
            $("#" + errorId).remove();
            return true;
        }
    },
    isValid: function () {
        if (this.required) {
            if (this.getValue() != "") {
                return true;
            }
            else {
                return false;
            }
        }
        else {
            return true;
        }
    },
    uploadFile: function () {
        var value = "FileIds";
        var getFileIdUrl = localFileServerUrl + "/GetEncryptStr.ashx?str=" + value;
        var multiFile = this;
        $.get(getFileIdUrl, function (enValue) {
            mini.open({
                url: SlUploadUrl + "?value=" + value + "&key=" + enValue + "&url=" + window.location.host + "&" + "FileMode=multi&IsLog=undefined&Filter=" + escape(multiFile.filter) + "&MaximumUpload=" + multiFile.maximumupload + "&MaxNumberToUpload=" + multiFile.maxnumbertoupload + "&AllowThumbnail=" + multiFile.allowthumbnail + "&RelateId=&Src=" + multiFile.src,
                width: 500,
                height: 300,
                ondestroy: function (rtnValue) {
                    if (rtnValue.substring(0, 3) == "err") {
                        alert(rtnValue);
                    }
                    else {
                        multiFile.addValue(rtnValue);
                    }
                }
            });
        });
    },
    downloadFiles: function () {
        if (this.files.length > 0) {
            getMiniFile(DownloadUrl, escape(this.files.join(",")));
        }
    },
    getAttrs: function (el) {
        var attrs = mini.ux.MultiFile.superclass.getAttrs.call(this, el);
        mini._ParseString(el, attrs,
            [
                "required", "readonly", "disabled", "maximumupload", "maxnumbertoupload", "filter", "allowthumbnail", "src"]
        );
        return attrs;
    }
});

function DoViewFile(fileId, file) {
    if (!file)
        return;
    getFile(ViewUrl, fileId);
}

function getMiniFileName(fileName) {
    var start = fileName.indexOf('_');
    var end = fileName.lastIndexOf('_');
    if (end == start)
        end = fileName.length;
    return fileName.substr(start + 1, end - start - 1);
}

function getMiniFile(url, fileId) {
    var getFileIdUrl = localFileServerUrl + "/GetEncryptStr.ashx?str=" + fileId;
    $.get(getFileIdUrl, function (enFileId) {
        window.open(url + "?FileId=" + fileId + "&key=" + enFileId + "&url=" + window.location.host);
    });
}

function delFilesCallback(ctrlId) {
    if (ctrlId == "")
        return;
    var miniCtrl = mini.get(ctrlId);
    miniCtrl.removeValue();
}

function clearFilesCallback(ctrlId) {
    if (ctrlId == "")
        return;
    var minictrl = mini.get(ctrlId);
    minictrl.setValue("");
}

mini.regClass(mini.ux.MultiFile, "multifile");