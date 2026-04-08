
using SCSynth.SCNodes;
using System.Text;
using VL.Lang;
using VL.Lib.Collections;
using static VL.Core.Import.ProcessNodeFactory;

namespace SCSynth.Utils
{
    
    public static class Globals
    {
        /// <summary>
        /// Default Server Port
        /// </summary>
        public const int SCSynthPort = 57110;
        /// <summary>
        /// Default Language Port
        /// </summary>
        public const int SCLangPort = 57110;

        public static readonly string[] CommandsLexicon = {

            "/none", //0
            "/notify", //1
            "/status", //2
            "/quit", //3
            "/cmd", //4
            "/d_recv", //5
            "/d_load", //6
            "/d_loadDir", //7
            "/d_freeAll", //8
            "/s_new", //9
            "/n_trace", //10
            "/n_free", //11
            "/n_run", //12
            "/n_cmd", //13
            "/n_map", //14
            "/n_set", //15
            "/n_setn", //16
            "/n_fill", //17
            "/n_before", //18
            "/n_after", //19
            "/u_cmd", //20
            "/g_new", //21
            "/g_head", //22
            "/g_tail", //23
            "/g_freeAll", //24
            "/c_set", //25
            "/c_setn", //26
            "/c_fill", //27
            "/b_alloc", //28
            "/b_allocRead", //29
            "/b_read", //30
            "/b_write", //31
            "/b_free", //32
            "/b_close", //33
            "/b_zero", //34
            "/b_set", //35
            "/b_setn", //36
            "/b_fill", //37
            "/b_gen", //38
            "/dumpOSC", //39
            "/c_get", //40
            "/c_getn", //41
            "/b_get", //42
            "/b_getn", //43
            "/s_get", //44
            "/s_getn", //45
            "/n_query", //46
            "/b_query", //47
            "/n_mapn", //48
            "/s_noid", //49
            "/g_deepFree", //50
            "/clearSched", //51
            "/sync", //52
            "/d_free", //53
            "/b_allocReadChannel", //54
            "/b_readChannel", //55
            "/g_dumpTree", //56
            "/g_queryTree", //57
            "/error", //58
            "/s_newargs", //59
            "/n_mapa", //60
            "/n_mapan", //61
            "/n_order", //62
            "/p_new", //63
            "/version", //64
            "/rtMemoryStatus", //65
            "/b_setSampleRate", //66
            "/number_of_commands" //67


        };

        public static readonly string[] CommandsResponsesLexicon =
        {
            "/status.reply",
            "/synced",
            "/version.reply",
            "/rtMemoryStatus.reply",



        };

    }

    public static class Utils
    {
        
        public static Spread<SCNode> GetAllChildren(SCNode Node, bool IncludeSelf=false)
        {
            List<SCNode> Nodes = new List<SCNode>();
            
            if(Node == null) return Nodes.ToSpread();

            Nodes.Add(Node);

            var inputs = Node.Inputs;

            if (inputs.Any())
            {
                foreach (var input in inputs)
                {
                    if (input != null)
                        Nodes.AddRange(GetAllChildren(input));
                }
            }

            return Nodes.ToSpread();
        }
    }

    
}
