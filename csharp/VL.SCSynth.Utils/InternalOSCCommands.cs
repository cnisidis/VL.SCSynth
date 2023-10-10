using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace VL.SCSynth.Utils
{
    public record InternalOSCCommand<T>
    {
        public Guid Id { get; init; }
        public T Args { get;set; }
        public bool Bundled { get; set; }
        public bool SendOnChange { get; set; }

        //public static readonly DownStreamOSCCommand<T> Default = new DownStreamOSCCommand();

        internal InternalOSCCommand(T Args, bool Bundled=false, bool SendOnChange = true)
        {
            this.Id = Guid.NewGuid();
            this.Args = Args;
            this.Bundled = false;
            this.SendOnChange = SendOnChange;
            
        }

    }

    public record InternalOSCResponse
    {

    }
}
