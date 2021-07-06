using AutocadAutomation.BlocksClass;
using AutocadAutomation.StringTable;
using AutocadAutomation.TypeBlocks;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocadAutomation
{
    class TableListComponents
    {
        List<BlockForListComponents> _listBlockForListComponents;
        List<StringTableListComponents> _listStringTableListComponents;
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
                                blockForListComponents.Add(new BlockForListComponents(  id,
                                                                                        dictAttr["TAG"],
                                                                                        dictAttr["DESCRIPTION"],
                                                                                        dictAttr["NOTE"],
                                                                                        dictAttr["IN_SPECIFICATION"]));
                            }
                        }
                    }
                }
            }
            return blockForListComponents
                                        .OrderBy(u => u.Description)
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
                        FullDescription = item.Description + " " + item.Tag,
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
                        _listStringTableListComponents.Last().FullDescription = _listStringTableListComponents.Last().FullDescription + ", " + item.Tag;
                        _listStringTableListComponents.Last().Count++;
                    }
                    else
                    {
                        _listStringTableListComponents.Add(new StringTableListComponents()
                        {
                            IdBlock = new List<ObjectId>() { item.IdBlock },
                            PosItem = posItem,
                            FullDescription = item.Description + " " + item.Tag,
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
        public void SyncBlocksDrawing(Database db)
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
    }
}
