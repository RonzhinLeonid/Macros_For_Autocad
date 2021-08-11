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
        static public Dictionary<string, int> GetDictionaryCableMagazine()
        {
            var component = new Dictionary<string, int>();
            component.Add("TAG", 0);
            component.Add("START", 0);
            component.Add("FINISH", 0);
            component.Add("MARK_CABLE", 0);
            component.Add("CORES_CABLE", 0);
            component.Add("LENGTH", 0);
            component.Add("IN_SPECIFICATION", 0);
            return component;
        }
        static public Dictionary<string, int> GetDictionaryTubeСonnections()
        {
            var component = new Dictionary<string, int>();
            component.Add("TAG", 0);
            component.Add("DESCRIPTION", 0);
            component.Add("CONECTION", 0);
            component.Add("MATERIAL", 0);
            component.Add("IN_SPECIFICATION", 0);
            return component;
        }
    }
}
