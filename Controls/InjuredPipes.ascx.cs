using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Text;
//using App_Code.Evaluation_defects_API;

public partial class Controls_InjuredPipes : System.Web.UI.UserControl
{
    private void CreateReport()
    {
        if (SessionStorage_EvalDef.GetItem("DefectReport") != null)
        {
            if (!IsPostBack)
            {
                DefectReportStruct_EvalDef drs = new DefectReportStruct_EvalDef();
                OracleDefects_EvalDef od = new OracleDefects_EvalDef();

                drs = (DefectReportStruct_EvalDef)SessionStorage_EvalDef.GetItem("DefectReport");
                string errStr = "";
                DataTable dt;                
                txtMG.Text += " <b>" + drs.MgName + "</b>";
                txtThread.Text += " <b>" + drs.ThreadName + "</b>";
                TxtUchastok.Text += " <b> от" + drs.IntKmStart.ToString() + " - км " + drs.IntKmEnd.ToString() + " </b>";
                txtCondition.Text += " <b>" + drs.FiltrName + "</b>";

                txtRegimTransp.Text += " <b>" + drs.TransportMode + "</b>";

              //  dt = od.GetInjuredPipe(drs.IntPipeKey, drs.IntKmStart, drs.IntKmEnd, drs.FiltrKey, drs.IntModeKey, out errStr);
                if (errStr != "")
                {
                    Response.Write(errStr);
                }
                else
                {
                    //txtPipeCount.Text += " <b>" + dt.Rows.Count.ToString() + " </b>";
                    //GridView1.DataSource = dt;
                    //GridView1.DataBind();
                    //SessionStorage_EvalDef.AddItem("InjuredPipe", dt);
                }
            }
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        CreateReport();
    }
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //DataSet ds = new DataSet();
        DataTable ds = new DataTable();
        ds = (DataTable)SessionStorage_EvalDef.GetItem("InjuredPipe");
        GridView1.PageIndex = e.NewPageIndex;
        string errStr = "";
        if (errStr != "")
        {
            Response.Write(errStr);
        }
        else
        {
            GridView1.DataSource = ds;
            GridView1.DataBind();
        }
    }
    protected void RemontMapPipe_Click(object sender, EventArgs e)
    {
    }

    protected void GridView1_OnSelectedIndexChanged(object sender, EventArgs e)
    {
    }

    protected void Button1_Click(object sender, EventArgs e)
    {


        DefectReportStruct_EvalDef drs = new DefectReportStruct_EvalDef();
        drs = (DefectReportStruct_EvalDef)SessionStorage_EvalDef.GetItem("DefectReport");
       // DataTable dt = new OracleDefects_EvalDef().GetGetDefectSummaryForRepair(drs.IntPipeKey, drs.IntKmStart, drs.IntKmEnd, drs.IntModeKey, drs.FiltrKey);
        StringBuilder cSection = new StringBuilder();
        StringBuilder urlRedirect = new StringBuilder();
        //foreach (DataRow row in dt.Rows)
        //{
        //    foreach (DataColumn column in dt.Columns)
        //    {
        //        if (row[column] != null)
        //        {
        //            if (column.Caption.ToLower() == "nKey".ToLower())
        //            {
        //                cSection.Append(row[column]);
        //                cSection.Append("!");
        //            }
        //        }
        //    }
        //}

        //DataSet ds = new DataSet();
        DataTable ds = new DataTable();
        ds = (DataTable)SessionStorage_EvalDef.GetItem("InjuredPipe");

        foreach (DataRow row in ds.Rows)
        {
            foreach (DataColumn column in ds.Columns)
            {
                if (row[column] != null)
                {
                    if (column.Caption.ToLower() == "pipeElementMont".ToLower())//"numVtd".ToLower())
                    {
                        cSection.Append(row[column]);
                        cSection.Append("!");
                    }
                }
            }
        }

        urlRedirect.Append("remontMap-bin-release/main.aspx?firstPoin=");
        urlRedirect.Append((drs.IntKmStart * 1000).ToString());
        urlRedirect.Append("&SecondPoint=");
        urlRedirect.Append((drs.IntKmEnd * 1000).ToString());
        urlRedirect.Append("&keyThred=");
        urlRedirect.Append(drs.IntPipeKey.ToString());
        urlRedirect.Append("&cSection=");
        urlRedirect.Append(cSection.ToString().TrimEnd('!'));
        urlRedirect.Append("&cTranportMode=");
        urlRedirect.Append(drs.IntModeKey.ToString());
        urlRedirect.Append("&cDefStandart=");
        urlRedirect.Append(drs.DefectStandartKey.ToString());

        //Response.Redirect(urlRedirect.ToString());



        Response.Write("<script>window.open('" + urlRedirect.ToString() + "', 'NewZnakName', 'width=1024,height=768,toolbar=no,scrollbars=no,directories=no,status=no,menubar=no,resizable=yes')</script>");

        //Response.Redirect("remontMap-bin-release/main.swf?firstPoin=200000&SecondPoint=300000&keyThred=542203&cSection=215117006!215117006!215117006!215117006!215117006!215120306!215158806!215189606!215189606!215189606!215189606!215189606!215196206!215196206!215196206!215196206!215309506!215309506!215309506!215309506!215309506!215309506!215309506!215309506!215313906!215313906!215320506!215321606!215321606!215397506!215638206!217414706!217459806!217471906!217471906!217471906!217487306&cTranportMode=84472301&cDefStandart=258901");
    }
}
