using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocadAutomation.BlocksClass
{
    class BlockForListComponents : BlockBase
    {
        int _posItem = 0;
        string _description;
        string _note;
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
