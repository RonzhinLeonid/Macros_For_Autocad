using AutocadAutomation.BlocksClass;
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
    internal class TableListComponents
    {
        private List<BlockForListComponents> _listBlockForListComponents;
        private List<StringTableListComponents> _listStringTableListComponents;
        public List<BlockForListComponents> ListBlockForListComponents => _listBlockForListComponents;
        public List<StringTableListComponents> ListStringTableListComponents => _listStringTableListComponents;

        public TableListComponents(Database db)
        {
            _listBlockForListComponents = HetListBlockForTableComponents(db);
        }

        private List<BlockForListComponents> HetListBlockForTableComponents(Database db)
        {
            List<BlockForListComponents> blockForListComponents = new List<BlockForListComponents>();
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
                            var dictionaryElement = WorkWithAttribute.IsMeet(WorkWithAttribute.FillDictionaryAttributes(attrC, Types.GetDictionaryComponent()));
                            if (dictionaryElement)
                            {
                                var dictAttr = WorkWithAttribute.GetDictionaryAttributes(attrC);
                                blockForListComponents.Add(new BlockForListComponents(id,
                                                                                        dictAttr["TAG"],
                                                                                        dictAttr["DESCRIPTION"],
                                                                                        dictAttr["NOTE"],
                                                                                        dictAttr["IN_SPECIFICATION"]));
                            }
                        }
                    }
                }
            }
            return blockForListComponents.OrderBy(u => u.Description)
                                         .ThenBy(u => u.Note)
                                         .ThenBy(u => u.Tag)
                                         .ToList();
        }

        public void GetTableListComponents()
        {
            _listStringTableListComponents = new List<StringTableListComponents>();
            int posItem = 1;
            string tempDicript = "";
            string tempNote = "";
            foreach (var item in _listBlockForListComponents)
            {
                if (!item.InSpecification) continue;
                if (tempDicript != item.Description)
                {
                    _listStringTableListComponents.Add(new StringTableListComponents()
                    {
                        IdBlock = new List<ObjectId>() { item.IdBlock },
                        PosItem = posItem,
                        AllTag = item.Tag,
                        FullDescription = item.Description,
                        Count = 1,
                        Note = item.Note
                    });
                    posItem++;
                    tempDicript = item.Description;
                    tempNote = item.Note;
                }
                else
                {
                    if (tempNote == item.Note)
                    {
                        _listStringTableListComponents.Last().IdBlock.Add(item.IdBlock);
                        _listStringTableListComponents.Last().AllTag = _listStringTableListComponents.Last().AllTag + ", " + item.Tag;
                        _listStringTableListComponents.Last().Count++;
                    }
                    else
                    {
                        _listStringTableListComponents.Add(new StringTableListComponents()
                        {
                            IdBlock = new List<ObjectId>() { item.IdBlock },
                            PosItem = posItem,
                            AllTag = item.Tag,
                            FullDescription = item.Description,
                            Count = 1,
                            Note = item.Note
                        });
                        posItem++;
                        tempDicript = item.Description;
                        tempNote = item.Note;
                    }
                }
            }
        }

        public void SyncBlocksPosItemAttr(Database db)
        {
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                foreach (var stringTable in _listStringTableListComponents)
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
                                att.TextString = stringTable.PosItem.ToString();
                            }
                        }
                    }
                }
                tr.Commit();
            }
        }

        public void SyncBlocksAllAttr(Database db, ObservableCollection<BlockForListComponents> collection)
        {
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                for (int i = 0; i < _listBlockForListComponents.Count; i++)
                {
                    BlockReference selectedBlock = tr.GetObject(_listBlockForListComponents[i].IdBlock, OpenMode.ForWrite) as BlockReference; // получить BlockReference
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
            //component.Add("TAG", 0);
            //component.Add("POS_ITEM", 0);
            //component.Add("DESCRIPTION", 0);
            //component.Add("NOTE", 0);
            //component.Add("IN_SPECIFICATION", 0);
        }
    }
}