using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using System.Collections;
namespace ZLPlugin.Commands
{
    /*
     * 统计所有选中块的数目
     * */
    class ZLQMCommand:BasicCommand
    {
        public override Result excute()
        {
            Database acCurDb = acDoc.Database;
            PromptSelectionResult resutl = acDoc.Editor.GetSelection();
            Dictionary<string, float> sumDict = new Dictionary<string, float>();
            Dictionary<string, Entity> entityDict = new Dictionary<string, Entity>();
            string selectLayer = "";
            ObjectId textStyle = acCurDb.Textstyle;
            string blName;
            List<List<object>> exResult = new List<List<object>>();
            if (resutl.Status == PromptStatus.OK)
            {
                acDoc.Editor.WriteMessage("Select the entity Count:" + resutl.Value.Count);
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
                                if (acEnt.GetType() == typeof(BlockReference))
                                {
                                    blName = (acEnt as BlockReference).Name;
                                    if (sumDict.ContainsKey(blName))
                                    {
                                        sumDict[blName] += 1;
                                    }
                                    else
                                    {
                                        sumDict[blName] = 1;
                                    }
                                    entityDict[blName] = acEnt;
                                }
                            }

                        }
                    }
                    List<object> tempList;
                    foreach (string key in sumDict.Keys)
                    {
                        tempList = new List<object>();
                        if(entityDict.ContainsKey(key))
                        {
                            tempList.Add(entityDict[key]);
                        }
                        tempList.Add(key);
                        tempList.Add(sumDict[key]);
                        if (Constants.treeInfo != null && Constants.treeInfo.ContainsKey(key))
                        {
                            foreach (string s in Constants.treeInfo[key])
                            {
                                tempList.Add(s);
                            }
                        }
                        //result += key + ":" + sumDict[key] + "\n";
                        exResult.Add(tempList);
                        log("\nKey tempList Count" + key + exResult.Count + " " + tempList.Count);
                    }
                    acTrans.Commit();
                    acTrans.Dispose();
                }
            }
            log("Count" + exResult.Count);
            return new Result(exResult);
        }
    }
}
