using AutocadAutomation.StringTable;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using cad = Autodesk.AutoCAD.ApplicationServices.Application;
using System.Globalization;
using AutocadAutomation.BlocksClass;

namespace AutocadAutomation
{
    internal static class Tables
    {
        private const string textStyleName = "SocTrade";
        private const string tableComponentStyleCell = "Наименование компонентов";

        private const string tableComponentStyleName = "Перечень компонентов";
        private const int startRowTableComponents = 2;

        private const string tableCableMagazineStyleName = "Кабельный журнал";
        private const int startRowTableCableMagazine = 3;

        private const string tableTubeConnectionsStyleName = "Трубные соединения";
        private const int startRowTableTubeConnections = 1;

        private const string tableGeneralSpecificationStyleName = "Общая спецификация";
        private const int startRowTableGeneralSpecification = 2;

        public static void CrateTableComponents(Document adoc, Database db, List<StringTableListComponents> listComponents, Point3d point)
        {
            using (adoc.LockDocument())
            {
                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    DBDictionary dict = tr.GetObject(db.TableStyleDictionaryId, OpenMode.ForRead) as DBDictionary;
                    if (!dict.Contains(tableComponentStyleName))   //необходимо переделать проверку, если нужного шаблона нет, то запустить метод по созданию этого шаблона
                    {
                        MessageBox.Show($"Проверьте шаблон чертежа. Стиль таблицы \"{tableComponentStyleName}\" отсутствует!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        TableStyle ts = tr.GetObject(dict.GetAt(tableComponentStyleName), OpenMode.ForRead) as TableStyle;
                        string[] cellStyles = ts.CellStyles.Cast<string>().ToArray();
                        string cellStyle = cellStyles.FirstOrDefault(item => item.ToLower().Contains(tableComponentStyleCell.ToLower()));

                        TableTemplate template;
                        try
                        {
                            template = tr.GetObject(ts.Template, OpenMode.ForRead) as TableTemplate;
                        }
                        catch (Exception)
                        {
                            MessageBox.Show($"Проверьте стиль таблицы. нет привязки к таблице", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        // Включим отображение толщин линий,
                        // дабы увидеть результат нашей работы
                        //cad.SetSystemVariable("LWDISPLAY", 1);
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
                        if (tableInstance.Rows.Count > startRowTableComponents)
                            tableInstance.DeleteRows(startRowTableComponents, tableInstance.Rows.Count - startRowTableComponents);

                        tableInstance.InsertRows(startRowTableComponents, 8, listComponents.Count);
                        int row = startRowTableComponents;
                        foreach (var item in listComponents)
                        {
                            tableInstance.Cells[row, 0].TextString = item.PosItem.ToString();
                            tableInstance.Cells[row, 1].TextString = item.AllTag;
                            tableInstance.Cells[row, 2].TextString = item.FullDescription;
                            tableInstance.Cells[row, 3].TextString = item.Count.ToString();
                            tableInstance.Cells[row, 4].TextString = item.Note;
                            if (!string.IsNullOrEmpty(cellStyle))
                                tableInstance.Cells[row, 2].Style = cellStyle;
                            row++;
                        }
                        tr.Commit();
                    }
                }
            }
        }

        public static void CrateTableCableMagazine(Document adoc, Database db, List<BlockForCableMagazine> listComponents, Point3d point)
        {
            using (adoc.LockDocument())
            {
                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    DBDictionary dict = tr.GetObject(db.TableStyleDictionaryId, OpenMode.ForRead) as DBDictionary;
                    if (!dict.Contains(tableCableMagazineStyleName))   //необходимо переделать проверку, если нужного шаблона нет, то запустить метод по созданию этого шаблона
                    {
                        MessageBox.Show($"Проверьте шаблон чертежа. Стиль таблицы \"{tableCableMagazineStyleName}\" отсутствует!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        TableStyle ts = tr.GetObject(dict.GetAt(tableCableMagazineStyleName), OpenMode.ForRead) as TableStyle;
                        string[] cellStyles = ts.CellStyles.Cast<string>().ToArray();
                        string cellStyle = cellStyles.FirstOrDefault(item => item.ToLower().Contains(tableComponentStyleCell.ToLower()));

                        TableTemplate template;
                        try
                        {
                            template = tr.GetObject(ts.Template, OpenMode.ForRead) as TableTemplate;
                        }
                        catch (Exception)
                        {
                            MessageBox.Show($"Проверьте стиль таблицы. нет привязки к таблице", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        // Включим отображение толщин линий,
                        // дабы увидеть результат нашей работы
                        //cad.SetSystemVariable("LWDISPLAY", 1);
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
                        if (tableInstance.Rows.Count > startRowTableCableMagazine)
                            tableInstance.DeleteRows(startRowTableCableMagazine, tableInstance.Rows.Count - startRowTableCableMagazine);

                        tableInstance.InsertRows(startRowTableCableMagazine, 8, listComponents.Count);
                        int row = startRowTableCableMagazine;
                        foreach (var item in listComponents)
                        {
                            if (item.InSpecification)
                            {
                                tableInstance.Cells[row, 0].TextString = item.Tag;
                                tableInstance.Cells[row, 1].TextString = item.Start;
                                tableInstance.Cells[row, 2].TextString = item.Finish;
                                if (item.MarkCable == "" && item.CoresCable == "" && item.Length == 0)
                                {
                                    CellRange range = CellRange.Create(tableInstance, row, 1, row, 12);
                                    tableInstance.MergeCells(range);
                                }
                                else
                                {
                                    tableInstance.Cells[row, 7].TextString = item.MarkCable;
                                    if (item.MarkCable.ToLower() == "комплектно" && item.CoresCable == "")
                                    {
                                        CellRange range = CellRange.Create(tableInstance, row, 7, row, 8);
                                        tableInstance.MergeCells(range);
                                    }
                                    else
                                        tableInstance.Cells[row, 8].TextString = item.CoresCable;
                                    tableInstance.Cells[row, 9].TextString = item.Length.ToString();
                                }
                                row++; 
                            }
                            else
                                tableInstance.DeleteRows(tableInstance.Rows.Count - 1, 1);
                        }
                        tr.Commit();
                    }
                }
            }
        }

        public static void CrateTableTubeСonnections(Document adoc, Database db, List<BlockForTubeСonnections> listComponents, Point3d point)
        {
            using (adoc.LockDocument())
            {
                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    DBDictionary dict = tr.GetObject(db.TableStyleDictionaryId, OpenMode.ForRead) as DBDictionary;
                    if (!dict.Contains(tableTubeConnectionsStyleName))   //необходимо переделать проверку, если нужного шаблона нет, то запустить метод по созданию этого шаблона
                    {
                        MessageBox.Show($"Проверьте шаблон чертежа. Стиль таблицы \"{tableTubeConnectionsStyleName}\" отсутствует!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        TableStyle ts = tr.GetObject(dict.GetAt(tableTubeConnectionsStyleName), OpenMode.ForRead) as TableStyle;
                        string[] cellStyles = ts.CellStyles.Cast<string>().ToArray();
                        string cellStyle = cellStyles.FirstOrDefault(item => item.ToLower().Contains(tableComponentStyleCell.ToLower()));

                        TableTemplate template;
                        try
                        {
                            template = tr.GetObject(ts.Template, OpenMode.ForRead) as TableTemplate;
                        }
                        catch (Exception)
                        {
                            MessageBox.Show($"Проверьте стиль таблицы. нет привязки к таблице", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        // Включим отображение толщин линий,
                        // дабы увидеть результат нашей работы
                        //cad.SetSystemVariable("LWDISPLAY", 1);
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
                        if (tableInstance.Rows.Count > startRowTableTubeConnections)
                            tableInstance.DeleteRows(startRowTableTubeConnections, tableInstance.Rows.Count - startRowTableTubeConnections);

                        tableInstance.InsertRows(startRowTableTubeConnections, 8, listComponents.Count);
                        int row = startRowTableTubeConnections;
                        foreach (var item in listComponents)
                        {
                            if (item.InSpecification)
                            {
                                tableInstance.Cells[row, 0].TextString = item.Tag;
                                tableInstance.Cells[row, 1].TextString = item.Description;
                                tableInstance.Cells[row, 2].TextString = item.Conection;
                                tableInstance.Cells[row, 3].TextString = item.Material;
                                if (!string.IsNullOrEmpty(cellStyle))
                                    tableInstance.Cells[row, 1].Style = cellStyle;
                                row++;
                            }
                            else
                                tableInstance.DeleteRows(tableInstance.Rows.Count - 1, 1);
                        }
                        tr.Commit();
                    }
                }
            }
        }

        public static void CrateTableGeneralSpecification(Document adoc, Database db, List<StringTableGeneralSpecification> listComponents, Point3d point)
        {
            using (adoc.LockDocument())
            {
                using (Transaction tr = db.TransactionManager.StartTransaction())
                {
                    DBDictionary dict = tr.GetObject(db.TableStyleDictionaryId, OpenMode.ForRead) as DBDictionary;
                    if (!dict.Contains(tableGeneralSpecificationStyleName))   //необходимо переделать проверку, если нужного шаблона нет, то запустить метод по созданию этого шаблона
                    {
                        MessageBox.Show($"Проверьте шаблон чертежа. Стиль таблицы \"{tableGeneralSpecificationStyleName}\" отсутствует!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else
                    {
                        TableStyle ts = tr.GetObject(dict.GetAt(tableGeneralSpecificationStyleName), OpenMode.ForRead) as TableStyle;
                        string[] cellStyles = ts.CellStyles.Cast<string>().ToArray();
                        string cellStyle = cellStyles.FirstOrDefault(item => item.ToLower().Contains(tableComponentStyleCell.ToLower()));

                        TableTemplate template;
                        try
                        {
                            template = tr.GetObject(ts.Template, OpenMode.ForRead) as TableTemplate;
                        }
                        catch (Exception)
                        {
                            MessageBox.Show($"Проверьте стиль таблицы. нет привязки к таблице", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        // Включим отображение толщин линий,
                        // дабы увидеть результат нашей работы
                        //cad.SetSystemVariable("LWDISPLAY", 1);
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
                        if (tableInstance.Rows.Count > startRowTableGeneralSpecification)
                            tableInstance.DeleteRows(startRowTableGeneralSpecification, tableInstance.Rows.Count - startRowTableGeneralSpecification);

                        tableInstance.InsertRows(startRowTableGeneralSpecification, 8, listComponents.Count);
                        int row = startRowTableGeneralSpecification;
                        string tempManufac = null;
                        foreach (var item in listComponents)
                        {
                            if (tempManufac != item.Manufac && tempManufac != null)
                            {
                                tableInstance.InsertRows(row, 8, 1);
                                row++;
                            }
                            tableInstance.Cells[row, 0].TextString = item.PosItem;
                            tableInstance.Cells[row, 1].TextString = item.FullDescription;
                            tableInstance.Cells[row, 2].TextString = item.AllTag;
                            tableInstance.Cells[row, 3].TextString = item.CatNumber;
                            tableInstance.Cells[row, 4].TextString = item.Manufac;
                            tableInstance.Cells[row, 6].TextString = item.Count.ToString();
                            tableInstance.Cells[row, 8].TextString = item.Note;
                            if (!string.IsNullOrEmpty(cellStyle))
                                tableInstance.Cells[row, 1].Style = cellStyle;
                            row++;
                            tempManufac = item.Manufac;
                        }
                        tr.Commit();
                    }
                }
            }
        }
    }
}