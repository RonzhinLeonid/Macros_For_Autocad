using AutocadAutomation.StringTable;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocadAutomation
{
    internal static class Tables
    {
        private const string tableStyle2Name = "Перечень компонентов";
        private const int startRowTableComponents = 2;

        public static void CrateTableComponents(Document adoc, Database db, List<StringTableListComponents> listComponents, Point3d point)
        {
            using (adoc.LockDocument())
            {
                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    DBDictionary dict = tr.GetObject(db.TableStyleDictionaryId, OpenMode.ForRead) as DBDictionary;
                    if (dict.Contains(tableStyle2Name))   //необходимо переделать проверку, если нужного шаблона нет, то запустить метод по созданию этого шаблона
                    //TableStyleWithTemplate_Command();
                    {
                        TableStyle ts = tr.GetObject(dict.GetAt(tableStyle2Name), OpenMode.ForRead) as TableStyle;
                        TableTemplate template = tr.GetObject(ts.Template, OpenMode.ForRead) as TableTemplate;
                        // Включим отображение толщин линий,
                        // дабы увидеть результат нашей работы
                        Autodesk.AutoCAD.ApplicationServices.Application.SetSystemVariable("LWDISPLAY", 1);
                        // Создаём новую таблицу, на основе
                        // созданного нами шаблона.
                        Table tableInstance = new Table();
                        tableInstance.CopyFrom(template, TableCopyOptions.FillTarget);
                        tableInstance.GenerateLayout();
                        tableInstance.Position = point;
                        BlockTable bt = tr.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
                        BlockTableRecord modelSpace = tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;
                        modelSpace.AppendEntity(tableInstance);
                        tr.AddNewlyCreatedDBObject(tableInstance, true);
                        tableInstance.InsertRows(startRowTableComponents, 8, listComponents.Count);
                        int row = startRowTableComponents;
                        foreach (var item in listComponents)
                        {
                            tableInstance.Cells[row, 0].TextString = item.PosItem.ToString();
                            tableInstance.Cells[row, 1].TextString = item.AllTag.ToString();
                            tableInstance.Cells[row, 2].TextString = item.FullDescription.ToString();
                            tableInstance.Cells[row, 3].TextString = item.Count.ToString();
                            tableInstance.Cells[row, 4].TextString = item.Note.ToString();
                            row++;
                        }
                        tr.Commit();
                    }
                }
            }
        }
    }
}