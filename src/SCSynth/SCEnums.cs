
using System.ComponentModel;


namespace SCSynth;
/// <summary>
/// 
/// </summary>
public enum Protocol
{
    
    UDP =0,
    
    TCP =1
}

public enum Verbosity
{
    /// <summary>
    /// Normal Verbosity
    /// </summary>
    Normal =0,
    /// <summary>
    /// Suppress Messages
    /// </summary>
    SuppressMessages =-1,
    /// <summary>
    /// Suppress All
    /// </summary>
    SuppressAll = -2
}



[DefaultValue(DumpOSCType.PrintParsedContent)]
public enum DumpOSCType
{
    Off =0,
    PrintParsedContent = (1),
    PrintContentHex = 2,
    PrintAll =3
}

public enum NotifyMode
{
    Off = 0,
    Receive = (1),
    
}

public enum ErrorMode
{
    Off =0,
    On =1,
    SuppressOffLocaly = -1,
    SuppressOnLocaly = -2
}

public enum RunNodeMode
{
    
    DoNotExecute = 0,
    Execute = 1,
}

public enum AddAction
{
    AddToHead =0,
    AddToTail =1,
    AddBefore =2,
    AddAfter =3,
    Replace =4
}

public enum NodePosition
{
    NONE=0,
    HEAD,
    TAIL,
    AFTER,
    BEFORE
}


