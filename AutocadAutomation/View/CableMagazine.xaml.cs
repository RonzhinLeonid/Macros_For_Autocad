using AutocadAutomation.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AutocadAutomation.View
{
    /// <summary>
    /// Логика взаимодействия для CableMagazine.xaml
    /// </summary>
    public partial class CableMagazine : Window
    {
        private DataTableCableMagazine _data;
        public CableMagazine(DataTableCableMagazine data)
        {
            InitializeComponent();
            _data = data;
            this.DataContext = _data;
        }
    }
}
