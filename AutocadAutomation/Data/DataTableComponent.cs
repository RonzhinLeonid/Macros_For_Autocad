using AutocadAutomation.BlocksClass;
using AutocadAutomation.StringTable;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows;

namespace AutocadAutomation.Data
{
    public class DataTableComponent
    {
        private ObservableCollection<BlockForListComponents> _collect;

        public ObservableCollection<BlockForListComponents> Collect
        {
            get { return _collect; }
            set { _collect = value; }
        }

        public ICommand OKCommand
        {
            get
            {
                return new DelegateCommand((p) =>
                {
                    var temp = p as Window;
                    temp.DialogResult = true;
                    temp.Close();
                }, (p) => true);
            }
        }

        public ICommand CanselCommand
        {
            get
            {
                return new DelegateCommand((p) =>
                {
                    var temp = p as Window;
                    temp.DialogResult = false;
                    temp.Close();
                }, (p) => true);
            }
        }
    }
}