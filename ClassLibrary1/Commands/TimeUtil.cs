using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZLPlugin.Commands
{
    class TimeUtil
    {
        public static bool checkOutOfTime()
        {
            DateTime time = DateTime.Now;
            if (!(time.Year == 2014 && time.Month <= 12 && time.Month >= 7))
            {
                CadPlugin.logToEditor("\n 你所使用的插件已过期，请联系作者:zhou-han-peng@163.com QQ:104979306");
                return true;
            }
            return false;
        }
    }
}
