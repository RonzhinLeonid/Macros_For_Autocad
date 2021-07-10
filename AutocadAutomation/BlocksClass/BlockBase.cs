using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocadAutomation.BlocksClass
{
    public class BlockBase
    {
        private ObjectId _idBlock;
        private string _tag;
        private bool _inSpecification;

        public ObjectId IdBlock
        {
            get
            {
                return _idBlock;
            }

            set
            {
                _idBlock = value;
            }
        }

        public string Tag
        {
            get
            {
                return _tag;
            }

            set
            {
                _tag = value;
            }
        }

        public bool InSpecification
        {
            get
            {
                return _inSpecification;
            }

            set
            {
                _inSpecification = value;
            }
        }

        public BlockBase(ObjectId idBlock, string tag, string inSpecification)
        {
            _idBlock = idBlock;
            _tag = tag;
            _inSpecification = inSpecification.ToUpper() == "ДА" ? true : false;
        }
    }
}