# VL.SCSynth
SuperCollider Server-Client Lib - foundation for vvvv and SC integration

## What is this?

VL.SCSynth (SuperCollider Server)[^5] is a minimal implementation of a standalone client and server executor (scsynth)[^10] using partially [VL.IO.OSC](https://github.com/vvvv/VL.IO.OSC) lib. 

<cite>Read more about SuperCollider [here](https://supercollider.github.io/)</cite>

It supports all major OSC messages for manipulating the server itself and its Nodes, such as SynthDefs, Synths, Groups and Busses (check [this list](#scserver-options) below).

Therefore it is bundled with all the appropriate functionalities to build your own messages and queries.

SCServer comes with an integrated SCClient (for receiving scynth responses). However SCClient can be used to access an active SuperCollider server by any machine, locally or internally. 

[^10]: [scsynth](https://github.com/supercollider/supercollider/wiki/scsynth-development) – A real-time audio server

## The Basics

#### Useful Notes

- SCSynth (the node) : It is a wrapper (higher level) of SCClient. SCSynth can additionally execute a SC Server (the actual scsynth.exe) in CLI mode (no IDE / no UI) on a predefined port.

- SCClient (the node) : This is the core communication handler and keeps the connection alive between vvvv and SC. It is responsible to Send and Receive all the commands via and from SC.

#### Important Notes

Every time you connect or disconnect a synth or a group from your node tree it forces SCRoot to freeAll and rebuild it from scratch. This means that in certain scenarios (i.e. SynthDefs which are using /doneActions) things may get weird.

Specifically, a Synth with a doneAction:0 , 2 (which forces it to be free after it is consumed) will break your vvvv node tree. If you want to prevent this, till we find a fix, better use doneAction:1 and use the t_trigger (triger controls) so the Synth is being paused and reused whenever is needful.

#### Preparations

In order to use VL.SCSynth in its full potential you will need to define / create a synthdefs folder next to your VL Patch (same as we do for Shaders or whenever a VL Factory is involved).

This is absolutely needful in order to get instances (synths) of your synth definitions as nodes in your vvvv patch.

### Use SynthDefs to Instanciate Synths

<b>Check the example in your Help Browser (F1) "Custom SynthDefs"</b>

<ol>
<li>Create a folder named "synthdefs" next to your project file (root)</li>
<li>Populate the "synthdefs" folder with scsyndef files (compiled synthdefs)</li>
<li>Call SYNTHDEFS category from your nodebrowser</li>
<li>Pick and introduce a Synth (instance) based on the provided synthdef files</li>
</ol>


## Super Collider
### How to save a SynthDef File

In order to use a synthdef in vvvv context, first you have to compile and store it. Grace to the sclang.exe compilations is happenning under the hood, saving a file needs just a method to write/store your newly made synth definition on your disk.


### WriteDefFile

There are two options atm, both are using the .writeDefFile method.  The most straight forward is to use the it without any arguments, this will compile and save (write on your disk) a .scsyndef file at your default synthdefs folder which can be found under the file menu (Open user support directory).

<code>
(
SynthDef(\ASynth, { |out=0, freq=440, amp=0.1,  t_gate=1|
    var sig = SinOsc.ar(freq) * EnvGen.kr(Env.perc,t_gate, doneAction: 0);
    Out.ar(out, sig * amp);
}).writeDefFile(); 
)
</code>


### Copy&Paste your synthdef

![Open user support directory image](img/SC_FileMenu.png)

![synthdef folder at user support directory image](img/user_support_dir.png)




### Save it directly 
The other way is to predefine a custom folder (probably your projects' synthdef folder) before you execute the method by passing a dir argument.
<code>
.writeDefFile("C:/yourproject/synthdefs", true);
</code>

**important** 
- the path is unix like (fwd slashes)
- prefer to add true on overwrite to avoid implications.


## Troubleshooting

- Server 'localhost' exited with exit code -1073741819. : Most probably there is something in the default synthdef folder which is preventing scsynth.exe to boot properly. Discard all the files (or the last added once) and try to execute scsynth.exe again.

Another reason can be the port you are trying to reserve, keep in mind that the default port is ... and if the IDE (sclang) is up and running it will be bind on this one.



# Sponsoring

The development of this library was mainly sponsored by:

 - [<b>3e8.studio</b>](https://www.3e8.studio) 
<!-- ![](/img/3e8_logo_dark.png) -->







