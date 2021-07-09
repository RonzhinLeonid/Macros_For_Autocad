using AutocadAutomation.Data;
using AutocadAutomation.StringTable;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AutocadAutomation.View
{
    /// <summary>
    /// Логика взаимодействия для ListComponents.xaml
    /// </summary>
    public partial class ListComponents : Window
    {
        private DataTableComponent _collection;

        public ListComponents(DataTableComponent collection)
        {
            InitializeComponent();
            _collection = collection;
            this.DataContext = _collection;
        }
    }
}