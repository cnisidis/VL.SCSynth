
using Stride.Core.Extensions;
using System.Xml.Schema;
using VL.Lib.Collections;

namespace SCSynth
{
    public class RootNode:Group
    {
        public List<Group> Groups;
        public List<Synth> Synths;
        public List<ISCNode> Order;

        private int scId = 1;

        public RootNode():base()
        { 
        
            Groups = new List<Group>();
            Synths = new List<Synth>();
            Order = new List<ISCNode>();
        }

        public void ClearAll()
        {
            Groups.Clear();
            Synths.Clear();
            Order.Clear();
        }


        public void Split(out Spread<ISCNode> Order, out Spread<Group> Groups, out Spread<Synth> Synths)
        {
            Order = this.Order.ToSpread();
            Synths = this.Synths.ToSpread();
            Groups = this.Groups.ToSpread();
        }


        public void Update(ISCNode SCNode)
        {
            if(SCNode != null)
            {
                if(SCNode.GetType() == typeof(Group))
                {
                    if(!SCNode.GetInputs().IsNullOrEmpty())
                    {
                        foreach (var g in SCNode.GetInputs())
                        {
                            if(g.GetType() == typeof(Group))
                            {
                                var group = (Group)g;
                                group.AssignIDs();
                            }
                            else
                            {
                                SCNode.SetSCId(2);
                            }
                            
                        }
                    }
                        
                }
                else if(SCNode.GetType() == typeof(Synth))
                {
                    SCNode.SetSCId(2);
                }
            }
            
        }

    }
}
