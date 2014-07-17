using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.DatabaseServices;

namespace ZLPlugin.Commands
{
    class TestCommand:BasicCommand
    {
        public override Result excute()
        {
            List<List<object>> list1 = new List<List<object>>();
            List<object> list2 = new List<object>();
            using (Transaction acTrans = acDoc.Database.TransactionManager.StartTransaction())
            {
                PromptEntityResult resultTxt = acDoc.Editor.GetEntity(new PromptEntityOptions("\n选择文本"));
                if (resultTxt.Status == PromptStatus.OK)
                {
                    Entity txt = acTrans.GetObject(resultTxt.ObjectId,
                                                                 OpenMode.ForWrite) as Entity;
                    list2.Add(txt);
                    list2.Add("3");
                    list1.Add(list2);
                }
                list1.Add(new List<object>(new string[] { "5", "6" }));
                list1.Add(new List<object>(new string[] { "TestComand", "Num" }));
                acTrans.Commit();
                acTrans.Dispose();
            }
            return new Result(list1);
        }
    }
}
