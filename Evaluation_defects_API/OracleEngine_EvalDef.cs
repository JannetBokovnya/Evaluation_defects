using System;
using System.Data;
using System.Data.OracleClient;
using System.Web.Configuration;
using App_Code.Admin_module_API;
using log4net;

/// <summary>
/// Summary description for oracleEngine_EvalDef
/// </summary>
public class OracleEngine_EvalDef
{
    /// <summary>
    /// Блок глобальных переменных
    /// </summary>
    protected readonly string ConStr;//строка подключения к мета данным
    private static readonly ILog Log = LogManager.GetLogger(typeof(Auth).Name);

    /// <summary>
    /// Конструктор класса
    /// </summary>
    public OracleEngine_EvalDef()
    {
        ConStr = WebConfig.GetDBConnection();
    }

}