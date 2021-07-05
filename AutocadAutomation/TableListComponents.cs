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
        List<BlockForListComponents> _blockForListComponents;
        public List<BlockForListComponents> BlockForListComponents => _blockForListComponents;

        public TableListComponents(Database db)
        {
            _blockForListComponents = HetListBlockForTableComponents(db);
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
                                        .ThenBy(u => u.Tag)
                                        .ThenBy(u => u.Note)
                                        .ToList();
        }
        public List<StringTableListComponents> GetTableListComponents()
        {
            //List<StringTableListComponents> stringTableListComponents = new List<StringTableListComponents>();
            //var listDescription = _blockForListComponents.Select(p => p.Description).Distinct();

            //как то надо добавить еще note
            var list2 = _blockForListComponents.GroupBy(g => g.Description, p => new {p.Note,  p.Tag });

            var list = _blockForListComponents.GroupBy(g => new { g.Description, g.Tag, g.Note }, p => p.Tag)
                .Select((p, ind) => new StringTableListComponents() {PosItem = ind + 1, FullDescription = p.Key.Description + " " + string.Join(", ", p), Count = p.Count(), Note = p.Key.Note} ).ToList();
            return list;
        }
    }
}
