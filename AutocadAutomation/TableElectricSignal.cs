using AutocadAutomation.BlocksClass;
using AutocadAutomation.Data;
using AutocadAutomation.TypeBlocks;
using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocadAutomation
{
    class TableElectricSignal
    {
        private List<BlockForElecticSignal> _listBlockForElectricSignal;
        public List<BlockForElecticSignal> ListBlockForElectricSignal => _listBlockForElectricSignal;
        public TableElectricSignal(Database db)
        {
            GetListBlockForTubeConnections(db);
        }

        private void GetListBlockForTubeConnections(Database db)
        {
            _listBlockForElectricSignal = new List<BlockForElecticSignal>();
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                BlockTableRecord btr = (BlockTableRecord)tr.GetObject(SymbolUtilityServices.GetBlockModelSpaceId(db), OpenMode.ForRead);
                foreach (var id in btr)
                {
                    BlockReference selectedBlock = id.GetObject(OpenMode.ForRead) as BlockReference;
                    if (selectedBlock != null)
                    {
                        AttributeCollection attrC = selectedBlock.AttributeCollection;
                        if (attrC.Count > 0)
                        {
                            var dictionaryElement = WorkWithAttribute.IsMeet(WorkWithAttribute.FillDictionaryAttributes(attrC, Types.GetDictionaryElectricSignal()));
                            if (dictionaryElement)
                            {
                                var dictAttr = WorkWithAttribute.GetDictionaryAttributes(attrC);
                                _listBlockForElectricSignal.Add(new BlockForElecticSignal(id,
                                                                                        dictAttr["TAG"],
                                                                                        dictAttr["DESCRIPTION"],
                                                                                        dictAttr["TERMINAL"],
                                                                                        dictAttr["CABLE_GLAND"],
                                                                                        dictAttr["NOTE1"],
                                                                                        dictAttr["NOTE2"],
                                                                                        dictAttr["IN_SPECIFICATION"],
                                                                                        selectedBlock.Position));
                            }
                        }
                    }
                }
            }
            _listBlockForElectricSignal = _listBlockForElectricSignal.OrderBy(u => SortCable.PadNumbers(u.Tag))
                                                                            .ToList();
        }

        public void SyncBlocksAllAttr(Database db, ObservableCollection<BlockForElecticSignal> collection)
        {
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                for (int i = 0; i < collection.Count; i++)
                {
                    BlockReference selectedBlock = tr.GetObject(collection[i].IdBlock, OpenMode.ForWrite) as BlockReference; // получить BlockReference
                    AttributeCollection attrIdCollection = selectedBlock.AttributeCollection;
                    foreach (ObjectId idAttRef in attrIdCollection)
                    {
                        AttributeReference att = tr.GetObject(idAttRef, OpenMode.ForWrite) as AttributeReference;
                        switch (att.Tag.ToUpper())
                        {
                            case "TAG":
                                if (att.TextString != collection[i].Tag)
                                    att.TextString = collection[i].Tag;
                                break;
                            case "DESCRIPTION":
                                if (att.TextString != collection[i].Description)
                                    att.TextString = collection[i].Description;
                                break;
                            case "TERMINAL":
                                if (att.TextString != collection[i].Terminal)
                                    att.TextString = collection[i].Terminal;
                                break;
                            case "CABLE_GLAND":
                                if (att.TextString != collection[i].CableGland)
                                    att.TextString = collection[i].CableGland;
                                break;
                            case "NOTE1":
                                if (att.TextString != collection[i].Note1)
                                    att.TextString = collection[i].Note1;
                                break;
                            case "NOTE2":
                                if (att.TextString != collection[i].Note2)
                                    att.TextString = collection[i].Note2;
                                break;
                            case "IN_SPECIFICATION":
                                if (att.TextString != collection[i].InSpecification.ToString())
                                    att.TextString = collection[i].InSpecification ? "Да" : "Нет";
                                break;
                            default:
                                break;
                        }
                    }
                }
                tr.Commit();
            }
        }
    }
}
