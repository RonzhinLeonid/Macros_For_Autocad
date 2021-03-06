using AutocadAutomation.BlocksClass;
using AutocadAutomation.Data;
using AutocadAutomation.StringTable;
using AutocadAutomation.TypeBlocks;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocadAutomation
{
    class TableCableMagazine
    {
        private List<BlockForCableMagazine> _listBlockForCableMagazine;
        public List<BlockForCableMagazine> ListBlockForCableMagazine => _listBlockForCableMagazine;
        public TableCableMagazine(Database db)
        {
            GetListBlockForCableMagazine(db);
        }

        private void GetListBlockForCableMagazine(Database db)
        {
            _listBlockForCableMagazine = new List<BlockForCableMagazine>();
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
                            var dictionaryElement = WorkWithAttribute.IsMeet(WorkWithAttribute.FillDictionaryAttributes(attrC, Types.GetDictionaryCableMagazine()));
                            if (dictionaryElement)
                            {
                                var dictAttr = WorkWithAttribute.GetDictionaryAttributes(attrC);
                                _listBlockForCableMagazine.Add(new BlockForCableMagazine(id,
                                                                                        dictAttr["TAG"],
                                                                                        dictAttr["START"],
                                                                                        dictAttr["FINISH"],
                                                                                        dictAttr["MARK_CABLE"],
                                                                                        dictAttr["CORES_CABLE"],
                                                                                        dictAttr["LENGTH"],
                                                                                        dictAttr["IN_SPECIFICATION"],
                                                                                        selectedBlock.Position));
                            }
                        }
                    }
                }
            }
            _listBlockForCableMagazine = _listBlockForCableMagazine.OrderBy(u => SortCable.PadNumbers(u.Tag))
                                                                            .ToList();
        }

        public void SyncBlocksAllAttr(Database db, ObservableCollection<BlockForCableMagazine> collection)
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
                            case "START":
                                if (att.TextString != collection[i].Start)
                                    att.TextString = collection[i].Start;
                                break;
                            case "FINISH":
                                if (att.TextString != collection[i].Finish)
                                    att.TextString = collection[i].Finish;
                                break;
                            case "MARK_CABLE":
                                if (att.TextString != collection[i].MarkCable)
                                    att.TextString = collection[i].MarkCable;
                                break;
                            case "CORES_CABLE":
                                if (att.TextString != collection[i].CoresCable)
                                    att.TextString = collection[i].CoresCable;
                                break;
                            case "LENGTH":
                                if (att.TextString != collection[i].Length.ToString())
                                    att.TextString = collection[i].Length.ToString();
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
