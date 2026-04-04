using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSynth.SCNodes
{
    public interface ISCNode
    {
        //Super Collider internal ID
        public int scId { get; set; }

        //Generic VL Id for further handling
        public Guid Id { get; set; }

        public bool Enabled { get; set; }
        public bool hasChildren { get; }
        public int Order { get; set; }

        public Group ParentGroup { get; set; }

        public AddActions AddAction { get; set; }

        public List<ISCNode> GetInputs();

        public void ToString(out string Result)
        {
            Result = "Id:" + scId.ToString();
        }

    }

}
