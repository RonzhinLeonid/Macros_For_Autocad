using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocadAutomation.StringTable
{
    public class StringTableListComponents
    {
        private List<ObjectId> _idBlock;
        private int _posItem;
        private string _allTag;
        private string _fullDescription;
        private int _count;
        private string _note = "";

        public List<ObjectId> IdBlock
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

        public string AllTag
        {
            get
            {
                return _allTag;
            }

            set
            {
                _allTag = value;
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