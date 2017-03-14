(function ($) {
    $(function () {
        var $tooltip = $('<div id="vld-tooltip" class="am-hide">提示信息！</div>');
        $tooltip.appendTo(document.body);
        if (document.documentElement.clientWidth > 640) {
            $("#leftmenuclose").css("height", document.documentElement.clientHeight);
            $("#businessunititem-content").css("height", document.documentElement.clientHeight - 230);
        }
        else {
            $("#businessunititem-content").removeClass("am-scrollable-vertical");
        }

        var pathname = window.location.pathname.toLowerCase();
        //关于众引
        if (pathname.indexOf("aboutus.html") > -1) {
            var tabindex = GetQueryString("tabIndex");
            if (tabindex == "")
                tabindex = "1";
            $("#aboutustabimg" + tabindex).attr("src", "./images/ico-lineselected.png");
            $("#aboutustabspan" + tabindex).addClass("aboutus-tabnumberselect");
            $("#aboutuscontentimg").attr("src", "./images/aboutus-tab" + tabindex + ".png");
            $(".aboutustab").click(function () {
                var index = $(this).attr("index");
                if (tabindex != index)
                    location.href = './aboutus.html?tabIndex=' + index;
            });
        }
        //业务单元列表
        if (pathname.indexOf("businessunit.html") > -1) {
            var pagesize = 5, pageindex = GetQueryString("pageIndex"), key = GetQueryString("key"), types = GetQueryString("types");
            if (pageindex == "") {
                pageindex = "0";
            }
            var pi = parseInt(pageindex);
            $.ajax({
                type: 'post',
                url: "./port/data.ashx?Method=GetBusinessUnitList&pageSize=" + pagesize + "&pageIndex=" + pageindex + "&key=" + key + "&types=" + types,
                success: function (data) {
                    if (data) {
                        var list = data.data, pre = pi > 0, next = (pi + 1) * pagesize < data.total, html = "";
                        //数据填充
                        for (var i = 0; i < pagesize; i++) {
                            if (list.length > i) {
                                html += '<li class="businessunit-li"><div class="businessunit-item"><div class="businessunit-itemimgdiv"><img class="businessunit-itemimg" src="'
                                + list[i].ImgPath + '" /></div><div class="businessunit-itemtext"><p class="businessunit-itemtitle am-text-truncate am-margin-0">'
                                + list[i].Title + '</p><hr class="am-margin-top-xs am-margin-bottom-xs businessunit-itemline" /><p class="businessunit-itemdesc am-margin-0">'
                                + list[i].Description + '</p><a class="businessunit-itemlink" href="./businessunititem.html?ID='
                                + list[i].ID + '">&gt;点击查看详情</a></div></div></li>';
                            }
                            else {
                                html += '<li class="businessunit-li"><div class="businessunit-emptyitem"></div></li>';
                            }
                        }
                        //分页栏
                        html += '<li class="businessunit-li"><div class="businessunit-emptyitem"><ol class="am-pagination am-fr businessunit-pagination"><li ' +
                            (pre ? '' : 'class="am-disabled"') + '><a href="' +
                            (pre ? ('./businessunit.html?pageIndex=' + (pi - 1) + '&key=' + key + '&types=' + types) : '#') +
                            '">&lt;</a></li><li ' +
                            (next ? '' : 'class="am-disabled"') + '><a href="' +
                            (next ? ('./businessunit.html?pageIndex=' + (pi + 1) + '&key=' + key + '&types=' + types) : '#') +
                            '">&gt;</a></li></ol></div></li>';
                        $("#ItemList").html(html);
                    }
                },
                error: function (xhr, type) {
                }
            });
            var typeenum = ['01', '02', '03', '04', '05', '06', '07', '08', '09'];
            var typestate = new Array();
            for (var t in typeenum) {
                if (types.indexOf(typeenum[t]) > -1) {
                    typestate[typeenum[t]] = true;
                    $(".btn-type[index='" + typeenum[t] + "']").removeClass("am-btn-default").addClass("am-btn-warning");
                }
                else {
                    typestate[typeenum[t]] = false;
                }
            }
            $(".btn-type").click(function () {
                var typeindex = $(this).attr("index");
                if (typestate[typeindex])
                    typestate[typeindex] = false;
                else
                    typestate[typeindex] = true;
                types = "";
                for (var t in typeenum) {
                    if (typestate[typeenum[t]])
                        types += (typeenum[t] + ",");
                }
                if (types.length > 0)
                    types = types.substr(0, types.length - 1);
                location.href = './businessunit.html?key=' + key + '&types=' + types;
            });
        }
        //业务单元详情
        if (pathname.indexOf("businessunititem.html") > -1) {
            $.ajax({
                type: 'post',
                url: "./port/data.ashx?Method=GetBusinessUnitItem&ID=" + GetQueryString('ID'),
                success: function (data) {
                    if (data && data.length && data.length > 0) {
                        var item = data[0];
                        $("#ItemTitle").html(item.Title);
                        $("#ItemTime").html(item.PublishTime.replace("T"," "));
                        $("#ItemBody").html(decodeURI(item.Body));
                    }
                },
                error: function (xhr, type) {
                }
            });
        }
        //联络我们
        if (pathname.indexOf("connectus.html") > -1) {
            var map = new BMap.Map("l-map");
            map.addControl(new BMap.NavigationControl());
            var point = new BMap.Point("121.491192", "31.209025");    // 创建点坐标
            map.centerAndZoom(point, 14);                     // 初始化地图,设置中心点坐标和地图级别。
            map.enableScrollWheelZoom();
            addComplexMarket("众引传播", "121.491192", "31.209025");

            function addMarker(title, lng, lat) {
                var point = new BMap.Point(lng, lat);
                var marker = new BMap.Marker(point);
                map.addOverlay(marker);
                marker.setTitle(title);
                marker.addEventListener("click", function (point) {
                    var searchInfoWindow = new BMapLib.SearchInfoWindow(map, null, {
                        title: title,
                        width: 290,
                        height: 0,
                        panel: "panel", //检索结果面板
                        enableAutoPan: true, //自动平移
                        enableSendToPhone: true, //是否显示发送到手机按钮
                        searchTypes: [
                            BMAPLIB_TAB_TO_HERE  //到这里去
                        ]
                    });
                    searchInfoWindow.open(point.point);
                });
                //增加跳动效果
                marker.setAnimation(BMAP_ANIMATION_BOUNCE);
            }

            function addComplexMarket(title, lng, lat) {
                var mouseoverTxt = title + " 点击查询路线";
                var myCompOverlay = new ComplexCustomOverlay(new BMap.Point(lng, lat), title, mouseoverTxt);
                map.addOverlay(myCompOverlay);
            }

            // 复杂的自定义覆盖物
            function ComplexCustomOverlay(point, text, mouseoverText) {
                this._point = point;
                this._text = text;
                this._overText = mouseoverText;
            }
            ComplexCustomOverlay.prototype = new BMap.Overlay();
            ComplexCustomOverlay.prototype.initialize = function (map) {
                this._map = map;
                var div = this._div = document.createElement("div");
                div.style.position = "absolute";
                div.style.zIndex = BMap.Overlay.getZIndex(this._point.lat);
                div.style.backgroundColor = "#EE5D5B";
                div.style.border = "1px solid #BC3B3A";
                div.style.color = "white";
                div.style.height = "18px";
                div.style.padding = "2px";
                div.style.lineHeight = "18px";
                div.style.whiteSpace = "nowrap";
                div.style.MozUserSelect = "none";
                div.style.fontSize = "12px"
                var span = this._span = document.createElement("span");
                div.appendChild(span);
                span.appendChild(document.createTextNode(this._text));
                var that = this;

                var arrow = this._arrow = document.createElement("div");
                arrow.style.background = "url(http://map.baidu.com/fwmap/upload/r/map/fwmap/static/house/images/label.png) no-repeat";
                arrow.style.position = "absolute";
                arrow.style.width = "11px";
                arrow.style.height = "10px";
                arrow.style.top = "22px";
                arrow.style.left = "10px";
                arrow.style.overflow = "hidden";
                div.appendChild(arrow);

                div.onmouseover = function () {
                    this.style.backgroundColor = "#6BADCA";
                    this.style.borderColor = "#0000ff";
                    this.getElementsByTagName("span")[0].innerHTML = that._overText;
                    arrow.style.backgroundPosition = "0px -20px";
                }

                div.onmouseout = function () {
                    this.style.backgroundColor = "#EE5D5B";
                    this.style.borderColor = "#BC3B3A";
                    this.getElementsByTagName("span")[0].innerHTML = that._text;
                    arrow.style.backgroundPosition = "0px 0px";
                }

                div.onclick = function () {
                    var searchInfoWindow = new BMapLib.SearchInfoWindow(map, null, {
                        title: title,
                        width: 290,
                        height: 0,
                        panel: "panel", //检索结果面板
                        enableAutoPan: true, //自动平移
                        enableSendToPhone: true, //是否显示发送到手机按钮
                        searchTypes: [
                            BMAPLIB_TAB_TO_HERE  //到这里去
                        ]
                    });
                    searchInfoWindow.open(that._point);
                }

                map.getPanes().labelPane.appendChild(div);

                return div;
            }
            ComplexCustomOverlay.prototype.draw = function () {
                var map = this._map;
                var pixel = map.pointToOverlayPixel(this._point);
                this._div.style.left = pixel.x - parseInt(this._arrow.style.left) + "px";
                this._div.style.top = pixel.y - 30 + "px";
            }
        }

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
            if (a == null) return "";
            return a[1];
        }
    });
})(jQuery);