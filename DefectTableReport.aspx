<%@ Page Language="C#" MasterPageFile="~/System/TopMasterPage.master" AutoEventWireup="true" CodeFile="DefectTableReport.aspx.cs" Inherits="DefectTableReport" Title="Информационно-аналитическая система диагностики и ремонта объектов линейной части магистральных газопроводов АО ИЦА" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register Assembly="MattBerseth.WebControls.AJAX" Namespace="MattBerseth.WebControls.AJAX.GridViewControl" TagPrefix="mb" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .style1
        {
            height: 75px;
        }
    </style>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" Runat="Server">

    <table >
        <tr>
            <td class="style1">
                <h1><img src="Images/a2.gif" width="7" height="7" align="middle" /> Информационно-аналитическая система диагностики и ремонта объектов линейной части магистральных газопроводов АО "ИЦА"
</h1>
                </td>
        </tr>
        <tr>
            <td align="left">

                
                
                <table width="1024px">
                    <tr>
                        <td>
                            <asp:Label ID="lblMG" runat="server" Text="Газопровод:" meta:resourcekey="lblMGResource1"></asp:Label> 
                            </td>
                        <td>
                            <asp:Label ID="lblThread" runat="server" Text="Нитка:" meta:resourcekey="lblThreadResource1"></asp:Label>
                             </td>
                        <td>
                            <asp:Label ID="lblDiapozon" runat="server" Text="Участок:" meta:resourcekey="lblDiapozonResource1"></asp:Label> 
                            </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:Label ID="lblFiltr" runat="server" Text="Условие отбора:" meta:resourcekey="lblFiltrResource1"></asp:Label>
                            </td>
                        <td>
                            <asp:Label ID="lblDefectCount" runat="server" Text="Количество дефектов:" meta:resourcekey="lblDefectCountResource1"></asp:Label>
                            </td>
                        <td>
                            <asp:Label ID="lblTranspMode" runat="server" Text="Режим транспортировки: " meta:resourcekey="lblTranspModeResource1"></asp:Label>
                        </td>
                    </tr>
                </table>
                
          
                
                
                
                
                
            </td>
        </tr>
        <tr>
            <td>           
            
                <asp:GridView ID="GridView1" runat="server" CssClass="yui-datatable-theme" 
                    AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" 
                    onpageindexchanging="GridView1_PageIndexChanging" 
                    onsorting="GridView1_Sorting" meta:resourcekey="GridView1Resource1">
                <RowStyle CssClass="data-row" />
                            <AlternatingRowStyle CssClass="alt-data-row" />
                    <Columns>
                        <asp:BoundField HeaderText="Номер дефекта-Номер трубы" DataField="cname"  SortExpression="cname" meta:resourcekey="BoundFieldResource1" />
                        <asp:BoundField HeaderText="Километраж (м)" DataField="nkm"  SortExpression="nkm" meta:resourcekey="BoundFieldResource2"/>
                        <asp:BoundField HeaderText="Тип дефекта" DataField="cDefType"  SortExpression="cDefType" meta:resourcekey="BoundFieldResource3"/>
                        <asp:BoundField HeaderText="Длина (мм)" DataField="nlength"  SortExpression="nlength" meta:resourcekey="BoundFieldResource4"/>
                        <asp:BoundField HeaderText="Ширина (мм)" DataField="nwidth"  SortExpression="nwidth" meta:resourcekey="BoundFieldResource5"/>
                        <asp:BoundField HeaderText="Глубина (мм)" DataField="ndepth"  SortExpression="ndepth" meta:resourcekey="BoundFieldResource6"/>
                        <asp:BoundField HeaderText="Толщина стенки трубы (мм)" DataField="thinkness"  SortExpression="thinkness" meta:resourcekey="BoundFieldResource7"/>
                        <asp:BoundField HeaderText="Угловое положение (час)" DataField="nclockwise_pos"  SortExpression="nclockwise_pos" meta:resourcekey="BoundFieldResource8"/>
                        <asp:BoundField HeaderText="Давление в газопроводе (МПа)" DataField="nWorkPress"  SortExpression="nWorkPress" meta:resourcekey="BoundFieldResource9"/>
                        <asp:BoundField HeaderText="Предельное давление &quot;ASME&quot;(МПа)" DataField="nMaxPress"  SortExpression="nMaxPress" meta:resourcekey="BoundFieldResource10"/>
                        <asp:BoundField HeaderText="КБД &quot;ASME&quot;" DataField="nKBD"  SortExpression="nKBD" meta:resourcekey="BoundFieldResource11"/>
                        <asp:BoundField HeaderText="КБД &quot;DNV&quot;" DataField="nKBD_DNV"  SortExpression="nKBD_DNV" meta:resourcekey="BoundFieldResource12"/>
                        <asp:BoundField HeaderText="Опасность &quot;DNV&quot; (Норвегия)" DataField="Danger_DNV"  SortExpression="Danger_DNV" meta:resourcekey="BoundFieldResource13"/>
                        <asp:BoundField HeaderText="Опасность &quot;ASME&quot; (США)" DataField="Danger_ASME"  SortExpression="Danger_ASME" meta:resourcekey="BoundFieldResource14"/>
                        <asp:BoundField HeaderText="Опасность &quot;ВБН&quot; (Украина)" DataField="Danger_VBN"  SortExpression="Danger_VBN" meta:resourcekey="BoundFieldResource15"/>
                        <asp:BoundField HeaderText="Опасность &quot;ВНИИГАЗ&quot; (Россия)" DataField="Danger_VNI"  SortExpression="Danger_VNI" meta:resourcekey="BoundFieldResource16"/>
                        <asp:BoundField HeaderText="Опасность &quot;RSTRENG&quot; (США)"    DataField="Danger_RStr"  SortExpression="Danger_RStr" meta:resourcekey="BoundFieldResource17"/>
                        <asp:BoundField HeaderText="Опасность по отчету ВТД"  DataField="cdefthreat_level"  SortExpression="cdefthreat_level" meta:resourcekey="BoundFieldResource18"/>
                        <asp:BoundField HeaderText="Время до реагирования &quot;ВНИИГАЗ&quot;" DataField="Time_VNI" SortExpression="Time_VNI" meta:resourcekey="BoundFieldResource19"/>
                        <asp:BoundField HeaderText="Шурфование" DataField="shurf"  SortExpression="customerid" meta:resourcekey="BoundFieldResource20"/>
                        <asp:BoundField HeaderText="Ремонт" DataField="repare"  SortExpression="repare" meta:resourcekey="BoundFieldResource21"/>
                    </Columns>
                </asp:GridView>
                
             
            </td>
        </tr>
        <tr>
            <td>
                &nbsp;</td>
        </tr>
    </table>
</asp:Content>

