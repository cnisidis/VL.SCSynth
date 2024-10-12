
using System.CodeDom;
using VL.Lib.Collections;

namespace SCSynth
{
    public class Group : ISCNode
    {
        private int scId;
        public AddActions AddAction { get; set; }

        public Guid Id { get; set; }

        public Spread<ISCNode> Inputs{get; set;}

        public Boolean isChanged { get; set; }
        public Group()
        {
            this.Id = Guid.NewGuid();
            this.isChanged = false;
        }


        public void SetSCId(int index)
        {
            this.scId = index;  
        }

        public int GetSCId()
        {
            return this.scId;
        }

        public IEnumerable<ISCNode> GetInputs()
        {
            return Inputs.ToSpread();
        }
        public void ToString(out string Result)
        {
            Result = "Id:" + scId.ToString();
        }

        public void AssignIDs()
        {
            if(this.Inputs != null) 
            {
                var index = this.GetSCId() + 1;
                foreach (var input in this.Inputs)
                {
                    if(input.GetType() == typeof(Synth))
                    {
                        index += 1;
                    }

                    else if(input.GetType() == typeof(Group))
                    {
                        index += 1000;
                    }
                    
                    input.SetSCId(index);
                    
                }
            }
        }
    }
}
