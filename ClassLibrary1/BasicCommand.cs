using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;

namespace ZLPlugin
{
    //命令的基本类型
    public class BasicCommand
    {
        public BasicCommand()
        {
            acDoc = Application.DocumentManager.MdiActiveDocument;
            editor = acDoc.Editor;
        }

        protected Document acDoc;
        protected Editor editor;

        public virtual Result excute() {
            return null;
        }

        public void log(string value)
        {
            if (editor != null)
            {
                editor.WriteMessage(value + "\n");
            }
        }
    }

    public class Result
    {
        public Result(object value)
        {
            result = value;
        }
        //可能是字符串，也可以数组[entityId_12, str]
        public object result;

        public override string ToString()
        {
            return result.ToString();
        }
    }
    
}
