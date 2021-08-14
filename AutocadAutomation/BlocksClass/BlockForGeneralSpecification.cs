using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocadAutomation.BlocksClass
{
    public class BlockForGeneralSpecification : BlockBase
    {
        private string _posItem;
        private string _description;
        private string _parametr1;
        private string _parametr2;
        private string _parametr3;
        private string _parametr4;
        private string _parametr5;
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

        public string Parametr1
        {
            get
            {
                return _parametr1;
            }
            set
            {
                _parametr1 = value;
            }
        }

        public string Parametr2
        {
            get
            {
                return _parametr2;
            }
            set
            {
                _parametr2 = value;
            }
        }

        public string Parametr3
        {
            get
            {
                return _parametr3;
            }
            set
            {
                _parametr3 = value;
            }
        }

        public string Parametr4
        {
            get
            {
                return _parametr4;
            }
            set
            {
                _parametr4 = value;
            }
        }

        public string Parametr5
        {
            get
            {
                return _parametr5;
            }
            set
            {
                _parametr5 = value;
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

        public BlockForGeneralSpecification(ObjectId idBlock,
                                        string tag,
                                        string description,
                                        string parametr1,
                                        string parametr2,
                                        string parametr3,
                                        string parametr4,
                                        string parametr5,
                                        string catNumber,
                                        string manufac,
                                        string note,
                                        string inSpecification) : base(idBlock, tag, inSpecification)
        {
            _description = description;
            _parametr1 = parametr1;
            _parametr2 = parametr2;
            _parametr3 = parametr3;
            _parametr4 = parametr4;
            _parametr5 = parametr5;
            _catNumber = catNumber;
            _manufac = manufac;
            _note = note;
        }
    }
}
