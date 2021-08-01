using AutocadAutomation.BlocksClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AutocadAutomation.Data
{
    public class DataTableCableMagazine
    {
        private ObservableCollection<BlockForCableMagazine> _collect;

        public ObservableCollection<BlockForCableMagazine> Collect
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
