function GetQueryString(prop, url) {
    var re = new RegExp("[?|&]" + prop + "=([^\\&]*)", "i");
    if (!url) {
        url = document.location.search;
    }
    else {
        if (url.indexOf("?") >= 0) {
            url = url.split("?")[1];
        }
        else {
            url = "";
        }
    }
    var a = re.exec(url);
    if (a == null) return null;
    return a[1];
}

/**<doc type="function" name="Global.LinkTo">
<desc>链接到一个应用系统的一个页面</desc>
<input>
<param name="appname" type="string">链接的应用系统的标识名称[Global.AppServer]</param>
<param name="url" type="string">链接的目标地址，相对地址</param>
<param name="target" type="empty/object/string">链接到的目标窗口,可以是本窗口、Frame、IFrame或是新窗口</param>
<param name="style"  type="empty/string">新窗口的样式</param>
</input>
</doc>**/
function LinkTo(url, target, style) {
    var desurl = url.trim();
    var win = null;
    if (desurl == null || desurl == "") {
        return false;
    }
    try { var target = eval(target); } catch (e) { }
    if (!target || target == "null") {
        window.location.href = desurl;
    } else if (typeof (target) == "object")
        target.location.href = desurl;
    else if (typeof (target) == "string") {
        if (!style) style = "compact";
        else style += ",resizable=yes";
        if (target == "")
            target = "_SELF";
        if (target.toUpperCase() == "_BLANK") {
            win = window.open(desurl, "", style);
        }
        else {
            win = window.open(url, target, style);
        }
        if (!win) alert("哎呀，你阻止了弹出窗口！");
    }
    if (win) return win;
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
StringParam.prototype.GetCount = function() {
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
StringParam.prototype.Add = function(key, value, notescape) {
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
StringParam.prototype.Set = function(key, value, notclear) {
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
StringParam.prototype.Insert = function(key, value, before) {
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
StringParam.prototype.Get = function(key) {
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
StringParam.prototype.GetArray = function(key) {
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
StringParam.prototype.Remove = function(key) {
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
StringParam.prototype.Clear = function() {
    this.Keys.splice(0, this.Keys.length);
    this.Values.splice(0, this.Values.length);
}

/**<doc type="protofunc" name="StringParam.Clone">
<desc>复制本集合</desc>
<output type="object">StringParam对象</output>
</doc>**/
StringParam.prototype.Clone = function() {
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
StringParam.prototype.ToString = function(noescape) {
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
//
//--------------------------------StringParam 对象定义结束---------------------------------------------------
//

/**
</doc>**/
/**<doc type="function" name="Global.ArrayFind">
<desc>查找指定数组中值位置</desc>
<output>返回数组中指定值位置</output>
</doc>**/
ArrayFind = function(ary, value) {
    for (var i = 0; i < ary.length; i++)
        if (ary[i] == value) return i;
    return -1;
}

// 选颜色
var oInputColor;
var oImgColor;
function selColor(obj, oImg) {
    oInputColor = mini.get(obj);
    oImgColor = oImg;
    if (document.all) {
        var arr = showModalDialog("/js/common/selcolor.htm", "", "dialogWidth:290px; dialogHeight:280px; status:0; help:0");
        if (arr != null && arr != "") {
            oInputColor.setValue(arr);
            oImg.style.backgroundColor = arr;
        }
    } else {
        var fcolorWin = window.open("", "fcolorWin", "width=290,height=280");
        if (fcolorWin) {
            fcolorWin.focus();
        } else {
            alert("Please turn off your PopUp blocking software");
            return;
        }

        launchParameters = new Object();
        launchParameters['wbtb'] = window;
        launchParameters['commandName'] = 'setColor';
        fcolorWin.launchParameters = launchParameters;
        fcolorWin.location.href = "selcolor.htm";
    }
}