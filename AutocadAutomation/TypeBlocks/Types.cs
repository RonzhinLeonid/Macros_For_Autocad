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
        static public Dictionary<string, int> GetDictionaryGeneralSpecification()
        {
            var component = new Dictionary<string, int>();
            component.Add("TAG", 0);
            component.Add("POS_ITEM", 0);
            component.Add("DESCRIPTION", 0);
            component.Add("PARAMETR1", 0);
            component.Add("PARAMETR2", 0);
            component.Add("PARAMETR3", 0);
            component.Add("PARAMETR4", 0);
            component.Add("PARAMETR5", 0);
            component.Add("CAT_NUMBER", 0);
            component.Add("MANUFACTURER", 0);
            component.Add("MASS", 0);
            component.Add("NOTE", 0);
            component.Add("IN_SPECIFICATION", 0);
            return component;
        }
        static public Dictionary<string, int> GetDictionaryElectricSignal()
        {
            var component = new Dictionary<string, int>();
            component.Add("TAG", 0);
            component.Add("DESCRIPTION", 0);
            component.Add("TERMINAL", 0);
            component.Add("CABLE_GLAND", 0);
            component.Add("NOTE1", 0);
            component.Add("NOTE2", 0);
            component.Add("IN_SPECIFICATION", 0);
            return component;
        }
    }
}
