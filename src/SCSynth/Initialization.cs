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

        private IResourceProvider<SCManager>? _managerProvider;
        public override void Configure(AppHost appHost)
        {
             //Register the engine provider so patches can access it
            
            if (_managerProvider is null)
            {
                _managerProvider = ResourceProvider.NewPooledSystemWide("VL.SCSynth", _ => new SCManager());
            }
            appHost.Services.RegisterService(_managerProvider);
            

            appHost.RegisterNodeFactory("VL.SCSynth-Factory", (directory, nodeFactory) =>
            {
                var invalidated = NodeBuilding.WatchDir(directory)
                    .Where(e => (e.ChangeType == WatcherChangeTypes.Created || e.ChangeType == WatcherChangeTypes.Deleted || e.ChangeType == WatcherChangeTypes.Renamed || e.ChangeType == WatcherChangeTypes.All) && e.Name == synthDefsSubdir);

                var builder = ImmutableArray.CreateBuilder<IVLNodeDescription>();
                var synthDefsDir = Path.Combine(directory, synthDefsSubdir);

                Console.WriteLine("Factory initialized");

                //Create an scmanager on the fly, to track synthdefs and changes per patch here
                //For Global Manager and its usage see the notes bellow.

                SCManager manager = new SCManager();

                if (Directory.Exists(synthDefsDir))
                {
                    Console.WriteLine("Directory:", directory);
                    Console.WriteLine("SynthDefs Directory: {0}", synthDefsDir);
                    Console.WriteLine("SynthDefs Directory: {0}", synthDefsSubdir);
                    // Additionaly watch out for new/deleted/renamed files
                    invalidated = invalidated.Merge(
                    NodeBuilding.WatchDir(synthDefsDir).Where(e => e.ChangeType == WatcherChangeTypes.Changed || e.ChangeType == WatcherChangeTypes.Deleted || e.ChangeType == WatcherChangeTypes.Created || e.ChangeType == WatcherChangeTypes.Renamed || e.ChangeType == WatcherChangeTypes.All));
                    
                    string[] compiledSynthDefs = Directory.GetFiles(synthDefsDir, "*.scsyndef");
                    if (compiledSynthDefs.Length != 0)
                    {
                        //IMPORTANT:
                        //USE SC Manager (Globally) to track changes and update files from standard folders, like the supercollider user library folder
                        //_managerProvider.GetHandle().Resource.ClearAll();
                        //_managerProvider.GetHandle().Resource.LoadFiles(compiledSynthDefs);

                        manager.LoadFiles(compiledSynthDefs);
                        manager.GetSynthDefs().ForEach( synthDef => {
                            var synthDesc = new SynthDescritpion(nodeFactory, null, synthDef);
                            builder.Add(synthDesc);
                        } );

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
