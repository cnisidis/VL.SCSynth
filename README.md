# VL.SCSynth
SuperCollider Server-Client Lib - foundation for vvvv and SC integration


## What is this?

VL.SCSynth (SuperCollider Server)[^5] is a minimal implementation of a standalone server executor (scsynth)[^10] using the [VL.IO.OSC](https://github.com/vvvv/VL.IO.OSC) lib. 

<cite>Read more about SuperCollider [here](https://supercollider.github.io/)</cite>

It supports all major OSC messages for manipulating the server itself and its nodes, such as SynthDefs, Synths, Groups and Busses (check [this list](#scserver-options) below).

Therefore it is bundled with all the appropriate functionalities to build your own messages and queries.

SCServer comes with an integrated SCClient (for receiving scynth responses). However SCClient can be used to access an active SuperCollider server by any machine, locally or internally. 



## Use SynthDefs to Instanciate Synths

![Expose Data Pin](/img/SCSynth-Factory.png)

<ol>
<li>Create a folder named "synthdefs" next to your project file (root)</li>
<li>Populate the "synthdefs" folder with scsyndef files (compiled synthdefs)</li>
<li>Add VL.SCSynth to your patch project</li>
<li>Call SYNTHDEFS category from your nodebrowser</li>
<li>Pick and introduce a Synth (instance) based on the provided synthdef files</li>
</ol>



## Before You Go
SCServer is still in progress, so changes will be often and may cause breaking changes. 

One important thing (which still needs some polishing) is that you have to set the SuperCollider installation folder manually every time you introduce the SCServer node. 

[^5]: See [Client vs Server Documentation](https://doc.sccode.org/Guides/ClientVsServer.html)

[^10]: [scsynth](https://github.com/supercollider/supercollider/wiki/scsynth-development) – A real-time audio server



# Spnsoring

Development of this library was mainly sponsored by:

 - [<b>3e8.studio</b>](https://www.3e8.studio) 
<!-- ![](/img/3e8_logo_dark.png) -->