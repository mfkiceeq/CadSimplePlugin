using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Colors;

using ZLPlugin.Commands;
using Autodesk.AutoCAD.Geometry;
using System.Collections;
using Autodesk.AutoCAD;
[assembly:CommandClass(typeof(ZLPlugin.CadPlugin))]
namespace ZLPlugin
{
    public class CadPlugin
    {
        public const int FREE_USE_TIME = 30;

        public bool canUse = false;

        public bool isPrivate = false;

        public CadPlugin()
        {
            if (KeyUtil.checkRegisted())
            {
                if (TimeUtil.checkOutOfTime())
                {
                    canUse = false;
                }
                else
                {
                    canUse = true;
                }
            }
            else
            {
                if (!KeyUtil.checkUseTimeOut())
                {
                    canUse = true;
                    int useTime = KeyUtil.addUseTime();
                    logToEditor("未注册版，还可免费体验" + (FREE_USE_TIME - useTime + 1) + "次");
                }
                else
                {
                    canUse = false;
                }
            }
        }

        //乔木命令：统计所有选中块的数目
        [CommandMethod("qm")]
        public void qmCommand()
        {
            if (!checkCanUse()) return;
            FileUtil.initTreeInfo();
            BasicCommand command = new ZLQMCommand();
            logToCadText(command.excute().result);
        }
        
        //灌木命令：统计所有灌木面积和
        [CommandMethod("gm")]
        public void MethodSumAllText()
        {
            if (!checkCanUse()) return;
            BasicCommand command = new ZLQHCommand();
            logToCadText(command.excute().result);
        }
        
        [CommandMethod("dy")]
        public void dyCommand()
        {
            if (!checkCanUse()) return;
            BasicCommand command = new ZLDYCommand();
            command.excute();
        }

        //将面积放到数字后面
        [CommandMethod("fg")]
        public void mjCommand()
        {
            if (!checkCanUse()) return;
            BasicCommand command = new ZLMJCommand();
            command.excute();
        }

        [CommandMethod("zcm")]
        public void keyGenCommand()
        {
            if (!isPrivate)
            {
                return;
            }
            KeyGenForm rForm = new KeyGenForm();
            Application.ShowModelessDialog(rForm);
        }

        public void testCommand()
        {
            return;
            RegisterForm rForm = new RegisterForm();
            Application.ShowModelessDialog(rForm);
            logToEditor(KeyUtil.GetDiskVolumeSeriaNumber());
            logToEditor(KeyUtil.GetCpu());
            logToEditor(KeyUtil.GetMNum());
            logToEditor(KeyUtil.GetRegisterKey(KeyUtil.GetMNum()));
            return;
            if (TimeUtil.checkOutOfTime()) return;
            FileUtil.initTreeInfo();
            BasicCommand command = new TestCommand();
            logToCadText(command.excute().result);
        }

        protected bool checkCanUse()
        {
            if (this.canUse)
            {
                return true;
            }
            else
            {
                if (KeyUtil.checkRegisted())
                {
                    if (TimeUtil.checkOutOfTime())
                    {
                        canUse = false;
                    }
                    else
                    {
                        canUse = true;
                    }
                    return canUse;
                }
                else
                {
                    canUse = false;
                }
                Application.ShowModelessDialog(new RegisterForm());
            }
            logToEditor("请输入注册码");
            return canUse;
        }
        public static void logToEditor(string value)
        {
            Application.DocumentManager.MdiActiveDocument.Editor.WriteMessage(value + "\n");
        }

        public static void logToCadText(object excuteResult)
        {
            Document acDoc = Application.DocumentManager.MdiActiveDocument;
            Database acCurDb = acDoc.Database;
            Dictionary<string, float> sumDict = new Dictionary<string, float>();
            ObjectId textStyle = acCurDb.Textstyle;

            using (Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                BlockTable acBlkTbl;
                acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId, OpenMode.ForRead) as BlockTable;
                BlockTableRecord acBlkTbRec;
                acBlkTbRec = acTrans.GetObject(acBlkTbl[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                PromptPointResult point = acDoc.Editor.GetPoint("\n结果的位置:");
                if (point.Status == PromptStatus.OK)
                {
                    ObjectId oId;
                    if (textStyle != null)
                    {
                        oId = textStyle;
                    }
                    else
                    {
                        oId = acCurDb.Textstyle;
                    }
                    List<Entity> entities = getResultEntitys(excuteResult, oId, point.Value);
                    logToEditor("Entity Count" + entities.Count.ToString());
                    if (entities.Count > 0)
                    {
                        foreach (Entity en in entities)
                        {
                            acBlkTbRec.AppendEntity(en);
                            acTrans.AddNewlyCreatedDBObject(en, true);
                        }
                    }
                }
                acTrans.Commit();
            }
        }

        public static List<Entity> getResultEntitys(object result, ObjectId textStyle, Point3d location)
        {
            List<Entity> entities = new List<Entity>();
            Point3d startLoction = location.Add(new Vector3d(0,0,0));
            int fontHeight = 500;
            
            if (result.GetType() == typeof(BlockReference))
            {
                logToEditor("Result is BlockReference" + result.ToString());
                BlockReference en = (result as BlockReference).Clone() as BlockReference;
                en.Position = location;
                entities.Add(en);
            }
            else if (result is IEnumerable && !(result is string))
            {
                foreach (object item in (result as IEnumerable))
                {
                    entities.AddRange(getResultEntitys(item, textStyle, location));
                    if (item is IEnumerable && !(item is string))
                    {
                        location = location.Add(new Vector3d(0, -fontHeight*4, 0));
                    }
                    else
                    {
                        location = location.Add(new Vector3d(fontHeight*8, 0, 0));
                    }
                }
            }
            else
            {
                logToEditor("Result is String" + result);
                MText objText = new MText();
                objText.TextHeight = fontHeight ;
                objText.SetDatabaseDefaults();
                objText.Location = startLoction;
                objText.Contents = result.ToString();
                objText.TextStyleId = textStyle;
                entities.Add(objText);
            }
            return entities;
        }
    }
}
