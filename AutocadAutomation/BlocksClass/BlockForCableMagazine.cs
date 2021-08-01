using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutocadAutomation.BlocksClass
{
    public class BlockForCableMagazine : BlockBase
    {
        private string _start;
        private string _finish;
        private string _markCable;
        private string _coresCable;
        private int _length;
        private Point3d _position;

        public string Start
        {
            get
            {
                return _start;
            }

            set
            {
                _start = value;
            }
        }
        public string Finish
        {
            get
            {
                return _finish;
            }

            set
            {
                _finish = value;
            }
        }
        public string MarkCable
        {
            get
            {
                return _markCable;
            }

            set
            {
                _markCable = value;
            }
        }
        public string CoresCable
        {
            get
            {
                return _coresCable;
            }

            set
            {
                _coresCable = value;
            }
        }
        public int Length
        {
            get
            {
                return _length;
            }

            set
            {
                _length = value;
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
        public BlockForCableMagazine(ObjectId idBlock,
                                       string tag,
                                       string start,
                                       string finish,
                                       string markCable,
                                       string coresCable,
                                       string length,
                                       string inSpecification,
                                       Point3d position) : base(idBlock, tag, inSpecification)
        {
            _start = start;
            _finish = finish;
            _markCable = markCable;
            _coresCable = coresCable;
            var rez = int.TryParse(length, out _length);
            if (!rez)
            {
                _length = 0;
                MessageBox.Show($"Длинна кабеля {tag} не является числом!", "Внимание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            _position = position;
        }
    }
}
