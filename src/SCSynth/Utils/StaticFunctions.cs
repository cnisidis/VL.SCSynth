
using VL.Lib.Collections;

namespace SCSynth.Utils
{
    public static class SCSynthDFS
    {
        public static Spread<ISCNode> DFS(ISCNode SCNode)
        {
            var layer = new RootLayer();
            Stack<ISCNode> stack = new Stack<ISCNode>();
            List<ISCNode> Order = new List<ISCNode>(); // { SCNode }
            List<Synth> Synths = new List<Synth>();
            List<Group>  Groups = new List<Group>();
            
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
                        
                    
                    if (v.GetType() == typeof(Synth))
                    {
                        index += 1;
                        v.scId = index;
                        Synths.Add((Synth)v);
                        
                        
                    }
                    else if (v.GetType() == typeof(Group))
                    {
                        index += 1000;
                        v.scId = index;
                        Groups.Add((Group)v);
                        

                    }


                }
            }
            return Order.ToSpread();
        }
    }
}
