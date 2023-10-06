(
SynthDef(\vvvvExample, {
  arg freq = 440, amp = 0.1, lofreq = 1.2; //Parameters
	var sig = SinOsc.ar(freq * SinOsc.kr(lofreq), mul: amp) ! 2;
  Out.ar(0, sig);
}).writeDefFile;
)
