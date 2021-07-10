using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocadAutomation.BlocksClass
{
    public class BlockForListComponents : BlockBase
    {
        private int _posItem = 0;
        private string _description;
        private string _note;

        public int PosItem
        {
            get
            {
                return _posItem;
            }

            set
            {
                _posItem = value;
            }
        }

        public string Description
        {
            get
            {
                return _description;
            }

            set
            {
                _description = value;
            }
        }

        public string Note
        {
            get
            {
                return _note;
            }

            set
            {
                _note = value;
            }
        }

        public BlockForListComponents(ObjectId idBlock,
                                        string tag,
                                        string description,
                                        string note,
                                        string inSpecification) : base(idBlock, tag, inSpecification)
        {
            _description = description;
            _note = note;
        }
    }
}