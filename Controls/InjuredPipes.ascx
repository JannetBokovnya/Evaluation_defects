<%@ Control Language="C#" AutoEventWireup="true" CodeFile="InjuredPipes.ascx.cs" Inherits="Controls_InjuredPipes" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="My.WebControls.AJAX.GridViewControl" Namespace="My.WebControls.AJAX.GridViewControl.GridViewControl"
    TagPrefix="mb" %>
    <%@ Register Assembly="MattBerseth.WebControls.AJAX" Namespace="MattBerseth.WebControls.AJAX.GlowButton"
    TagPrefix="bb" %>
 <h1>
     <asp:ScriptManager ID="ScriptManager1" runat="server">
     </asp:ScriptManager>
     <img src="Images/a2.gif" width="7" height="7" align="middle" /> Список поврежденных 
     труб на участке газопровода, подлежащих ремонту
</h1>
<table style="width:100%;">
    <tr>
        <td colspan="3">
            &nbsp;</td>
    </tr>
    <tr>
        <td width="33%">
            <asp:Button ID="Button1" runat="server" CssClass="i2" onclick="Button1_Click" 
                Text="Ремонтная карта" meta:resourcekey="Button1Resource1" />
        </td>
        <td width="33%">
           <asp:Button ID="RemontMapPipe" runat="server" CssClass="i2" onclick="RemontMapPipe_Click" Enabled="False" 
                Text="Ремонтная карта трубы" meta:resourcekey="RemontMapPipeResource1" /></td>
        <td width="33%">
            &nbsp;</td>
    </tr>
    <tr>
        <td width="33%">
            <table style="width:100%;">
                <tr>
                    <td >
                        <asp:Label ID="txtMG" runat="server" Text="Газопровод: " meta:resourcekey="txtMGResource1"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="txtCondition" runat="server" Text="Условие отбора: " meta:resourcekey="txtConditionResource1"></asp:Label>
                    </td>
                </tr>
            </table>
        </td>
        <td width="33%">
            <table style="width:100%;">
                <tr>
                    <td >
                        <asp:Label ID="txtThread" runat="server" Text="Нитка: " meta:resourcekey="txtThreadResource1"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="txtRegimTransp" runat="server" Text="Режим транспортировки: " meta:resourcekey="txtRegimTranspResource1"></asp:Label>
                    </td>
                </tr>
            </table>
        </td>
        <td width="33%">
            <table style="width:100%;">
                <tr>
                    <td>
                        <asp:Label ID="TxtUchastok" runat="server" Text="Участок: " meta:resourcekey="TxtUchastokResource1"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:Label ID="txtPipeCount" runat="server" Text="Количество труб: " meta:resourcekey="txtPipeCountResource1"></asp:Label>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td colspan="3">
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" OnSelectedIndexChanged="GridView1_OnSelectedIndexChanged"
                Width="2000px" AllowPaging="True" PageSize="50" 
                onpageindexchanging="GridView1_PageIndexChanging" 
                CssClass="yui-datatable-theme" meta:resourcekey="GridView1Resource1">
                 <RowStyle CssClass="data-row" />
                            <AlternatingRowStyle CssClass="alt-data-row" />
                <Columns>
                    <asp:BoundField DataField="numVtd" HeaderText="Номер трубы (по данным ВТД)" meta:resourcekey="BoundFieldResource1" />
                    <asp:BoundField DataField="kmStart" HeaderText="Километраж начала трубы (м)" meta:resourcekey="BoundFieldResource2" />
                    <asp:BoundField DataField="len" HeaderText="Длина (м)" meta:resourcekey="BoundFieldResource3" />
                    <asp:BoundField DataField="thinkness" HeaderText="Толщина стенки (мм)" meta:resourcekey="BoundFieldResource4" />
                    <asp:BoundField DataField="leftReper" 
                        HeaderText="Реперная точка &quot;слева&quot;" meta:resourcekey="BoundFieldResource5" >
                    <ControlStyle Width="200px" />
                    <FooterStyle Width="200px" />
                    <HeaderStyle Width="200px" />
                    <ItemStyle Width="200px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="distLeft" HeaderText="От реперной точки (м)" meta:resourcekey="BoundFieldResource6" />
                    <asp:BoundField DataField="rightReper" 
                        HeaderText="Реперная точка &quot;справа&quot;" meta:resourcekey="BoundFieldResource7" >
                    <ControlStyle Width="200px" />
                    <HeaderStyle Width="200px" />
                    <ItemStyle Width="200px" />
                    </asp:BoundField>
                    <asp:BoundField DataField="distRight" HeaderText="От реперной точки (м)" meta:resourcekey="BoundFieldResource8" />
                    <asp:BoundField DataField="pipeType" HeaderText="Тип трубы" meta:resourcekey="BoundFieldResource9" />
                    <asp:BoundField DataField="NANGLE_SECOND_PRODOL" 
                        HeaderText="Ориентация продольного шва (час)" meta:resourcekey="BoundFieldResource10" />
                    <asp:BoundField DataField="PipeMaker" HeaderText="Изготовитель" meta:resourcekey="BoundFieldResource11" />
                    <asp:BoundField DataField="markSteel" HeaderText="Марка стали трубы" meta:resourcekey="BoundFieldResource12" />
                    <asp:BoundField DataField="countNeDef" 
                        HeaderText="Количество дефектов" meta:resourcekey="BoundFieldResource13" />
                    <asp:BoundField DataField="sumNeDef" 
                        HeaderText="Сумарная длина дефектов (мм)" meta:resourcekey="BoundFieldResource14" />
                    <asp:BoundField DataField="countUsDef" 
                        HeaderText="Количество условно недопустимых дефектов" Visible="False" meta:resourcekey="BoundFieldResource15" />
                    <asp:BoundField DataField="cRecom" HeaderText="Рекомендуемый способ ремонта" meta:resourcekey="BoundFieldResource16" />
                </Columns>
            </asp:GridView>
                <mb:GridViewControlExtender ID="GridViewControlExtender1" runat="server" TargetControlID="GridView1" RowHoverCssClass="row-over" Enabled="True"  />
            
        </td>
    </tr>
    <tr>
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
        <td>
            &nbsp;</td>
    </tr>
</table>
