///For examples, see:
///https://thegraybook.vvvv.org/reference/extending/writing-nodes.html#examples

namespace VL.SCServer;

public enum Protocol
{
    UDP =0,
    TCP =1
}

public enum Verbosity
{
    Normal =0,
    SuppressMessages =-1,
    SuppressAll = -2
}