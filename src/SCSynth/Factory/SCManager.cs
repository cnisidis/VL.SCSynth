using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using VL.Lib.Collections;

namespace SCSynth.Factory
{
    public class SCManager:IDisposable
    {
        public class SynthDef
        {
            public string name;
            int num_constants;
            List<float> _constants;
            int num_parameters;
            List<float> _parameters_init_val;
            int num_parameters_names;
            List<ParamName> _parameter_names;
            int num_ugens;
            List<UgenSpec> _ugen_specs;
            int num_variants;
            List<object> _variants;


            byte[] raw;
            int length;


            public SynthDef()
            {
                _constants = new List<float>();
                _parameters_init_val = new List<float>();
                _parameter_names = new List<ParamName>();
                _ugen_specs = new List<UgenSpec>();
            }

            public void Decompile(byte[] bytes, ref int index)
            {
                int nameLength = 0;

                //SynthDef name
                this.name = Decompiler.FromPString(bytes.Skip(index), out nameLength);
                index += nameLength;
                //Number of Constants
                this.num_constants = BitConverter.ToInt32(bytes.Skip(index).Take(4).Reverse().ToArray());
                index += 4;


                for (int j = 0; j < this.num_constants; j++)
                {
                    _constants.Add(BitConverter.ToSingle(bytes.Skip(index).Take(4).Reverse().ToArray()));
                    index += 4;
                }
                //Number of Parameters
                this.num_parameters = BitConverter.ToInt32(bytes.Skip(index).Take(4).Reverse().ToArray());
                index += 4;

                //Parameter Initial Values
                for (int j = 0; j < this.num_parameters; j++)
                {
                    float parameterValue = BitConverter.ToSingle(bytes.Skip(index).Take(4).Reverse().ToArray());
                    this._parameters_init_val.Add(parameterValue);
                    index += 4;

                }
                //Number of Parameters Names
                this.num_parameters_names = BitConverter.ToInt32(bytes.Skip(index).Take(4).Reverse().ToArray());
                index += 4;

                for (int j = 0; j < this.num_parameters_names; j++)
                {
                    var pName = new ParamName();
                    pName.Decompile(bytes.Skip(index).ToArray(), ref index);
                    this._parameter_names.Add(pName);

                }

                this.num_ugens = BitConverter.ToInt32(bytes.Skip(index).Take(4).Reverse().ToArray());
                index += 4;

                for (int j = 0; j < this.num_ugens; j++)
                {
                    var ugenSpec = new UgenSpec();
                    ugenSpec.Decompile(bytes.Skip(index).ToArray(), ref index);
                    this._ugen_specs.Add(ugenSpec);
                }

                this.length = index;
                this.raw = bytes.Take(this.length).ToArray();
            }

            public byte[] GetBytes()
            {
                return this.raw;
            }

            public int Length()
            {
                return this.length;
            }

            public void GetConstants(out List<float> Constants)
            {
                Constants = this._constants;
            }

            public void GetParameters(out List<ParamName> ParametersNames, out List<float> ParametersInitValues)
            {
                ParametersNames = this._parameter_names;
                ParametersInitValues = this._parameters_init_val;
            }

            public void GetUGensSpecs(out List<UgenSpec> UGenSpecs)
            {
                UGenSpecs = this._ugen_specs;
            }

            public void GetStats(out int ConstantsCount, out int ParametersCount, out int UGensCount, out int VariantsCount)
            {
                ConstantsCount = this.num_constants;
                ParametersCount = this.num_parameters;
                UGensCount = this.num_ugens;
                VariantsCount = this.num_variants;
            }

            public void Split(out Spread<float> Constants, out Spread<ParamName> ParameterNames, out Spread<UgenSpec> UGenSpecs)
            {
                Constants = this._constants.ToSpread();
                ParameterNames = this._parameter_names.ToSpread();
                UGenSpecs = this._ugen_specs.ToSpread();
            }
        }

        public class UgenSpec
        {
            public string name;
            
            int calcRate;
            int num_inputs;
            int num_outputs;
            int special_index;
            List<InputSpec> inputs_spec;
            List<OutputSpec> outputs_spec;

            public UgenSpec()
            {
                inputs_spec = new List<InputSpec>();
                outputs_spec = new List<OutputSpec>();
            }

            public void Decompile(byte[] bytes, ref int index)
            {
                var length = 0;
                this.name = Decompiler.FromPString(bytes, out length);
                index += length;

                this.calcRate = (int)bytes.Skip(length).ToArray()[0];
                index += 1;
                length += 1;
                this.num_inputs = BitConverter.ToInt32(bytes.Skip(length).Take(4).Reverse().ToArray());
                index += 4;
                length += 4;
                this.num_outputs = BitConverter.ToInt32(bytes.Skip(length).Take(4).Reverse().ToArray());
                index += 4;
                length += 4;
                this.special_index = BitConverter.ToInt16(bytes.Skip(length).Take(2).Reverse().ToArray());
                index += 2;
                length += 2;

                for (int i = 0; i < num_inputs; i++)
                {
                    var inputSpec = new InputSpec();
                    inputSpec.Index = BitConverter.ToInt32(bytes.Skip(length).Take(4).Reverse().ToArray());
                    index += 4;
                    length += 4;
                    inputSpec.ConstantIndex = BitConverter.ToInt32(bytes.Skip(length).Take(4).Reverse().ToArray());
                    inputs_spec.Add(inputSpec);
                    index += 4;
                    length += 4;
                }
                for (int i = 0; i < num_outputs; i++)
                {
                    var outputSpec = new OutputSpec();
                    outputSpec.calcRate =(int)bytes[length];
                    outputs_spec.Add(outputSpec);
                    index += 1;
                    length += 1;
                }

            }

            public void Split(out string Name, out int CRate, out int SpecialIndex, out int InputsCount, out Spread<InputSpec> Inputs, out int OutputsCount, out Spread<OutputSpec> Outputs)
            {
                Name = this.name;
                CRate = this.calcRate;
                SpecialIndex = this.special_index;
                InputsCount = this.num_inputs;
                Inputs = this.inputs_spec.ToSpread();
                OutputsCount = this.num_outputs;
                Outputs = this.outputs_spec.ToSpread();
            }

            public Spread<InputSpec> Inputs() => inputs_spec.ToSpread();
            public Spread<OutputSpec> Otuputs() => outputs_spec.ToSpread();
        }

        public class InputSpec
        {
            public int Index; //-1 is a constant
            /// <summary>
            /// Output or Constant index if isConstant == true
            /// </summary>
            public int ConstantIndex;
            public bool isConstant => this.Index == -1 ? true : false;


            public InputSpec()
            {

            }

            public void Split(out int Index, out bool isConstant, out int ConstantIndex)
            {
                Index = this.Index;
                isConstant = this.isConstant;
                ConstantIndex = this.ConstantIndex;
            }


        }

        public class OutputSpec
        {
            public int calcRate;

            public OutputSpec()
            {
                calcRate = 0;
            }


        }

        public class ParamName
        {
            public string Name;
            public int Index;

            public ParamName() { }

            public void Decompile(byte[] bytes, ref int index)
            {

                var length = 0;
                this.Name = Decompiler.FromPString(bytes, out length);
                index += length;
                //Console.WriteLine("index: {0} L:{1}", index, length);

                this.Index = (int)BitConverter.ToInt32(bytes.Skip(length).Take(4).Reverse().ToArray());
                //Console.WriteLine(this.Index);
                index += 4;

            }

            public void Split(out string Name, out int Index)
            {
                Name = this.Name;
                Index = this.Index;
            }
        }

        public int SynthDefsCountPerFile;
        public int FileVersion;
        public string FileCode;
        List<SynthDef> _synthDefs;

        HashSet<string> _synthDefsDict;

        public SCManager()
        {
            _synthDefs = new List<SynthDef>();
            _synthDefsDict = new HashSet<string>();
        }


        public void LoadFiles(IEnumerable<string> files)
        {
            
            Console.WriteLine("Loading files ... ");
            foreach (var file in files) {
                LoadFile(file);
            }
        }

        public void LoadFile(string file)
        {
            _synthDefsDict.TryGetValue(file, out var synthdef);
            
            if (synthdef == null)
            {
                Console.WriteLine("Decompiling: {0:G}", file);
                this.Decompile(file);
                _synthDefsDict.Add(file);
            }
            else
            {
                Console.WriteLine("File {0:G} already exists in SC Manager {0:G}", file);
                return;
            }

        }

        public void Decompile(string filePath)
        {
            
            byte[] bytes;
            Console.WriteLine("NEW DECOMP \n Decompile Synthdef ... ", filePath);
            bytes = File.ReadAllBytes(filePath);
            int index = 0;
            //file code
            var fileCode = Encoding.ASCII.GetString(bytes.Skip(0).Take(4).ToArray());
            index += 4;
            //file version
            var fileVersion = BitConverter.ToInt32(bytes.Skip(index).Take(4).Reverse().ToArray());
            index += 4;
            //synth defs in file
            var synthDefsCount = BitConverter.ToInt16(bytes.Skip(index).Take(2).Reverse().ToArray());
            index += 2;
            this.SynthDefsCountPerFile = synthDefsCount;
            this.FileVersion = fileVersion;
            this.FileCode = fileCode;

            for (int i = 0; i < synthDefsCount; i++)
            {
                SynthDef synthDef = new SynthDef();
                synthDef.Decompile(bytes, ref index);
                this._synthDefs.Add(synthDef);
            }
        }
        public void GetSynthDefs(out Spread<SynthDef> SynthDefs)
        {
            SynthDefs = this._synthDefs.ToSpread();
        }

        public void Dispose()
        {
           this.ClearAll();
        }

        public void ClearAll()
        {
            _synthDefs.Clear();
            _synthDefsDict.Clear();
        }
    }

}
