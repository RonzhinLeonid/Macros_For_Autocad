using AutocadAutomation.BlocksClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
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
        private int _delta = 5;

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        public int Delta
        {
            get { return _delta; }
            set
            {
                _delta = value;
                OnPropertyChanged("Delta");
            }
        }
        public Options Option
        {
            get { return _options; }
            set 
            {
                _options = value;
                OnPropertyChanged("Option");
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
                    SortCollection(Delta);
                },
                (p) => true);
            }
        }

        private void SortCollection(int delta)
        {
            ObservableCollection<BlockForCableMagazine> tempCollect;
            switch (_options)
            {
                case Options.LeftRightUpDown:
                    tempCollect = new ObservableCollection<BlockForCableMagazine>(Collect.OrderByDescending(u => u.Position.Y, new ComparerCoordinatesWithDelta(delta))
                                                                                            .ThenBy(u => u.Position.X, new ComparerCoordinatesWithDelta(delta)));
                    NumerableCollection(tempCollect);
                    break;
                case Options.LeftRightDownUp:
                    tempCollect = new ObservableCollection<BlockForCableMagazine>(Collect.OrderBy(u => u.Position.Y, new ComparerCoordinatesWithDelta(delta))
                                                                                            .ThenBy(u => u.Position.X, new ComparerCoordinatesWithDelta(delta)));
                    NumerableCollection(tempCollect);
                    break;
                case Options.RightLeftUpDown:
                    tempCollect = new ObservableCollection<BlockForCableMagazine>(Collect.OrderByDescending(u => u.Position.Y, new ComparerCoordinatesWithDelta(delta))
                                                                                            .ThenByDescending(u => u.Position.X, new ComparerCoordinatesWithDelta(delta)));
                    NumerableCollection(tempCollect);
                    break;
                case Options.RightLeftDownUp:
                    tempCollect = new ObservableCollection<BlockForCableMagazine>(Collect.OrderBy(u => u.Position.Y, new ComparerCoordinatesWithDelta(delta))
                                                                                            .ThenByDescending(u => u.Position.X, new ComparerCoordinatesWithDelta(delta)));
                    NumerableCollection(tempCollect);
                    break;
                case Options.UpDownLeftRight:
                    tempCollect = new ObservableCollection<BlockForCableMagazine>(Collect.OrderBy(u => u.Position.X, new ComparerCoordinatesWithDelta(delta))
                                                                                            .ThenByDescending(u => u.Position.Y, new ComparerCoordinatesWithDelta(delta)));
                    NumerableCollection(tempCollect);
                    break;
                case Options.UpDownRightLeft:
                    tempCollect = new ObservableCollection<BlockForCableMagazine>(Collect.OrderByDescending(u => u.Position.X, new ComparerCoordinatesWithDelta(delta))
                                                                                            .ThenByDescending(u => u.Position.Y, new ComparerCoordinatesWithDelta(delta)));
                    NumerableCollection(tempCollect);
                    break;
                case Options.DownUpLeftRight:
                    tempCollect = new ObservableCollection<BlockForCableMagazine>(Collect.OrderBy(u => u.Position.X, new ComparerCoordinatesWithDelta(delta))
                                                                                            .ThenBy(u => u.Position.Y, new ComparerCoordinatesWithDelta(delta)));
                    NumerableCollection(tempCollect);
                    break;
                case Options.DownUpRightLeft:
                    tempCollect = new ObservableCollection<BlockForCableMagazine>(Collect.OrderByDescending(u => u.Position.X, new ComparerCoordinatesWithDelta(delta))
                                                                                            .ThenBy(u => u.Position.Y, new ComparerCoordinatesWithDelta(delta)));
                    NumerableCollection(tempCollect);
                    break;
                case Options.NonSortCoordinates:
                    tempCollect = new ObservableCollection<BlockForCableMagazine>(Collect.OrderBy(u => SortCable.PadNumbers(u.Tag)));
                    NumerableCollection(tempCollect);
                    break;
            }
        }

        private void NumerableCollection(ObservableCollection<BlockForCableMagazine> tempCollect)
        {
            int temp = 1;
            Collect.Clear();
            foreach (var item in tempCollect)
            {
                if (item.InSpecification)
                    item.Tag = "W" + temp++;
                Collect.Add(item);
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
