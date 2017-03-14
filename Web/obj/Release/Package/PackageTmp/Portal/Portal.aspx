<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Portal.aspx.cs" Inherits="v.Portal.Portal" %>
<!doctype html>
<html>
<head runat="server">
    <title>众引官网后台管理系统</title>
    <meta http-equiv="content-type" content="text/html; charset=UTF-8" />
    <script src="../css/MiniCssInc.js" type="text/javascript"></script>
    <script src="../js/combine/MiniPageInc.js" type="text/javascript"></script>
</head>
<body>
    
<!--Layout-->
<div id="layout1" class="mini-layout" style="width:100%;height:100%;">
    <div class="header" region="north" height="70" showSplit="false" showHeader="false">
        <h1 style="margin:0;padding:15px;cursor:default;font-family:微软雅黑,黑体,宋体;">众引官网后台管理系统</h1>
        <div style="position:absolute;top:18px;right:10px;">    
            <a class="mini-button mini-button-iconTop" iconCls="icon-close" onclick="onClick"  plain="true" >注销</a>        
            
        </div>
        
    </div>
    <div title="south" region="south" showSplit="false" showHeader="false" height="30" >
        <div style="line-height:28px;text-align:center;cursor:default">Copyright © 众引传播集团版权所有 </div>
    </div>
    <div title="center" region="center" style="border:0;" bodyStyle="overflow:hidden;">
        <!--Splitter-->
        <div class="mini-splitter" style="width:100%;height:100%;" borderStyle="border:0;">
            <div size="180" maxSize="250" minSize="100" showCollapseButton="true" style="border:0;">
                <!--OutlookTree-->
                <div id="leftTree" class="mini-tree" onnodeselect="onNodeSelect" style="width:100%;height:100%;"
                    textField="text" idField="id" parentField="pid" showTreeIcon="true"  showTreeLines="false" >
                </div>
            </div>
            <div showCollapseButton="false" style="border:0;">
                <!--Tabs-->
                <div id="mainTabs" class="mini-tabs" activeIndex="2" style="width:100%;height:100%;"      
                     plain="false" onactivechanged="onTabsActiveChanged" >
                </div>
            </div>        
        </div>
    </div>
</div>

    

    <script type="text/javascript">
        mini.parse();

        var tree = mini.get("leftTree");
        var list = <%=menus%>;
        tree.loadList(list, "id", "pid");

        function showTab(node) {
            var tabs = mini.get("mainTabs");

            var id = "tab$" + node.id;
            var tab = tabs.getTab(id);
            if (!tab) {
                tab = {};
                tab._nodeid = node.id;
                tab.name = id;
                tab.title = node.text;
                tab.showCloseButton = true;

                //这里拼接了url，实际项目，应该从后台直接获得完整的url地址
                tab.url = node.url;

                tabs.addTab(tab);
            }
            tabs.activeTab(tab);
        }

        function onNodeSelect(e) {
            var node = e.node;
            var isLeaf = e.isLeaf;

            if (isLeaf) {
                showTab(node);
            }
        }

        function onClick(e) {
            var messageId = mini.loading("正在注销，马上转到登录页面...", "注销登录");
            setTimeout(function () {
                window.location.href = "Login.aspx";
            }, 1500);
        }
        function onQuickClick(e) {
            tree.expandPath("datagrid");
            tree.selectNode("datagrid");
        }

        function onTabsActiveChanged(e) {
            var tabs = e.sender;
            var tab = tabs.getActiveTab();
            if (tab && tab._nodeid) {

                var node = tree.getNode(tab._nodeid);
                if (node && !tree.isSelectedNode(node)) {
                    tree.selectNode(node);
                }
            }
        }
    </script>
</body>
</html>
