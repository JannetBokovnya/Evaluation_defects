/// <summary>
/// Summary description for oracleQuerys_EvalDef
/// </summary>
public static class QueryEvalDef
{
    ////(Пока без использования фильтра)
   
    public const string GetMgQuery = "DB_API.EVALUATION_DEFECTS.GetMG"; //1

    public const string GetThreadsForMgQuery = "DB_API.EVALUATION_DEFECTS.getThreadsForMG"; //1

    public const string GetModesForThreadQuery = "DB_API.EVALUATION_DEFECTS.getModesForThread"; //3;

    public const string GetDefectListQuery = "DB_API.EVALUATION_DEFECTS.GetDefect_List";//4

    public const string GetDefectParamsQuery = "DB_API.EVALUATION_DEFECTS.GetDefect_Params";//5

    public const string GetDefectCalcQuery = "DB_API.EVALUATION_DEFECTS.getDefectCalc";//6
   
    public const string GetDefectTypeListQuery = "DB_API.EVALUATION_DEFECTS.getDefectTypeList";

    public const string GetDefectSummaryQuery = "DB_API.EVALUATION_DEFECTS.getDefectSummary";

}
