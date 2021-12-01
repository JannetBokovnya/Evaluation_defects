<%@ Page Language="C#" AutoEventWireup="true" CodeFile="InjuredPipeReport.aspx.cs" Inherits="InjuredPipeReport" %>

<%@ Register src="Controls/InjuredPipes.ascx" tagname="InjuredPipes" tagprefix="uc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link type="text/css" href="../../css/extender/yui-datatable.css" rel="stylesheet" />
    <link href="../../css/style.css" type="text/css" rel="STYLESHEET"/>

    <link href="../../css/style.css" type="text/css" rel="STYLESHEET" />
    <link type="text/css" href="../../css/extender/tablecloth.css" rel="stylesheet" />
    <link type="text/css" href="../../css/extender/yui-datatable.css" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <uc1:InjuredPipes ID="InjuredPipes1" runat="server" />
    
    </div>
    </form>
</body>
</html>
