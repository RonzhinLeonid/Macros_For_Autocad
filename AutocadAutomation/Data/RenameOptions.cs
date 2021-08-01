using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocadAutomation.Data
{
    public class RenameOptions
    {
        public enum Options
        {
            LeftRightUpDown,
            LeftRightDownUp,
            RightLeftUpDown,
            RightLeftDownUp,
            UpDownLeftRight,
            UpDownRightLeft,
            DownUpLeftRight,
            DownUpRightLeft,
            NonSortCoordinates
        }
    }
}                   