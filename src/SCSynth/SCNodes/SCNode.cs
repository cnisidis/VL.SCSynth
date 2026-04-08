

using SCSynth.Commands;
using SCSynth.Messaging;
using System.Data;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using VL.Lib.Collections;

namespace SCSynth.SCNodes
{

    public enum SCNodeType
    {
        SYNTH,
        GROUP,
        BUFFER,
        BUS
    }

    
    public class SCNode:IObservable<SCMessage>
    {
        /// <summary>
        /// The inner Object (SCNode)
        /// </summary>
        SCNode innerObject;
        /// <summary>
        /// if the inner object or any of its children gets invalidated
        /// </summary>
        public bool Invalidated { get; set; }
        private bool _invalidated;
        /// <summary>
        /// On Change of any parameter
        /// </summary>
        bool Changed;

        /// <summary>
        /// Itnernal auto assigned ID (during the Tree build) will be used by SuperCollider to track the specific node.
        /// </summary>
        public int SCId { set; get; }
        /// <summary>
        /// Last Known Parent is the last known group that the node was connected on. 
        /// This will be used when it gets validated first, during the BuildTree procedure.
        /// </summary>
        public int lastKnownParent { get; set; } = 1;

        /// <summary>
        /// This ID will be used only in vvvv context
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();
        

        private readonly Subject<SCMessage> _message = new();
        public IObservable<SCMessage> message => _message;

        public string LastResponse { get; private set; }
        /// <summary>
        /// Enable / Disbale a Node (this is invalidating the node and removes it temporarely from the Tree
        /// </summary>
        /// 
        private bool _enabled;
        public bool Enabled
        {
            get => _enabled;
            set
            {
                if (_enabled != value)
                {
                    _enabled = value;
                    Invalidate();
                    
                }
            }
        }

        public void Invalidate() { _message.OnNext(new SCMessage(SCMessageType.Invalidate, SCId.ToString())); }


        public bool hasChildren=>Inputs.Where(x=>x!=null).Any();

        public Spread<SCNode> Inputs;

        public Dictionary<string, ControlParameter> ControlParameters;


        public SCNode(bool Enable=true)
        {
            _message = new Subject<SCMessage>();
            this.Enabled = Enable;
            
            ControlParameters = new();
            
            Inputs = new List<SCNode>().ToSpread();

            Initialize();

        }

        public virtual void Update()
        {
            
        }

        public void SetRoot(Root Root)
        {
            if(Root != null) { Console.WriteLine("Connected To Root"); }
        }

        public virtual void Initialize()
        {
            // If this is called in the constructor, it's often too early.
            // Ensure this is called once the node is fully "configured".
            foreach (var param in ControlParameters.Values)
            {
                param.OnChanged = (param) =>
                {
                    Set(param);
                };
            }
        }

        public virtual void SetInputs(Spread<SCNode> Nodes)
        {
            this.Inputs = Nodes.Where(x=>x != null).ToSpread();
        }

        public void SendQuery(SCCommand command)
        {
            
            if(command != null)
            {
                
                // Send it upstream
                _message.OnNext(new SCMessage(SCMessageType.NodeQuery, "SuperCollider Query", command));

            }
            
        }
        
        
        // Delegate the subscription to our internal subject
        public IDisposable Subscribe(IObserver<SCMessage> observer) => _message.Subscribe(observer);

        public virtual void Free()
        {
            _message.OnNext(new SCMessage(SCMessageType.NodeScalar, "", new SCCommand(SCCommandType.N_FREE, SCId)));
        }

        public virtual void Run(bool run)
        {
            _message.OnNext(new SCMessage(SCMessageType.NodeScalar, "", new SCCommand(SCCommandType.N_RUN, Tuple.Create(SCId, run))));
        }

        public virtual void Set(ControlParameter parameter)
        {
            _message.OnNext(new SCMessage(SCMessageType.NodeScalar,"", new SetParameter(this.SCId, parameter.Name, parameter.Value) ));
        }

        public virtual void SetRange(Spread<ControlParameter> parameters)
        {

        }

    }

    

    public class DummySCNode : SCNode
    {


        private readonly Subject<SCMessage> _message = new Subject<SCMessage>();
        public IObservable<SCMessage> message => _message;
        public DummySCNode() : base()
        {
            
            
            
        }

        

        

        public IDisposable Subscribe(IObserver<SCNode> observer)
        {
            throw new NotImplementedException();
        }

        
    }
}
