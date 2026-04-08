using SCSynth.Commands;
using SCSynth.Messaging;
using SCSynth.OSC;
using SCSynth.SCNodes;

using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Runtime.Intrinsics.Wasm;
using VL.Lib.Collections;


namespace SCSynth
{
    public class Root:IDisposable
    {
        private SCNode? _node;
        public List<Group> Groups { get; private set; }
        public List<Synth> Synths { get; private set; }

        public SCMessage LatestStatus;
        private bool _invalidated;
        
        public Spread<SCNode> AllNodes { get; private set; }
        public int NodesCount = 0;

        // A stream that holds the "Current" collection of active nodes
        private readonly BehaviorSubject<IEnumerable<SCNode>> _activeNodes = new(Enumerable.Empty<SCNode>());
        private readonly CompositeDisposable _cleanup = new();

        private readonly Subject<SCMessage> _manualCommands = new();
        public IObservable<SCMessage> SCCommands => _manualCommands;

        private readonly Subject<Spread<byte>> _udpSender = new();

        private readonly List<Spread<byte>> _frameBuffer = new();

        public HashSet<string> SynthDefinitionNames { get; set; }

        public Root()
        {
            AllNodes = new List<SCNode>().ToSpread();
            Groups = new List<Group>();
            Synths = new List<Synth>();
            _frameBuffer = new List<Spread<byte>>();
            NodesCount = 0;

            var treeSource = _activeNodes
                .Select(nodes => nodes.Merge())
                .Switch();

            // 2. The Manual Source
            var manualSource = _manualCommands; // A simple Subject<SCMessage>

            // 3. THE MERGE (The River)
            //var finalPipeline = Observable.Merge(treeSource, manualSource);

            // 1. Setup the listener
            var sub = treeSource.Subscribe(msg =>
            {
                switch (msg.type)
                {
                    case SCMessageType.Invalidate:
                        // Throttle BuildTree so we don't spam the CPU
                        Observable.Return(Unit.Default)
                                  .Delay(TimeSpan.FromMilliseconds(50))
                                  .Subscribe(_ => BuildTree());
                        break;

                    case SCMessageType.NodeQuery:
                        
                        break;

                    case SCMessageType.NodeScalar:
                        
                        var encode = ((SCCommand)msg.value).GetBytes();
                        _frameBuffer.Add(encode);
                        //Console.WriteLine("Parameter Changed");
                        break;

                    case SCMessageType.Info:
                        Console.WriteLine("New Info Message");
                        break;

                    
                }
            });

            _cleanup.Add(sub);
            _invalidated = false;
            
            Update(null);
            //ClearTree();
            
            // 2. IMPORTANT: Trigger the first build so we start listening to the first batch of nodes
            BuildTree();
        }

        public void SetBuffers(Spread<SCBuffer>? Buffers)
        {

        }

        public void SetCommands(Spread<SCCommand>? Commands)
        {

        }

        public void Update(SCNode? Node)
        {
            
            //messages = _activeNodes.Select(nodes => nodes.Merge()).Switch();
            if (Node == null && _invalidated==false) {
                //Console.WriteLine("Null");
                _invalidated = true; 
                ClearTree();  
            }
            
            if (_node!=Node)
            {
                
                this._node = Node;
                BuildTree();
                
            }
            if (_node == null) { BuildTree(); return; }
            if (_invalidated) { BuildTree(); }

        }

        public void OnUpdate(out IObservable<Spread<byte>> Sender, out int count)
        {

            Sender = _udpSender;
            count = _frameBuffer.Count;
            if (count > 0)
            {
                byte[] timetag = OscEncoder.GetNtpTimestamp(20);
                var bundle = OscEncoder.EncodeBundle(_frameBuffer.ToSpread(), timetag);
                _udpSender.OnNext(bundle);
                _frameBuffer.Clear();
            }
        }

        

        

        public void BuildTree()
        {
            
            this.Groups = new List<Group>();
            this.Synths = new List<Synth>();

            if (_node == null)
            {
                _activeNodes.OnNext(Enumerable.Empty<SCNode>());
                return;
            }

            // 2. Gather children
            var children = Utils.Utils.GetAllChildren(_node, true).Where(x => x != null).ToList();
            //children.ForEach(x => x.lastKnownParent = 0);
            this.AllNodes = children.ToSpread();

            if (_node!=null && _node.GetType() == typeof(Group))
            {

                if (AllNodes.Any())
                {
                    Groups = AllNodes.Where(x => x.GetType().Equals(typeof(Group))).Cast<Group>().ToList();
                    Groups.ForEach((group, idx) => group.SCId = 1000 * (idx + 1));

                    Synths = AllNodes.Where(x => x.GetType().Equals(typeof(Synth))).Cast<Synth>().ToList();
                }

            }
            else if (_node!=null && _node.GetType() == typeof(Synth))
            {
                Console.WriteLine("A Single Synth is Connected on Root");
                Synths.Add((Synth)_node);
            }

            //Messages.Merge().Append(new SCMessage("","'"));
            AllNodes.ForEach(x => x.Invalidated = false);
            
            Groups.ForEach((group, group_idx) => { 
                group.Inputs.Where(x => !x.Equals(null))
                .ForEach((inp, inp_idx) => {
                    if (inp.Equals(typeof(Group)))
                    {
                        
                        inp.SCId = group.SCId + 1000 * group_idx;
                    }
                        
                    else if (inp.GetType().Equals(typeof(Synth)) )
                    {
                        inp.SCId = group.SCId + inp_idx+1;
                    }

                    inp.lastKnownParent = group.SCId;

                }); 
                
            });
            // 2. Push them into the tracker. Switch() handles the rest.
            _activeNodes.OnNext(AllNodes);
            
            //StartListening();
            _invalidated = false;

            ClearTree();
            CreateTree();
        }
        /// <summary>
        /// Clears Tree on SuperCollider Side
        /// </summary>
        public void ClearTree()
        {

            this._manualCommands.OnNext(new SCMessage(SCMessageType.System, "Clear Tree", new SCCommand[] 
            {
                new FreeAll(1),
                new ClearScheduled(),
                new CreateNewGroup(1, 0, AddAction.AddToHead ),

            }, true));
        }

        public void CreateTree()
        {
            List<SCCommand> commands = new List<SCCommand>();
            List<Tuple<int, int, int>> pgr = new List<Tuple<int, int, int>>();
            //Collect and encode groups.
            var idx = 0;
            foreach(var group in Groups)
            {
                if(group!=null)
                {
                    var id = group.SCId;
                    var pID = group.lastKnownParent;
                    pgr.Add(Tuple.Create(id, (int)AddAction.AddToTail, pID));
                }
                
            }
            var flatArgs = pgr.SelectMany(t => new object[] { t.Item1, t.Item2, t.Item3 }).ToArray();
            
            _manualCommands.OnNext(new SCMessage(SCMessageType.System, "Create Groups", new CreateNewGroups(flatArgs)));
            
            foreach (var synth in Synths.Where(x=>x !=null))
            {
                
                    _manualCommands.OnNext(new SCMessage(SCMessageType.System, "Create Synths", new CreateNewSynth(synth)));
                    
            }
            
        }

        public void GetNodes(out Spread<Synth> Synths, out Spread<Group> Groups)
        {
            Synths = this.Synths.ToSpread();
            Groups = this.Groups.ToSpread();
        }

        public Spread<SCNode> GetAllNodes(out int Count)
        {
            Count = AllNodes.Count;
            return AllNodes.Cast<SCNode>().ToSpread();
        }

        public void ClearAll()
        {
            Synths.Clear();
            Groups.Clear();
            
        }


        public void LoadLocalDirectorySynthDefs(string dir)
        {
            if (Directory.Exists(dir))
                _manualCommands.OnNext(new SCMessage(SCMessageType.System, "Load SynthDefs from Dir", new LoadDir(dir)));
            else
                Console.WriteLine("Select a valid dir");
        }

        public void Dispose() {
            ClearTree();
            _frameBuffer.Clear();
            _cleanup.Dispose(); 
            _udpSender.Dispose();
            _manualCommands.Dispose();
            _activeNodes.Dispose();
            
        }
    }
}
