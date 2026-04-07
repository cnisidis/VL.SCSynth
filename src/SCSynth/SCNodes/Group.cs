using System.Xml.Linq;
using VL.Core;
using VL.Lib.Collections;
using static VL.Core.Import.ProcessNodeFactory;

namespace SCSynth.SCNodes
{

    public class Group : SCNode
    {
        public Spread<ControlParameter> ExternalControls { get; set; }
        private Spread<SCNode> _prevInputs;
        
        public Group(NodeContext context) : base()
        {
            
            _prevInputs = new List<SCNode>().ToSpread();
            Initialize();
        }

        public override void Update()
        {

            base.Update();

            //Invalidated = _prevInputs.Select((x, idx) => x != Inputs[idx]).Any();
            if (Inputs.Equals(null)) { _prevInputs = new List<SCNode>().ToSpread(); return; }
            else
            {

                // 1. Check if the counts even match first
                if (Inputs.Count == _prevInputs.Count)
                {
                    bool sequencesAreIdentical = true;
                    for (int i = 0; i < Inputs.Count; i++)
                    {
                        if (Inputs[i].Id != _prevInputs[i].Id)
                        {
                            sequencesAreIdentical = false;
                            break;
                        }
                    }

                    // 2. ONLY print if they are NOT identical
                    if (!sequencesAreIdentical)
                    {
                        Console.WriteLine("Inputs changed! Invalidating...");
                        Invalidate();
                    }
                }
                else
                {
                    // Counts differ, obviously changed
                    Invalidate();
                }

                // 3. Update the 'history' for the next frame
                _prevInputs = Inputs.ToSpread();


            }
           

        }

        public override void SetInputs(Spread<SCNode> Nodes)
        {
            base.SetInputs(Nodes.Where(x=>x !=null).ToSpread());
        }

        


    }
    public class OldGroup : ISCNode
    {
        public int scId { get; set; }
        public AddActions AddAction { get; set; }
        public OldGroup ParentGroup { get; set; }
        public Guid Id { get; set; }
        public bool hasParentGroup { get; set; }
        public Spread<ISCNode> Inputs{get; set;}
        public bool hasChildren => Inputs.Where(x=>x !=null).Any();
        public bool Enabled { get; set; }
        public bool isChanged { get; set; }
        public int Order { get; set; }
        Spread<ISCNode> _prevNodes;
        NodeContext _context;
        public OldGroup(NodeContext context)
        {
            Id = Guid.NewGuid();
            isChanged = false;
            Inputs = new List<ISCNode>().ToSpread();
            _prevNodes = Inputs;
            hasParentGroup = false;
            ParentGroup = null;
            _context = context;
        }

        public bool Changed()
        {
            if (!_prevNodes.Equals(Inputs))
            {
                _prevNodes = Inputs;
                
                return true;
            }
            
            else
            {
                if (Inputs.Count > 0 && Inputs!=null)
                {
                    var chg = Inputs.Where(x=>x != null && x.GetType() == typeof(OldGroup)).
                                                    Cast<OldGroup>().Any(x=>x.isChanged) ? true : false;
                    
                    return chg;
                }
                else
                {
                    return false;
                }
                    
                
            }
            
        }


        public void Update()
        {
            
            isChanged = Changed();
            
            
            if (ParentGroup != null) { 
                hasParentGroup = true; 
            }
            else
            {
                hasParentGroup= false;
            }
            //Set Order of Input Nodes and Assign Parent Group
            if (isChanged)
            {
                Inputs.Where(x => x != null).ForEach((x, index) =>
                {
                    x.ParentGroup = this;
                    x.Order = index+1;
                });
                   
            }

            
            
        }

        public List<ISCNode> GetInputs()
        {
            return Inputs.ToList();
        }
        public void ToString(out string Result)
        {
            Result = "Id:" + scId.ToString();
        }
    }
}
