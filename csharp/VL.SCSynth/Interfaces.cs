
namespace VL.SCSynth;

public interface ISCNode
{
    public int scId { get; set; }

    public Guid Id { get; set; }

    public AddActions AddAction { get; set; }

    public List<ISCNode> GetInputs();

}

 