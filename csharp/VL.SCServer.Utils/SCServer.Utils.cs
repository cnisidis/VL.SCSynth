
using System.ComponentModel;

using System;
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


public static class Commands
{
    public enum ServerCommand
    {
        BUFFER_ALLOCATE = 0, //"/b_alloc";
        BUFFER_ALLOCATE_READ = 1, //"/b_allocRead";
        BUFFER_ALLOCATE_READ_CHANNEL = 2, // "/b_allocReadChannel";
        BUFFER_CLOSE = 3, //"/b_close"
        BUFFER_FILL = 4,// "/b_fill";
        BUFFER_FREE = 5,// "/b_free";
        BUFFER_GENERATE = 6,// "/b_gen";
        BUFFER_GET = 7,//"/b_get";
        BUFFER_GET_CONTIGUOUS = 8,// "/b_getn";
        BUFFER_QUERY = 9,// "/b_query";
        BUFFER_READ = 10,// "/b_read";
        BUFFER_READ_CHANNEL = 11,// "/b_readChannel";
        BUFFER_SET = 12,// "/b_set";
        BUFFER_SET_CONTIGUOUS = 13,//"/b_setn";
        BUFFER_WRITE = 14,// "/b_write";
        BUFFER_ZERO = 15,// "/b_zero";
        CLEAR_SCHEDULE = 16,// "/clearSched";
        COMMAND = 17,// "/cmd";
        CONTROL_BUS_FILL = 18,// "/c_fill";
        CONTROL_BUS_GET = 19,// "/c_get";
        CONTROL_BUS_GET_CONTIGUOUS = 20,// "/c_getn";
        CONTROL_BUS_SET = 21,// "/c_set";
        CONTROL_BUS_SET_CONTIGUOUS = 22,//"/c_setn";
        DUMP_OSC = 23,//"/dumpOSC";
        ERROR = 24, //"/error";
        GROUP_DEEP_FREE = 25, //"/g_deepFree";
        GROUP_DUMP_TREE = 26, //"/g_dumpTree";
        GROUP_FREE_ALL = 27, //"/g_freeAll";
        GROUP_HEAD = 28, //"/g_head";
        GROUP_NEW = 29, //"/g_new";
        GROUP_QUERY_TREE = 30, //"/g_queryTree";
        GROUP_TAIL = 31, //"/g_tail";
        NODE_AFTER = 32, //"/n_after";
        NODE_BEFORE = 33, //"/n_before";
                          //NODE_COMMAND = None
        NODE_FILL = 34, //"/n_fill";
        NODE_FREE = 35, //"/n_free";
        NODE_MAP_TO_AUDIO_BUS = 36, //"/n_mapa";
        NODE_MAP_TO_AUDIO_BUS_CONTIGUOUS = 37, //"/n_mapan";
        NODE_MAP_TO_CONTROL_BUS = 38,// "/n_map";
        NODE_MAP_TO_CONTROL_BUS_CONTIGUOUS = 39, //"/n_mapn";
        NODE_ORDER = 40, //"/n_order";
        NODE_QUERY = 41, //"/n_query";
        NODE_RUN = 42, //"/n_run";
        NODE_SET = 43, //"/n_set";
        NODE_SET_CONTIGUOUS = 44, //"/n_setn";
        NODE_TRACE = 45, //"/n_trace";
        NOTHING = 46, //null;
        NOTIFY = 47, //"/notify";
        PARALLEL_GROUP_NEW = 48, //"/p_new";
        QUIT = 49, //"/quit";
        STATUS = 50, //"/status";
        SYNC = 51, //"/sync";
        SYNTHDEF_FREE = 52, //"/d_free";
        SYNTHDEF_FREE_ALL = 53, //"/d_freeAll";
        SYNTHDEF_LOAD = 54, //"/d_load";
        SYNTHDEF_LOAD_DIR = 55, //"/d_loadDir";
        SYNTHDEF_RECEIVE = 56, //"/d_recv";
        SYNTH_GET = 57, //"/s_get";
        SYNTH_GET_CONTIGUOUS = 58,  //"/s_getn";
        SYNTH_NEW = 59, //"/s_new";
                        //SYNTH_NEWARGS = None
        SYNTH_NOID = 60, //"/s_noid";
        UGEN_COMMAND = 61, //"/u_cmd";
        VERSION = 62, //"/version";
    }
    
    public static Dictionary<ServerCommand, string> AvailableCommands = new Dictionary<ServerCommand, string> {
            { ServerCommand.BUFFER_ALLOCATE, "/b_alloc" },
            { ServerCommand.BUFFER_ALLOCATE_READ, "/b_allocRead" },
            { ServerCommand.BUFFER_ALLOCATE_READ_CHANNEL, "/b_allocReadChannel" },
            { ServerCommand.BUFFER_CLOSE, "/b_close" },
            { ServerCommand.BUFFER_FILL, "/b_fill" },
            { ServerCommand.BUFFER_FREE, "/b_free" },
            { ServerCommand.BUFFER_GENERATE, "/b_gen" },
            { ServerCommand.BUFFER_GET, "/b_get" },
            { ServerCommand.BUFFER_GET_CONTIGUOUS, "/b_getn" },
            { ServerCommand.BUFFER_QUERY, "/b_query" },
            { ServerCommand.BUFFER_READ, "/b_read" },
            { ServerCommand.BUFFER_READ_CHANNEL, "/b_readChannel" },
            { ServerCommand.BUFFER_SET, "/b_set" },
            { ServerCommand.BUFFER_SET_CONTIGUOUS, "/b_setn" },
            { ServerCommand.BUFFER_WRITE, "/b_write" },

            { ServerCommand.BUFFER_ZERO, "/b_zero" },
            { ServerCommand.CLEAR_SCHEDULE, "/clearSched" },
            { ServerCommand.COMMAND, "/cmd" },
            { ServerCommand.CONTROL_BUS_FILL, "/c_fill" },
            { ServerCommand.CONTROL_BUS_GET, "/c_get" },
            { ServerCommand.CONTROL_BUS_GET_CONTIGUOUS, "/c_getn" },
            { ServerCommand.CONTROL_BUS_SET, "/c_set" },
            { ServerCommand.CONTROL_BUS_SET_CONTIGUOUS, "/c_setn" },
            { ServerCommand.DUMP_OSC, "/dumpOSC" },
            { ServerCommand.ERROR, "/error" },
            { ServerCommand.GROUP_DEEP_FREE, "/g_deepFree" },

            { ServerCommand.GROUP_DUMP_TREE, "/g_dumpTree" },
            { ServerCommand.GROUP_FREE_ALL, "/g_freeAll" },
            { ServerCommand.GROUP_HEAD, "/g_head" },
            { ServerCommand.GROUP_NEW, "/g_new" },
            { ServerCommand.GROUP_QUERY_TREE, "/g_queryTree" },
            { ServerCommand.GROUP_TAIL, "/g_tail" },
            { ServerCommand.NODE_AFTER, "/n_after" },
            { ServerCommand.NODE_BEFORE, "/n_before" },

            //NODE_COMMAND = None
            { ServerCommand.NODE_FILL, "/n_fill" },
            { ServerCommand.NODE_FREE, "/n_free" },
            { ServerCommand.NODE_MAP_TO_AUDIO_BUS, "/n_mapa" },

            { ServerCommand.NODE_MAP_TO_AUDIO_BUS_CONTIGUOUS, "/n_mapan" },
            { ServerCommand.NODE_MAP_TO_CONTROL_BUS, "/n_map" },
            { ServerCommand.NODE_MAP_TO_CONTROL_BUS_CONTIGUOUS, "/n_mapn" },
            { ServerCommand.NODE_ORDER, "/n_order" },
            { ServerCommand.NODE_QUERY, "/n_query" },
            { ServerCommand.NODE_RUN, "/n_run" },
            { ServerCommand.NODE_SET, "/n_set" },
            { ServerCommand.NODE_SET_CONTIGUOUS, "/n_setn" },
            { ServerCommand.NODE_TRACE, "/n_trace" },

            { ServerCommand.NOTHING, "" },
            { ServerCommand.NOTIFY, "/notify" },
            { ServerCommand.PARALLEL_GROUP_NEW, "/p_new" },
            { ServerCommand.QUIT, "/quit" },
            { ServerCommand.STATUS, "/status" },
            { ServerCommand.SYNC, "/sync" },
            { ServerCommand.SYNTHDEF_FREE, "/d_free" },
            { ServerCommand.SYNTHDEF_FREE_ALL, "/d_freeAll" },

            { ServerCommand.SYNTHDEF_LOAD, "/d_load" },
            { ServerCommand.SYNTHDEF_LOAD_DIR, "/d_loadDir" },
            { ServerCommand.SYNTHDEF_RECEIVE, "/d_recv" },
            { ServerCommand.SYNTH_GET, "/s_get" },
            { ServerCommand.SYNTH_GET_CONTIGUOUS, "/s_getn" },
            { ServerCommand.SYNTH_NEW, "/s_new" },
            { ServerCommand.SYNTH_NOID, "/s_noid" },
            { ServerCommand.UGEN_COMMAND, "/u_cmd" },
            { ServerCommand.VERSION, "/version" },



        };

}


[DefaultValue(DumpOSCOptions.PrintParsedContent)]
public enum DumpOSCOptions
{
    Off =0,
    PrintParsedContent = (1),
    PrintContentHex = 2,
    PrintAll =3
}

public enum NotifyOptions
{
    Off = 0,
    Receive = (1),
    
}

public enum ErrorOptions
{
    Off =0,
    On =1,
    SuppressOffLocaly = -1,
    SuppressOnLocaly = -2
}

public enum RunNodeFlags
{
    
    DoNotExecute = 0,
    Execute = 1,
}

public enum AddActions
{
    AddToHead =0,
    AddToTail =1,
    AddBefore =2,
    AddAfter =3,
    Replace =4
}

public enum NodeType
{
    Synth =0,
    Group =1
}