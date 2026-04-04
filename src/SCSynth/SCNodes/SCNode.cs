using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSynth.SCNodes
{
    public class SCNode
    {
        SCNode innerObject;

        bool Invalidated;
        bool Changed;
        bool hasChildren=>Inputs.Any();

        public List<object> Inputs;

        public SCNode(SCNode Node)
        {
            this.innerObject = Node;
            if(this.innerObject.hasChildren)
            {
                //Subscribe Children to parent for Change and Invalidate
            }
        }


    }
}
