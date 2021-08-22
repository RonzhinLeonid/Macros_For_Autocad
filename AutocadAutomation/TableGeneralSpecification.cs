using AutocadAutomation.BlocksClass;
using AutocadAutomation.Data;
using AutocadAutomation.StringTable;
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
    class TableGeneralSpecification
    {
        private List<BlockForGeneralSpecification> _listBlockForGeneralSpecification;
        private List<StringTableGeneralSpecification> _listStringTableGeneralSpecification;
        public List<BlockForGeneralSpecification> ListBlockForGeneralSpecification => _listBlockForGeneralSpecification;
        public List<StringTableGeneralSpecification> ListStringTableGeneralSpecification => _listStringTableGeneralSpecification;

        public TableGeneralSpecification(Database db)
        {
            GetListBlockForGeneralSpecification(db);
        }

        private void GetListBlockForGeneralSpecification(Database db)
        {
            _listBlockForGeneralSpecification = new List<BlockForGeneralSpecification>();
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
                            var dictionaryElement = WorkWithAttribute.IsMeet(WorkWithAttribute.FillDictionaryAttributes(attrC, Types.GetDictionaryGeneralSpecification()));
                            if (dictionaryElement)
                            {
                                var dictAttr = WorkWithAttribute.GetDictionaryAttributes(attrC);
                                _listBlockForGeneralSpecification.Add(new BlockForGeneralSpecification(id,
                                                                                        dictAttr["TAG"],
                                                                                        dictAttr["DESCRIPTION"],
                                                                                        dictAttr["PARAMETR1"],
                                                                                        dictAttr["PARAMETR2"],
                                                                                        dictAttr["PARAMETR3"],
                                                                                        dictAttr["PARAMETR4"],
                                                                                        dictAttr["PARAMETR5"],
                                                                                        dictAttr["CAT_NUMBER"],
                                                                                        dictAttr["MANUFACTURER"],
                                                                                        dictAttr["NOTE"],
                                                                                        dictAttr["IN_SPECIFICATION"]));
                            }
                        }
                    }
                }
            }
            _listBlockForGeneralSpecification = _listBlockForGeneralSpecification.OrderBy(u => u.Manufac)
                                                                     .ThenBy(u => u.CatNumber)
                                                                     .ThenBy(u => SortCable.PadNumbers(u.Tag))
                                                                     .ToList();
        }

        public void GetTableGeneralSpecification()
        {
            _listStringTableGeneralSpecification = _listBlockForGeneralSpecification.Where(item => item.InSpecification)
               .GroupBy(p => new { p.Manufac, p.CatNumber, p.Note })
               .Select(b => new StringTableGeneralSpecification {   IdBlock = b.Select(bn => bn.IdBlock).ToList(),
                                                                    AllTag = String.Join(", ", b.Select(bn => bn.Tag)),
                                                                    FullDescription = String.Join(", ", b.Select(par => new List<string>() {par.Description,
                                                                                                                                            par.Parametr1, 
                                                                                                                                            par.Parametr2, 
                                                                                                                                            par.Parametr3,
                                                                                                                                            par.Parametr4, 
                                                                                                                                            par.Parametr5 })
                                                                                                                            .First())
                                                                                                                            .Trim(' ', ','),
                                                                    CatNumber = b.Select(bn => bn.CatNumber).First(),
                                                                    Count = b.Count(),
                                                                    Manufac = b.Select(bn => bn.Manufac).First(),
                                                                    Note = b.Select(bn => bn.Note).First()
                                                                }).ToList();

            int posItemPref = 0;
            int posItemPostf = 1;
            string tempManufac = null;
            foreach (var item in _listStringTableGeneralSpecification)
            {
                if (tempManufac != item.Manufac)
                {
                    posItemPostf = 1;
                    posItemPref++;
                }
                item.PosItem = $"{posItemPref}.{posItemPostf++}";
                tempManufac = item.Manufac;
            }
        }

        public void SyncBlocksPosItemAttr(Database db)
        {
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                foreach (var stringTable in _listStringTableGeneralSpecification)
                {
                    foreach (var item in stringTable.IdBlock)
                    {
                        BlockReference selectedBlock = tr.GetObject(item, OpenMode.ForWrite) as BlockReference; // получить BlockReference
                        AttributeCollection attrIdCollection = selectedBlock.AttributeCollection;
                        foreach (ObjectId idAttRef in attrIdCollection)
                        {
                            AttributeReference att = tr.GetObject(idAttRef, OpenMode.ForWrite) as AttributeReference;
                            if (att.Tag.ToUpper() == "POS_ITEM")
                            {
                                att.TextString = stringTable.PosItem;
                            }
                        }
                    }
                }
                tr.Commit();
            }
        }

        public void SyncBlocksAllAttr(Database db, ObservableCollection<BlockForGeneralSpecification> collection)
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
                            case "PARAMETR1":
                                if (att.TextString != collection[i].Parametr1)
                                    att.TextString = collection[i].Parametr1;
                                break;
                            case "PARAMETR2":
                                if (att.TextString != collection[i].Parametr2)
                                    att.TextString = collection[i].Parametr2;
                                break;
                            case "PARAMETR3":
                                if (att.TextString != collection[i].Parametr3)
                                    att.TextString = collection[i].Parametr3;
                                break;
                            case "PARAMETR4":
                                if (att.TextString != collection[i].Parametr4)
                                    att.TextString = collection[i].Parametr4;
                                break;
                            case "PARAMETR5":
                                if (att.TextString != collection[i].Parametr5)
                                    att.TextString = collection[i].Parametr5;
                                break;
                            case "CAT_NUMBER":
                                if (att.TextString != collection[i].CatNumber)
                                    att.TextString = collection[i].CatNumber;
                                break;
                            case "MANUFACTURER":
                                if (att.TextString != collection[i].Manufac)
                                    att.TextString = collection[i].Manufac;
                                break;
                            case "NOTE":
                                if (att.TextString != collection[i].Note)
                                    att.TextString = collection[i].Note;
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
