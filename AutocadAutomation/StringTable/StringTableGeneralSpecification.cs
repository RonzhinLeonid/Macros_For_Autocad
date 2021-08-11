using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocadAutomation.StringTable
{
    class StringTableGeneralSpecification : BaseStringTable
    {
        private string _posItem;
        private string _catNumber;
        private string _manufac;

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
    }
}
