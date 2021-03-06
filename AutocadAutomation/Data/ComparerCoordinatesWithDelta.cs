using AutocadAutomation.BlocksClass;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocadAutomation.Data
{
    class ComparerCoordinatesWithDelta : IComparer<double>
    {
        int _delta;
        public ComparerCoordinatesWithDelta(int delta)
        {
            _delta = delta;
        }

        public int Compare(double x, double y)
        {
            if (x < y - _delta)
                return (-1);
            else if (x > y + _delta)
                return (1);
            else
                return (0);
        }
    }
}
