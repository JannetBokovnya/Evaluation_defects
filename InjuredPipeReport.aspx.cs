using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class InjuredPipeReport : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

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
}
