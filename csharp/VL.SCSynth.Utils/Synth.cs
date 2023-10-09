using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VL.Lib.IO.Notifications;

namespace VL.SCSynth.Utils
{
    public class Synth : ISCNode
    {
        /// <summary>
        /// Synth Def origins from
        /// </summary>
        public string SynthDefName { get; set; }    
        /// <summary>
        /// SuperCollider Node Id: this should always match the id on the SC node graph
        /// </summary>
        public int scId { get; set; }
        
        //Add synth parameters Enumerable* TODO 
        public CallerInfo? callerInfo { get; set; }

        public Synth()
        { 
            callerInfo = new CallerInfo(0, typeof(Synth));
            callerInfo.parent = this.callerInfo;
            callerInfo.prev = null;
            callerInfo.next = null;
        }

        public void Update(IEnumerable<ISCNode>? Parameters, int Id)
        {
            //Parameters are not implemented yet
            scId = Id;
            callerInfo.hasChildren = false;
            
        }

        public void AutoID(CallerInfo caller)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ISCNode> GetInputs()
        {
            return new ISCNode[] { this };
        }

        public void Identify(CallerInfo caller)
        {
            callerInfo.scId = scId;
            try
            {
                if (caller != null)
                {

                    callerInfo.parent = caller;
                    if(caller.hasChildren)
                    {
                        //if caller has children get last input and set this.CallerInfo.prev
                    }
                }
                //If Synth is part of a group (which is the most common case, then prev node should be assigned by retreiving the callers.inputs[prev].scid, that's why it is essential of keeping track of the input order (index)) 
            }
            finally 
            { 
            
                
            }


        }

        public bool Notify(INotification notification, CallerInfo caller)
        {
            throw new NotImplementedException();
        }
    }
}
