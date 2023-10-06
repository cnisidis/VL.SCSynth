# VL.SCServer
Super Collider Server Lib - foundation for vvvv and SC integration


## What is this?

VL.SCServer (Super Collider Server) is a minimal implementation of a standalone server executor (scsynth). 

It supports all major OSC messages for manipulating the server itself and its nodes, such as SynthDefs, Synths, Groups and Busses.

Therefore it is bundled with all the appropriate functionalities to build your own messages and queries.

## Some usefull notes

SuperCollider Server replies on the same port it listens to, since TCP is not supported yet in this version, you can get the OSC responses (via UDP) by exposing the hidden "Data" output pin on the OSCClient.

<cite>This is already managed internaly in order to get Server's Stauts, Verions and other useful information.</cite>



## Server Options

So far only few options are being supported (the plan is to add all the available options). The idea was to keep it simple and minimal by adding olny essential things.

- So you can choose if Server will use either the UDP <s>or the TCP</s> Protocol.

- Define Verbosity level.

- Bind a certain IP address.


<i>for more info regarding the scsynth arguments see:</i>

[scsynth_main.cpp on github](https://github.com/supercollider/supercollider/blob/develop/server/scsynth/scsynth_main.cpp)





## Server Commands & Messages (WIP)
Implemented according to [Server Command Reference](https://doc.sccode.org/Reference/Server-Command-Reference.html)


### Basic
|Node Name (in gamma)|OSC Address|Notes|
|---------|-----------|-----|
|ServerStatus|/status|(internal) Returns an object of type: ServerStatus|
|ServerVersion|/version|(internal) Returns an object of type: ServerVersion|
|DumpOSC|/dumpOSC|Console Log|
|Notify|/notify|Console Log|
|Error|/error|Console Log|


### SynthDefs
|Node Name (in gamma)|OSC Address|Notes|
|---------|-----------|-----|
|ReceiveSynthDef|/d_recv|Returns /done message|
|LoadSynthDef|/d_load|Returns /done message|
|LoadSynthDefDirectory|/d_loadDir|Returns /done message|
|DeleteSynthDef|/d_free|-|


### Nodes (Synth, Group, Bus)
|Node Name (in gamma)|OSC Address|Notes|
|---------|-----------|-----|
|DeleteNode|/n_free|-|
|RunNode|/n_run|-|
|SetNodeValue<T, U>|/n_set|Using SynthParameter Object|


## SynthParameter

A <i>Record<U,T></i> type object holding two fields:

- Name or Index can be either type of String or Integer32
- Value can be an Integer32, Float32 or String


## Before You Go
SCServer is still in progress, so changes will be often and may cause breaking changes. 

One important thing (which still needs some polishing) is that you have to set the SuperCollider installation folder manually every time you introduce the SCServer node. 
