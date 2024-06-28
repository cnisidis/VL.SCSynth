# VL.SCSynth
SuperCollider Server-Client Lib - foundation for vvvv and SC integration

## What is this?

VL.SCSynth (SuperCollider Server)[^5] is a minimal implementation of a standalone server executor (scsynth)[^10] using the [VL.IO.OSC](https://github.com/vvvv/VL.IO.OSC) lib. 

<cite>Read more about SuperCollider [here](https://supercollider.github.io/)</cite>

It supports all major OSC messages for manipulating the server itself and its nodes, such as SynthDefs, Synths, Groups and Busses (check [this list](#scserver-options) below).

Therefore it is bundled with all the appropriate functionalities to build your own messages and queries.

SCServer comes with an integrated SCClient (for receiving scynth responses). However SCClient can be used to access an active SuperCollider server by any machine, locally or internally. 

[^10]: [scsynth](https://github.com/supercollider/supercollider/wiki/scsynth-development) – A real-time audio server

## Use SynthDefs to Instanciate Synths

<b>Check the example in your Help Browser (F1) "Custom SynthDefs"</b>

<ol>
<li>Create a folder named "synthdefs" next to your project file (root)</li>
<li>Populate the "synthdefs" folder with scsyndef files (compiled synthdefs)</li>
<li>Add VL.SCSynth to your patch project</li>
<li>Call SYNTHDEFS category from your nodebrowser</li>
<li>Pick and introduce a Synth (instance) based on the provided synthdef files</li>
</ol>

# Sponsoring

The development of this library was mainly sponsored by:

 - [<b>3e8.studio</b>](https://www.3e8.studio) 
<!-- ![](/img/3e8_logo_dark.png) -->







