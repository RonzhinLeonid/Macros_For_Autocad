using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.Geometry;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using AutocadAutomation.StringTable;
using AutocadAutomation.Data;
using AutocadAutomation.BlocksClass;
using AutocadAutomation.View;
using System.ComponentModel;

namespace AutocadAutomation
{
    public class Specifications
    {
        private Document adoc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
        private Database db;
        private Editor ed;

        #region Таблица компонентов
        [CommandMethod("CreateTableComponents")]
        public void CreateTableComponents_Method()
        {
            db = adoc.Database;
            ed = adoc.Editor;

            var tableListComponents = new TableListComponents(db);
            tableListComponents.GetTableListComponents();
            var listStringTable = tableListComponents.ListStringTableListComponents;
            if (!listStringTable.Any())
            {
                MessageBox.Show($"Компонентов для создания таблицы не обнаружено!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            tableListComponents.SyncBlocksPosItemAttr(db);

            Point3d point;
            var po = new PromptPointOptions("\nУкажите точку вставки таблицы.") { AllowNone = false };
            var r = ed.GetPoint(po);
            if (r.Status == PromptStatus.OK)
                point = r.Value;
            else if (r.Status == PromptStatus.Cancel)
            {
                return;
            }
            else
                return;
            Tables.CrateTableComponents(adoc, db, listStringTable, point);
        }

        [CommandMethod("EditListComponents")]
        public void EditListComponents_Method()
        {
            db = adoc.Database;
            ed = adoc.Editor;

            var tableListComponents = new TableListComponents(db);
            if (!tableListComponents.ListBlockForListComponents.Any())
            {
                MessageBox.Show($"Компонентов для редактирования не обнаружено!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DataTableComponent data = new DataTableComponent() { Collect = new ObservableCollection<BlockForListComponents>(tableListComponents.ListBlockForListComponents) };
            ListComponents window = new ListComponents(data);
            if (Autodesk.AutoCAD.ApplicationServices.Application.ShowModalWindow(window) == true)
                tableListComponents.SyncBlocksAllAttr(db, data.Collect);
        }
        #endregion

        #region Кабельный журнал
        [CommandMethod("CreateTableCableMagazine")]
        public void CreateTableCableMagazine_Method()
        {
            db = adoc.Database;
            ed = adoc.Editor;

            var tableCableMagazine = new TableCableMagazine(db);

            var listCables = tableCableMagazine.ListBlockForCableMagazine;
            if (!listCables.Any())
            {
                MessageBox.Show($"Компонентов для создания таблицы не обнаружено!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Point3d point;
            var po = new PromptPointOptions("\nУкажите точку вставки таблицы.") { AllowNone = false };
            var r = ed.GetPoint(po);
            if (r.Status == PromptStatus.OK)
                point = r.Value;
            else if (r.Status == PromptStatus.Cancel)
            {
                return;
            }
            else
                return;
            Tables.CrateTableCableMagazine(adoc, db, listCables, point);
        }

        [CommandMethod("EditListCableMagazine")]
        public void EditListCableMagazine_Method()
        {
            db = adoc.Database;
            ed = adoc.Editor;

            var tableCableMagazine = new TableCableMagazine(db);
            if (!tableCableMagazine.ListBlockForCableMagazine.Any())
            {
                MessageBox.Show($"Кабелей для редактирования не обнаружено!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DataTableCableMagazine data = new DataTableCableMagazine() { Collect = new ObservableCollection<BlockForCableMagazine>(tableCableMagazine.ListBlockForCableMagazine) };
            CableMagazine window = new CableMagazine(data);
            if (Autodesk.AutoCAD.ApplicationServices.Application.ShowModalWindow(window) == true)
                tableCableMagazine.SyncBlocksAllAttr(db, data.Collect);
        }
        #endregion

        #region Трубные соединения
        [CommandMethod("CreateTableTubeСonnections")]
        public void CreateTableTubeСonnections_Method()
        {
            db = adoc.Database;
            ed = adoc.Editor;

            var tableTubeСonnections = new TableTubeConnections(db);

            var listTubeСonnections = tableTubeСonnections.ListBlockForTubeСonnections;
            if (!listTubeСonnections.Any())
            {
                MessageBox.Show($"Компонентов для создания таблицы не обнаружено!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Point3d point;
            var po = new PromptPointOptions("\nУкажите точку вставки таблицы.") { AllowNone = false };
            var r = ed.GetPoint(po);
            if (r.Status == PromptStatus.OK)
                point = r.Value;
            else if (r.Status == PromptStatus.Cancel)
            {
                return;
            }
            else
                return;
            Tables.CrateTableTubeСonnections(adoc, db, listTubeСonnections, point);
        }

        [CommandMethod("EditListTubeСonnections")]
        public void EditListTubeСonnections_Method()
        {
            db = adoc.Database;
            ed = adoc.Editor;

            var tableTubeСonnections = new TableTubeConnections(db);
            if (!tableTubeСonnections.ListBlockForTubeСonnections.Any())
            {
                MessageBox.Show($"Компонентов для редактирования не обнаружено!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DataTableTubeConnections data = new DataTableTubeConnections() { Collect = new ObservableCollection<BlockForTubeСonnections>(tableTubeСonnections.ListBlockForTubeСonnections) };
            TubeConnections window = new TubeConnections(data);
            if (Autodesk.AutoCAD.ApplicationServices.Application.ShowModalWindow(window) == true)
                tableTubeСonnections.SyncBlocksAllAttr(db, data.Collect);
        }
        #endregion

        #region Общая спецификация
        [CommandMethod("CreateTableGeneralSpecification")]
        public void CreateTableGeneralSpecification_Method()
        {
            db = adoc.Database;
            ed = adoc.Editor;

            var tableGeneralSpecification = new TableGeneralSpecification(db);
            tableGeneralSpecification.GetTableGeneralSpecification();

            var listStringTable = tableGeneralSpecification.ListStringTableGeneralSpecification;

            if (!listStringTable.Any())
            {
                MessageBox.Show($"Компонентов для создания таблицы не обнаружено!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            tableGeneralSpecification.SyncBlocksPosItemAttr(db);

            Point3d point;
            var po = new PromptPointOptions("\nУкажите точку вставки таблицы.") { AllowNone = false };
            var r = ed.GetPoint(po);
            if (r.Status == PromptStatus.OK)
                point = r.Value;
            else if (r.Status == PromptStatus.Cancel)
            {
                return;
            }
            else
                return;
            Tables.CrateTableGeneralSpecification(adoc, db, listStringTable, point);
        }

        [CommandMethod("EditListGeneralSpecification")]
        public void EditListGeneralSpecification_Method()
        {
            db = adoc.Database;
            ed = adoc.Editor;

            var tableGeneralSpecification = new TableGeneralSpecification(db);
            if (!tableGeneralSpecification.ListBlockForGeneralSpecification.Any())
            {
                MessageBox.Show($"Блоков для редактирования не обнаружено!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            DataTableGeneralSpecification data = new DataTableGeneralSpecification() { Collect = new ObservableCollection<BlockForGeneralSpecification>(tableGeneralSpecification.ListBlockForGeneralSpecification) };
            GeneralSpecification window = new GeneralSpecification(data);
            if (Autodesk.AutoCAD.ApplicationServices.Application.ShowModalWindow(window) == true)
                tableGeneralSpecification.SyncBlocksAllAttr(db, data.Collect);
        }
        #endregion

        #region Электрические сигналы
        [CommandMethod("CreateTableElectricSignal")]
        public void CreateTableElectricSignal_Method()
        {
            db = adoc.Database;
            ed = adoc.Editor;

            var tableTubeСonnections = new TableElectricSignal(db);

            var listCables = tableTubeСonnections.ListBlockForElectricSignal;
            if (!listCables.Any())
            {
                MessageBox.Show($"Компонентов для создания таблицы не обнаружено!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Point3d point;
            var po = new PromptPointOptions("\nУкажите точку вставки таблицы.") { AllowNone = false };
            var r = ed.GetPoint(po);
            if (r.Status == PromptStatus.OK)
                point = r.Value;
            else if (r.Status == PromptStatus.Cancel)
            {
                return;
            }
            else
                return;
            Tables.CrateTableElecticSignal(adoc, db, listCables, point);
        }

        [CommandMethod("EditListElectricSignal")]
        public void EditListElectricSignal_Method()
        {
            db = adoc.Database;
            ed = adoc.Editor;

            var tableElectricSignal = new TableElectricSignal(db);
            if (!tableElectricSignal.ListBlockForElectricSignal.Any())
            {
                MessageBox.Show($"Компонентов для редактирования не обнаружено!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            var data = new DataTableElecticSignal() { Collect = new ObservableCollection<BlockForElecticSignal>(tableElectricSignal.ListBlockForElectricSignal) };
            var window = new ElectricSignal(data);
            if (Autodesk.AutoCAD.ApplicationServices.Application.ShowModalWindow(window) == true)
                tableElectricSignal.SyncBlocksAllAttr(db, data.Collect);
        }
        #endregion
    }
}