using System;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using Telerik.Web.UI;

/// <summary>
/// Summary description for pageOperation_EvalDev
/// </summary>
public class PageOperation_EvalDev
{
    public static void FillDdLs(ref RadComboBox ddl, DataTable dt, string textField, string valueField)
    {
        string sValue, sText;
        ddl.Items.Clear();

        //проходим по всем строкам 
        foreach (DataRow row in dt.Rows)
        {
            sText = row[textField].ToString();
            sValue = row[valueField].ToString();

            ddl.Items.Add(new RadComboBoxItem(sText, sValue));
        }
    }

    public static void FillDdLsDataTable(ref RadComboBox ddl, DataTable dt, string textField, string valueField)
    {
        string sValue = "";
        string sText = "";
        ddl.Items.Clear();
        //проходим по всем строкам 
        if (dt.Rows.Count >= 0)
        {
            foreach (DataRow row in dt.Rows)
            {
                foreach (DataColumn column in dt.Columns)
                {
                    if (row[column] != null)
                    {
                        if (column.Caption.ToLower() == valueField.ToLower())
                        {
                            sValue = row[column].ToString();
                        }
                        else if (column.Caption.ToLower() == textField.ToLower())
                        {
                            sText = row[column].ToString();
                        }
                    }

                }

                ddl.Items.Add(new RadComboBoxItem(sText, sValue));
            }
        }
    }

    public static void SetDefectImagePreview(ref DataTable dt, ref Image pic, string pathToImage, string curentDefectType)
    {
        //получение имени картинки по типу дефекта
        string imageName = dt.AsEnumerable()
                        .Where(r => r.Field<string>("cDefectType").Equals(curentDefectType))
                        .Select(r => r.Field<string>("cFileName"))
                        .FirstOrDefault();

        pic.ImageUrl = (imageName != null) ? pathToImage + imageName : pathToImage + "empty.gif";
    }
}
