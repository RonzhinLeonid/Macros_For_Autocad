using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocadAutomation.StringTable
{
    class StringTableListComponents
    {
        int _posItem;
        string _fullDescription;
        int _count;
        string _note = "";
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
        public string FullDescription
        {
            get
            {
                return _fullDescription;
            }

            set
            {
                _fullDescription = value;
            }
        }
        public int Count
        {
            get
            {
                return _count;
            }

            set
            {
                _count = value;
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
    }
}
