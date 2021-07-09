using AutocadAutomation.StringTable;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocadAutomation.Data
{
    public class DataTableComponent
    {
        private ObservableCollection<StringTableListComponents> _collect;

        public ObservableCollection<StringTableListComponents> Collect
        {
            get { return _collect; }
            set { _collect = value; }
        }
    }
}