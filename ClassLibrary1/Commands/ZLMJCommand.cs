using System;
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

            PromptEntityResult result = acDoc.Editor.GetEntity(new PromptEntityOptions("选择多边形/文本"));
            using(Transaction acTrans = acDoc.Database.TransactionManager.StartTransaction())
            {
                if (result.Status == PromptStatus.OK)
                {
                    Entity acEnt = acTrans.GetObject(result.ObjectId,
                                                             OpenMode.ForWrite) as Entity;

                    acEnt.Highlight();
                    if (acEnt.GetType() == typeof(Polyline))
                    {
                        //double area = Math.Ceiling((acEnt as Polyline).Area/1000000);
                        double area = (acEnt as Polyline).Area;
                        bool resultDone = false;
                        while( !resultDone)
                        {
                            PromptEntityResult resultTxt = acDoc.Editor.GetEntity(new PromptEntityOptions("\n选择文本"));

                            if (resultTxt.Status == PromptStatus.OK)
                            {
                                Entity txt = acTrans.GetObject(resultTxt.ObjectId,
                                                                 OpenMode.ForWrite) as Entity;
                                string txtContent;
                                if (txt.GetType() == typeof(MText))
                                {
                                    area = Math.Ceiling(area / 1000000);
                                    txtContent = formatTxt((txt as MText).Contents);
                                    txtContent += area.ToString();
                                    (txt as MText).Color = Color.FromRgb(0xFF, 0xFF, 0xFF);
                                    (txt as MText).Contents = txtContent;
                                    resultDone = true;
                                }
                                else if (txt.GetType() == typeof(DBText))
                                {
                                    area = Math.Ceiling(area / 1000000);
                                    txtContent = formatTxt((txt as DBText).TextString);
                                    txtContent += area.ToString();
                                    (txt as DBText).Color = Color.FromRgb(0xFF, 0xFF, 0xFF);
                                    (txt as DBText).TextString = txtContent;
                                    resultDone = true;
                                }
                                else if (txt.GetType() == typeof(Polyline))
                                {
                                    area -= (txt as Polyline).Area;
                                }
                            }
                            else
                            {
                                resultDone = true;
                            }
                        }
                    }
                    else if (acEnt.GetType() == typeof(MText) || acEnt.GetType() == typeof(DBText))
                    {
                        PromptEntityResult resultTxt = acDoc.Editor.GetEntity(new PromptEntityOptions("\n选择文本"));

                        if (resultTxt.Status == PromptStatus.OK)
                        {
                            Entity txt = acTrans.GetObject(resultTxt.ObjectId,
                                                             OpenMode.ForWrite) as Entity;
                            String txtContent = "";
                            if (acEnt.GetType() == typeof(MText))
                            {
                                txtContent = (acEnt as MText).Contents;
                            }
                            else if (acEnt.GetType() == typeof(DBText))
                            {
                                txtContent = (acEnt as DBText).TextString;
                            }

                            if (txt.GetType() == typeof(MText))
                            {
                                (txt as MText).Contents = txtContent;
                            }
                            else if (txt.GetType() == typeof(DBText))
                            {
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
