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

        #region Метод не реализован
        //public static void CrateTableStyleTableComponents(Document doc, Database db)
        //{
        //    using (doc.LockDocument())
        //    {
        //        //                AcadDocument activeDocument = default(AcadDocument);
        //        //                #if !NEWER_THAN_AUTOCAD_2009
        //        //                activeDocument = (Int.AcadDocument)cad.DocumentManager.MdiActiveDocument.AcadDocument;
        //        //                else
        //        //                activeDocument = (Int.AcadDocument)
        //        //                Ap.DocumentExtension.GetAcadDocument(cad.DocumentManager.MdiActiveDocument);
        //        //#endif
        //        using (Transaction tr = db.TransactionManager.StartTransaction())
        //        {
        //            DBDictionary tableStylesDictionary = tr.GetObject(db.TableStyleDictionaryId, OpenMode.ForWrite) as DBDictionary;
        //            TableStyle tableStyle = null;
        //            ObjectId tableStyleId = ObjectId.Null;
        //            if (tableStylesDictionary.Contains(tableComponentStyleName)) return;
        //            else
        //            {
        //                TextStyleTable tst = tr.GetObject(db.TextStyleTableId, OpenMode.ForWrite) as TextStyleTable;
        //                ObjectId textStyleId = ObjectId.Null;
        //                if (tst.Has(textStyleName))
        //                    textStyleId = tst[textStyleName];
        //                else
        //                {
        //                    MessageBox.Show($"Проверьте шаблон чертежа, нет Необходимого текстового стиля!", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //                    return;
        //                }

        //                tableStyle = new TableStyle();
        //                tableStyleId = tableStylesDictionary.SetAt(tableComponentStyleName, tableStyle);
        //                tr.AddNewlyCreatedDBObject(tableStyle, true);
        //                tableStyle.Description = "Этот табличный стиль создан программно.";
        //            }
        //            tableStyle = tr.GetObject(tableStyleId, OpenMode.ForWrite) as TableStyle;

        //            Table table = new Table();
        //            table.SetDatabaseDefaults();
        //            table.TableStyle = tableStyleId;
        //            table.Position = new Point3d(0, 0, 0);
        //            const int columnsCount = 5;
        //            const int rowsCount = 3;
        //            const int rowHeight = 8;
        //            int[] columnsWidth = new int[columnsCount] { 15, 20, 110, 10, 30 };

        //            table.InsertRows(0, rowHeight, rowsCount);
        //            table.InsertColumns(0, 15, columnsCount);

        //            String[,] str = new String[columnsCount, rowsCount];
        //            for (int i = 0; i < columnsCount; i++)
        //            {
        //                for (int j = 0; j < rowsCount; j++)
        //                {
        //                    str[i, j] = String.Empty;
        //                }
        //            }
        //            str[0, 0] = "Таблица 3 - Перечень компонентов";
        //            str[0, 1] = "№";
        //            str[1, 1] = "Поз.обозначения";
        //            str[2, 1] = "Наименование";
        //            str[3, 1] = "Кол.";
        //            str[4, 1] = "Примечание";

        //            //for (int i = 0; i < columnsCount; i++)
        //            //{
        //            //    for (int j = 0; j < rowsCount; j++)
        //            //    {
        //            //        //table.SetTextHeight(i, j, 5);
        //            //        //table.SetTextString(i, j, str[i, j]);
        //            //        //table.SetAlignment(i, j, CellAlignment.MiddleCenter);
        //            //        // Назначаем значение конкретной
        //            //        // ячейке
        //            //        table.Cells[i, j].SetValue(str[i, j], ParseOption.ParseOptionNone);
        //            //        table.Cells[i, j].Alignment = CellAlignment.MiddleCenter;
        //            //    }
        //            //}
        //            for (int i = 0; i < rowsCount; i++)
        //            {
        //                table.Rows[i].Alignment = CellAlignment.MiddleCenter;
        //            }
        //            for (int i = 0; i < columnsWidth.Length; i++)
        //            {
        //                table.Columns[i].Width = columnsWidth[i];
        //            }
        //            CellRange rngSection = CellRange.Create(table, 0, 0, 1, 4);
        //            table.MergeCells(rngSection);

        //            TableStyle tableStyle2 = new TableStyle();
        //            tableStyle2.CopyFrom(tableStyle);

        //            ObjectId tableStyle2Id = tableStylesDictionary.SetAt(tableComponentStyleName, tableStyle2);
        //            tr.AddNewlyCreatedDBObject(tableStyle2, true);

        //            TableTemplate template = new TableTemplate(table, TableCopyOptions.TableCopyColumnWidth | TableCopyOptions.TableCopyRowHeight | TableCopyOptions.ConvertFormatToOverrides);
        //            db.AddDBObject(template);
        //            tr.AddNewlyCreatedDBObject(template, true);
        //            tableStyle2.Template = template.ObjectId;

        //            tr.Commit();
        //        }
        //    }
        //} 
        #endregion
    }
}