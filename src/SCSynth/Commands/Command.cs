using SCSynth.OSC;
using SCSynth.SCNodes;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Security.Cryptography;
using VL.Lib.Collections;


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
        AddAction Action;
        int Target;

        object args;
        public CreateNewGroup(int SCId, int TargetSCId, AddAction Action= AddAction.AddToTail)
            :base(SCCommandType.G_NEW)
        {
            this.SCId = SCId;
            this.Action = Action;
            this.Target = TargetSCId;

        }

        public CreateNewGroup(Group group, AddAction Action=AddAction.AddToTail):base(SCCommandType.G_NEW, group)
        {
            this.SCId=group.SCId;
            this.Action = Action;
            this.Target = group.lastKnownParent;
        }

        public override Spread<byte> GetBytes()
        {
            return OscEncoder.EncodeMessage("/g_new", new object[] {SCId, Action, Target });
        }

    }

    public class BulkCreateNewGroups:SCCommand
    {
        Spread<Spread<byte>> data;
        Spread<CreateNewGroup> commands;
        public BulkCreateNewGroups(Spread<CreateNewGroup> newGroupsCommands):base(SCCommandType.G_NEW)
        {
            commands = newGroupsCommands;
            List<Spread<byte>> result = new();
            foreach(var cmd in newGroupsCommands)
            {
                result.Add(cmd.GetBytes());
            }

            data = result.ToSpread();
        }
        public override Spread<byte> GetBytes()
        {
            return OSC.OscEncoder.EncodeBundle(data.ToSpread(), new byte[8] { 0, 0, 0, 0, 0, 0, 0, 0 });
        }
    }

    public class CreateNewGroups:SCCommand
    {
        object[] args;
        public CreateNewGroups(int[] SCIds, AddAction[] Actions, int[] Targets):base(SCCommandType.G_NEW)
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
        string? synthDefName;
        int SCId;
        AddAction Action;
        int TargetSCId;
        Spread<ControlParameter> param;

        public CreateNewSynth(Synth synth):base(SCCommandType.S_NEW)
        {

            if (synth.SynthDef?.name == null) { return; } 
            this.synthDefName = synth.SynthDef.name;
            
            SCId = synth.SCId;
            this.Action = AddAction.AddToTail;
            TargetSCId = synth.lastKnownParent;
            param = synth.ControlParameters.Values.ToSpread();
            //param = synth.ControlParameters.Values.Select(x => new object[] {x.Name, x.InitValue }).ToArray();
            
        }

        public override Spread<byte> GetBytes()
        {
            return OscEncoder.EncodeMessage("/s_new", new object[] { synthDefName, SCId, (int)this.Action, TargetSCId, param }.ToArray()); //param needs to ba added
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
