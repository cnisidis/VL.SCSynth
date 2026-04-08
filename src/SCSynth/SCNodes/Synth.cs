
using SCSynth.Factory;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using VL.Lib.Collections;


namespace SCSynth.SCNodes
{
   
    /// <summary>
    /// A SuperCollider Synth Instance.
    /// </summary>
    public class Synth:SCNode
    {
        /// <summary>
        /// SynthDef of the specific synth
        /// </summary>
        /// 
        public SCManager.SynthDef? SynthDef;
        /// <summary>
        /// Synth Name -> can be found in the synthdef.
        /// </summary>
        public string Name;

        private string _name;
        
        public Synth(SCManager.SynthDef? SynthDef, string Name="vvvv dummy synth"):base()
        {
            this.SynthDef = SynthDef;
            this.Name = this.SynthDef != null ? this.SynthDef.name : Name ;
            
            base.Initialize();
            
        }

        public override void Update()
        {
            if(_name != Name)
            {
                _name = Name;
                Invalidate();
            }
            base.Update();
        }

        
        
    }

    
}
