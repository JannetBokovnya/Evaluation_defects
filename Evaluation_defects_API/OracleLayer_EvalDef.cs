using System;
using System.Data;
using App_Code.Admin_module_API;
using log4net;

///// <summary>
///// Summary description for OracleLayer_EvalDef
///// </summary>
[System.ComponentModel.DataObject]
public class OracleLayer_EvalDef : OracleEngine_EvalDef
{


    private static readonly ILog Log = LogManager.GetLogger(typeof(Auth).Name);

    /// <summary>
    /// возвращает список МГ
    /// </summary>
    /// <returns></returns>
    [System.ComponentModel.DataObjectMethodAttribute(System.ComponentModel.DataObjectMethodType.Select, false)]
    public DataTable GetMg(out string errMsg)
    {

        DataTable dt = new DataTable();
        errMsg = "";
        try
        {
            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn connOra = dbConnAuth.connOra();
            connOra.ConnectionString(ConStr);
            dt = connOra.ExecuteQuery<DataTable>(QueryEvalDef.GetMgQuery);
        }
        catch (Exception ex)
        {
            Log.Error(ex);
            errMsg = "Ошибка выполнения запроса. " + ex.Message;
        }

        return dt;

       // return GetDataEngine(QueryEvalDef.GetMgQuery, ConStr, out errMsg);
    }

    public DataTable GetThreadsForMgDataTable(double inMgKey, out string errMsg)
    {

        DataTable dt = new DataTable();
        errMsg = "";

        DBConn.DBParam[] oip = new DBConn.DBParam[1];
        oip[0] = new DBConn.DBParam
        {
            ParameterName = "in_nMgKey",
            DbType = DBConn.DBTypeCustom.Double,
            Value = inMgKey
        };
        try
        {
            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn connOra = dbConnAuth.connOra();
            connOra.ConnectionString(ConStr);
            dt = connOra.ExecuteQuery<DataTable>(QueryEvalDef.GetThreadsForMgQuery, oip);
        }
        catch (Exception ex)
        {
            Log.Error(ex);
            errMsg = "Ошибка выполнения запроса. " + ex.Message;
        }

        return dt;

    }

    //список режимов
    public DataTable GetModesForThread(double inThreadKey, out string errMsg)
    {
        DataTable dt = new DataTable();
        errMsg = "";

        DBConn.DBParam[] oip = new DBConn.DBParam[1];
        oip[0] = new DBConn.DBParam
        {
            ParameterName = "in_nThreadKey",
            DbType = DBConn.DBTypeCustom.Double,
            Value = inThreadKey
        };
        try
        {
            DbConnAuth dbConnAuth = new DbConnAuth();
            DBConn.Conn connOra = dbConnAuth.connOra();
            connOra.ConnectionString(ConStr);
            dt = connOra.ExecuteQuery<DataTable>(QueryEvalDef.GetModesForThreadQuery, oip);
        }
        catch (Exception ex)
        {
            Log.Error(ex);
            errMsg = "Ошибка выполнения запроса. " + ex.Message;
        }

        return dt;
    }


   
}
