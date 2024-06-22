
## SCServer Options

So far only few options are being supported (the plan is to add all the available options). The idea was to keep it simple and minimal by adding only essential things.

- So you can choose if Server will use either the UDP <s>or the TCP</s> Protocol.

- Define Verbosity level.

- Bind a certain IP address.

- Bind a port for vice-versa communication.


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


## Some useful notes

SuperCollider Server replies on the same port it listens to, since TCP is not supported yet in this version, you can get the OSC responses (via UDP) by exposing the hidden "Data" output pin on the OSCClient.

![Expose Data Pin](/img/OSCClient-Data-pin_00.png)

<cite>This functionality is already managed internally in order to get the status of the server, it's versions, and other useful information.</cite>

Both SCServer and SCClient are inheriting their methods from ISCSynth. This name was picked in order to signify that both are implementing and sharing methods to communicate directly with any active scsynth.

In example, once you started scsynth (either via SCServer or directly or by using the SC IDE), you can access it by introducing a SCClient on the VL side, respecting the IP Address and the binded port.

[^5]: See [Client vs Server Documentation](https://doc.sccode.org/Guides/ClientVsServer.html)