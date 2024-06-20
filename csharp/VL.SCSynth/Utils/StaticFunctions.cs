using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VL.SCSynth.Utils
{
    public static class SCSynthDFS
    {
        public static Tuple<List<ISCNode>, List<ISCNode>, List<ISCNode>> DFS(ISCNode GroupNode)
        {
            Stack<ISCNode> stack = new Stack<ISCNode>();
            List<ISCNode> order = new List<ISCNode>();
            List<ISCNode> Groups = new List<ISCNode>();
            List<ISCNode> Synths = new List<ISCNode>();
            stack.Push(GroupNode);
            while (stack.Count > 0)
            {
                ISCNode v = stack.Pop();
                if (v != null && !order.Contains(v))
                {

                    order.Add(v);
                    if (v.GetType() == typeof(SCGroup))
                    {
                        var nei = (ISCNode)v;//.GetNeighbours();
                        if (nei.GetInputs() != null || nei.GetInputs().Count != 0) 
                        {
                            foreach (ISCNode ne in nei.GetInputs())
                            {
                                if (ne != null && !order.Contains(ne))
                                {
                                    stack.Push(ne);
                                }
                            }
                        }

                    }
                    else if (v.GetType() == typeof(SCSynth))
                    {
                        Synths.Add(v);
                    }
                    else if (v.GetType() == typeof(SCGroup))
                    {
                        var group = (SCGroup)v;
                        Groups.Add(group);
                    }
                }
            }
            return Tuple.Create(order, Groups, Synths);
        }
    }
}
