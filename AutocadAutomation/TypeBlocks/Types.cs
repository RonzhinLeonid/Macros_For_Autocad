using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocadAutomation.TypeBlocks
{
    static class Types
    {
        static public Dictionary<string, int> GetDictionaryComponent()
        {
            var component = new Dictionary<string, int>();
            component.Add("TAG", 0);
            component.Add("POS_ITEM", 0);
            component.Add("DESCRIPTION", 0);
            component.Add("NOTE", 0);
            component.Add("IN_SPECIFICATION", 0);
            return component;
        }
    }
}
