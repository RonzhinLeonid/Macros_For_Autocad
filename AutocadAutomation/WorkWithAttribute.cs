using Autodesk.AutoCAD.DatabaseServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutocadAutomation
{
    static class WorkWithAttribute
    {
        static public Dictionary<string, int> FillDictionaryAttributes(AttributeCollection attrC, Dictionary<string, int> dictionaryBlock)
        { 
            //var dictionaryBlock = new Dictionary<string, int>();
            foreach (ObjectId idAtrRef in attrC)
            {
                using (var atrRef = idAtrRef.GetObject(OpenMode.ForRead) as AttributeReference)
                {
                    if (atrRef != null)
                    {
                        if (dictionaryBlock.ContainsKey(atrRef.Tag))
                            dictionaryBlock[atrRef.Tag] = 1;
                        else
                            dictionaryBlock.Add(atrRef.Tag, 0);
                    }
                }
            }
            return dictionaryBlock;
        }
        static public bool IsMeet(Dictionary<string, int> dictionaryBlock)
        {
            return dictionaryBlock.All(key => key.Value == 1);
        }
        static public Dictionary<string, string> GetDictionaryAttributes(AttributeCollection attrC)
        {
            var dictionartAttribute = new Dictionary<string, string>();
            foreach (ObjectId idAtrRef in attrC)
            {
                using (var atrRef = idAtrRef.GetObject(OpenMode.ForRead) as AttributeReference)
                {
                    if (atrRef != null)
                    {
                        dictionartAttribute.Add(atrRef.Tag, atrRef.TextString);
                    }
                }
            }
            return dictionartAttribute;
        }
    }
}
