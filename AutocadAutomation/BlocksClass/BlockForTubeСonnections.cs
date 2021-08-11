using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocadAutomation.BlocksClass
{
    public class BlockForTubeСonnections : BlockBase
    {
        private string _description;
        private string _conection;
        private string _material;
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

        public string Conection
        {
            get
            {
                return _conection;
            }

            set
            {
                _conection = value;
            }
        }
        public string Material
        {
            get
            {
                return _material;
            }

            set
            {
                _material = value;
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

        public BlockForTubeСonnections(ObjectId idBlock,
                                        string tag,
                                        string description,
                                        string conection,
                                        string material,
                                        string inSpecification,
                                        Point3d position) : base(idBlock, tag, inSpecification)
        {
            _description = description;
            _conection = conection;
            _material = material;
            _position = position;
        }
    }
}
