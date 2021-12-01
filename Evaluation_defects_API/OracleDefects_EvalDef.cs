using System;
using System.Data;
using App_Code.Admin_module_API;
using log4net;

/// <summary>
/// Summary description for OracleDefects_EvalDef
/// </summary>
public class OracleDefects_EvalDef : OracleLayer_EvalDef
{

    private static readonly ILog Log = LogManager.GetLogger(typeof(Auth).Name);
    /// <summary>
    /// возвращает список дефектов
    /// </summary>
    /// <param name="inNPipeKey">ключ нитки</param>
    /// <param name="inNKmStart">км начала</param>
    /// <param name="inNKmEnd">км конца</param>
    /// <param name="inNFilterKey">ключ фильтра</param>
    /// <param name="inNModeKey">ключ режима</param>
    /// <param name="errMsg">сообщ</param>
    /// <returns></returns>
    public DataTable GetDefect_List(double inNPipeKey, double inNKmStart, double inNKmEnd, out string errMsg)
    {
        DataTable dt = new DataTable();

        errMsg = "";

        DBConn.DBParam[] oip = new DBConn.DBParam[3];
        oip[0] = new DBConn.DBParam()
        {
            ParameterName = "in_nThreadKey",
            DbType = DBConn.DBTypeCustom.Double,
            Value = inNPipeKey
        };

        oip[1] = new DBConn.DBParam
        {
            ParameterName = "in_nKmStart",
            DbType = DBConn.DBTypeCustom.Double,
            Value = inNKmStart
        };

        oip[2] = new DBConn.DBParam
        {
            ParameterName = "in_nKmEnd",
            DbType = DBConn.DBTypeCustom.Double,
            Value = inNKmEnd
        };

        try
        {
            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn connOra = dbConnAuth.connOra();
            connOra.ConnectionString(ConStr);
            dt = connOra.ExecuteQuery<DataTable>(QueryEvalDef.GetDefectListQuery, oip);
        }
        catch (Exception ex)
        {
            Log.Error(ex);
            errMsg = "Ошибка выполнения запроса. GetDefectListQuery " + ex.Message;
        }

        return dt;

    }

    /// <summary>
    /// Возвращает список дефектов со всеми параметрами для формы "Список дефектов"
    /// </summary>
    /// <param name="inNPipeKey"></param>
    /// <param name="inNKmStart"></param>
    /// <param name="inNKmEnd"></param>
    /// <param name="inNModeKey"></param>
    /// <param name="errMsg"></param>
    /// <returns></returns>
    public DataTable GetDefectSummary(double inNPipeKey, double inNKmStart, double inNKmEnd, double inNModeKey, out string errMsg)
    {
        DataTable dt = new DataTable();

        errMsg = "";

        DBConn.DBParam[] oip = new DBConn.DBParam[4];
        oip[0] = new DBConn.DBParam()
        {
            ParameterName = "in_nThreadKey",
            DbType = DBConn.DBTypeCustom.Double,
            Value = inNPipeKey
        };

        oip[1] = new DBConn.DBParam
        {
            ParameterName = "in_nKmStart",
            DbType = DBConn.DBTypeCustom.Double,
            Value = inNKmStart
        };

        oip[2] = new DBConn.DBParam
        {
            ParameterName = "in_nKmEnd",
            DbType = DBConn.DBTypeCustom.Double,
            Value = inNKmEnd
        };

        oip[3] = new DBConn.DBParam
        {
            ParameterName = "in_nModeKey",
            DbType = DBConn.DBTypeCustom.Double,
            Value = inNModeKey
        };



        try
        {
            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn connOra = dbConnAuth.connOra();
            connOra.ConnectionString(ConStr);
            dt = connOra.ExecuteQuery<DataTable>(QueryEvalDef.GetDefectSummaryQuery, oip);
        }
        catch (Exception ex)
        {
            Log.Error(ex);
            errMsg = "Ошибка выполнения запроса.GetDefectSummaryQuery " + ex.Message;
        }


        return dt;
    }
    /// <summary>
    /// получить параметры дефекта
    /// </summary>
    /// <param name="inNDefKey"></param>
    /// <param name="inNModeKey"></param>
    /// <param name="errMsg"></param>
    /// <returns></returns>
    public DataTable GetDefect_Params(double inNDefKey, double inNModeKey, out string errMsg)
    {
        DataTable dt = new DataTable();
        errMsg = "";

        if (inNModeKey != 0d)
        {
            DBConn.DBParam[] oip = new DBConn.DBParam[2];
            oip[0] = new DBConn.DBParam
            {
                ParameterName = "in_nDefectKey",
                DbType = DBConn.DBTypeCustom.Double,
                Value = inNDefKey
            };

            oip[1] = new DBConn.DBParam
            {
                ParameterName = "in_nModeKey",
                DbType = DBConn.DBTypeCustom.Double,
                Value = inNModeKey
            };

            try
            {
                DbConnAuth dbConnAuth = new DbConnAuth();
                DBConn.Conn connOra = dbConnAuth.connOra();
                connOra.ConnectionString(ConStr);
                dt = connOra.ExecuteQuery<DataTable>(QueryEvalDef.GetDefectParamsQuery, oip);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                errMsg = "Ошибка выполнения запроса. GetDefectParamsQuery " + ex.Message;
            }

        }

        return dt;
    }

    /// <summary>
    /// получить список типов дефектов
    /// </summary>
    /// <param name="errMsg">сообщение</param>
    /// <returns></returns>
    public DataTable GetDefectTypeList(out string errMsg)
    {
        DataTable dt = new DataTable();
        errMsg = "";

        try
        {
            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn connOra = dbConnAuth.connOra();
            connOra.ConnectionString(ConStr);
            dt = connOra.ExecuteQuery<DataTable>(QueryEvalDef.GetDefectTypeListQuery);
        }
        catch (Exception ex)
        {
            Log.Error(ex);
            errMsg = "Ошибка выполнения запроса. " + ex.Message;
        }

        return dt;


    }
    /// <summary>
    /// Возвращает параметры оценки дефекта по всем заявленым стандартам
    /// </summary>
    /// <param name="inNDefKey"></param>
    /// <param name="inNModeKey"></param>
    /// <param name="errMsg"></param>
    /// <returns></returns>
    public DataTable GetDefectCalc(double inNDefKey, double inNModeKey, out string errMsg)
    {

        DataTable dt = new DataTable();
        errMsg = "";

        if (inNModeKey != 0d)
        {
            DBConn.DBParam[] oip = new DBConn.DBParam[2];
            oip[0] = new DBConn.DBParam
            {
                ParameterName = "in_nDefectKey",
                DbType = DBConn.DBTypeCustom.Double,
                Value = inNDefKey
            };

            oip[1] = new DBConn.DBParam
            {
                ParameterName = "in_nModeKey",
                DbType = DBConn.DBTypeCustom.Double,
                Value = inNModeKey
            };

            try
            {
                DbConnAuth dbConnAuth = new DbConnAuth();
                DBConn.Conn connOra = dbConnAuth.connOra();
                connOra.ConnectionString(ConStr);
                dt = connOra.ExecuteQuery<DataTable>(QueryEvalDef.GetDefectCalcQuery, oip);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
                errMsg = "Ошибка выполнения запроса. " + ex.Message;
            }
        }

        return dt;
    }





}
