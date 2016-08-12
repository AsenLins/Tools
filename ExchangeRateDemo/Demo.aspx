<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Demo.aspx.cs" Inherits="ExchangeRateDemo.YaHoo.Demo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>雅虎汇率接口调用</title>
    <style type="text/css">
      div{ margin-top:10px;}
    </style>
</head>
<body>
    <form id="form1" action="Demo.aspx" runat="server">
    <div>
      <div>原货币代码:<input type="text" name="OriginRate" id="OriginRate"  value="CNY" /></div>
      <div>目标货币代码:<input type="text" name="TargetRate" id="TargetRate" value="HKD" /></div>
      <div>需转换的金额:<input type="text" name="Money" id="Money" value="30" /></div>
      <div><input type="submit" value="转换" /></div>
    </div>
    </form>
</body>
</html>
