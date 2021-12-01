<%@ Page Title="Оценка дефектов" Language="C#" MasterPageFile="~/System/TopMasterPage.master"
    AutoEventWireup="true" CodeFile="EvaluationDefects.aspx.cs" Inherits="Modules_Evaluation_defects_EvaluationDefects"
    EnableViewState="true" culture="auto" meta:resourcekey="PageResource1" uiculture="auto" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="uc" TagName="DefectsList" Src="Controls/ctrDefectsList.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <title>Оценка дефектов</title>
    <style type="text/css">
        .fsSetParam {
            width: auto !important;
            padding-top: 0 !important;
            margin-top: 0 !important;
        }

        fieldset.fsSetParam > fieldset {
            margin-top: 7px;
        }
        .typeDefect {
            margin-top: 7px;
        }

        #<%=grdDefects.ClientID%> .rgAdvPart {
           display: none !important;
        }

        table.resultTable tr td {
            padding: 3px;
        }
        fieldset {
            background-color: #f9f9f9;
        }
        .RadGrid {
            background-color: #e5e5e5 !important;
        }

        .divInfo {
            position: absolute; 
            background: url(../../Images/legend-24.png); 
            background-position: left top; 
            background-repeat: no-repeat; 
            width: 40px; 
            height: 40px; 
            left: 7px; 
            top: 10px;

            -ms-filter: "progid:DXImageTransform.Microsoft.Alpha(Opacity=50)";
            filter: alpha(opacity=50);
            -moz-opacity: 0.5;
            -khtml-opacity: 0.5;
            opacity: 0.5;
        }
        .divInfo:hover {
            -ms-filter: "progid:DXImageTransform.Microsoft.Alpha(Opacity=100)";
            filter: alpha(opacity=100);
            -moz-opacity: 1;
            -khtml-opacity: 1;
            opacity: 1;
        }

        .hideControl {
            display: none;
        }
        .rgAltRow, .rgRow
        {
            cursor: pointer !important;
        }

    </style>
       
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="Body" runat="Server">
    <asp:Panel ID="con" runat="server" CssClass="pageContent" style="width:100%; height:100%;" Visible="True">
        <asp:Label runat="server" ID="lblView2Err"></asp:Label>
        <table style="width: 100%; height:100%; vertical-align: top;"  class="commonTableClass">
            <tr>
                <td valign="top" style="width: 360px;">
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional"  >
                        <ContentTemplate> 
                            <fieldset class="fsSetParam">
                                <legend class="legendtext"><%=GetLocalResourceObject("fldtChooseArea") %></legend>
                                <table class="setterValues" width="100%" >
                                    <tr>
                                        <td style="width: 50px;">
                                            <asp:Label Font-Bold="True" runat="server" Text="МГ: " meta:resourcekey="Label1Resource1"/>
                                        </td>
                                        <td>
                                            <telerik:RadComboBox ID="DDLMG" runat="server" AutoPostBack="True" Width="100%" 
                                                                 OnSelectedIndexChanged="DDLMG_SelectedIndexChanged"  meta:resourcekey="DDLMGResource1"/>  
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label Font-Bold="True" runat="server" Text="Нить: " meta:resourcekey="Label37Resource1"/>
                                        </td>
                                        <td>
                                            <telerik:RadComboBox ID="DDLThread" runat="server" AutoPostBack="True" Enabled="False" Width="100%"
                                                                 meta:resourcekey="DDLThreadResource1" OnSelectedIndexChanged="DDLThread_SelectedIndexChanged" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label Font-Bold="True" runat="server" Text="Км, м " meta:resourcekey="Label84Resource1"></asp:Label>
                                        </td>
                                        <td>
                                            <telerik:RadNumericTextBox ID="txtSharedKmStart" runat="server" Width="49%" MinValue="0"
                                                                       meta:resourcekey="txtSharedKmStartResource1">
                                                <NumberFormat DecimalDigits="3" AllowRounding="true" />
                                                
                                            </telerik:RadNumericTextBox>
                                            <telerik:RadNumericTextBox ID="txtSharedKmEnd" runat="server" Width="49%" MinValue="0"
                                                                       meta:resourcekey="txtSharedKmEndResource1">
                                                <NumberFormat DecimalDigits="3" AllowRounding="true" />
                                            </telerik:RadNumericTextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:Label Font-Bold="True" runat="server" Text="Режимы " meta:resourcekey="Label8Resource1" />
                                        </td>
                                        <td>
                                            <telerik:RadComboBox ID="DDlTranspMode" runat="server" AutoPostBack="True" Enabled="False" Width="100%"
                                                 meta:resourcekey="DDtranspModeResource1" OnSelectedIndexChanged="DDlTranspMode_OnSelectedIndexChanged" />
                                        </td> 
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td>
                                            <telerik:RadButton runat="server" ID="buAllDefect" Text="Отобрать дефекты" OnClientClicked="ShowBusyIndicator" OnClick="AllDefect_Click" Width="100%" Height="29px"
                                                               meta:resourcekey="Button4Resource1" Enabled="False" />
                                            <%--PostBackUrl="~/Modules/Evaluation_defects/EvaluationDefects.aspx"--%>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>

                            <fieldset class="fsSetParam" style="margin-top: 7px !important;" >
                                <legend class="legendtext"><%=GetLocalResourceObject("fldtDefectsTable") %></legend>
                                <table style="width: 100%; height:100%; vertical-align: top;">
                                   <%-- <tr style="display: none;">
                                        <td>
                                            <table style="width: 100%;">
                                                <tr>
                                                    <td>
                                                        <asp:Label runat="server" Font-Bold="True" Text="Дефекты" Visible="False" meta:resourcekey="Label10Resource1"></asp:Label>
                                                    </td>
                                                    <td style="padding-bottom: 3px;">
                                                        <telerik:RadComboBox ID="DDLFiltr" runat="server" AutoPostBack="True" Visible="False" OnSelectedIndexChanged="DDLFiltr_SelectedIndexChanged" meta:resourcekey="DDLFiltrResource1">
                                                            <Items>
                                                        
                                                                <telerik:RadComboBoxItem Selected="True" Value="1" meta:resourcekey="ListItemResource7"/>
                                                                <telerik:RadComboBoxItem Value="2" meta:resourcekey="ListItemResource8"/>
                                                                <telerik:RadComboBoxItem Value="3" meta:resourcekey="ListItemResource9"/>
                                                                <telerik:RadComboBoxItem Value="4" meta:resourcekey="ListItemResource10"/>
                                                                <telerik:RadComboBoxItem Value="5" meta:resourcekey="ListItemResource11"/>
                                                                <telerik:RadComboBoxItem Value="6" meta:resourcekey="ListItemResource12"/>
                                                                <telerik:RadComboBoxItem Value="7" meta:resourcekey="ListItemResource13"/>
                                                                <telerik:RadComboBoxItem Value="8" meta:resourcekey="ListItemResource14"/>
                                                                <telerik:RadComboBoxItem Value="9" meta:resourcekey="ListItemResource15"/>
                                                                <telerik:RadComboBoxItem Value="10" meta:resourcekey="ListItemResource16"/>
                                                                <telerik:RadComboBoxItem Value="11" meta:resourcekey="ListItemResource17"/>
                                                                <telerik:RadComboBoxItem Value="12" meta:resourcekey="ListItemResource18"/>
                                                                <telerik:RadComboBoxItem Value="13" meta:resourcekey="ListItemResource19"/>
                                                                <telerik:RadComboBoxItem Value="14" meta:resourcekey="ListItemResource20"/>
                                                                <telerik:RadComboBoxItem Value="15" meta:resourcekey="ListItemResource21"/>
                                                            </Items>
                                                        </telerik:RadComboBox>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>--%>
                                    <tr>
                                        <td valign="top">
                                            <div style="position: relative;">
                                                <telerik:RadGrid ID="grdDefects" runat="server" AutoGenerateColumns="False" CellPadding="4" 
                                                                 ForeColor="#333333" GridLines="None" Width="99%" DataKeyNames="nDefectKey" 
                                                                 OnPageIndexChanged="grdDefects_OnPageIndexChanged" ShowHeader="True" AllowPaging="True" 
                                                                 OnItemDataBound="grdDefects_OnItemDataBound" OnSelectedIndexChanged="grdDefects_OnSelectedIndexChanged"
                                                                 PageSize="50" meta:resourcekey="grdDefectsResource1" 
                                                                  ><%--onitemcommand="RadGrid1_ItemCommand"--%>
                                        
                                                    <HeaderStyle  HorizontalAlign="Center" Font-Bold="True"/>
                                                    <PagerStyle Mode="NextPrevAndNumeric" PagerTextFormat="" />
                                                
                                        
                                                    <MasterTableView Width="100%">
                                                        <NoRecordsTemplate/>
                                                        <Columns>
                                                            <telerik:GridHyperLinkColumn DataNavigateUrlFields="nDefectKey" DataTextField="cDefectName" 
                                                                                         HeaderText="Номер" meta:resourcekey="grdDefectsCol1"
                                                                                         DataNavigateUrlFormatString="~/Modules/Evaluation_defects/EvaluationDefects.aspx?id={0}" />
                                                            <telerik:GridBoundColumn DataField="nDefectKey" HeaderText="Ключ дефекта" Display="False" HeaderStyle-Width="0px" />
                                                            <telerik:GridBoundColumn DataField="nPipeElementMontajId" HeaderText="Ключ элемента монтажа" Display="False" HeaderStyle-Width="0px"/>
                                                           <%-- <telerik:GridBoundColumn DataField="cDefectName" HeaderText="Номер" meta:resourcekey="grdDefectsCol1"/>--%>
                                                            <telerik:GridBoundColumn DataField="nKm" HeaderText="Километраж" meta:resourcekey="grdDefectsCol2"/>
                                                            <telerik:GridBoundColumn DataField="cDefectType" HeaderText="Тип дефекта" meta:resourcekey="grdDefectsCol3"/>
                                                        </Columns>
                                                    </MasterTableView>
                                                    <ClientSettings EnablePostBackOnRowClick="True">
                                                        <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="2"></Scrolling>
                                                    </ClientSettings>
                                                    <%--<ClientSettings EnablePostBackOnRowClick="true">
                                                        <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="2"></Scrolling>
                                                        
                                                        <Selecting AllowRowSelect="True" />                     
    
                                                        

                                                    </ClientSettings>--%>
                                                  <%--  // функция OnCellSelected на javaScript пока не использую - нужно разобраться т.к. может вызвать только статическую функцию асп--%>
                                                      <%--<ClientSettings Selecting-CellSelectionMode="MultiCell">
                                                        <ClientEvents OnCellSelected="OnCellSelected" />
                                                    </ClientSettings>  --%>
                                                    <ItemStyle BackColor="#f9f9f9" />
                                                    <AlternatingItemStyle BackColor="#e8f0f5" />
                                                    <PagerStyle PageButtonCount="5"></PagerStyle>
                                                </telerik:RadGrid>
                                            
                                                <asp:Image ID="imgBusyIndicator" runat="server" ImageUrl="~/Images/progressbar_circle_48.GIF" CssClass="hideControl"
                                                           style="position: absolute; top: 40%; left: 50%; margin-left: -24px;"/> 
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top">
                                            <asp:Label runat="server" Text="Количество дефектов" meta:resourcekey="Label35Resource1"/>
                                            <telerik:RadTextBox ID="txtDefectCount" runat="server" ReadOnly="True" Width="50px" meta:resourcekey="txtDefectCountResource1"/>
                                            <telerik:RadButton ID="btnShowDefects" runat="server" Text="Таблица дефектов" AutoPostBack="False" Width="130"
                                                                Enabled="False" meta:resourcekey="btnShowDefectsResource1" OnClientClicked="showDefectList"/> 
                                            <%--вызов js функцию открытия формы--%>
                                            <%--OnClientClicked="showDefectList"--%>
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
  
                            <uc:DefectsList ID="ctrDefectsList" runat="server" style="display: none;" />

                        </ContentTemplate>
                    </asp:UpdatePanel>
                </td>
                <td rowspan="2" valign="top" style="padding-left: 0; margin-top: 0;">
                  
                    <fieldset class="con fsSetParam">
                        <legend class="legendtext"><%=GetLocalResourceObject("fldtResult") %></legend>
                        <table style="width: 100%; ">
                            <tr>
                                <td valign="top"  style="width: 200px; display: block; position: relative; ">
                                    <asp:Image ID="ImagePrev" runat="server" BorderColor="Black" BorderStyle="Solid" Width="200" Height="200"
                                               BorderWidth="1px" ImageUrl="Images/defect_type/empty.gif" meta:resourcekey="ImagePrevResource1" />
                                    <%--<asp:Panel ID="pnlImageInfo" runat="server" ToolTip="" CssClass="divInfo" Visible="False"/>--%>
                                </td>
                                 <td valign="top"  >
                                     
                                    <telerik:RadGrid runat="server" ID="rgResult" AutoPostBack="True" ShowHeader="False" Width="100%" >
                                        <ItemStyle BackColor="#f9f9f9" Height="34" />
                                        <AlternatingItemStyle BackColor="#e8f0f5" Height="33" />
                                        <MasterTableView Width="100%" NoDetailRecordsText=""/>
                                    </telerik:RadGrid>
                                         
                                </td>
                            </tr>
                        </table>

                       <%-- <fieldset>
                            
                        </fieldset> --%>
                         <fieldset >
                        <legend class="legendtext"><%=GetLocalResourceObject("fldDetalCalc") %></legend>
                        <telerik:RadGrid runat="server" ID="rgResultInfo" ShowHeader="True" CssClass="typeDefect">
                            <HeaderStyle HorizontalAlign="Center"/>
                            <ItemStyle BackColor="#f9f9f9" Height="30" />
                            <AlternatingItemStyle BackColor="#e8f0f5" Height="30" />
                        </telerik:RadGrid>
                        </fieldset>

                        <fieldset>
                            <legend class="legendtext"><%=GetLocalResourceObject("fldtRepaire") %></legend>
                            <%--class="resultTable"--%>
                            <table id="heightTable"  style="width: 100%; height: 100% " >
                                 <div class="box" style="position: relative;">
                                <tr>
                                    <td>
                                        <asp:Label runat="server" Font-Bold="True" Text="Розраховано" 
                                                   meta:resourcekey="Label101Resource1"/>
                                    </td>
                                    <td>
                                       <%-- <asp:CheckBox ID="CheckBoxCalc" runat="server" style="margin-right: 30px;" 
                                                       Enabled="False"/>--%>
                                        <asp:Label ID="labelCalc" runat="server" Width="50px"/>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" Font-Bold="True" Text="Шурфування" 
                                                   meta:resourcekey="Label2Resource1"/>
                                    </td>
                                    <td>
                                        <%--<asp:CheckBox ID="ckNBPITTING_CONFIRM" runat="server" style="margin-right: 30px;" 
                                                      meta:resourcekey="ckNBPITTING_CONFIRMResource1"  Enabled="False"/>--%>
                                        <asp:Label ID="labelPing" runat="server" Width="50px"/>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" Font-Bold="True" Text="Ремонт" 
                                                   meta:resourcekey="Label103Resource1"/>
                                    </td>
                                    
                                    <td>
                                        <%--<asp:CheckBox ID="ckNDEFECT_USTR" runat="server" meta:resourcekey="ckNDEFECT_USTRResource1" Enabled="False"/>--%>
                                        <asp:Label ID="labelUstr" runat="server" Width="50px"/>
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td>
                                        <asp:Label runat="server" Font-Bold="True" Text="Дата розрахунку"
                                                   meta:resourcekey="Label102Resource1"/>
                                    </td>

                                    <td>
                                        <asp:Label ID="LabelDateCalc" runat="server" Width="50px"
                                                   />
                                    </td>
                                    <td>
                                        <asp:Label runat="server" Font-Bold="True" Text="Дата шурфования"
                                                   meta:resourcekey="Label26Resource1"/>
                                    </td>

                                    <td>
                                        <asp:Label ID="txtNBPITTING_CONFIRM" runat="server" Width="50px"
                                                   meta:resourcekey="txtNBPITTING_CONFIRMResource1"/>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" Font-Bold="True" Text="Точность размеров (%)" 
                                                   meta:resourcekey="Label27Resource1"/>
                                    </td>
                                    <td>
                                        <asp:Label ID="txtNSIZE_PRECISION" runat="server" Width="50px" meta:resourcekey="txtNSIZE_PRECISIONResource1"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label runat="server" Font-Bold="True" Text="Рекомендований ремонт" meta:resourcekey="Label3Resource1"/>
                                    </td>
                                    
                                     <td>
                                        <asp:Label ID="LabelRecomRemont" runat="server" Width="100px" />
                                    </td>
                                    <td>
                                        <asp:Label runat="server" Font-Bold="True" Text="Дата ремонта" meta:resourcekey="Label28Resource1"/>
                                    </td>
                                    <td>
                                        <asp:Label ID="txtDDATA_USTR_DEFECT" runat="server" Width="50px" meta:resourcekey="txtDDATA_USTR_DEFECTResource1"/>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" Font-Bold="True" Text="Метод ремонта" meta:resourcekey="Label29Resource1"/>
                                    </td>
                                    <td>
                                        <asp:Label ID="txtcMETOD_USTR_DEFECT_KEY" runat="server" Width="100px" 
                                                   meta:resourcekey="txtcMETOD_USTR_DEFECT_KEYResource1"/>
                                    </td>
                                </tr>
                                </div>
                            </table>
                            
                            
                            
                            
                            
                            
                            

                          <%--   <legend class="legendtext"><%=GetLocalResourceObject("fldtPreasure") %></legend>
                           <table style="width: 100%;" class="resultTable">
                                <tr>
                                    <td>
                                        <asp:Label runat="server" Font-Bold="True" Text="Давление (МПа)" meta:resourcekey="Label31Resource1"/>
                                    </td>
                                    <td>
                                        <asp:Label ID="txtPress" runat="server" Width="30px" meta:resourcekey="txtPressResource1"/>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" Font-Bold="True" Text="Температура (&deg;С)" meta:resourcekey="Label32Resource1"/>
                                    </td>
                                    <td>
                                        <asp:Label ID="txtTempr" runat="server" Width="30px" meta:resourcekey="txtTemprResource1"/>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" Font-Bold="True" Text="Напряжение допустимое" meta:resourcekey="Label33Resource1"/>
                                    </td>
                                    <td>
                                        <asp:Label ID="txtndsAllow" runat="server" Width="30px" meta:resourcekey="txtndsAllowResource1"/>
                                    </td>
                                    <td>
                                        <asp:Label runat="server" Font-Bold="True" Text="Напряжение действующее" meta:resourcekey="Label34Resource1"/>
                                    </td>
                                    <td>
                                        <asp:Label ID="txtNdsReal" runat="server" Width="30px" meta:resourcekey="txtNdsRealResource1"/>
                                    </td>
                                </tr>
                            </table>--%>
                        </fieldset>
                    </fieldset>

                </td>
            </tr>
        </table>
    </asp:Panel>
    <style>
    /* заменяет дефолтный градиент выделения ячейки телерика на одноцветное*/
    #<%=grdDefects.ClientID%> .rgSelectedRow, #<%=grdDefects.ClientID%> .rgSelectedRow > td
    {
        background-color: #FAE185 !important;
        background-image: none !important;
    }
</style>

    <script type="text/javascript">

        window.ready = ResizeRadGrids();
        window.onresize = function (event) {
            ResizeRadGrids();
        };

        function ResizeRadGrids() {
            ResizeDefectList();
            ResizeFullDefectList();
        }

        <%--  function ResizeDefectList() {
            var screenHeight = $(window).height();
            var contentHeight = 300;
            var conInGridHeight = 80;
            var radGridHeight = screenHeight - contentHeight;

            // Прорисовка списка дефектов с пейджингом
            if ($('#<%=grdDefects.ClientID%> .rgDataDiv').length > 0) {
                radGridHeight = radGridHeight - conInGridHeight;
                $('#<%=grdDefects.ClientID%> .rgDataDiv').css('height', radGridHeight + 'px');
            } else {
                $('#<%=grdDefects.ClientID %>').css('height', radGridHeight + 'px');
            }
            
        }--%>

        function ResizeDefectList() {
            var screenHeight = $(window).height();
            var contentHeight = 320; //300
            var conInGridHeight = 80;
            var radGridHeight = screenHeight - contentHeight;

            // Прорисовка списка дефектов с пейджингом
            if ($('#<%=grdDefects.ClientID%> .rgDataDiv').length > 0) {
                radGridHeight = radGridHeight - conInGridHeight;
                $('#<%=grdDefects.ClientID%> .rgDataDiv').css('height', radGridHeight + 'px');
            } else {
                $('#<%=grdDefects.ClientID %>').css('height', radGridHeight + 'px');
            }

            //var contentInDefectRepear = 50;
            //var defectRepair = screenHeight - contentHeight;
            //defectRepair = defectRepair - contentInDefectRepear;
            //$(".box").height(defectRepair);

            <%--var contentInDefectRepear = 160;
            var defectRepair = screenHeight - contentHeigh;
            defectRepair = defectRepair - contentInDefectRepear;
            $('#<%=.ClientID%> ').css('height', defectRepair + 'px');
            //$(#heightTable).height($(document.body).height());
            //$('#<%=heightTable.ClientID%> ').css('height', defectRepair + 'px');--%>
           
        }


        function CleanDefect() {
            $("#<%=grdDefects.ClientID %>").remove();
            $("#<%=txtDefectCount.ClientID %>").val("");
            $("#<%=ImagePrev.ClientID %>").attr('src', 'Images/defect_type/empty.gif');
            $("#<%=txtNSIZE_PRECISION.ClientID %>").text("");
            $("#<%=txtDDATA_USTR_DEFECT.ClientID %>").text("");
            $("#<%=txtcMETOD_USTR_DEFECT_KEY.ClientID %>").text("");
           <%-- $("#<%=txtTempr.ClientID %>").text("");
            $("#<%=txtPress.ClientID %>").text("");
            $("#<%=txtndsAllow.ClientID %>").text("");
            $("#<%=txtNdsReal.ClientID %>").text("");--%>
        }

        function OnCellSelected(sender, eventArgs) {
            var Rows = eventArgs.get_gridDataItem();
            var columnName = eventArgs.get_column().get_uniqueName();
            var data = Rows.get_cell(columnName);
            var keyd = data.attributes['1'].textContent;
            // alert(data.attributes['1'].textContent);
            callCMethod(keyd);
        }
        //Вызов из JS ghjwtlehe asp 
        function callCMethod(parameterId) {
            PageMethods.CalledMethod(parameterId, OnSuccess);
        }

        function OnSuccess(userContext, methodName) {
           // alert(response);
        }






        function ShowBusyIndicator() {
           
            $("#<%=imgBusyIndicator.ClientID %>").removeClass("hideControl");
        }

        Sys.Application.add_load(ResizeRadGrids);
    </script>

</asp:Content>
