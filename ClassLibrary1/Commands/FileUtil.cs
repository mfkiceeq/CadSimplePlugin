using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ZLPlugin.Commands
{
    class FileUtil
    {
        public static void initTreeInfo()
        {
            if (Constants.treeInfo == null)
            {
                try
                {
                    Dictionary<string, List<string>> info = new Dictionary<string, List<string>>();
                    FileStream file = new FileStream(Constants.TREE_INFO_PATH, FileMode.OpenOrCreate);
                    StreamReader reader = new StreamReader(file);
                    string line = reader.ReadLine();
                    string[] tempArr;
                    while (line != null)
                    {
                        tempArr = line.Split(new char[] { ' ' });
                        if (tempArr.Length > 0)
                        {
                            
                            List<string> tempList = new List<string>(tempArr);
                            tempList.RemoveAt(0);
                            info[tempArr[0]] = tempList;
                            
                            line = reader.ReadLine();
                            CadPlugin.logToEditor(tempArr[0]);
                        }
                    }
                    Constants.treeInfo = info;
                    reader.Close();
                }
                catch (System.Exception ex)
                {
                    CadPlugin.logToEditor(ex.ToString());
                }
            }
        }
    }
}
