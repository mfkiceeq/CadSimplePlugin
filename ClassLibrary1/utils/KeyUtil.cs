using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using Microsoft.Win32;
using Autodesk.AutoCAD.ApplicationServices;

namespace ZLPlugin.Commands
{
    class KeyUtil
    {
        public static string GetDiskVolumeSeriaNumber()
        {
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObject disk = new ManagementObject("win32_logicaldisk.deviceid=\"d:\"");
            disk.Get();
            return disk.GetPropertyValue("VolumeSerialNumber").ToString();
        }

        public static string GetCpu()
        {
            string strCpu = null;
            ManagementClass myCpu = new ManagementClass("win32_Processor");
            ManagementObjectCollection myCpuCollection = myCpu.GetInstances();
            foreach (ManagementObject cpuObj in myCpuCollection) 
            {
                strCpu = cpuObj.Properties["Processorid"].Value.ToString();
                break;
            }
            return strCpu;
        }

        public static string GetMNum()
        {
            string strNum = GetCpu() + GetDiskVolumeSeriaNumber();
            string strMNum = strNum.Substring(0, 24);
            return strMNum;
        }

        public static string GetRegisterKey(string mnum)
        {
            string key = mnum + "ZHP";

            //获取加密服务  
            System.Security.Cryptography.MD5CryptoServiceProvider md5CSP = new System.Security.Cryptography.MD5CryptoServiceProvider();

            //获取要加密的字段，并转化为Byte[]数组  
            byte[] testEncrypt = System.Text.Encoding.Unicode.GetBytes(key);

            //加密Byte[]数组  
            byte[] resultEncrypt = md5CSP.ComputeHash(testEncrypt);

            //将加密后的数组转化为字段(普通加密)  
            //string testResult = System.Text.Encoding.Unicode.GetString(resultEncrypt);
            //return testResult;
            return BitConverter.ToString(resultEncrypt).Replace("-", "").Substring(0, 16);
        }

        public static bool checkRegisted()
        {
            string regKey = (string)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\CadPlugin", "regKey", "");
            if (regKey == GetRegisterKey(GetMNum()))
            {
                return true;
            }
            return false;
        }

        public static bool checkUseTimeOut()
        {
            
            Int32 tLong = CadPlugin.FREE_USE_TIME;
            try
            {
                tLong = (Int32)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\CadPlugin", "UseTimes", 0);
                if (tLong < CadPlugin.FREE_USE_TIME)
                {
                    Application.ShowAlertDialog("感谢您使用了此软件，您还可使用" + (CadPlugin.FREE_USE_TIME - tLong) + "次");
                }
            }
            catch
            {
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\CadPlugin", "UseTimes", 0, RegistryValueKind.DWord);
                Application.ShowAlertDialog("欢迎新用户使用本软件" + tLong + "次");
            }
            tLong = (Int32)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\CadPlugin", "UseTimes", 0);
            if (tLong < CadPlugin.FREE_USE_TIME)
            {
                return false;
            }
            else
            {
                Application.ShowAlertDialog("试用次数已到,请注册");
                return true;
            }
        }

        public static int addUseTime()
        {
            int tLong = 0;
            try
            {
                tLong = (Int32)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\CadPlugin", "UseTimes", 0);
            }
            catch
            {
                Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\CadPlugin", "UseTimes", 0, RegistryValueKind.DWord);
            }
            tLong += 1;
            Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\CadPlugin", "UseTimes", tLong);
            return tLong;
        }

        public static void writeRegisterKey()
        {
            string regKey = GetRegisterKey(GetMNum());
            Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\CadPlugin", "regKey", regKey, RegistryValueKind.String);
            DateTime time = DateTime.Now;
            Registry.SetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\CadPlugin", "regTime", time.Ticks, RegistryValueKind.QWord);
        }
    }
}
