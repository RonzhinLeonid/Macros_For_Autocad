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
            var listComponents = tableListComponents.ListStringTableListComponents;
            if (!listComponents.Any())
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
            Tables.CrateTableComponents(adoc, db, listComponents, point);
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
    }
}