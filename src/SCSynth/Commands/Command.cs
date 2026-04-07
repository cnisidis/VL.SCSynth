using SCSynth.OSC;
using SCSynth.SCNodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using VL.Lib.Collections;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SCSynth.Commands
{
    public class SCCommand
    {
        public SCCommandType Type { get; set; }
        public bool hasAResponse;
        public object Args;
        public object Response { get; set; }
        public int Order;
        public bool isAsync;

        public byte[] encoded;
        public SCCommand(SCCommandType Type, object? args=null)
        {
            this.Type = Type;
            Args = args;
        }

        
        public virtual void SetResponse(object value)
        {
            this.Response = value;
        }

        public void Split(out SCCommandType Type, out object Args)
        {
            Type = this.Type;
            Args = this.Args;
        }

        public virtual Spread<byte> GetBytes()
        {
            return new byte[] { }.ToSpread();
        }
        
    }

    public class FreeAll : SCCommand
    {
        public int baseGroup = 0;
        public FreeAll(int baseGroup = 0) : base(SCCommandType.G_FREEALL, baseGroup)
        {
            this.baseGroup = baseGroup;
        }

        public override Spread<byte> GetBytes()
        {

            return OscEncoder.EncodeMessage("/g_freeAll", baseGroup);
        }
       
    }

    public class ClearScheduled : SCCommand
    {
        public ClearScheduled():base(SCCommandType.CLEARSCHED)
        {
            
        }

        public override Spread<byte> GetBytes()
        {
            return OscEncoder.EncodeMessage("/clearSched", null);
        }

    }

    public class CreateNewGroup:SCCommand
    {
        public int SCId;
        AddActions Action;
        int Target;

        object args;
        public CreateNewGroup(int SCId, int TargetSCId, AddActions Action= AddActions.AddToTail)
            :base(SCCommandType.G_NEW)
        {
            this.SCId = SCId;
            this.Action = Action;
            this.Target = TargetSCId;

        }

        public override Spread<byte> GetBytes()
        {
            return OscEncoder.EncodeMessage("/g_new", new object[] {SCId, Action, Target });
        }

    }

    public class CreateNewGroups:SCCommand
    {
        object[] args;
        public CreateNewGroups(int[] SCIds, AddActions[] Actions, int[] Targets):base(SCCommandType.G_NEW)
        {
            args = new object[] {SCIds, Actions, Targets};
        }

        public CreateNewGroups(object[] args) : base(SCCommandType.G_NEW, args)
        {
            this.args = args;
        }

        public CreateNewGroups(Spread<Group> groups):base(SCCommandType.G_NEW)
        {
            args = groups.SelectMany(x => new object[] { x.SCId }).ToArray();
        }



        public override Spread<byte> GetBytes()
        {
            return OscEncoder.EncodeMessage("/g_new", this.args);
        }

    }

    public class CreateNewSynth:SCCommand
    {
        string synthDefName;
        int SCId;
        AddActions Action;
        int TargetSCId;
        object param;

        public CreateNewSynth(Synth synth):base(SCCommandType.S_NEW)
        {
            synthDefName = synth.SynthDef.name;
            SCId = synth.SCId;
            this.Action = AddActions.AddToTail;
            TargetSCId = synth.lastKnownParent;

            param = synth.ControlParameters.Values.SelectMany(x => new object[] {x.Name, x.Value }).ToArray();
        }

        public override Spread<byte> GetBytes()
        {
            return OscEncoder.EncodeMessage("/s_new", new object[] { synthDefName, SCId, (int)this.Action, TargetSCId, param });
        }
    }

    public class SetParameter:SCCommand
    {
        public string name;
        public object value;
        public int SCId;
        public SetParameter(int SCId, string name, float value):base(SCCommandType.N_SET, value)
        {
            this.SCId= SCId;    
            this.name = name;
            this.value = value;
        }
        public override Spread<byte> GetBytes()
        {
            return OscEncoder.EncodeMessage("/n_set", new object[] { SCId, name, value });
        }

    }

    public class Status:SCCommand
    {
        public Status():base(SCCommandType.STATUS)
        {

        }

        public override Spread<byte> GetBytes()
        {
            return OscEncoder.EncodeMessage("/status");
        }
    }

    public class Sync:SCCommand
    {
        public int Identifier;
        public Sync(int Identifier):base(SCCommandType.SYNC, Identifier)
        {
            this.Identifier = Identifier;
        }

        public override Spread<byte> GetBytes()
        {
            return OscEncoder.EncodeMessage("/sync");
        }
    }

    public class Version:SCCommand
    {
        public Version():base(SCCommandType.VERSION)
        {

        }

        public override Spread<byte> GetBytes()
        {
            return OscEncoder.EncodeMessage("/version");
        }
    }

    public class DefinitionReceive:SCCommand
    {
        public object data;
        public DefinitionReceive(Spread<Spread<Byte>> Bytes):base(SCCommandType.D_RECV, Bytes)
        {
            this.data = Bytes.SelectMany(x=>x.ToArray()).ToArray();
        }
        public override Spread<byte> GetBytes()
        {
            return OscEncoder.EncodeMessage("/d_recv", data);
        }
    }

    public class LoadDefinition:SCCommand
    {
        string filePath = "";
        public LoadDefinition(string filePath):base(SCCommandType.D_LOAD, filePath)
        {
            this.filePath = filePath;
        }

        public override Spread<byte> GetBytes()
        {
            return OscEncoder.EncodeMessage("/d_recv", filePath);
        }
    }

    public class LoadDir:SCCommand
    {
        string dir;
        public LoadDir(string Directory):base(SCCommandType.D_LOADDIR, Directory)
        {
            this.dir = Directory;
        }

        public override Spread<byte> GetBytes()
        {
            return OscEncoder.EncodeMessage("/d_loadDir", dir);
        }

    }

    public class FreeDefinition:SCCommand
    {
        string definition;
        public FreeDefinition(string definition):base(SCCommandType.D_FREE)
        {
            this.definition = definition;   
        }

        public override Spread<byte> GetBytes()
        {
            return OscEncoder.EncodeMessage("/d_free", definition);
        }
    }


}
