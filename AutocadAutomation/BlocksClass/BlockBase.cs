using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocadAutomation.BlocksClass
{
    class BlockBase
    {
        ObjectId _objectId;
        string _tag;
        bool _inSpecification;

        public ObjectId ObjectId
        {
            get
            {
                return _objectId;
            }

            set
            {
                _objectId = value;
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
        public BlockBase(ObjectId objectId, string tag, string inSpecification)
        {
            _objectId = objectId;
            _tag = tag;
            _inSpecification = inSpecification.ToUpper() == "ДА" ? true :false;
        }
    }
}
