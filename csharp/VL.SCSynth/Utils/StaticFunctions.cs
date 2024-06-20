using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VL.SCSynth.Utils
{
    public static class SCSynthDFS
    {
        public static Tuple<List<ISCNode>, List<SCGroup>, List<SCSynth>> DFS(ISCNode SCNode)
        {
            Stack<ISCNode> stack = new Stack<ISCNode>();
            List<ISCNode> order = new List<ISCNode>();
            List<SCGroup> Groups = new List<SCGroup>();
            List<SCSynth> Synths = new List<SCSynth>();
            stack.Push(SCNode);

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
                        
                        Synths.Add((SCSynth)v);
                    }
                    else if (v.GetType() == typeof(SCGroup))
                    {
                        
                        Groups.Add((SCGroup)v);
                    }
                }
            }
            return Tuple.Create(order, Groups, Synths);
        }
    }
}
