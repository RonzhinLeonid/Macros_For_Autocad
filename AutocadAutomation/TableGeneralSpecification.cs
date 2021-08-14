﻿using AutocadAutomation.BlocksClass;
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
                                                                     .ThenBy(u => u.Description)
                                                                     .ThenBy(u => u.Tag)
                                                                     .ToList();
        }

        public void GetTableGeneralSpecification()
        {
            _listStringTableGeneralSpecification = new List<StringTableGeneralSpecification>();
            //string posItem = "";
            int posItemPref = 1;
            string tempDicript = "";
            string tempNote = "";

            var ListManuf = _listBlockForGeneralSpecification.GroupBy(p => p.Manufac);
            foreach (var manuf in ListManuf)
            {
                int posItemPostf = 1;
                foreach (var item in manuf)
                {
                    if (!item.InSpecification) continue;

                    string fullDicript = GetFullDesc(item);

                    if (tempDicript != item.Description)
                    {
                        _listStringTableGeneralSpecification.Add(new StringTableGeneralSpecification()
                        {
                            IdBlock = new List<ObjectId>() { item.IdBlock },
                            PosItem = $"{posItemPref}.{posItemPostf}",
                            AllTag = item.Tag,
                            FullDescription = fullDicript,
                            Manufac = item.Manufac,
                            CatNumber = item.CatNumber,
                            Count = 1,
                            Note = item.Note
                        });
                        posItemPostf++;
                        tempNote = item.Note;
                    }
                    else
                    {
                        if (tempNote == item.Note)
                        {
                            _listStringTableGeneralSpecification.Last().IdBlock.Add(item.IdBlock);
                            _listStringTableGeneralSpecification.Last().AllTag = _listStringTableGeneralSpecification.Last().AllTag + ", " + item.Tag;
                            _listStringTableGeneralSpecification.Last().Count++;
                        }
                        else
                        {
                            _listStringTableGeneralSpecification.Add(new StringTableGeneralSpecification()
                            {
                                IdBlock = new List<ObjectId>() { item.IdBlock },
                                PosItem = $"{posItemPref}.{posItemPostf}",
                                AllTag = item.Tag,
                                FullDescription = item.Description,
                                Manufac = item.Manufac,
                                CatNumber = item.CatNumber,
                                Count = 1,
                                Note = item.Note
                            });
                            posItemPostf++;
                            tempDicript = item.Description;
                            tempNote = item.Note;
                        }
                    }
                }
                posItemPref++;
            }
        }

        private string GetFullDesc(BlockForGeneralSpecification item)
        {
            return $"{item.Description} {item.Parametr1} {item.Parametr2} {item.Parametr3} {item.Parametr4} {item.Parametr5}".Trim(' ');
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
