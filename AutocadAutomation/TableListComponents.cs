using AutocadAutomation.BlocksClass;
using AutocadAutomation.StringTable;
using AutocadAutomation.TypeBlocks;
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
                                                                                        dictAttr["POS_ITEM"],
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
            //List<StringTableListComponents> stringTableListComponents = new List<StringTableListComponents>();
            //var listDescription = _blockForListComponents.Select(p => p.Description).Distinct();

            //как то надо добавить еще note
            //var list2 = _listBlockForListComponents.GroupBy(g => g.Description, p => new {p.Note,  p.Tag });

           // var list = _listBlockForListComponents.GroupBy(g => new { g.Description, g.Tag, g.Note }, p => p.Tag)
            //    .Select((p, ind) => new StringTableListComponents() {PosItem = ind + 1, FullDescription = p.Key.Description + " " + string.Join(", ", p), Count = p.Count(), Note = p.Key.Note} ).ToList();
            //return list;
        }
    }
}
