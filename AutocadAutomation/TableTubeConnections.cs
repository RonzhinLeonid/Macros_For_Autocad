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
    class TableTubeConnections
    {
        private List<BlockForTubeСonnections> _listBlockForTubeСonnections;
        public List<BlockForTubeСonnections> ListBlockForTubeСonnections => _listBlockForTubeСonnections;
        public TableTubeConnections(Database db)
        {
            GetListBlockForTubeConnections(db);
        }

        private void GetListBlockForTubeConnections(Database db)
        {
            _listBlockForTubeСonnections = new List<BlockForTubeСonnections>();
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
                            var dictionaryElement = WorkWithAttribute.IsMeet(WorkWithAttribute.FillDictionaryAttributes(attrC, Types.GetDictionaryTubeСonnections()));
                            if (dictionaryElement)
                            {
                                var dictAttr = WorkWithAttribute.GetDictionaryAttributes(attrC);
                                _listBlockForTubeСonnections.Add(new BlockForTubeСonnections(id,
                                                                                        dictAttr["TAG"],
                                                                                        dictAttr["DESCRIPTION"],
                                                                                        dictAttr["CONECTION"],
                                                                                        dictAttr["MATERIAL"],
                                                                                        dictAttr["IN_SPECIFICATION"],
                                                                                        selectedBlock.Position));
                            }
                        }
                    }
                }
            }
            _listBlockForTubeСonnections = _listBlockForTubeСonnections.OrderBy(u => SortCable.PadNumbers(u.Tag))
                                                                            .ToList();
        }

        public void SyncBlocksAllAttr(Database db, ObservableCollection<BlockForTubeСonnections> collection)
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
                            case "CONECTION":
                                if (att.TextString != collection[i].Conection)
                                    att.TextString = collection[i].Conection;
                                break;
                            case "MATERIAL":
                                if (att.TextString != collection[i].Material)
                                    att.TextString = collection[i].Material;
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
