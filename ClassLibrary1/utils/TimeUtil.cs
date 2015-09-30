using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace ZLPlugin.Commands
{
    class TimeUtil
    {
        public static bool checkOutOfTime()
        {
            DateTime time = DateTime.Now;
            long registerTime = (long)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\CadPlugin", "regTime", 0);
            TimeSpan timePast = new TimeSpan(time.Ticks - registerTime);
            if (timePast.Days >= 365)
            {
                string tip = "\n 你所使用的插件已过期，请联系作者:zhou-han-peng@163.com QQ:104979306";
                Autodesk.AutoCAD.ApplicationServices.Application.ShowAlertDialog(tip);
                CadPlugin.logToEditor("\n 你所使用的插件已过期，请联系作者:zhou-han-peng@163.com QQ:104979306");
                return true;
            }
            return false;
        }
    }
}
