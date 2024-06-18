using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VL.Core;
using VL.Core.CompilerServices;
using VL.Lib.Collections;

namespace VL.SCSynth.GraphNodes
{
    public class DynamicSynthDefDesc
    {
        public string Update(DynamicSynthDesc enumInput)
        {
            if (enumInput.IsValid())
                return enumInput.Definition.Entries[enumInput.SelectedIndex()];
            else
                return "No valid entry selected";
        }

        public DynamicSynthDefFile GetDefinition()
        {
            return DynamicSynthDefFile.Instance;
        }

        public void AddEnumEntry(string entry)
        {
            DynamicSynthDefFile.Instance.AddEntry(entry, null);
        }

        public void RemoveEnumEntry(string entry)
        {
            DynamicSynthDefFile.Instance.RemoveEntry(entry);
        }
    }
    public class DynamicSynthDefFile : ManualDynamicEnumDefinitionBase<DynamicSynthDefFile>
    {
        //this is optional an can be used if any initialization before the call to GetEntries is needed
        protected override void Initialize()
        {
            
            string currentDir = AppHost.Global.AppPath;
            if (Directory.Exists(currentDir))
            {
                Console.WriteLine(currentDir);
                Directory.GetFiles(currentDir);

            }

            //add two default entries on initialization
            //AddEntry("abara", null);
            //AddEntry("kadabara", null);
            //AddEntry("kadabara", null);
        }

        //add this to get a node that can access the Instance from everywhere
        public static DynamicSynthDefFile Instance => ManualDynamicEnumDefinitionBase<DynamicSynthDefFile>.Instance;
    }

    [Serializable]
    public class DynamicSynthDesc : DynamicEnumBase<DynamicSynthDesc, DynamicSynthDefFile>
    {
        public DynamicSynthDesc(string value) : base(value)
        {
        }

        [CreateDefault]
        public static DynamicSynthDesc CreateDefault()
        {
            //use method of base class if nothing special required
            return CreateDefaultBase();
        }
    }



}
