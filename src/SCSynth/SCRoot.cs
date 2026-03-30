using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VL.Lib.Collections;
using VL.Lib.IO;

namespace SCSynth
{
    public class SCRoot
    {
        //Collection of nodes
        private List<ISCNode> nodes { set; get; }
        
        public bool hasChanged { get; private set; }

        
        private ISCNode _node;

        private Dictionary<string, byte[]> _synthdefs;

        List<Group> _groups;
        List<Synth> _synths;


        public Dictionary<Guid, int> SynthsMap { get; private set; }

        public SCRoot()
        {
            nodes = new List<ISCNode>();
            _groups = new List<Group> { };
            _synths = new List<Synth>();
            SynthsMap = new Dictionary<Guid, int>();
        }

        //Builds tree in a simpler way
        public void Update(ISCNode node)
        {
            if (node == null) { nodes.Clear(); return; }
            
            else if (node.GetInputs().Count == 0) { nodes.Clear(); }
            
            nodes = GetChildren(node);
            var index = 1;

            
            nodes.Where(x => x != null).ForEach((x, idx) => {
                if (x.ParentGroup == null && x.GetType().Equals(typeof(Group))) x.scId = 1000;
                else if (x.GetType().Equals(typeof(Group))) x.scId = 1000 * (idx + 1);
                else x.scId = x.ParentGroup.scId + x.Order;

            

            });

            
            
            if ( _node!=node )
            {

              
                _node = node;
                hasChanged = true;
            }
            else if(nodes.Where(x => x != null && x.GetType() == typeof(Group)).Cast<Group>().Any(x => x.isChanged))
            {
                hasChanged = true;
            }
            else
            {
                hasChanged = false;
            }

            
            
            PrepareTree();
        }

        //Build Tree
        public void PrepareTree()
        {
            if (hasChanged)
            {
                Console.WriteLine("preparing tree");
                SynthsMap.Clear();
                //collect all synth definitions
                _groups = nodes.Where(x => x != null && x.GetType().Equals(typeof(Group))).Cast<Group>().ToList();
                _synths = nodes.Where(x => x != null && x.GetType().Equals(typeof(Synth))).Cast<Synth>().ToList();
                //var bufers = nodes.Where(x => x != null && x.GetType() == typeof(SCBuffer));

                
                
                foreach(Synth synth in _synths)
                {
                    var path = synth.synthDefFilePath;
                    var filename = System.IO.Path.GetFileName(path);
                    Console.WriteLine(filename);
                    SynthsMap.TryAdd(synth.Id, synth.scId);
                }

            }
        }

        public int GetMappedSynthSCId(Synth synth)
        {
            if (synth == null) return -1;
            var scid = -1;
            SynthsMap.TryGetValue(synth.Id, out scid);
            return scid;
        }

        public int GetMappedSynthSCId(Guid id)
        {
            
            var scid = -1;
            SynthsMap.TryGetValue(id, out scid);
            return scid;
        }

        /// <summary>
        /// GetChildrern of children (recursvive)
        /// </summary>
        /// <param name="node">Any given node (Group or Synth)</param>
        /// <returns>A List with all found children of type ISCNode</returns>
        public List<ISCNode> GetChildren(ISCNode node)
        {
            List<ISCNode> nodes = new List<ISCNode>();
            nodes.Add(node);
            if (node == null) return null;
            
            var inputs = node.GetInputs();
            
            if (inputs.Any())
            {
                foreach (var input in inputs) { 
                    if(input != null)
                        nodes.AddRange( GetChildren(input) );
                }
            }
            return nodes;
        }

        public Spread<Synth> GetSynths()
        {
            return _synths.ToSpread();
        }

        public Spread<Group> GetGroups()
        {
            return _groups.ToSpread();
        }

        public Spread<ISCNode> GetNodes()
        {
            return _groups.ToSpread().Concat<ISCNode>(_synths.ToSpread()).ToSpread();
        }
    }
}
