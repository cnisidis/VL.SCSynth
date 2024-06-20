using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VL.Lib.Collections;

namespace VL.SCSynth
{
    public class SCGroup : ISCNode
    {
        public int scId { get; set; }
        public AddActions AddAction { get; set; }
        
        public Spread<ISCNode> Inputs{get; set;}
        public SCGroup()
        {

        }

        public List<ISCNode> GetInputs()
        {
            return Inputs.ToList();
        }
    }
}
