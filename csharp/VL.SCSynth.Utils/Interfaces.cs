using Stride.Core.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VL.Lib.IO.Notifications;

namespace VL.SCSynth.Utils
{
    public interface ISCNode : IBehavior, IAutoID, INodeTreeMember
    {
        new void AutoID(CallerInfo caller);
        new bool Notify(INotification notification, CallerInfo caller);

        new IEnumerable<ISCNode> GetInputs();
        public int scId { get; set; }

        public CallerInfo callerInfo { get; set; }

        new void Identify(CallerInfo caller);

        public AddActions AddAction {  get; set; }
    }

    public interface IBehavior
    {
        /// <summary>
        /// If notification got processed return true.
        /// </summary>
        bool Notify(INotification notification, CallerInfo caller);
    }


    public interface IAutoID
    {
        void AutoID(CallerInfo caller);
    }

    public interface INodeTreeMember
    {
        void Identify(CallerInfo caller);
    }
  
    public interface IOSCCommand
    {

    }
}
