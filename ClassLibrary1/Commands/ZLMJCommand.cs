﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Colors;

namespace ZLPlugin.Commands
{
    class ZLMJCommand:BasicCommand
    {
        public override Result excute()
        {

            PromptEntityResult result = acDoc.Editor.GetEntity(new PromptEntityOptions("选择多边形"));
            using(Transaction acTrans = acDoc.Database.TransactionManager.StartTransaction())
            {
                if (result.Status == PromptStatus.OK)
                {
                    Entity acEnt = acTrans.GetObject(result.ObjectId,
                                                             OpenMode.ForWrite) as Entity;
                    if (acEnt.GetType() == typeof(Polyline))
                    {
                        double area = Math.Ceiling((acEnt as Polyline).Area/1000000);
                        PromptEntityResult resultTxt = acDoc.Editor.GetEntity(new PromptEntityOptions("\n选择文本"));

                        if (resultTxt.Status == PromptStatus.OK)
                        {
                            Entity txt = acTrans.GetObject(resultTxt.ObjectId,
                                                             OpenMode.ForWrite) as Entity;
                            string txtContent;
                            if (txt.GetType() == typeof(MText))
                            {
                                txtContent = formatTxt((txt as MText).Contents);
                                txtContent += area.ToString();
                                (txt as MText).Color = Color.FromRgb(0xFF, 0xFF, 0xFF);
                                (txt as MText).Contents = txtContent;
                            }
                            else if (txt.GetType() == typeof(DBText))
                            {
                                txtContent = formatTxt((txt as DBText).TextString);
                                txtContent += area.ToString();
                                (txt as DBText).Color = Color.FromRgb(0xFF, 0xFF, 0xFF);
                                (txt as DBText).TextString = txtContent;
                            }
                        }
                    }
                    acTrans.Commit();
                    acTrans.Dispose();
                }
            }
            return new Result("Done");
        }

        public string formatTxt(string str)
        {
            Array chars = str.ToCharArray();
            //editor.WriteMessage("\n num start:" + str + chars.ToString());
            string txt = "";
            foreach (char ch in chars)
            {
                if (ch >= '0' && ch <= '9')
                {
                    break;
                }
                else
                {
                    txt += ch.ToString();
                }
            }
            return txt;
        }
    }
}
