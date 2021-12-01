using System;
using System.Collections.Generic;

/// <summary>
/// Summary description for SessionStorage_EvalDef
/// </summary>
public static class SessionStorage_EvalDef
{
    //private static SessionStateItemCollection SessionsArray;
    private static Dictionary<string, object> _sessionsArray;

    /// <summary>
    /// инициализация хранилища сессий
    /// </summary>
    public static void InitStorage()
    {
        if (_sessionsArray == null)
            _sessionsArray = new Dictionary<string, object>();//new SessionStateItemCollection();
    }

    /// <summary>
    /// Очищаем хранилище сессий
    /// </summary>
    public static void DisposeStorage()
    {
        if (_sessionsArray != null)
        {

            _sessionsArray = null;
        }
    }


    /// <summary>
    /// добавить новую сессию в хранилище
    /// </summary>
    /// <param name="key">ключ</param>
    /// <param name="data">значение</param>
    /// <returns>результат добавления</returns>
    public static bool AddItem(string key, object data)
    {
        bool result = true;
        try
        {
            InitStorage();
            foreach (string s in _sessionsArray.Keys)
            {
                if (_sessionsArray.Keys.ToString() == key)
                {
                    result = false;
                    break;
                }
            }
            if (!result)
                return false;
            _sessionsArray[key] = data;
        }
        catch
        {
            throw;
        }
        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    /// <param name="data"></param>
    /// <returns></returns>
    public static bool UpdateItem(string key, object data)
    {
        return AddItem(key, data);
    }

    /// <summary>
    /// возращает содержимое сессии по 
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static object GetItem(string key)
    {
        object lDSessionsArray = null;
        try
        {
            InitStorage();

            if (_sessionsArray.ContainsKey(key))
            {
                lDSessionsArray = _sessionsArray[key];
            }
        }
        catch
        {
            throw;
        }
        return lDSessionsArray;
    }
}
