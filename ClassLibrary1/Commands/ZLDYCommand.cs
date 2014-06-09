using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;

namespace ZLPlugin.Commands
{
    class ZLDYCommand:BasicCommand
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
                                    log((acEnt as MText).Contents + "\n");
                                }
                                else if (acEnt.GetType() == typeof(DBText))
                                {
                                    selectStrs.Add((acEnt as DBText).TextString);
                                    log((acEnt as DBText).TextString + "\n");
                                }
                                else if (acEnt.GetType().ToString() == "")
                                {
                                }
                            }
                        }
                    }
                    acTrans.Commit();
                }
            }
            return new Result("Done");
        }
    }
}
