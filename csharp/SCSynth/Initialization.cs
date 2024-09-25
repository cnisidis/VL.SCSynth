using System.Collections.Immutable;
using System.Reactive.Linq;
using VL.Core;
using VL.Core.CompilerServices;
using SCSynth.Factory;
using VL.Lib.Basics.Resources;

// Tell VL where to find our initializer
[assembly: AssemblyInitializer(typeof(SCSynth.Initialization))]

namespace SCSynth
{

    public class Initialization : AssemblyInitializer<Initialization>
    {

        const string synthDefsSubdir = "synthdefs";

        //private IResourceProvider<GlobalSCEngine>? _engineProvider;
        public override void Configure(AppHost appHost)
        {
            // Register the engine provider so patches can access it
            /*
            if (_engineProvider is null)
            {
                _engineProvider = ResourceProvider.NewPooledSystemWide("VL.SCSynth", _ => new GlobalSCEngine());
            }
            appHost.Services.RegisterService(_engineProvider);
            */

            appHost.RegisterNodeFactory("VL.SCSynth-Factory", (directory, nodeFactory) =>
            {
                var invalidated = NodeBuilding.WatchDir(directory)
                    .Where(e => (e.ChangeType == WatcherChangeTypes.Created || e.ChangeType == WatcherChangeTypes.Deleted || e.ChangeType == WatcherChangeTypes.Renamed || e.ChangeType == WatcherChangeTypes.All) && e.Name == synthDefsSubdir);

                var builder = ImmutableArray.CreateBuilder<IVLNodeDescription>();
                var synthDefsDir = Path.Combine(directory, synthDefsSubdir);

                Console.WriteLine("Factory initialized");



                if (Directory.Exists(synthDefsDir))
                {
                    Console.WriteLine("Directory:", directory);
                    Console.WriteLine("SynthDefs Directory: {0}", synthDefsDir);
                    Console.WriteLine("SynthDefs Directory: {0}", synthDefsSubdir);
                    // Additionaly watch out for new/deleted/renamed files
                    invalidated = invalidated.Merge(
                    NodeBuilding.WatchDir(synthDefsDir).Where(e => e.ChangeType == WatcherChangeTypes.Changed || e.ChangeType == WatcherChangeTypes.Deleted || e.ChangeType == WatcherChangeTypes.Created || e.ChangeType == WatcherChangeTypes.Renamed || e.ChangeType == WatcherChangeTypes.All));
                    //.Where(e => e.ChangeType == WatcherChangeTypes.All));
                    // || string.Equals(e.Name, runwayLocal, StringComparison.OrdinalIgnoreCase)

                    // Read files in folder decompile and store the data
                    //var ext = new List<string> { "scsyndef" };
                    //var compiledSynthDefs = Directory.EnumerateFiles(synthDefsDir, "*.*", SearchOption.TopDirectoryOnly).Where(s => ext.Contains(Path.GetExtension(s).TrimStart('.').ToLowerInvariant()));
                    string[] compiledSynthDefs = Directory.GetFiles(synthDefsDir, "*.scsyndef");
                    if (compiledSynthDefs.Length != 0)
                    {
                        Console.WriteLine("Decompile available synthdefs");
                        foreach (var compiledSynthDef in compiledSynthDefs)
                        {
                            Console.WriteLine(compiledSynthDef);
                            var decompiledSynthdefs = Factory.Decompiler.DecompileSynthDefsFromFile(compiledSynthDef);
                            foreach (var synthDef in decompiledSynthdefs)
                            {
                                Console.WriteLine(synthDef.Key);
                                builder.Add(new SCSynthDescritpion(nodeFactory, synthDef.Key, synthDef.Value, compiledSynthDef));
                                Console.WriteLine("Synthdef: {0} was added", synthDef.Key);
                            }
                            // builder.Add(new ModelDescription(nodeFactory, infos[0], infos[1], infos[2])); 
                        }
                    }
                    else
                    {
                        Console.WriteLine("No synthdefs found in {0}", synthDefsDir);
                    }

                }
                else
                {
                    Console.WriteLine(synthDefsDir, "Folder {0} Is Empty");
                }
                return new(builder.ToImmutable(), invalidated);
            });
        }
    
        
    }
 
}
