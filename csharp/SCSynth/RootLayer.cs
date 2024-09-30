
using VL.Lib.Collections;

namespace SCSynth
{
    public class RootLayer
    {
        public List<Group> Groups;
        public List<Synth> Synths;
        public List<ISCNode> Order;

        public RootLayer() 
        { 
        
            Groups = new List<Group>();
            Synths = new List<Synth>();
            Order = new List<ISCNode>();
        }

        public void ClearAll()
        {
            Groups.Clear();
            Synths.Clear();
            Order.Clear();
        }


        public void Split(out Spread<ISCNode> Order, out Spread<Group> Groups, out Spread<Synth> Synths)
        {
            Order = this.Order.ToSpread();
            Synths = this.Synths.ToSpread();
            Groups = this.Groups.ToSpread();
        }

    }
}
