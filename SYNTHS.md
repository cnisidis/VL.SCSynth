## Variants 

Variants in SC context are SynthDefs providing Synth instances presets.
For further reading you may check this [link](http://doc.sccode.org/Classes/SynthDef.html#Variants) from the official documentation.

Since it is yet matter of design, variants will come as separrate nodes with the form of "variant.alpha", "variant.gamma" and so on.

Another approach would be to have an input pin of a dynamic enum, and switch the presets on the fly.



## Parameters

A <i>class</i> type object holding four fields and one method:

- Name (type of String) derives from sytnthdef
- Value (type of Float32)
- InitValue (type of Float32) derives from sytnthdef
- Index (type of Integer32)

## Root

A Root is representing the 0 Group (the base with additional functionalites) which is usually assigned to the Server by default in superCollider. 

Therefore, all the synths and the groups you may create should have a unique ID and always greater than 0.

In example:
//MISSING IMAGE!

Ordered of groups and synths will be reassigned every time you rebuild the NodeTree.
