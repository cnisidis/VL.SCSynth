using Stride.Core.Diagnostics;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using VL.Lib.IO.Notifications;


namespace VL.SCSynth.Utils
{

    static class StackHelpers
    {
        public const int maxStackCount = 10;
    }

    
    public class SpectralGroup:ISCNode
    {
        IEnumerable<ISCNode> Inputs;
        int stackCount;
        public int scId { get; set; }

        public CallerInfo callerInfo { get; set; }
        
        
        public AddActions AddAction { get; set; }

        public SpectralGroup()
        {
            scId = 0;
            callerInfo = new CallerInfo(0, typeof(SpectralGroup));
        }

        public IEnumerable<ISCNode> GetInputs()
        {
            List<ISCNode> outputs = new List<ISCNode>();
            
            if (Inputs.Count()>0)
            {   
                if(Inputs != null)
                {
                    foreach (var input in Inputs)
                    {
                        if (input != null)
                        {
                            outputs = outputs.Concat(input.GetInputs()).ToList();
                        }
                        
                    }
                }
            }
            
            var result = Inputs.Concat(outputs).Select(x => x).OfType<ISCNode>().ToList();
            return result;
        }

        public void Update(IEnumerable<ISCNode>? input, out ISCNode output, out int Id)
        {
            
            Inputs = input;
            output = this;
            Id = scId;
            if(Inputs.Count() > 0)
            {
                callerInfo.hasChildren = true;
            }
        }

        
        //TO REMOVE ???
        public void AutoID(CallerInfo caller)
        {
            
            if(caller == null)
            {
                this.scId = 1;
                
            }
            else
            {
                if(Inputs == null)
                {
                   
                }
                
                if (caller.prev == null)
                {
                    this.scId = caller.scId + 100;
                    
                }
                else
                {
                    this.scId = caller.prev.scId + 1;
                }
                
            }
            callerInfo.scId = (this.scId);
            
            if (stackCount >= StackHelpers.maxStackCount)
                return; 
            
            stackCount++;
            try
            {
                if(Inputs.TryGetSpan(out var span))
                {
                    int index = 0;
                    //Allocation free iteration
                    foreach (var i in Inputs)
                    {
                        
                        if(index > 0)
                        {
                            i.callerInfo.prev = Inputs.ToArray()[index-1].callerInfo;
                            i.scId = i.callerInfo.prev.scId+1;
                            if(index < Inputs.Count()-1)
                            {
                                i.callerInfo.next = Inputs.ToArray()[index+1].callerInfo;   
                            }
                            else
                            {
                                i?.AutoID(callerInfo);
                            }
                            
                        }
                        else
                        {
                            callerInfo.scId = this.scId;
                        }
                        i?.AutoID(callerInfo);

                        index++;
                        
                    }    
                        
                }
                else
                {
                    int index = 0;
                    foreach (var i in Inputs)
                    {
                        i.scId = this.scId + 1;
                        if (index > 0)
                        {
                            i.callerInfo.prev = Inputs.ToArray()[index - 1].callerInfo;
                            i.scId = i.callerInfo.prev.scId + 1;
                            if (index < Inputs.Count())
                            {
                                i.callerInfo.next = Inputs.ToArray()[index + 1].callerInfo;
                            }
                            
                        }
                        else
                        {
                            callerInfo.scId = this.scId;
                        }
                        i?.AutoID(callerInfo);

                        index++;
                    }
                    
                    
                }
                
            }
            finally 
            {
                
                stackCount--; 
            }
        }

        public bool Notify(INotification notification, CallerInfo caller) 
        {
            if (stackCount >= StackHelpers.maxStackCount)
                return true;

            stackCount++;

            try
            {
                foreach (var i in Inputs.Reverse())
                    if (i != null && i.Notify(notification, caller))
                        return true;
                return false;
            }
            finally 
            { 
                stackCount--; 
            }
        }

        public void Identify(CallerInfo caller)
        {
            if (caller != null)
            {
                callerInfo.scId = scId;
                callerInfo.parent = caller;
            }
            
            if (stackCount >= StackHelpers.maxStackCount)
                return;

            stackCount++;
            try
            {
                if (Inputs.TryGetSpan(out var span))
                {
                    int index = 0;
                    //Allocation free iteration
                    foreach (var i in Inputs)
                    {
                        if(i != null )
                        {
                            i.callerInfo.parent = callerInfo;
                            if (index > 0)
                            {
                                    
                                    i.callerInfo.prev = Inputs.ToArray()[index - 1].callerInfo;
                                    
                            }
                            if (index < Inputs.Count() - 1)
                            {
                              
                                    CallerInfo? next = Inputs.ToArray()[index + 1].callerInfo;
                                    if (next != null)
                                        i.callerInfo.next = next;
                                    else
                                        i.callerInfo.next = new CallerInfo(-1, typeof(SpectralGroup));
                               
                            }

                            i?.Identify(callerInfo);

                            
                        }
                        index++;
                    }

                }
                else
                {
                    int index = 0;
                    //Allocation free iteration
                    foreach (var i in Inputs)
                    {
                        if (i != null)
                        {
                            i.callerInfo.parent = this.callerInfo;
                            
                            if (index > 0)
                            {
                                i.callerInfo.prev = Inputs.ToArray()[index - 1].callerInfo;
                                if (index < Inputs.Count() - 1)
                                {
                                    i.callerInfo.next = Inputs.ToArray()[index + 1].callerInfo;
                                }
                                
                            }
                            i?.Identify(this.callerInfo);
                            index++;
                        }

                    }


                }

            }
            finally
            {

                stackCount--;
            }
        }
    }
}
