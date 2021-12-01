using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
//using App_Code.Evaluation_defects_API;
using log4net;

public partial class DefectTableReport : System.Web.UI.Page
{
    private static readonly ILog log = LogManager.GetLogger(typeof(DefectTableReport).Name);

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //if (Session["DefectReport"] != null)
            if (SessionStorage_EvalDef.GetItem("DefectReport") != null)
            {
                if (!IsPostBack)
                {
                    DefectReportStruct_EvalDef drs = new DefectReportStruct_EvalDef();

                    OracleDefects_EvalDef od = new OracleDefects_EvalDef();
 
                    drs = (DefectReportStruct_EvalDef)SessionStorage_EvalDef.GetItem("DefectReport");

                    string errStr = "";
                    DataTable dt = new DataTable();
                    dt = od.GetDefectSummary(drs.IntPipeKey, drs.IntKmStart, drs.IntKmEnd, drs.IntModeKey, out errStr);
                    lblMG.Text += " <b>" + drs.MgName + "</b>";
                    lblThread.Text += " <b>" + drs.ThreadName + "</b>";
                    lblDiapozon.Text += " <b> от" + drs.IntKmStart.ToString() + " - км " + drs.IntKmEnd.ToString() + " </b>";
                    lblFiltr.Text += " <b>" + drs.FiltrName + "</b>";
                    lblDefectCount.Text += " <b>" + drs.SelectedDefectCount + " </b>";//+" из "+drs.AllDefectCount+ " </b>";
                    lblTranspMode.Text += " <b>" + drs.TransportMode + "</b>";



                    if (errStr != "")
                    {
                        log.Error(errStr);
                        Response.Write(errStr);
                    }
                    else
                    {
                        GridView1.PageSize = (dt.Rows.Count < 300 ? 300 : 20);
                        GridView1.DataSource = dt;
                        GridView1.DataBind();


                        ColorMarkDefect(ref dt);
                        Session["sortRule"] = "cname";
                    }
                }
            }
        }
        catch
        {
            throw;
        }
    }

    //Перегружаем "Культуру" для данной страницы (этот метод вызвается самым первым, раньше всех других)
    protected override void InitializeCulture()
    {

        if (Request.Form["ctl00$Content$pnlLogin$rblLanguage"] != null)//Если пользователь выбрал язык в окне Логина
        {
            String selectedLanguage = Request.Form["ctl00$Content$pnlLogin$rblLanguage"];
            UICulture = selectedLanguage;
            Culture = selectedLanguage;

            Thread.CurrentThread.CurrentCulture =
                CultureInfo.CreateSpecificCulture(selectedLanguage);
            Thread.CurrentThread.CurrentUICulture = new
                CultureInfo(selectedLanguage);

            //Сохраняем выбранную пользователем культуру в сессионной переменной Session["lang"]
            HttpContext.Current.Session["lang"] = (selectedLanguage == "ru-RU") ? "ru-RU" : "en-US";
        }
        else if (HttpContext.Current.Session["lang"] != null)//Если выполнен переход на страницу Логина (нажата кнопка "Выйти")
        {
            String selectedLanguage = Session["lang"].ToString();
            UICulture = selectedLanguage;
            Culture = selectedLanguage;

            Thread.CurrentThread.CurrentCulture =
                CultureInfo.CreateSpecificCulture(selectedLanguage);
            Thread.CurrentThread.CurrentUICulture = new
                CultureInfo(selectedLanguage);

        }
        else//Во всех других случаях, когда сессионная переменная Session["lang"] - пуста
        {
            //Сохраняем в сессии культуру "русского языка" (по-умолчанию)
            HttpContext.Current.Session["lang"] = "ru-RU";
        }
        base.InitializeCulture();
    }

    private void ColorMarkDefect(ref DataTable dt)
    {
        try
        {
            for (int i = 0; i < GridView1.Rows.Count; i++)
            {
                GridView1.Rows[i].Cells[0].Text = "<a href=\"" + ConfigurationSettings.AppSettings["EditPassport"] + "0&passportKey=" + dt.Rows[i][0].ToString() + "&keyUser=" + Session["loggedin"].ToString() + "\"target=\"_blank\">" + GridView1.Rows[i].Cells[0].Text + "</a>";
                //dnv
                if (GridView1.Rows[i].Cells[12].Text.Trim().ToLower() == "недопустимый")
                {
                    GridView1.Rows[i].Cells[12].BackColor = System.Drawing.Color.Red;
                }
                else if (GridView1.Rows[i].Cells[12].Text.Trim().ToLower() == "допустимый")
                {
                    GridView1.Rows[i].Cells[12].BackColor = System.Drawing.Color.Green;
                }
                else if (GridView1.Rows[i].Cells[12].Text.Trim().ToLower().Replace(" ", "") == "условнонедопустимый")
                {
                    GridView1.Rows[i].Cells[12].BackColor = System.Drawing.Color.Yellow;
                }

                GridView1.Rows[i].Cells[12].ForeColor = System.Drawing.Color.Black;

                //-------------------------

                if (GridView1.Rows[i].Cells[13].Text.Trim().ToLower() == "недопустимый")
                {
                    GridView1.Rows[i].Cells[13].BackColor = System.Drawing.Color.Red;
                }
                else if (GridView1.Rows[i].Cells[13].Text.Trim().ToLower() == "допустимый")
                {
                    GridView1.Rows[i].Cells[13].BackColor = System.Drawing.Color.Green;
                }
                else if (GridView1.Rows[i].Cells[13].Text.Trim().ToLower().Replace(" ", "") == "условнонедопустимый")
                {
                    GridView1.Rows[i].Cells[13].BackColor = System.Drawing.Color.Yellow;
                }

                GridView1.Rows[i].Cells[13].ForeColor = System.Drawing.Color.Black;

                //-------------------------
                if (GridView1.Rows[i].Cells[14].Text.Trim().ToLower() == "недопустимый")
                {
                    GridView1.Rows[i].Cells[14].BackColor = System.Drawing.Color.Red;
                }
                else if (GridView1.Rows[i].Cells[14].Text.Trim().ToLower() == "допустимый")
                {
                    GridView1.Rows[i].Cells[14].BackColor = System.Drawing.Color.Green;
                }
                else if (GridView1.Rows[i].Cells[14].Text.Trim().ToLower().Replace(" ", "") == "условнонедопустимый")
                {
                    GridView1.Rows[i].Cells[14].BackColor = System.Drawing.Color.Yellow;
                }

                GridView1.Rows[i].Cells[14].ForeColor = System.Drawing.Color.Black;

                //-------------------------
                if (GridView1.Rows[i].Cells[15].Text.Trim().ToLower() == "недопустимый")
                {
                    GridView1.Rows[i].Cells[15].BackColor = System.Drawing.Color.Red;
                }
                else if (GridView1.Rows[i].Cells[15].Text.Trim().ToLower() == "допустимый")
                {
                    GridView1.Rows[i].Cells[15].BackColor = System.Drawing.Color.Green;
                }
                else if (GridView1.Rows[i].Cells[15].Text.Trim().ToLower().Replace(" ", "") == "условнонедопустимый")
                {
                    GridView1.Rows[i].Cells[15].BackColor = System.Drawing.Color.Yellow;
                }

                GridView1.Rows[i].Cells[15].ForeColor = System.Drawing.Color.Black;

                //-------------------------
                if (GridView1.Rows[i].Cells[16].Text.Trim().ToLower() == "недопустимый")
                {
                    GridView1.Rows[i].Cells[16].BackColor = System.Drawing.Color.Red;
                }
                else if (GridView1.Rows[i].Cells[16].Text.Trim().ToLower() == "допустимый")
                {
                    GridView1.Rows[i].Cells[16].BackColor = System.Drawing.Color.Green;
                }
                else if (GridView1.Rows[i].Cells[16].Text.Trim().ToLower().Replace(" ", "") == "условнонедопустимый")
                {
                    GridView1.Rows[i].Cells[16].BackColor = System.Drawing.Color.Yellow;
                }

                GridView1.Rows[i].Cells[16].ForeColor = System.Drawing.Color.Black;

                //-------------------------
                if (GridView1.Rows[i].Cells[17].Text.Trim().ToLower() == "закритическая")
                {
                    GridView1.Rows[i].Cells[17].BackColor = System.Drawing.Color.Red;
                }
                else if (GridView1.Rows[i].Cells[17].Text.Trim().ToLower() == "докритическая" |
                    GridView1.Rows[i].Cells[17].Text.Trim().ToLower() == "Незначительная".ToLower())
                {
                    GridView1.Rows[i].Cells[17].BackColor = System.Drawing.Color.Green;
                }
                else if (GridView1.Rows[i].Cells[17].Text.Trim().ToLower().Replace(" ", "") == "Критическая".ToLower())
                {
                    GridView1.Rows[i].Cells[17].BackColor = System.Drawing.Color.Yellow;
                }

                GridView1.Rows[i].Cells[17].ForeColor = System.Drawing.Color.Black;

            }
        }
        catch
        {
            throw;
        }
    }
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            DataTable dt;
            OracleDefects_EvalDef od = new OracleDefects_EvalDef();
            DefectReportStruct_EvalDef drs = new DefectReportStruct_EvalDef();
            drs = (DefectReportStruct_EvalDef)SessionStorage_EvalDef.GetItem("DefectReport");//(defectReportStruct_EvalDef)Session["DefectReport"];
            GridView1.PageIndex = e.NewPageIndex;
            string errStr = "";
            dt = od.GetDefectSummary(drs.IntPipeKey, drs.IntKmStart, drs.IntKmEnd, drs.IntModeKey,  out errStr);
            if (errStr != "")
            {
                log.Error(errStr);
                Response.Write(errStr);
            }
            else
            {
                GridView1.DataSource = dt;
                GridView1.DataBind();
                ColorMarkDefect(ref dt);
            }
        }
        catch
        {
            throw;
        }
    }
    protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
    {
        try
        {
            DataTable dt;
            OracleDefects_EvalDef od = new OracleDefects_EvalDef();
            DefectReportStruct_EvalDef drs = new DefectReportStruct_EvalDef();
            //drs = (defectReportStruct_EvalDef)Session["DefectReport"];
            drs = (DefectReportStruct_EvalDef)SessionStorage_EvalDef.GetItem("DefectReport");
            string errStr = "";
            dt = od.GetDefectSummary(drs.IntPipeKey, drs.IntKmStart, drs.IntKmEnd, drs.IntModeKey, out errStr);

            //DataTable dt = ds.Tables[0];
            //создадим DataView
            DataView dv = new DataView(dt);


            //Session["sortRule"] = e.SortExpression.ToUpper();
            //определимся с тем, что и как нужно сортировать
            if (e.SortExpression + " ASC" == Session["sortRule"].ToString())
                Session["sortRule"] = e.SortExpression + " DESC";
            else Session["sortRule"] = e.SortExpression + " ASC";

            //отсортируем DataView по сформированному правилу
            dv.Sort = Session["sortRule"].ToString();

            //связывание данных
            GridView1.DataSource = dv;
            GridView1.DataBind();
            ColorMarkDefect(ref dt);
        }
        catch
        {
            throw;
        }
    }
}
