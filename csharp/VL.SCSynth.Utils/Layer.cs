using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VL.IO.OSC;

namespace VL.SCSynth.Utils
{
    public class Layer
    {
        int baseId=0;
        public ISCNode? Input;

        public CallerInfo? callerInfo;
        
        public IEnumerable<ISCNode> Nodes;
        

        public Layer(int id=0)
        {
            baseId = id;
            callerInfo = new CallerInfo (id, typeof(SpectralGroup));
            callerInfo.scId = baseId;
            callerInfo.parent = null;
            callerInfo.prev = null; 
            callerInfo.next = null;
            
        }

        public void Update(ISCNode? SCNode, bool AutoAssignIDs,  out int nodesCount )
        {
            nodesCount = 0;
            if( SCNode != null )
            {

                Input = SCNode;
                Input?.Identify(callerInfo);
                
                Nodes = new ISCNode[] { Input }.Union(Input.GetInputs());
                


                nodesCount = Nodes.Count();
                
                //Input.AutoID(callerInfo);
                
                int i = 0;
                

                if (AutoAssignIDs)
                {
                    int index = 0;
                    if (Input.GetType() == typeof(SpectralGroup))
                    {
                        
                        foreach (var node in Nodes)
                        {
                            if (node != null)
                            {
                                node.scId = baseId + index;
                                index++;
                            }

                        }
                    }
                }
                
                
            }
            else
            {
                Console.WriteLine("Input is null");
            }
            
            
        }

        public void GetNodesGraph(out IEnumerable<ISCNode> NodesTree)
        {
            NodesTree = Nodes;
        }
        

    }
}
