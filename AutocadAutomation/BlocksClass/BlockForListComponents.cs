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
        int _posItem;
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
                                        string posItem, 
                                        string description, 
                                        string note, 
                                        string inSpecification) : base(idBlock, tag, inSpecification)
        {
            _posItem = Convert.ToInt32(posItem);
            _description = description;
            _note = note;
        }
    }
}
