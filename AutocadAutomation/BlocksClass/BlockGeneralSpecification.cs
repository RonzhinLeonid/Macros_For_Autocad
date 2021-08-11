using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocadAutomation.BlocksClass
{
    public class BlockGeneralSpecification : BlockBase
    {
        private string _posItem;
        private string _description;
        private List<string> _parametrs;
        private string _catNumber;
        private string _manufac;
        private string _note;
        private Point3d _position;
        public string PosItem
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
        public List<string> Parametrs
        {
            get
            {
                return _parametrs;
            }
            set
            {
                _parametrs = value;
            }
        }

        public string CatNumber
        {
            get
            {
                return _catNumber;
            }
            set
            {
                _catNumber = value;
            }
        }

        public string Manufac
        {
            get
            {
                return _manufac;
            }
            set
            {
                _manufac = value;
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

        public Point3d Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
            }
        }

        public BlockGeneralSpecification(ObjectId idBlock,
                                        string tag,
                                        string description,
                                        List<string> parametrs,
                                        string catNumber,
                                        string manufac,
                                        string note,
                                        string inSpecification) : base(idBlock, tag, inSpecification)
        {
            _description = description;
            _parametrs = parametrs;
            _catNumber = catNumber;
            _manufac = manufac;
            _note = note;
        }
    }
}
