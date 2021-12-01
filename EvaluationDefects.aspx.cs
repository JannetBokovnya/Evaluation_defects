using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Services;
using System.Web.Services.Description;
using System.Web.UI;
using System.Web.UI.WebControls;
using log4net;
using Telerik.Web.UI;

public partial class Modules_Evaluation_defects_EvaluationDefects : System.Web.UI.Page
{

    private const string ImagePath = "Images/defect_type/";
    private static readonly ILog log = LogManager.GetLogger(typeof(Modules_Evaluation_defects_EvaluationDefects).Name);
    private OracleLayer_EvalDef ol = new OracleLayer_EvalDef();

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            SetModuleName("EVALUATIONDEFECTS");

            ScriptManager scriptManager = ScriptManager.GetCurrent(this.Page);
            scriptManager.RegisterPostBackControl(ctrDefectsList.FindControl("RadButton2"));

           
            
            //если страница загружается впервой 
            //тогда очищаем все элементы управления   формирования
            //списка дедфектов
            //и наполняем  список МГ
            if (!IsPostBack)
            {
                if (String.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    SessionStorage_EvalDef.DisposeStorage();
                }

                DDLMG.Attributes.Add("onchange", "CleanDefect()");
                DDLThread.Attributes.Add("onchange", "CleanDefect()");
                DDlTranspMode.Attributes.Add("onchange", "CleanDefect()");

                CleanDefectparam();

                // Наполняем список МГ
                FillMgList();

                string defectParam = Request.QueryString["id"]; //получаем ключ выбранного дефекта             

                if (!string.IsNullOrEmpty(defectParam))
                {
                    RestoreControlStateFromSession();

                    double tmp;
                    double.TryParse(defectParam, out tmp);
                    if (tmp > 0)
                    {
                        //сохраняем  ключ дефекта  
                        ViewState["defectKey"] = defectParam;

                        //наполнение таблиц расчетами для одного, выбранного дефекта
                        FillCalc();

                        if (SessionStorage_EvalDef.GetItem("defectDataSet") != null)
                        {
                            DataTable dt = (DataTable)SessionStorage_EvalDef.GetItem("defectDataSet");
                            txtDefectCount.Text = dt.Rows.Count.ToString();
                            //восстанавливаем позицию в  страницы в таблице дефектов
                            RestorePagePosition();

                            //выделяем цветом строку выбранного дефекта
                            int count = 0;
                            foreach (DataRow row in dt.Rows)
                            {
                                count++;

                                string metalDefectKey = row["nDefectKey"].ToString();
                                if (!String.IsNullOrEmpty(metalDefectKey))
                                {
                                    string defaultKey = metalDefectKey.Split('&')[0];

                                    int realDefectPos = 0;
                                    //Чтение порядкового номера дефекта
                                    if (SessionStorage_EvalDef.GetItem("CurrentPageIndex") != null)
                                    {
                                        int.TryParse(SessionStorage_EvalDef.GetItem("CurrentPageIndex").ToString(),
                                            out realDefectPos);
                                        realDefectPos = grdDefects.PageSize * realDefectPos;
                                    }

                                    if (defaultKey == Request.QueryString["id"])
                                    {
                                        grdDefects.Items[count - 1 - realDefectPos].Selected = true;
                                        break;
                                    }
                                }
                            }

                            //восстанавливаем контрол с таблицей -суммдефект
                            DataTable dtsumDef = new DataTable();
                            dtsumDef = (DataTable)SessionStorage_EvalDef.GetItem("dtSumDefect");
                            string defectListName = SessionStorage_EvalDef.GetItem("defectListName").ToString();

                            ctrDefectsList.FillGrid(dtsumDef, defectListName);


                        }
                    }
                    else
                    {
                        ShowError("Для выбранного дефекта отсутствуют параметры.",
                            "Для выбранного дефекта отсутствуют параметры.");
                    }
                }
                else
                {
                    rgResult.DataSource = EmptyDtForRgResult();
                    rgResult.DataBind();
                    rgResultInfo.DataSource = EmptyDtForRgResultInfo();
                    rgResultInfo.DataBind();
                }
            }
        }
        catch (Exception ex)
        {
            ShowError(ex.Message, "Ошибка: " + ex.Message);
        }
    }

    //Перегружаем "Культуру" для данной страницы (этот метод вызвается самым первым, раньше всех других)
    protected override void InitializeCulture()
    {
        if (HttpContext.Current.Session["lang"] != null)
        //Если выполнен переход на страницу Логина (нажата кнопка "Выйти")
        {
            String selectedLanguage = Session["lang"].ToString();
            UICulture = selectedLanguage;
            Culture = selectedLanguage;

            Thread.CurrentThread.CurrentCulture =
                CultureInfo.CreateSpecificCulture(selectedLanguage);
            Thread.CurrentThread.CurrentUICulture = new
                CultureInfo(selectedLanguage);

        }
        else //Во всех других случаях, когда сессионная переменная Session["lang"] - пуста
        {
            //Сохраняем в сессии культуру "русского языка" (по-умолчанию)
            HttpContext.Current.Session["lang"] = "ru-RU";
        }
        base.InitializeCulture();
    }

    private void SetModuleName(string pCModuleName)
    {
        try
        {
            System_TopMasterPage master = (System_TopMasterPage)Master;
            if (master != null) master.SetModuleName(pCModuleName);
        }
        catch (Exception ex)
        {
            ShowError(ex.Message, "Ошибка: " + ex.Message);
        }
    }



    private void RestorePagePosition()
    {
        try
        {
            if (SessionStorage_EvalDef.GetItem("CurrentPageIndex") != null)
            {
                DataTable dt = (DataTable)SessionStorage_EvalDef.GetItem("defectDataSet");
                grdDefects.DataSource = dt;
                grdDefects.CurrentPageIndex = (int)SessionStorage_EvalDef.GetItem("CurrentPageIndex");
                grdDefects.DataBind();
            }
        }
        catch (Exception ex)
        {
            ShowError(ex.Message, "Ошибка: " + ex.Message);
        }
    }

    private string GetStateDefect(string state, out string headerImgStr)
    {
        string returnVal = "";
        string color = "";
        if (state.ToLower() == "1")
        {
            color = "green";
            returnVal = GetLocalResourceObject("lblPermissible").ToString();
        }
        else if (state.ToLower() == "2".ToLower())
        {
            color = "yellow";
            returnVal = GetLocalResourceObject("lblMiddlePermissible").ToString();
        }
        else if (state.ToLower() == "3".ToLower())
        {
            color = "red";
            returnVal = GetLocalResourceObject("lblInPermissible").ToString();
        }

        headerImgStr = "<span style=\"vertical-align:middle; margin: 0 5px 2px 0; display: inline-block;width:10px;height:10px;background:"
                       + color + ";\">&nbsp;</span>";
        return returnVal;
    }

    private void FillDefectParam(ref DataTable dt)
    {
        try
        {

            //проходим по всем строкам заполняем поля
            string currentDefect = "";
            string errStr;

 

            DataColumnCollection columns = dt.Columns;
            DataTable dtResult = EmptyDtForRgResult();


            if (columns.Contains("сTypeDefectId"))
                dtResult.Rows[0][1] = dt.Rows[0]["сTypeDefectId"];//Тип дефекта. Значение 

            if (columns.Contains("nClockwise_Pos"))
                dtResult.Rows[0][3] = dt.Rows[0]["nClockwise_Pos"];//Угловое расположение (час)

            if (columns.Contains("nTypeCalculationsId"))
                dtResult.Rows[1][1] = dt.Rows[0]["cTypeCalculationsId"];//Расчетный тип дефекта. НСИ

            if (columns.Contains("cDepth_Pos"))
                dtResult.Rows[1][3] = dt.Rows[0]["cDepth_Pos"];//Расположение в металле. НСИ 

            if (columns.Contains("nLength"))
                dtResult.Rows[2][1] = dt.Rows[0]["nLength"];//Длина (мм)

            if (columns.Contains("nPrevSeam_Dist"))
                dtResult.Rows[2][3] = dt.Rows[0]["nPrevSeam_Dist"];//Расстояние до левого шва (м)

            if (columns.Contains("nWidth"))
                dtResult.Rows[3][1] = dt.Rows[0]["nWidth"];//Ширина (мм)

            if (columns.Contains("nNextSeam_Dist"))
                dtResult.Rows[3][3] = dt.Rows[0]["nNextSeam_Dist"];//Расстояние до правого шва (м)

            if (columns.Contains("nDepth"))
                dtResult.Rows[4][1] = dt.Rows[0]["nDepth"];//-- Глубина (мм)

            if (columns.Contains("dDetection_Date"))
                dtResult.Rows[4][3] = GetDate(dt.Rows[0]["dDetection_Date"].ToString());//Дата обнаружения дефекта

            if (columns.Contains("nPipeWallThickness"))
                dtResult.Rows[5][1] = dt.Rows[0]["nPipeWallThickness"];//Толщина стенки трубы (мм), на которой расположен дефект

            if (columns.Contains("cStressDescr"))
                dtResult.Rows[5][3] = dt.Rows[0]["cStressDescr"];//Описание стресс-коррозии


            rgResult.DataSource = dtResult;
            rgResult.DataBind();

            foreach (DataRow row in dt.Rows)
            {
  
                currentDefect = row["сTypeDefectId"].ToString();

                if (isCheced(row["isPitting_Confirm"].ToString()))
                {
                    labelPing.Text = "Так";
                }
                else
                {
                    labelPing.Text = "Нi"; 
                }
                txtNBPITTING_CONFIRM.Text = GetDate(row["dPitting_Date"].ToString()); //Дата шурфования
                txtNSIZE_PRECISION.Text = row["nSize_Precision"].ToString(); //Точность размеров (%)
                

                if (isCheced(row["isDefect_Fixed"].ToString()))
                {
                    labelUstr.Text = "Так";
                }
                else
                {
                    labelUstr.Text = "Нi";
                }

                txtDDATA_USTR_DEFECT.Text = GetDate(row["dFix_Date"].ToString()); //Дата ремонта
                txtcMETOD_USTR_DEFECT_KEY.Text = row["cFixMethod"].ToString(); //Метод ремонта. НСИ 
                labelCalc.Text = "Так";
                LabelRecomRemont.Text = row["сRecomMethodRepair"].ToString(); //метод устранения дефектов
                LabelDateCalc.Text = GetDate(row["dCalc_Date"].ToString());  //дата расчета
            }


            OracleDefects_EvalDef od = new OracleDefects_EvalDef();
            //список типов дефектов
            dt = od.GetDefectTypeList(out errStr);

            if (string.IsNullOrEmpty(errStr))
            {
                //картинка для дефекта
                PageOperation_EvalDev.SetDefectImagePreview(ref dt, ref ImagePrev, ImagePath, currentDefect);
                SetImageToolTip();
            }
            else
            {
                ShowError(errStr, errStr);
            }
        }
        catch (Exception ex)
        {
            ShowError(ex.Message, "Ошибка: " + ex.Message);
        }
    }


    #region Разные хелперы

    private void ShowError(string errMsg, string scriptMsgErr)
    {
        log.Error(errMsg);
        lblView2Err.Text = scriptMsgErr;
    }

    /// <summary>
    /// перевод юникстайм в дату
    /// </summary>
    /// <param name="dateUstr"></param>
    /// <returns></returns>
    private string GetDate(string dateUstr)
    {
        string getDateDefect = "";
        try
        {
            double timestamp = Convert.ToDouble(dateUstr);
            DateTime dateTime = new System.DateTime(1970, 1, 1, 0, 0, 0, 0);
            dateTime = dateTime.AddSeconds(timestamp);
            getDateDefect = dateTime.ToShortDateString();

        }
        catch (Exception ex)
        {

            string er = ex.ToString();
        }

        return getDateDefect;

    }

    private bool isCheced(string value)
    {
        return "True".Equals(value);

    }
    private void SetImageToolTip()
    {
        string imageName = ImagePrev.ImageUrl;
        int i = ImagePath.Length;
        imageName = imageName.Substring(i);

        string localResToolTip = "c" + imageName;

        var toolTip = GetLocalResourceObject(localResToolTip);

    }

    private DataTable EmptyDtForRgResult()
    {
        DataTable dtResult = new DataTable();
        dtResult.Columns.Add("FirstTitle");
        dtResult.Columns.Add("FirstValue");
        dtResult.Columns.Add("SecondTitle");
        dtResult.Columns.Add("SecondValue");
        dtResult.Rows.Add("<b>" + GetLocalResourceObject("Label12Resource1.Text") + "</b>", "",
            "<b>" + GetLocalResourceObject("Label18Resource1.Text") + "</b>", "");
        dtResult.Rows.Add("<b>" + GetLocalResourceObject("Label13Resource1.Text") + "</b>", "",
            "<b>" + GetLocalResourceObject("Label19Resource1.Text") + "</b>", "");
        dtResult.Rows.Add("<b>" + GetLocalResourceObject("Label14Resource1.Text") + "</b>", "",
            "<b>" + GetLocalResourceObject("Label21Resource1.Text") + "</b>", "");
        dtResult.Rows.Add("<b>" + GetLocalResourceObject("Label15Resource1.Text") + "</b>", "",
            "<b>" + GetLocalResourceObject("Label22Resource1.Text") + "</b>", "");
        dtResult.Rows.Add("<b>" + GetLocalResourceObject("Label20Resource1.Text") + "</b>", "",
            "<b>" + GetLocalResourceObject("Label23Resource1.Text") + "</b>", "");
        dtResult.Rows.Add("<b>" + GetLocalResourceObject("Label17Resource1.Text") + "</b>", "",
            "<b>" + GetLocalResourceObject("Label24Resource1.Text") + "</b>", "");
        return dtResult;
    }

    private DataTable EmptyDtForRgResultInfo()
    {
        DataTable dtResult = new DataTable();
        dtResult.Columns.Add("<b>" + GetLocalResourceObject("EncodingDefect") + "</b>");
        dtResult.Columns.Add("<b>" + GetLocalResourceObject("ASME") + "</b>");
        dtResult.Columns.Add("<b>" + GetLocalResourceObject("DNV") + "</b>");
       
        dtResult.Columns.Add("<b>" + GetLocalResourceObject("RSTRENG") + "</b>");
       
        dtResult.Columns.Add("<b>" + GetLocalResourceObject("Vtd") + "</b>");


        dtResult.Rows.Add("<b>" + GetLocalResourceObject("ResultInfo1") + "</b>",
            "", "", "", "");
        dtResult.Rows.Add("<b>" + GetLocalResourceObject("ResultInfo2") + "</b>",
            "", "", "", "");
        dtResult.Rows.Add("<b>" + GetLocalResourceObject("ResultInfo3") + "</b>",
            "", "", "", "");
        dtResult.Rows.Add("<b>" + GetLocalResourceObject("ResultInfo4") + "</b>",
            "", "", "", "");
        dtResult.Rows.Add("<b>" + GetLocalResourceObject("ResultInfo5") + "</b>",
            "", "", "", "");
        dtResult.Rows.Add("<b>" + GetLocalResourceObject("ResultInfo6") + "</b>",
            "", "", "", "");
        return dtResult;

        //ResultInfo1	Допустимость дефекта	
        //ResultInfo2	Безопасное давление (МПа)	
        //ResultInfo3	Предельное давление (МПа)	
        //ResultInfo4	Коэффициент безопасного давления	
        //ResultInfo5	Коэффициент запаса	
        //ResultInfo6	Остаточный ресурс (год)	



    }

    #endregion

    #region Расчеты по дефектам
    private void FillCalc()
    {
        try
        {
            if (ViewState["defectKey"] != null)
            {
                double tranportModeKey;
                double.TryParse(SessionStorage_EvalDef.GetItem("DDlTranspMode").ToString(), out tranportModeKey);

                double defectKey;
                double.TryParse(ViewState["defectKey"].ToString(), out defectKey);
                OracleDefects_EvalDef od = new OracleDefects_EvalDef();
                string errStr;


                    DataTable dtDefectParams = od.GetDefect_Params(defectKey, tranportModeKey, out errStr);

                    if (string.IsNullOrEmpty(errStr))
                    {
                        //параметры дефекта
                        FillDefectParam(ref dtDefectParams);


                        //внутреняя таблица расчетов - формируем структуру
                        DataTable dtResultInfo = EmptyDtForRgResultInfo();

                        //сейчас одна процедура вызова но внутри таблицу нужно транспонировать

                        DataTable dtCalcDefectAll = od.GetDefectCalc(defectKey, tranportModeKey, out errStr);
                        if (string.IsNullOrEmpty(errStr))
                        {
                            GetCalcDefectOnAlgoritm(ref dtResultInfo, dtCalcDefectAll);
                        }

                        rgResultInfo.DataSource = dtResultInfo;
                        rgResultInfo.DataBind();

                    }
                    else
                    {
                        ShowError(errStr, errStr);
                    }
                }


           
        }
        catch (Exception ex)
        {
            ShowError(ex.Message, "Ошибка: " + ex.Message);
        }
    }

    private void GetCalcDefectOnAlgoritm(ref DataTable dtResultInfo, DataTable dtAllCalc)
    {
        DataTable dt = new DataTable();
        string nameAlgoritm = "";
        string headerImg = "";

        foreach (DataRow row in dtAllCalc.Rows)
        {
            nameAlgoritm = row["cAlgorythm"].ToString();
            headerImg = "";
            switch (nameAlgoritm)
            {

                case "ASME":
                    dtResultInfo.Rows[0][1] = GetStateDefect(row["nDangerLevel"].ToString(), out headerImg);
                    dtResultInfo.Columns[1].ColumnName = headerImg + dtResultInfo.Columns[1].ColumnName;
                    dtResultInfo.Rows[1][1] = row["cMaxSafePressure"].ToString();
                    dtResultInfo.Rows[2][1] = row["cMaxAllowPressure"].ToString();
                    dtResultInfo.Rows[3][1] = row["cSafePressureRatio"].ToString();
                    dtResultInfo.Rows[4][1] = row["cSafetyFactor"].ToString();
                    dtResultInfo.Rows[5][1] = row["cResidualLife"].ToString();
                    break;
                case "DNV":
                    dtResultInfo.Rows[0][2] = GetStateDefect(row["nDangerLevel"].ToString(), out headerImg);
                    dtResultInfo.Columns[2].ColumnName = headerImg + dtResultInfo.Columns[2].ColumnName;
                    dtResultInfo.Rows[1][2] = row["cMaxSafePressure"].ToString();
                    dtResultInfo.Rows[2][2] = row["cMaxAllowPressure"].ToString();
                    dtResultInfo.Rows[3][2] = row["cSafePressureRatio"].ToString();
                    dtResultInfo.Rows[4][2] = row["cSafetyFactor"].ToString();
                    dtResultInfo.Rows[5][2] = row["cResidualLife"].ToString();

                    break;
                case "RStreng":
                    dtResultInfo.Rows[0][3] = GetStateDefect(row["nDangerLevel"].ToString(), out headerImg);
                    dtResultInfo.Columns[3].ColumnName = headerImg + dtResultInfo.Columns[3].ColumnName;
                    dtResultInfo.Rows[1][3] = row["cMaxSafePressure"].ToString();
                    dtResultInfo.Rows[2][3] = row["cMaxAllowPressure"].ToString();
                    dtResultInfo.Rows[3][3] = row["cSafePressureRatio"].ToString();
                    dtResultInfo.Rows[4][3] = row["cSafetyFactor"].ToString();
                    dtResultInfo.Rows[5][3] = row["cResidualLife"].ToString();
                    break;
                case "VTD":
                    dtResultInfo.Rows[0][4] = GetStateDefect(row["nDangerLevel"].ToString(), out headerImg);
                    dtResultInfo.Columns[4].ColumnName = headerImg + dtResultInfo.Columns[4].ColumnName;
                    dtResultInfo.Rows[1][4] = row["cMaxSafePressure"].ToString();
                    dtResultInfo.Rows[2][4] = row["cMaxAllowPressure"].ToString();
                    dtResultInfo.Rows[3][4] = row["cSafePressureRatio"].ToString();
                    dtResultInfo.Rows[4][4] = row["cSafetyFactor"].ToString();
                    dtResultInfo.Rows[5][4] = row["cResidualLife"].ToString();
                    break;

                default: break;

            }

        }

        //ResultInfo1 0	Допустимость дефекта	
        //ResultInfo2 1	Безопасное давление (МПа)	
        //ResultInfo3 2	Предельное давление (МПа)	
        //ResultInfo4 3	Коэффициент безопасного давления	
        //ResultInfo5 4	Коэффициент запаса	
        //ResultInfo6 5	Остаточный ресурс (год)	



    }


    #endregion

    #region Восстановление и очищение сессионных переменных
    /// <summary>
    /// восстановление состояний контролов из  сессии
    /// </summary>
    private void RestoreControlStateFromSession()
    {
        try
        {
            #region востановление выбранной нитки из  сессии

            #region очищаем все дочерние элементы управления

            DDLThread.Items.Clear(); //список нитей
            DDlTranspMode.Items.Clear(); //список режимов

            txtSharedKmStart.Text = "";
            txtSharedKmEnd.Text = "";

            #endregion

            DataTable dt = GetThreadsWithFillDdl();

            if (SessionStorage_EvalDef.GetItem("DDLThreadSelected") != null)
            {
                DDLThread.SelectedIndex = -1;
                DDLThread.Items.FindItemByValue(SessionStorage_EvalDef.GetItem("DDLThreadSelected").ToString()).Selected
                    = true;
                buAllDefect.Enabled = true;

            }

            #endregion




            if (SessionStorage_EvalDef.GetItem("DDLThreadSelected") != null)
            {
                double ikey;
                string errStr = string.Empty;

                double.TryParse(SessionStorage_EvalDef.GetItem("DDLThreadSelected").ToString(), out ikey);

                // получаем список режимов транспортировки для указанной нитки
                DataTable dtm = GetModesForThreadFillDdl();
                if (!string.IsNullOrEmpty(SessionStorage_EvalDef.GetItem("DDlTranspMode").ToString()))
                {
                    DDlTranspMode.SelectedIndex = -1;
                    DDlTranspMode.Items.FindItemByValue(SessionStorage_EvalDef.GetItem("DDlTranspMode").ToString())
                        .Selected = true;

                }

            }

            if (SessionStorage_EvalDef.GetItem("txtSharedKmStart") != null)
            {
                txtSharedKmStart.Text = SessionStorage_EvalDef.GetItem("txtSharedKmStart").ToString();
            }

            if (SessionStorage_EvalDef.GetItem("txtSharedKmEnd") != null)
            {
                txtSharedKmEnd.Text = SessionStorage_EvalDef.GetItem("txtSharedKmEnd").ToString();
            }

            // востановление выбранной списка дефектов из  сессии
            if (SessionStorage_EvalDef.GetItem("defectDataSet") != null)
            {

                dt = (DataTable)SessionStorage_EvalDef.GetItem("defectDataSet");
                FillDefectLists(dt);
            }
        }
        catch (Exception ex)
        {
            ShowError(ex.Message, "Ошибка: " + ex.Message);
        }
    }


    /// <summary>
    /// очищает вычисленные поля и списки в верху формы
    /// </summary>
    private void ClearStartField()
    {
        DDLMG.Items.Clear(); //очищаем список МГ
        DDLThread.Items.Clear(); //очищаем список ниток
        txtSharedKmEnd.Text = ""; //очищает  км  конца выбранного  участка
        txtSharedKmStart.Text = ""; // очищает  км начала выбранного участка
        DDlTranspMode.Items.Clear(); //очищаем список режимов
    }

    private void ClearControls()
    {
        try
        {
            FillDefectLists(null);

            #region очищаем все дочерние элементы управления

            DDLThread.Items.Clear(); //список нитей
            DDLThread.ClearSelection();
            DDLThread.Text = string.Empty;
            DDLThread.Enabled = false;

            DDlTranspMode.Items.Clear();
            DDlTranspMode.ClearSelection();
            DDlTranspMode.Text = string.Empty;
            DDlTranspMode.Enabled = false;

            txtSharedKmStart.Text = "";
            txtSharedKmEnd.Text = "";

            #endregion

            #region очищаем сессии дочерних элементов управления

            SessionStorage_EvalDef.AddItem("DDLThreadSelected", null);
            SessionStorage_EvalDef.AddItem("DDtranspModeSelected", null);
            SessionStorage_EvalDef.AddItem("DDLSelectedListSelected", null);
            SessionStorage_EvalDef.AddItem("DDLUchastokSelected", null);
            SessionStorage_EvalDef.AddItem("defectDataSet", null);
            //SessionStorage_EvalDef.AddItem("DDLFiltr", null);
            SessionStorage_EvalDef.AddItem("CurrentPageIndex", null);
            SessionStorage_EvalDef.AddItem("DefectDDLMGSelectedValue", null);

            #endregion
        }
        catch (Exception ex)
        {
            ShowError(ex.Message, "Ошибка: " + ex.Message);
        }
    }

    private void CleanDefectparam()
    {
        ImagePrev.ImageUrl = ImagePath + "empty.gif";
 
        labelPing.Text = "";
        txtNBPITTING_CONFIRM.Text = "";
        txtNSIZE_PRECISION.Text = "";
 
        labelUstr.Text = "";
        txtDDATA_USTR_DEFECT.Text = "";
        txtcMETOD_USTR_DEFECT_KEY.Text = "";
        LabelDateCalc.Text = "";
        labelCalc.Text = "";
     }


    #endregion


    #region Наполнение комбобоксов и выбор в комбобоксах
    /// <summary>
    /// наполнение списка МГ
    /// </summary>
    private void FillMgList()
    {
        try
        {
            // очищаем все поля  настроек
            ClearStartField();
            CleanDefectparam();
            string errStr;
            DataTable dt = ol.GetMg(out errStr);
            if (!String.IsNullOrEmpty(errStr))
            {
                ShowError(errStr, errStr);
            }
            else
            {
                PageOperation_EvalDev.FillDdLs(ref DDLMG, dt, "CNAME", "NKEY");
            }

            // востановление выбранного МГ из  сессии
            if (SessionStorage_EvalDef.GetItem("DefectDDLMGSelectedValue") != null)
            {
                DDLMG.SelectedIndex = -1;
                DDLMG.Items.FindItemByValue(SessionStorage_EvalDef.GetItem("DefectDDLMGSelectedValue").ToString())
                    .Selected = true;
            }
        }
        catch (Exception ex)
        {
            ShowError(ex.Message, "Ошибка: " + ex.Message);
        }
    }


    protected void DDLMG_SelectedIndexChanged(object sender, EventArgs e)
    {
        buAllDefect.Enabled = false;

        FillThread();
    }

    /// <summary>
    /// наполнение нитки
    /// </summary>
    private void FillThread()
    {
        try
        {
            //Очищаем дочерние элементы управления.
            ClearControls();
            CleanDefectparam();
            //наполнение списка нитей
            GetThreadsWithFillDdl();

            // запомнаем в сессии выбранный МГ
            SessionStorage_EvalDef.AddItem("DefectDDLMGSelectedValue", DDLMG.SelectedValue);

            // востановление выбранной нитки из  сессии
            if (SessionStorage_EvalDef.GetItem("DDLThreadSelected") != null)
            {
                DDLThread.SelectedIndex = -1;
                DDLThread.Items.FindItemByValue(SessionStorage_EvalDef.GetItem("DDLThreadSelected").ToString()).Selected
                    = true;
            }

        }
        catch (Exception ex)
        {
            ShowError(ex.Message, "Ошибка: " + ex.Message);
        }
    }


    protected void DDLThread_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            FillDefectLists(null);

            #region очищаем все дочерние элементы управления

            txtSharedKmStart.Text = "";
            txtSharedKmEnd.Text = "";

            #endregion

            #region очищаем сессии дочерних элементов управления

            SessionStorage_EvalDef.AddItem("DDLUchastokSelected", null);
            SessionStorage_EvalDef.AddItem("DDtranspModeSelected", null);
            SessionStorage_EvalDef.AddItem("DDLSelectedListSelected", null);
            SessionStorage_EvalDef.AddItem("defectDataSet", null);
            CleanDefectparam();

            #endregion

            #region заполняем км начало, конца и длину нитки

            decimal ikey;
            string errStr;

            //получаем км по dt полученных ниток
            if (SessionStorage_EvalDef.GetItem("ThreadsForMgDataTable") != null)
            {
                DataTable dtThread = (SessionStorage_EvalDef.GetItem("ThreadsForMgDataTable")) as DataTable;
                Decimal.TryParse(DDLThread.SelectedValue, out ikey);

                IEnumerable<DataRow> query = from t in dtThread.AsEnumerable()
                                             where t["nThread_Key"].ToString() == ikey.ToString()
                                             select t;
                foreach (DataRow p in query)
                {
                    double kmStart = Double.Parse(p["nkm_beg"].ToString());
                    double kmEnd = Double.Parse(p["nkm_end"].ToString());

                    txtSharedKmStart.Text = Math.Round(kmStart, 3).ToString(CultureInfo.InvariantCulture);
                    txtSharedKmEnd.Text = Math.Round(kmEnd, 3).ToString(CultureInfo.InvariantCulture);
                }

            }


            #endregion

            // запомнаем в сессии выбранную нитку
            SessionStorage_EvalDef.AddItem("DDLThreadSelected", DDLThread.SelectedValue);

            DDlTranspMode.Items.Clear();
            DDlTranspMode.ClearSelection();
            DDlTranspMode.Text = string.Empty;
            DDlTranspMode.Enabled = false;

            //разблокируем кнопку
            buAllDefect.Enabled = true;


            GetModesForThreadFillDdl(); //наполнение режимов


        }
        catch (Exception ex)
        {
            ShowError(ex.Message, "");
        }
    }

    /// <summary>
    /// список нитей из базы
    /// </summary>
    /// <returns></returns>
    private DataTable GetThreadsWithFillDdl()
    {
        double tmp;
        string errStr;

        ol = new OracleLayer_EvalDef();
        double.TryParse(DDLMG.SelectedValue, out tmp);
        DataTable dt = ol.GetThreadsForMgDataTable(tmp, out errStr);

        //запоминаем dt список выбранных ниток
        SessionStorage_EvalDef.AddItem("ThreadsForMgDataTable", dt);

        if (string.IsNullOrEmpty(errStr))
        {
            DDLThread.Enabled = true;
            //наполняем комбобокс DDLThread
            PageOperation_EvalDev.FillDdLsDataTable(ref DDLThread, dt, "cName", "nThread_Key");
        }
        else
        {
            ShowError(errStr, errStr);
        }

        return dt;
    }



    /// <summary>
    /// заполнение режимов транспортировки
    /// </summary>
    /// <returns></returns>
    private DataTable GetModesForThreadFillDdl()
    {
        double tmp;
        string errStr;

        ol = new OracleLayer_EvalDef();
        double.TryParse(DDLThread.SelectedValue, out tmp);
        DataTable dt = ol.GetModesForThread(tmp, out errStr);
        if (dt == null)
        {
            btnShowDefects.Enabled = false;
        }
       

        if (string.IsNullOrEmpty(errStr))
        {
            DDlTranspMode.Enabled = true;
            //наполняем комбобокс режимов
            PageOperation_EvalDev.FillDdLsDataTable(ref DDlTranspMode, dt, "cModeName", "nModeKey");
            //автоматически выбираем первую запись и сохраняем в сессии
            DDlTranspMode.SelectedIndex = 0;
            SessionStorage_EvalDef.AddItem("DDlTranspMode", DDlTranspMode.SelectedValue);
        }
        else
        {
            ShowError(errStr, errStr);
        }

        return dt;
    }

    protected void DDlTranspMode_OnSelectedIndexChanged(object sender, RadComboBoxSelectedIndexChangedEventArgs e)
    {
        // запомнаем в сессии выбранный режим
        SessionStorage_EvalDef.AddItem("DDlTranspMode", DDlTranspMode.SelectedValue);
        CleanDefectparam();
    }

    #endregion



    #region Список дефектов

    protected void AllDefect_Click(object sender, EventArgs e)
    {
        try
        {
            CleanDefectparam();

            OracleDefects_EvalDef od = new OracleDefects_EvalDef();

            FillDefectLists(null);

            #region чтение входных параметров для выбора списка дефекта

            double nkey;
            double.TryParse(DDLThread.SelectedValue, out nkey);

            double kmStart = Double.Parse(txtSharedKmStart.Text, CultureInfo.InvariantCulture);
            double kmEnd = Double.Parse(txtSharedKmEnd.Text,  CultureInfo.InvariantCulture);


            #endregion

            string errStr;
            DataTable dt = od.GetDefect_List(nkey, kmStart, kmEnd, out errStr);

            if (!string.IsNullOrEmpty(errStr))
                ShowError(errStr, errStr);

            if (dt.Rows.Count >= 0)
            {
                //загрузка дефектов в таблицу
                FillDefectLists(dt);

                //сохряняем в сессии
                SessionStorage_EvalDef.AddItem("defectDataSet", null);
                SessionStorage_EvalDef.AddItem("defectDataSet", dt);
                SessionStorage_EvalDef.AddItem("txtSharedKmStart", txtSharedKmStart.Text);
                SessionStorage_EvalDef.AddItem("txtSharedKmEnd", txtSharedKmEnd.Text);

            }


            if (dt.Rows.Count >= 0)
            {

                if (!string.IsNullOrEmpty(DDlTranspMode.SelectedValue))
                {

                    //это нужно для открытия контрола - показать все дефекты

                    string defectListName;
                    string mgName = DDLMG.SelectedItem.Text;
                    string threadName = DDLThread.SelectedItem.Text;
                    string kmBegin = txtSharedKmStart.Text;
                    string kmEnd1 = txtSharedKmEnd.Text;
                    defectListName = mgName + " " + threadName + " (" + kmBegin + " - " + kmEnd1 + " " +
                                     GetLocalResourceObject("Label83Resource1.Text") + ")";

                    double keyMode;
                    double.TryParse(DDlTranspMode.SelectedValue, out keyMode);
                    DataTable dtSumDefect = od.GetDefectSummary(nkey, kmStart, kmEnd, keyMode, out errStr);
                    if (!string.IsNullOrEmpty(errStr))
                    {
                        ShowError(errStr, errStr);
                    }
                    else
                    {
                        SessionStorage_EvalDef.AddItem("dtSumDefect", dtSumDefect);
                        SessionStorage_EvalDef.AddItem("defectListName", defectListName);
                        ctrDefectsList.FillGrid(dtSumDefect, defectListName);
                    }

                }

            }

            //скрываем индикатор busy
            imgBusyIndicator.CssClass = "hideControl";

        }
        catch (Exception ex)
        {
            ShowError(ex.Message, "Ошибка: " + ex.Message);
        }
    }

    private void FillDefectLists(DataTable dt)
    {
        if (dt != null)
        {
           
            if (string.IsNullOrEmpty(SessionStorage_EvalDef.GetItem("DDlTranspMode").ToString()))
            {
                btnShowDefects.Enabled = false;
            }
            else
            {
                btnShowDefects.Enabled = true;
            }
            
            txtDefectCount.Text = dt.Rows.Count.ToString();
        }
        else
        {
            btnShowDefects.Enabled = false;
            txtDefectCount.Text = "";
        }

        grdDefects.DataSource = null;
        grdDefects.DataSource = dt;
        grdDefects.DataBind();


    }

    protected void grdDefects_OnPageIndexChanged(object sender, GridPageChangedEventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)SessionStorage_EvalDef.GetItem("defectDataSet");
            grdDefects.CurrentPageIndex = e.NewPageIndex;
            SessionStorage_EvalDef.AddItem("CurrentPageIndex", e.NewPageIndex);

            FillDefectLists(dt);
        }
        catch (Exception ex)
        {
            ShowError(ex.Message, "Ошибка: " + ex.Message);
        }


    }

    #endregion


    protected void RadGrid1_ItemCommand(object source, GridCommandEventArgs e)
    {



        if (e.CommandName == "RowClick")
        {


            GridDataItem dataItem = e.Item as GridDataItem;
            string tt = dataItem["nDefectKey"].Text;
            string defectParam = dataItem["nDefectKey"].Text; //получаем ключ выбранного дефекта 

            if (!string.IsNullOrEmpty(defectParam))
            {
                RestoreControlStateFromSession();

                double tmp;
                double.TryParse(defectParam, out tmp);
                if (tmp > 0)
                {
                    //сохраняем  ключ дефекта  
                    ViewState["defectKey"] = defectParam;
                    FillCalc();

                    if (SessionStorage_EvalDef.GetItem("defectDataSet") != null)
                    {
                        DataTable dt = (DataTable)SessionStorage_EvalDef.GetItem("defectDataSet");
                        txtDefectCount.Text = dt.Rows.Count.ToString();
                        //восстанавливаем позицию в  страницы в таблице дефектов
                        RestorePagePosition();

 
                    }
                }
                else
                {
                    ShowError("Для выбранного дефекта отсутствуют параметры.",
                        "Для выбранного дефекта отсутствуют параметры.");
                }
            }
        }

        grdDefects.GetCallbackResult();
    }

    protected void grdDefects_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        GridDataItem item = (GridDataItem)grdDefects.SelectedItems[0];

    }



    protected void grdDefects_OnItemDataBound(object sender, GridItemEventArgs e)
    {
        //пока не используем - до лучших времен, когда свяжем с джаваскрипт!

        //if (e.Item is GridDataItem)
        //{
        //    GridDataItem dataItem = (GridDataItem)e.Item;

        //    if (dataItem["nDefectKey"].Text != "&nbsp;")
        //    {
        //        string defectkey = dataItem["nDefectKey"].Text;
        //        dataItem["cDefectName"].Text =
        //            "<span style='color:#a4abb2;text-decoration:underline;cursor:pointer' \">" +
        //            dataItem["cDefectName"].Text + "</span>";

        //        //нужно для вызова js функции
        //       // dataItem["cDefectName"].Attributes.Add("nDefectKey", defectkey);

        //    }

        //}
    }

    //нужно для вызова из джаваскрипта (как пример)
    [WebMethod()]
    [System.Web.Script.Services.ScriptMethod()]
    public static string CalledMethod()
    {
        return "Hello " + "keyDefect" + Environment.NewLine + "The Current Time is: "
               + DateTime.Now.ToString();

    }

    protected void DefectAll(string defectKey)
    {
        string t = defectKey;
    }


    protected void btnShowDefects_OnClick(object sender, EventArgs e)
    {
        string t = "ns.dfm,vn.skdmnfvm,";
        ClientScript.RegisterStartupScript
      (GetType(), "Javascript", "javascript: showDefectList(); ", true);
    }
}






