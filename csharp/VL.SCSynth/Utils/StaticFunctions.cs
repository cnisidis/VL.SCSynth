using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using VL.Lib.Collections;

namespace VL.SCSynth.Utils
{
    public static class SCSynthDFS
    {
        public static Spread<ISCNode> DFS(ISCNode SCNode)
        {
            var layer = new RootLayer();
            Stack<ISCNode> stack = new Stack<ISCNode>();
            List<ISCNode> Order = new List<ISCNode>(); // { SCNode }
            List<SCSynth> Synths = new List<SCSynth>();
            List<SCGroup>  Groups = new List<SCGroup>();
            
            stack.Push(SCNode);
            var index = 0;
            while (stack.Count > 0)
            {
                ISCNode v = stack.Pop();
                if (v != null && !Order.Contains(v))
                {
                    
                    Order.Add(v);
                    
                    var nei = (ISCNode)v;//.GetNeighbours();
                        
                    if (nei.GetInputs() != null || nei.GetInputs().Count != 0) 
                    {
                        foreach (ISCNode ne in nei.GetInputs())
                        {
                            if (ne != null && !Order.Contains(ne))
                            {
                                stack.Push(ne);
                            }
                        }
                    }
                        
                    
                    if (v.GetType() == typeof(SCSynth))
                    {
                        index += 1;
                        v.scId = index;
                        Synths.Add((SCSynth)v);
                        
                        
                    }
                    else if (v.GetType() == typeof(SCGroup))
                    {
                        index += 1000;
                        v.scId = index;
                        Groups.Add((SCGroup)v);
                        

                    }


                }
            }
            return Order.ToSpread();
        }
    }
}
