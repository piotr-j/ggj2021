using System;

public static class TimeUtils
{

    public static DateTime GetDate(StringVariable variable)
    {
        if (string.IsNullOrEmpty(variable.Value))
        {
            return new DateTime(1990, 1, 1);
        }
        
        return DateTime.Parse(variable.Value);
    }
    
    public static void SetDate(DateTime date, StringVariable variable)
    {
        variable.SetValue(date.ToString());
    }

    public static void SetDateNow(StringVariable variable)
    {
        SetDate(DateTime.Now, variable);
    }

    public static bool IsDayChanged(DateTime dateTime)
    {
        return dateTime.DayOfYear != DateTime.Now.DayOfYear;
    }
    
    public static bool IsDayChanged(StringVariable variable)
    {
        return IsDayChanged(GetDate(variable));
    }
}
