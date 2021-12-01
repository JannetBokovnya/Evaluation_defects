using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

using log4net;
using Telerik.Web.UI;


public partial class Modules_Evaluation_defects_Controls_ctrDefectsList : System.Web.UI.UserControl
{
    private static readonly ILog log = LogManager.GetLogger(typeof(Modules_Evaluation_defects_Controls_ctrDefectsList).Name);

   
    private void RestorePagePosition()
    {
        try
        {
            if (SessionStorage_EvalDef.GetItem("CurrentPageIndex") != null)
            {
                DataTable dt = (DataTable)SessionStorage_EvalDef.GetItem("DtDefectList");
                RadGrid1.DataSource = dt;
                RadGrid1.CurrentPageIndex = (int)SessionStorage_EvalDef.GetItem("CurrentPageIndexDtDefectList");
                RadGrid1.DataBind();


            }
        }
        catch (Exception ex)
        {
            ShowError(ex.Message, "Ошибка: " + ex.Message);
        }
    }
    public void FillGrid(DataTable dt, string threadName)
    {
        RadGrid1.DataSource = dt;
        RadGrid1.DataBind();
        SessionStorage_EvalDef.AddItem("DtDefectList", dt);

        Label2.Text = threadName;
    }

    //экспорт в excel
    protected void btnExportXsl_Click(object sender, EventArgs e)
    {
        try
        {
            GridView gv = new GridView { AutoGenerateColumns = false };


            foreach (GridBoundColumn column in RadGrid1.Columns)
            {
                if ((column.Visible) && (column.Display))
                {
                    BoundField col = new BoundField();
                    col.HeaderText = column.HeaderText;
                    //col.ItemStyle.HorizontalAlign = column.ItemStyle.HorizontalAlign;
                    col.ItemStyle.HorizontalAlign = HorizontalAlign.Right;
                    col.DataField = column.DataField;

                    gv.Columns.Add(col);
                }
            }


            DataTable dt = new DataTable();

            int columncount = 0;

            foreach (GridColumn column in RadGrid1.MasterTableView.Columns)
            {
                if (column.Visible && 
                    !String.IsNullOrEmpty(column.UniqueName) && 
                    !String.IsNullOrEmpty(column.HeaderText))
                {
                    columncount++;
                    dt.Columns.Add(column.UniqueName, typeof(string));
                }
            }

            DataRow dr;
            foreach (GridDataItem item in RadGrid1.MasterTableView.Items)
            {
                dr = dt.NewRow();

                for (int i = 0; i < columncount; i++)
                {
                    string colText = item[RadGrid1.MasterTableView.Columns[i].UniqueName].Text;

                    if (RadGrid1.MasterTableView.Columns[i].UniqueName == "cPipeNumber" ||
                        RadGrid1.MasterTableView.Columns[i].UniqueName == "cDefectName")
                    {
                        int index = colText.IndexOf('>');
                        if (index > 0)
                        {
                            colText = colText.Substring(index + 1);
                            index = colText.IndexOf('<');
                            if (index > 0)
                            {
                                colText = colText.Substring(0, index);
                            }
                        }

                    }

                    dr[i] = ConvertHtmlAllowDivToUserFrendlyText(colText);
                }

                dt.Rows.Add(dr);
            }

            gv.DataSource = dt;
            gv.DataBind();
            GridViewExportUtil_EvalDef.Export("export.xls", gv);
        }
        catch (Exception ex)
        {
            ShowError(ex.Message, "Ошибка: " + ex.Message);
        }
    }

    private void ShowError(string errMsg, string scriptMsgErr)
    {
        log.Error(errMsg);
        lblView2Err.Text = scriptMsgErr;
    }

    [WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    protected void btnLoadMap_Click(object sender, EventArgs e)
    {

  
        String csname1 = "PopupScript";
        Type cstype = this.GetType();

        // Get a ClientScriptManager reference from the Page class.
        ClientScriptManager cs = Page.ClientScript;

        // Check to see if the startup script is already registered.
        if (!cs.IsStartupScriptRegistered(cstype, csname1))
        {
            StringBuilder cstext1 = new StringBuilder();
            cstext1.Append("<script type=text/javascript> alert('Hello World!') </");
            cstext1.Append("script>");

            cs.RegisterStartupScript(cstype, csname1, cstext1.ToString());
        }



    }
    protected void RadGrid1_OnItemDataBound(object sender, GridItemEventArgs e)
    {
        if (e.Item is GridDataItem)
        {
            GridDataItem dataItem = (GridDataItem)e.Item;

            //dataItem["nKm"].Text = DelNullvalue(dataItem["cPipeNumber"].Text);
            dataItem["nKm"].Text = DelNullvalue(dataItem["nKm"].Text);
            dataItem["nLength"].Text = DelNullvalue(dataItem["nLength"].Text);
            dataItem["nX"].Text = DelNullvalue(dataItem["nX"].Text);
            dataItem["nY"].Text = DelNullvalue(dataItem["nY"].Text);
            dataItem["nWidth"].Text = DelNullvalue(dataItem["nWidth"].Text);
            dataItem["nDepth"].Text = DelNullvalue(dataItem["nDepth"].Text);
            dataItem["nClockwise_Pos"].Text = DelNullvalue(dataItem["nClockwise_Pos"].Text);


            if (dataItem["nPipeElementMontajId"].Text != "&nbsp;")
                dataItem["cPipeNumber"].Text =
                    "<span style='color:#1a6aba;text-decoration:underline;cursor:pointer' onclick=\"OpenPassport(" +
                    dataItem["nPipeElementMontajId"].Text + ")\">" + dataItem["cPipeNumber"].Text + "</span>";
            else
                dataItem["nPipeElementMontajId"].Text = "";
            if (dataItem["nDefectKey"].Text != "&nbsp;")
                dataItem["cDefectName"].Text = "<span style='color:#1a6aba;text-decoration:underline;cursor:pointer' onclick=\"OpenPassport(" + dataItem["nDefectKey"].Text + ")\">" + dataItem["cDefectName"].Text + "</span>";
                //#a4abb2
            else 
                dataItem["cPipeNumber"].Text = "";

            dataItem["nDangerLevelId_ASME"].Text = ChangeBindingText(dataItem["nDangerLevelId_ASME"].Text);
            dataItem["nDangerLevelId_DNV"].Text = ChangeBindingText(dataItem["nDangerLevelId_DNV"].Text);
            dataItem["nDangerLevelId_RSTRENG"].Text = ChangeBindingText(dataItem["nDangerLevelId_RSTRENG"].Text);
            dataItem["nDangerLevelId_VTD"].Text = ChangeBindingText(dataItem["nDangerLevelId_VTD"].Text);
        }


        GridSortingSettings sort = RadGrid1.SortingSettings;
        sort.SortToolTip = "Сортування";
        sort.SortedAscToolTip = "Сортування вверх";
        sort.SortedDescToolTip = "Сортування вниз";
        GridFilterMenu menu = RadGrid1.FilterMenu;
        menu.ToolTip = "Фильтр";

        foreach (RadMenuItem item in menu.Items)
        {    //change the text for the "StartsWith" menu item  
            if (item.Text == "StartsWith")
            {
                item.Text = "починаючи з";
            }
            if (item.Text == "NoFilter")
            {
                item.Text = "очистити фiльтр";
            }
            if (item.Text == "Contains")
            {
                item.Text = "містить";
            }
            if (item.Text == "EndsWith")
            {
                item.Text = "закінчуючи на";
            }
            if (item.Text == "DoesNotContain")
            {
                item.Text = "не містить";
            }
            if (item.Text == "EqualTo")
            {
                item.Text = "дорівнює";
            }
            if (item.Text == "NotEqualTo")
            {
                item.Text = "не дорівнює";
            }
            if (item.Text == "GreaterThan")
            {
                item.Text = "більше ніж";
            }
            if (item.Text == "LessThan")
            {
                item.Text = "менше ніж";
            }
            if (item.Text == "GreaterThanOrEqualTo")
            {
                item.Text = "більше або дорівнює";
            }
            if (item.Text == "LessThanOrEqualTo")
            {
                item.Text = "менше або дорівнює";
            }
            if (item.Text == "Between")
            {
                item.Text = "мiж";
            }
            if (item.Text == "NotBetween")
            {
                item.Text = "не мiж";
            }
            if (item.Text == "IsEmpty")
            {
                item.Text = "порожньо";
            }
            if (item.Text == "NotIsEmpty")
            {
                item.Text = "не порожньо";
            }
            if (item.Text == "IsNull")
            {
                item.Text = "нулл";
            }
            if (item.Text == "NotIsNull")
            {
                item.Text = "не нулл";
            }

        }
    }

    protected void RadGrid1_NeedDataSource(object sender, GridNeedDataSourceEventArgs e)
    {
        DataTable dt = (DataTable)SessionStorage_EvalDef.GetItem("DtDefectList");
        RadGrid1.DataSource = dt;
 

    }
    private string ChangeBindingText(string permission)
    {
        string result;
        switch (permission.ToUpper())
        {
            case "1":
                result = "<div style='width: 15px; height: 15px; background: green;'></div>";
                break;
            case "2":
                result = "<div style='width: 15px; height: 15px; background: orange;'></div>";
                break;
            case "3":
                result = "<div style='width: 15px; height: 15px; background: red;'></div>";
                break;
            default:
                result = "";
                break;
        }

        return result;
    }

    private string DelNullvalue(string itemValue)
    {
        string val = string.Empty;

        if (itemValue != "&nbsp;")
        {
            val = itemValue;
        }

        return val;
    }
    private string ConvertHtmlAllowDivToUserFrendlyText(string divHtml)
    {
        string result;
        switch (divHtml)
        {
            case "<div style='width: 15px; height: 15px; background: green;'></div>":
                result = GetLocalResourceObject("сPermissible").ToString();
                break;
            case "<div style='width: 15px; height: 15px; background: orange;'></div>":
                result = GetLocalResourceObject("сMiddlePermissible").ToString();
                break;
            case "<div style='width: 15px; height: 15px; background: red;'></div>":
                result = GetLocalResourceObject("сInPermissible").ToString();
                break;
            default:
                result = divHtml;
                break;
        }

        return result;
    }


    protected void RadGrid1_OnPageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)SessionStorage_EvalDef.GetItem("DtDefectList");
            RadGrid1.CurrentPageIndex = e.NewPageIndex;
            SessionStorage_EvalDef.AddItem("CurrentPageIndexDtDefectList", e.NewPageIndex);

            RadGrid1.DataSource = null;
            RadGrid1.DataSource = dt;
            RadGrid1.DataBind();
        }
        catch (Exception ex)
        {
            ShowError(ex.Message, "Ошибка: " + ex.Message);
        }
    }
}