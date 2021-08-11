using AutocadAutomation.BlocksClass;
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
        private List<BlockGeneralSpecification> _listBlockForGeneralSpecification;
        private List<StringTableGeneralSpecification> _listStringTableGeneralSpecification;
        public List<BlockGeneralSpecification> ListBlockForGeneralSpecification => _listBlockForGeneralSpecification;
        public List<StringTableGeneralSpecification> ListStringTableGeneralSpecification => _listStringTableGeneralSpecification;

        public TableGeneralSpecification(Database db)
        {
            GetListBlockForGeneralSpecification(db);
        }

        private void GetListBlockForGeneralSpecification(Database db)
        {
            _listBlockForGeneralSpecification = new List<BlockGeneralSpecification>();
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
                                var listParams = WorkWithAttribute.GetListParams(attrC);
                                _listBlockForGeneralSpecification.Add(new BlockGeneralSpecification(id,
                                                                                        dictAttr["TAG"],
                                                                                        dictAttr["DESCRIPTION"],
                                                                                        listParams,
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
                                                                     .ThenBy(u => u.Description)
                                                                     .ThenBy(u => u.Tag)
                                                                     .ToList();
        }

        public void GetTableListComponents()
        {
            _listStringTableGeneralSpecification = new List<StringTableGeneralSpecification>();
            string posItem = "";
            string tempDicript = "";
            string tempNote = "";
            foreach (var item in _listBlockForGeneralSpecification)
            {
                if (!item.InSpecification) continue;
                //if (tempDicript != item.Description)
                //{
                //    _listStringTableGeneralSpecification.Add(new StringTableGeneralSpecification()
                //    {
                //        IdBlock = new List<ObjectId>() { item.IdBlock },
                //        PosItem = posItem,
                //        AllTag = item.Tag,
                //        FullDescription = item.Description,
                //        Count = 1,
                //        Note = item.Note
                //    });
                //    posItem++;
                //    tempDicript = item.Description;
                //    tempNote = item.Note;
                //}
                //else
                //{
                //    if (tempNote == item.Note)
                //    {
                //        _listStringTableGeneralSpecification.Last().IdBlock.Add(item.IdBlock);
                //        _listStringTableGeneralSpecification.Last().AllTag = _listStringTableGeneralSpecification.Last().AllTag + ", " + item.Tag;
                //        _listStringTableGeneralSpecification.Last().Count++;
                //    }
                //    else
                //    {
                //        _listStringTableGeneralSpecification.Add(new StringTableListComponents()
                //        {
                //            IdBlock = new List<ObjectId>() { item.IdBlock },
                //            PosItem = posItem,
                //            AllTag = item.Tag,
                //            FullDescription = item.Description,
                //            Count = 1,
                //            Note = item.Note
                //        });
                //        posItem++;
                //        tempDicript = item.Description;
                //        tempNote = item.Note;
                //    }
                //}
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

        public void SyncBlocksAllAttr(Database db, ObservableCollection<BlockGeneralSpecification> collection)
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
                            //case "TAG":
                            //    if (att.TextString != collection[i].Tag)
                            //        att.TextString = collection[i].Tag;
                            //    break;
                            //case "DESCRIPTION":
                            //    if (att.TextString != collection[i].Description)
                            //        att.TextString = collection[i].Description;
                            //    break;
                            //case "NOTE":
                            //    if (att.TextString != collection[i].Note)
                            //        att.TextString = collection[i].Note;
                            //    break;
                            //case "IN_SPECIFICATION":
                            //    if (att.TextString != collection[i].InSpecification.ToString())
                            //        att.TextString = collection[i].InSpecification ? "Да" : "Нет";
                            //    break;
                            //default:
                            //    break;
                        }
                    }
                }
                tr.Commit();
            }
        }
    }
}
