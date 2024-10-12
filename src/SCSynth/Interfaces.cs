
namespace SCSynth;

public interface ISCNode
{
    

    //Generic VL Id for further handling
    public Guid Id { get; set; }

    public AddActions AddAction { get; set; }

    public IEnumerable<ISCNode> GetInputs();

    public void SetSCId(int index);

    public int GetSCId();

    public void ToString(out string Result);
    

}

 