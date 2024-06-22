using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VL.Lib.Collections;

namespace VL.SCSynth
{
    public class RootLayer
    {
        public List<SCGroup> Groups;
        public List<SCSynth> Synths;
        public List<ISCNode> Order;

        public RootLayer() 
        { 
        
            Groups = new List<SCGroup>();
            Synths = new List<SCSynth>();
            Order = new List<ISCNode>();
        }

        public void ClearAll()
        {
            Groups.Clear();
            Synths.Clear();
            Order.Clear();
        }


        public void Split(out Spread<ISCNode> Order, out Spread<SCGroup> Groups, out Spread<SCSynth> Synths)
        {
            Order = this.Order.ToSpread();
            Synths = this.Synths.ToSpread();
            Groups = this.Groups.ToSpread();
        }

    }
}
