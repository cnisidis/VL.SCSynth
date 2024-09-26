(
SynthDef(\vvvvExample, {
  arg Freq = 440, amp = 0.1, lofreq = 1.2; //Parameters
	var sig = SinOsc.ar(Freq * SinOsc.kr(lofreq), mul: amp) ! 2;
  Out.ar(0, sig);
}).writeDefFile("C:/Users/Ignaz/Desktop");
)

(
SynthDef.new(\SinOsc, {
	var frequency = \frequency.kr(440);
	var level = \level.kr(0.1);
	var position = \position.kr(0.0);
	var out = \out.kr(0);

	var env, sig;

	sig = SinOsc.ar(frequency!2);
	sig = Balance2.ar(sig[0], sig[1], position, level);

	Out.ar(out, sig);
}).writeDefFile("C:/vl-libs/VL.SCSynth/Help/Synth Definitions/synthdefs");
)

(
SynthDef.new(\SinOsc_Env, {
	var frequency = \frequency.kr(440);
	var level = \level.kr(0.1);
	var position = \position.kr(0.0);
	var out = \out.kr(0);

	var attack = \attack.kr(0.01);
	var sustain = \sustain.kr(1.0);
	var release = \release.kr(1.0);
	var c1 = \c1.kr(1.0);
	var c2 = \c2.kr(-1.0);
	var gate = \gate.kr(1);

	var env, sig;

	env = Env.new(
		[0, 1, 1, 0],
		[attack, sustain, release],
		[c1, 0, c2]
	).ar(Done.none, gate);

	sig = SinOsc.ar(frequency!2);
	sig = sig * env;
	sig = Balance2.ar(sig[0], sig[1], position, level);

	Out.ar(out, sig);
}).writeDefFile("C:/vl-libs/VL.SCSynth/Help/Synth Definitions/synthdefs");
)

(
SynthDef.new(\Pulse, {
	var frequency = \frequency.kr(440);
	var width = \width.kr(0.5);
	var level = \level.kr(0.1);
	var position = \position.kr(0.0);
	var out = \out.kr(0);

	var env, sig;

	sig = Pulse.ar(frequency!2, width);
	sig = Balance2.ar(sig[0], sig[1], position, level);

	Out.ar(out, sig);
}).writeDefFile("C:/vl-libs/VL.SCSynth/Help/Synth Definitions/synthdefs");
)

(
SynthDef.new(\Pulse_Env, {
	var frequency = \frequency.kr(440);
	var width = \width.kr(0.5);
	var level = \level.kr(0.1);
	var position = \position.kr(0.0);
	var out = \out.kr(0);

	var attack = \attack.kr(0.01);
	var sustain = \sustain.kr(1.0);
	var release = \release.kr(1.0);
	var c1 = \c1.kr(1.0);
	var c2 = \c2.kr(-1.0);
	var gate = \gate.kr(1);

	var env, sig;

	env = Env.new(
		[0, 1, 1, 0],
		[attack, sustain, release],
		[c1, 0, c2]
	).ar(Done.none, gate);

	sig = Pulse.ar(frequency!2, width);
	sig = sig * env;
	sig = Balance2.ar(sig[0], sig[1], position, level);

	Out.ar(out, sig);
}).writeDefFile("C:/vl-libs/VL.SCSynth/Help/Synth Definitions/synthdefs");
)

(
SynthDef.new(\SinOscFB, {
	var frequency = \frequency.kr(440);
	var feedback = \feedback.kr(440);
	var level = \level.kr(0.1);
	var position = \position.kr(0.0);
	var out = \out.kr(0);

	var env, sig;

	sig = SinOscFB.ar(frequency!2, feedback);
	sig = Balance2.ar(sig[0], sig[1], position, level);

	Out.ar(out, sig);
}).writeDefFile("C:/vl-libs/VL.SCSynth/Help/Synth Definitions/synthdefs");
)

(
SynthDef.new(\SinOscFB_Env, {
	var frequency = \frequency.kr(440);
	var feedback = \feedback.kr(440);
	var level = \level.kr(0.1);
	var position = \position.kr(0.0);
	var out = \out.kr(0);

	var attack = \attack.kr(0.01);
	var sustain = \sustain.kr(1.0);
	var release = \release.kr(1.0);
	var c1 = \c1.kr(1.0);
	var c2 = \c2.kr(-1.0);
	var gate = \gate.kr(1);

	var env, sig;

	env = Env.new(
		[0, 1, 1, 0],
		[attack, sustain, release],
		[c1, 0, c2]
	).ar(Done.none, gate);

	sig = SinOscFB.ar(frequency!2, feedback);
	sig = sig * env;
	sig = Balance2.ar(sig[0], sig[1], position, level);

	Out.ar(out, sig);
}).writeDefFile("C:/vl-libs/VL.SCSynth/Help/Synth Definitions/synthdefs");
)

// Triggers

x = Synth(\name);

(
x.set(\gate, 1);
x.set(\gate, 0);
)

