using VL.Core;
using VL.Lib.Collections;
using static VL.Core.Import.ProcessNodeFactory;

namespace SCSynth.SCNodes
{
    public class Group : ISCNode
    {
        public int scId { get; set; }
        public AddActions AddAction { get; set; }
        public Group ParentGroup { get; set; }
        public Guid Id { get; set; }
        public bool hasParentGroup { get; set; }
        public Spread<ISCNode> Inputs{get; set;}
        public bool hasChildren => Inputs.Where(x=>x !=null).Any();
        public bool Enabled { get; set; }
        public bool isChanged { get; set; }
        public int Order { get; set; }
        Spread<ISCNode> _prevNodes;
        NodeContext _context;
        public Group(NodeContext context)
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
                    var chg = Inputs.Where(x=>x != null && x.GetType() == typeof(Group)).
                                                    Cast<Group>().Any(x=>x.isChanged) ? true : false;
                    
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
