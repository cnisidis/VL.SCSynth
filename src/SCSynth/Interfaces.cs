
namespace SCSynth;

public interface ISCNode
{
    //Super Collider internal ID
    public int scId { get; set; }

    //Generic VL Id for further handling
    public Guid Id { get; set; }

    public AddActions AddAction { get; set; }

    public List<ISCNode> GetInputs();

    public void ToString(out string Result)
    {
        Result = "Id:" + scId.ToString();
    }

}

 