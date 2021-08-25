using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocadAutomation.BlocksClass
{
    public class BlockForElecticSignal : BlockBase
    {
        private string _description;
        private string _terminal;
        private string _cableGland;
        private string _note1;
        private string _note2;
        private Point3d _position;
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

        public string Terminal
        {
            get
            {
                return _terminal;
            }

            set
            {
                _terminal = value;
            }
        }
        public string CableGland
        {
            get
            {
                return _cableGland;
            }

            set
            {
                _cableGland = value;
            }
        }
        public string Note1
        {
            get
            {
                return _note1;
            }

            set
            {
                _note1 = value;
            }
        }
        public string Note2
        {
            get
            {
                return _note2;
            }

            set
            {
                _note2 = value;
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

        public BlockForElecticSignal(ObjectId idBlock,
                                        string tag,
                                        string description,
                                        string terminal,
                                        string cableGland,
                                        string note1,
                                        string note2,
                                        string inSpecification,
                                        Point3d position) : base(idBlock, tag, inSpecification)
        {
            _description = description;
            _terminal = terminal;
            _cableGland = cableGland;
            _terminal = terminal;
            _note1 = note1;
            _note2 = note2;
        }
    }
}
