using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using ZLPlugin;

namespace ZLPlugin.Commands
{
    class ZLQHCommand:BasicCommand
    {
        public override Result excute()
        {
            Database acCurDb = acDoc.Database;
            PromptSelectionResult resutl = acDoc.Editor.GetSelection();
            Dictionary<string, float> sumDict = new Dictionary<string, float>();
            string selectLayer = "";
            ObjectId textStyle = acCurDb.Textstyle;
            if (resutl.Status == PromptStatus.OK)
            {
                log("选择的个数:" + resutl.Value.Count);
                SelectionSet acSSet = resutl.Value;

                using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
                {
                    // Step through the objects in the selection set
                    foreach (SelectedObject acSSObj in acSSet)
                    {
                        // Check to make sure a valid SelectedObject object was returned
                        if (acSSObj != null)
                        {
                            // Open the selected object for write
                            Entity acEnt = acTrans.GetObject(acSSObj.ObjectId,
                                                             OpenMode.ForWrite) as Entity;
                            selectLayer = acEnt.Layer;
                            List<string> selectStrs = new List<string>();
                            if (acEnt != null)
                            {
                                if (acEnt.GetType() == typeof(MText))
                                {
                                    selectStrs.Add((acEnt as MText).Contents);
                                    textStyle = (acEnt as MText).TextStyleId;
                                }
                                else if (acEnt.GetType() == typeof(DBText))
                                {
                                    selectStrs.Add((acEnt as DBText).TextString);
                                }
                                else if (acEnt.GetType().ToString() == "")
                                {
                                }
                            }

                            foreach (string item in selectStrs)
                            {
                                Array result = getStrNumber(item, acDoc.Editor);
                                //acDoc.Editor.WriteMessage("\n Parse content " + result.GetValue(0) + "," + result.GetValue(1));
                                string numStr = result.GetValue(1) as string;
                                if (numStr.Length == 0)
                                {
                                    numStr = "0";
                                }
                                float num = 0;
                                try
                                {
                                    num = float.Parse(numStr);
                                }
                                catch (System.Exception e)
                                {
                                    acDoc.Editor.WriteMessage("\n文本的内容格式不正确，请修改后再运行命令:" + item);
                                    continue;
                                }
                                if (sumDict.ContainsKey(result.GetValue(0) as string))
                                {
                                    sumDict[result.GetValue(0) as string] += num;
                                }
                                else
                                {
                                    sumDict.Add(result.GetValue(0) as string, num);
                                }
                            }

                        }
                    }

                    string sumResult = "";
                    foreach (string key in sumDict.Keys)
                    {
                        sumResult += key + ":" + sumDict[key] + "\n";
                    }

                    List<object> tempList;
                    List<List<object>> exResult = new List<List<object>>();
                    foreach (string key in sumDict.Keys)
                    {
                        tempList = new List<object>();
                        tempList.Add(key);
                        tempList.Add(sumDict[key]);
                        //result += key + ":" + sumDict[key] + "\n";
                        exResult.Add(tempList);
                    }
                    acTrans.Commit();
                    return new Result(exResult);
                }
            }
            return new Result("未选择目标");
        }

        private Array getStrNumber(string str, Editor editor)
        {

            Array result = new string[2];
            Array chars = str.ToCharArray();
            //editor.WriteMessage("\n num start:" + str + chars.ToString());
            string name = "";
            foreach (char ch in chars)
            {
                if (ch >= '0' && ch <= '9')
                {
                    break;
                }
                else
                {
                    name += ch.ToString();
                }
            }
            result.SetValue(name, 0);
            string num = str.Substring(name.Length);
            //editor.WriteMessage("\n num before:" + num + num.IndexOf("=") + str);
            if (num.IndexOf("=") >= 0)
            {
                num = num.Substring(num.IndexOf("=") + 1);
            }
            string realNum = "";
            chars = num.ToCharArray();
            //editor.WriteMessage("\n num:" +num + chars.Length);
            bool needWarning = false;
            foreach (char ch in chars)
            {
                //editor.WriteMessage("\n char:" +ch.ToString());
                if (ch >= '0' && ch <= '9')
                {
                    realNum += ch.ToString();
                }
                else
                {
                    needWarning = true;
                    break;
                }
            }
            if (needWarning)
            {
                editor.WriteMessage("\n 解析可能有问题的文本:" + str + ",解析结果为名字：" + name + ", 数量为：" + realNum);
            }
            //editor.WriteMessage("\n num end:" +num +chars.ToString()+realNum);
            result.SetValue(realNum, 1);
            return result;
        }

        public string getProperties<T>(T t)
        {
            string tStr = string.Empty;
            if (t == null)
            {
                return tStr;
            }
            System.Reflection.PropertyInfo[] properties = t.GetType().GetProperties(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);

            if (properties.Length <= 0)
            {
                return tStr;
            }
            foreach (System.Reflection.PropertyInfo item in properties)
            {
                string name = item.Name;
                object value = item.GetValue(t, null);
                if (item.PropertyType.IsValueType || item.PropertyType.Name.StartsWith("String"))
                {
                    tStr += string.Format("{0}:{1},", name, value);
                }
                else
                {
                    getProperties(value);
                }
            }
            return tStr;
        }
    }
}
