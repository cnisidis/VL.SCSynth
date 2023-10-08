using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using VL.Lib.IO.Notifications;



namespace VL.SCSynth.Utils
{


    public class LinkedSCNodeBase : ISCNode
    {
        protected ISCNode Input;

        public int scId { get; set; }

        public CallerInfo callerInfo { get; set; }

        
        
        public virtual void AutoID(CallerInfo caller)
        {
            
            Input?.AutoID(caller);
        }

        public virtual IEnumerable<ISCNode> GetInputs()
        {
            return Input?.GetInputs();
        }


        public virtual bool Notify(INotification notification, CallerInfo caller)
        {
            return Input?.Notify(notification, caller) ?? false;
        }

        public virtual void Identify(CallerInfo caller)
        {
            Input?.Identify(caller);
        }
    }
    
    
}
