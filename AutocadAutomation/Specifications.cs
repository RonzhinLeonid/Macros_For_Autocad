using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using MgdAcApplication = Autodesk.AutoCAD.ApplicationServices.Application;
using AutocadAutomation.TypeBlocks;
using AutocadAutomation.BlocksClass;
using Autodesk.AutoCAD.Geometry;
using System.Reflection;

namespace AutocadAutomation
{
    public class Specifications
    {
        const string tableStyle2Name = "Перечень компонентов";
        [CommandMethod("DwgTest")]
        public void DwgFileDataSet_Method()
        {
            Document adoc = Application.DocumentManager.MdiActiveDocument;
            Database db = adoc.Database;
            Editor ed = adoc.Editor;

            var tableListComponents = new TableListComponents(db);
            var blockForListComponents = tableListComponents.BlockForListComponents;

            var listComponents = tableListComponents.GetTableListComponents();

            //List<BlockForListComponents> blockForListComponents = HetListBlockForTableComponents(db);

            Document doc = Application.DocumentManager.MdiActiveDocument;
            //Database db = doc.Database;
            using (doc.LockDocument())
            {
                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                   DBDictionary dict = tr.GetObject(db.TableStyleDictionaryId, OpenMode.ForRead) as DBDictionary;
                    if (dict.Contains(tableStyle2Name))
                    //TableStyleWithTemplate_Command();
                    {
                        TableStyle ts = tr.GetObject(dict.GetAt(tableStyle2Name),OpenMode.ForRead) as TableStyle;
                        TableTemplate template = tr.GetObject(ts.Template, OpenMode.ForRead) as TableTemplate;
                        // Включим отображение толщин линий, 
                        // дабы увидеть результат нашей работы
                        MgdAcApplication.SetSystemVariable("LWDISPLAY", 1);
                        // Создаём новую таблицу, на основе 
                        // созданного нами шаблона.
                        Table tableInstance = new Table();
                        tableInstance.CopyFrom(template, TableCopyOptions.FillTarget);
                        tableInstance.GenerateLayout();
                        tableInstance.Position = new Point3d(500, 500, 0);
                        BlockTable bt = tr.GetObject( db.BlockTableId, OpenMode.ForRead)  as BlockTable;
                        BlockTableRecord modelSpace = tr.GetObject(bt[BlockTableRecord.ModelSpace],OpenMode.ForWrite) as BlockTableRecord;
                        modelSpace.AppendEntity(tableInstance);
                        tr.AddNewlyCreatedDBObject(tableInstance, true);
                        tableInstance.InsertRows(1, 300, listComponents.Count);
                        for (int i = 0; i < listComponents.Count; i++)
                        {
                            tableInstance.Cells[i+1, 0].TextString = listComponents[i].PosItem.ToString();
                            tableInstance.Cells[i+1, 1].TextString = listComponents[i].FullDescription.ToString();
                            tableInstance.Cells[i+1, 2].TextString = listComponents[i].Count.ToString();
                            tableInstance.Cells[i+1, 3].TextString = listComponents[i].Note.ToString();

                            //for (int j = 0; j < tableInstance.Columns.Count;  j++)
                            //{
                            //    //tableInstance.Cells[i, j].TextHeight = 1;
                            //    //if (i == 0 && j == 0)
                            //    //    tableInstance.Cells[i, j].TextString =
                            //    //        "Заголовок";
                            //    //else
                            //     tableInstance.Cells[i, j].TextString =  i.ToString() + "," + j.ToString();

                            //    //tb.Cells[i, j].Alignment = CellAlignment.MiddleCenter;
                            //}
                        }
                        tr.Commit();
                    }
                    Object acadObject = MgdAcApplication.AcadApplication;
                    acadObject.GetType().InvokeMember("ZoomExtents", BindingFlags.InvokeMethod, null, acadObject, null);
                }
            }
        }


        
    }
}
