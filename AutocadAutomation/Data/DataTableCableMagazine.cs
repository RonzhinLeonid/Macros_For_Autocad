using AutocadAutomation.BlocksClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using static AutocadAutomation.Data.RenameOptions;

namespace AutocadAutomation.Data
{
    public class DataTableCableMagazine
    {
        private ObservableCollection<BlockForCableMagazine> _collect;
        private Options _options = Options.NonSortCoordinates;

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        public ObservableCollection<BlockForCableMagazine> Collect
        {
            get { return _collect; }
            set 
            { 
                _collect = value;
                OnPropertyChanged("Collect");
            }
        }

        public ICommand CheckedCommand
        {
            get
            {
                return new DelegateCommand((p) => Enum.TryParse(p.ToString(), out Options _options),
                (p) => true);
            }
        }

        public ICommand RenameCableCommand
        {
            get
            {
                return new DelegateCommand((p) =>
                {
                    switch (_options)
                    {
                        case Options.LeftRightUpDown:
                            break;
                        case Options.LeftRightDownUp:
                            break;
                        case Options.RightLeftUpDown:
                            break;
                        case Options.RightLeftDownUp:
                            break;
                        case Options.UpDownLeftRight:
                            break;
                        case Options.UpDownRightLeft:
                            break;
                        case Options.DownUpLeftRight:
                            break;
                        case Options.DownUpRightLeft:
                            break;
                        case Options.NonSortCoordinates:
                            //Collect = Collect.OrderBy(u => SortCable.PadNumbers(u.Tag)).ToObservableCollection();
                            break;
                    }
                },
                (p) => true);
            }
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
