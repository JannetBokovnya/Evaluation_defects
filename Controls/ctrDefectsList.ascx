<%@ Control Language="C#" AutoEventWireup="true" CodeFile="ctrDefectsList.ascx.cs" Inherits="Modules_Evaluation_defects_Controls_ctrDefectsList" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI, Version=2014.1.225.40, Culture=neutral, PublicKeyToken=121fae78165ba3d4" %>

<style type="text/css">
        <%--#<%=RadGrid1.ClientID%> .rgDataDiv {
            height: 100% !important;
        }--%>

    .userImageButton {
        behavior: url(../../Scripts/CSS3Converter/PIE.htc);
        background:#e5e5e5;
        border-radius: 4px;
        height: 32px !important;
        width: 24px !important;
        line-height: 32px; 
    }

    .RadGrid_Office2010Silver, .RadGrid_Office2010Silver .rgMasterTable, .RadGrid_Office2010Silver .rgDetailTable, 
    .RadGrid_Office2010Silver .rgGroupPanel table, .RadGrid_Office2010Silver .rgCommandRow table, .RadGrid_Office2010Silver .rgEditForm table, 
    .RadGrid_Office2010Silver .rgPager table, .GridToolTip_Office2010Silver {
        font-size: 11px;
    }
</style>
<asp:Panel ID="Def" runat="server" style="position: absolute; background: #f9f9f9; top: 0; bottom: 0; left: 0; right: 0; margin: 0; padding: 0; display: none;">
    <asp:Label runat="server" ID="lblView2Err"/>
     
    <div style="margin: 20px;">
        <div style=" float: left; height: 35px; text-align: center;">
            <div style="padding-top: 5px;">
                <telerik:RadButton runat="server" CssClass="userImageButton" ButtonType="LinkButton"  AutoPostBack="False" 
                                   OnClientClicked="hideDefectList">
                    <Icon PrimaryIconUrl="~/Images/left-24.png" PrimaryIconTop="4" PrimaryIconLeft="4" 
                            PrimaryIconWidth="32" PrimaryIconHeight="32" />
                </telerik:RadButton>
                <!-- ID используется в UpdatePanel в родительском контроле -->
                <telerik:RadButton ID="RadButton2" runat="server" CssClass="userImageButton" ButtonType="LinkButton" 
                                   OnClick="btnExportXsl_Click"  >
                    <Icon PrimaryIconUrl="~/Images/to_xls-24.png" PrimaryIconTop="3" PrimaryIconLeft="4" 
                            PrimaryIconWidth="32" PrimaryIconHeight="32" />
                </telerik:RadButton>
                 <telerik:RadButton ID="RadButton3" runat="server" CssClass="userImageButton" ButtonType="LinkButton" 
                                  AutoPostBack="False" OnClientClicked="OpenTest" ToolTip="Локалізувати на карті">
                     <%--OnClientClicked="OpenTest" OnClick="btnLoadMap_Click"--%>
                    <Icon PrimaryIconUrl="~/Images/thematic_24.png" PrimaryIconTop="3" PrimaryIconLeft="4" 
                            PrimaryIconWidth="32" PrimaryIconHeight="32" />
                </telerik:RadButton>
            </div>
        </div>
        <div style="width: 100%; text-align: center; height: 35px;">
            <asp:Label runat="server" CssClass="lblGroup" meta:resourcekey="LabelResource1">Список дефектов</asp:Label>
            <br/>
            <asp:Label ID="Label2" runat="server" Font-Size="11px" CssClass="lblGroup"/>
        </div>
    </div>
   <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel1" runat="server">
    </telerik:RadAjaxLoadingPanel> 
     <telerik:RadAjaxPanel runat="server" LoadingPanelID="RadAjaxLoadingPanel1">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
     <ContentTemplate>
     
        
  
    <div style="margin: 20px;">
        
        
        <telerik:RadGrid RenderMode="Lightweight" ID="RadGrid1" runat="server" AutoGenerateColumns="False" CellPadding="4" Font-Size="11pt"
                         ForeColor="#333333" GridLines="None" Width="100%" DataKeyNames="nDefectKey" OnPageIndexChanged="RadGrid1_OnPageIndexChanged"
                         AllowSorting="true"  ShowHeader="True" AllowPaging="True" PageSize="50" OnItemDataBound="RadGrid1_OnItemDataBound"
                         OnNeedDataSource="RadGrid1_NeedDataSource" AllowFilteringByColumn="True" >
                                       
            <HeaderStyle Width="100px" HorizontalAlign="Center" Font-Bold="True" />
            <ItemStyle Width="100px" BackColor="#f9f9f9" />
            <AlternatingItemStyle Width="100px" BackColor="#e8f0f5" />
            
            
            <MasterTableView Width="100%">
                <PagerStyle Mode="NextPrevAndNumeric" PageSizeLabelText="Page Size: " PageSizes="50,100,200" />

                <NoRecordsTemplate/>
                <Columns>
                    <telerik:GridBoundColumn DataField="cPipeNumber" HeaderText="Номер трубы" meta:resourcekey="GridBoundColumnResource11">
                        <ItemStyle HorizontalAlign="Center"  />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="nPipeElementMontajId" HeaderText="Ключ элемента монтажа" Display="False"/>
                    <telerik:GridBoundColumn DataField="cDefectName" HeaderText="Номер дефекта" meta:resourcekey="GridBoundColumnResource12">
                        <ItemStyle HorizontalAlign="Center" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="nDefectKey" HeaderText="Ключ дефекта1" Display="False" />
                    <telerik:GridBoundColumn DataField="cDefectTypeId" HeaderText="Тип дефекта" meta:resourcekey="GridBoundColumnResource2">
                        <ItemStyle HorizontalAlign="Center" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="nKm" HeaderText="Километраж" meta:resourcekey="GridBoundColumnResource3">
                        <ItemStyle HorizontalAlign="Right" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="nLength" HeaderText="Длина, мм" meta:resourcekey="GridBoundColumnResource4">
                        <ItemStyle HorizontalAlign="Right" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="nWidth" HeaderText="Ширина, мм" meta:resourcekey="GridBoundColumnResource5">
                        <ItemStyle HorizontalAlign="Right" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="nDepth" HeaderText="Глубина, мм" meta:resourcekey="GridBoundColumnResource6">
                        <ItemStyle HorizontalAlign="Right" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="nClockwise_Pos" HeaderText="Угол, часы" meta:resourcekey="GridBoundColumnResource7">
                        <ItemStyle HorizontalAlign="Right" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="nDangerLevelId_ASME" HeaderText="ASME" >
                        <ItemStyle HorizontalAlign="Center" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="nDangerLevelId_DNV" HeaderText="DNV" >
                        <ItemStyle HorizontalAlign="Center" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="nDangerLevelId_RSTRENG" HeaderText="RSTRENG" >
                        <ItemStyle HorizontalAlign="Center" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="nDangerLevelId_VTD" HeaderText="ВТД"  meta:resourcekey="GridBoundColumnResource13">
                        <ItemStyle HorizontalAlign="Center" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn  DataField="nX" HeaderText="X" Display="False" >
                        <ItemStyle HorizontalAlign="Center" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn  DataField="nY" HeaderText="Y" Display="False" >
                        <ItemStyle HorizontalAlign="Center" />
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn  HeaderText="Рекомендований ремонт" DataField="сRecomMethodRepair"  meta:resourcekey="GridBoundColumnResource14">
                        <ItemStyle HorizontalAlign="Center" />
                    
                    </telerik:GridBoundColumn>
                    <telerik:GridBoundColumn DataField="nPipeElementMontajId" Visible="False" />
                    <telerik:GridBoundColumn DataField="nDefectKey" HeaderText="Ключ дефекта" Visible="False" />
                </Columns>
            </MasterTableView>
            <ClientSettings>
                <Scrolling AllowScroll="True" UseStaticHeaders="True" SaveScrollPosition="true" FrozenColumnsCount="2"></Scrolling>
            </ClientSettings>

        </telerik:RadGrid>
              
    </div>

     </ContentTemplate>
     </asp:UpdatePanel>
    </telerik:RadAjaxPanel >

</asp:Panel>

<script type="text/javascript">
    function showDefectList() {
        
        $(".con").css('display', 'none');
        $("#<%=Def.ClientID %>").css('display', 'block');
    }

   

    function hideDefectList() {
        $("#<%=Def.ClientID %>").css('display', 'none');
        $(".con").css('display', 'block');
    }

    function ResizeFullDefectList() {
        var screenHeight = $(window).height() - 180;

        // Прорисовка всего списка дефектов
        if ($('#<%=RadGrid1.ClientID%> .rgDataDiv').length > 0) {
            $('#<%=RadGrid1.ClientID%> .rgDataDiv').css('height', screenHeight-50 + 'px');
        } else {
            $('#<%=RadGrid1.ClientID %>').css('height', screenHeight-50 + 'px');
        }
    }

    function OpenPassport(passportKey) {
        sendEvent('SHOW_PASSPORT', '{"NOBJKEY":"' + passportKey + '"}', 'PASSPORT_' + passportKey);
       // sendEvent('SHOW_PASSPORT', '{"NOBJKEY":"' + passportKey + '"}', 'PASSPORT' );
    }

    function OpenTest() {

   
        var masterTable = $find("<%=RadGrid1.ClientID%>").get_masterTableView();
        var dataItems = masterTable.get_dataItems();
        var columns = masterTable.get_columns();
        var uniqueNames = [];
        var cellValue = '';
        var str = '';
        var color = '0xff0800';
  
        //alert("dataItems.length=" + dataItems.length);

        for (var i = 0; i < dataItems.length; i++) {
            if (i===500) {
               // alert("Всего может быть отображено 200 дефектов!");
                break;
            }

            var row = dataItems[i];

            var nX = row.get_cell("nX").innerHTML;
            var nY = row.get_cell("nY").innerHTML;

            str = str + '<GRAPHICS><COLOR>'+color+'</COLOR><WIDTH>30</WIDTH><ALPHA>0.7</ALPHA><STYLE>circle</STYLE>' +
                 '<TYPEFIGYRE>point</TYPEFIGYRE><POINTS><X>' + nX.replace(",", ".") + '</X><Y>' + nY.replace(",", ".") + '</Y></POINTS>' +
                 '<SPATIALREFERENCE>4326</SPATIALREFERENCE><TOOLTIP>Дефекти металу</TOOLTIP></GRAPHICS>';

            //alert("str=" + str);

        }

        sendEvent("SHOW_THEMATICOBJECT_ON_MAP", '"' + str.toString() + '"', "MAP");
        

       
    }


    //for (var c = 0; c < columns.length; c++) {
    //    var uniquName = columns[c].get_uniqueName();
    //   // alert("uniquName=" + uniquName);
    //    if (uniquName === 'nX') {
    //        var cell = row.get_cell(columns[c]);
    //        alert("cell=" + cell);
    //        // cellValue = cell.innerText;
    //        alert("cell=" + cell);
    //    }
    //}


    //for (var j = 0; j < uniqueNames.length; j++) {
    //    //var column = masterTable.getColumnByUniqueName(uniqueNames[j]);
    //    var cell = row.get_cell(uniqueNames[j]);
    //    var cellValue = cell.innerText;
    //    tableCellsString += '<td>' + cellValue + '</td>';
    //}

</script>            