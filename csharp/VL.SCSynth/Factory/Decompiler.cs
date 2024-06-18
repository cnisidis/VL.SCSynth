using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace VL.SCSynth.Factory
{
    public class SynthPack
    {
        int fileVersion {  get; set; }
        string fileCode { get; set; }
        int index {  set; get; }
        public SynthPack()
        {

        }
    }
    public static class Decompiler
    {


        public static Dictionary<string, List<Parameter>> DecompileSynthDefsFromFile(string synthdefPath)
        {
            Console.WriteLine("Decompile Synthdef ...");
            Dictionary<string, List<Parameter>> SynthDefs = new Dictionary<string, List<Parameter>>();
            Console.WriteLine(synthdefPath);
            
            byte[] bytes = File.ReadAllBytes(synthdefPath);
            
            


            int index = 0;
            //file code
            var fileCode = Encoding.ASCII.GetString(bytes.Skip(0).Take(4).ToArray());
            index += 4;
            //file version
            var fileVersion = BitConverter.ToInt32(SwapBytes(bytes.Skip(index), 4));
            index += 4;
            //synth defs in file
            var synthDefsCount = BitConverter.ToInt16(SwapBytes(bytes.Skip(index), 2));
            index += 2;
            Console.WriteLine("Filecode: {0} FileVersion: {1}  SynthDefs count: {2}", fileCode, fileVersion.ToString(), synthDefsCount.ToString());
            //decompile synthdefs
            for (int i = 0; i < synthDefsCount; i++)
            {
                
                //var synthDef = DecompileSynthdef(bytes.Skip(index).ToArray());
                int nameLength = 0;

                //SynthDef name
                string synthDefName = FromPString(bytes.Skip(index), out nameLength);
                index += nameLength;
                Console.WriteLine("{0} : Dcompile Sytnhdef: {1}", i.ToString(), synthDefName);
                //Number of Constants
                int numberOfConstants = BitConverter.ToInt32(SwapBytes(bytes.Skip(index), 4));
                index += 4;


                for (int j = 0; j < numberOfConstants; j++)
                {
                    index += 4;
                }

                //Number of Parameters
                int numberOfParameters = BitConverter.ToInt32(SwapBytes(bytes.Skip(index), 4));
                index += 4;
                Console.WriteLine("Number of parameters found: {0}", numberOfParameters);
                List<Parameter> parameters = new List<Parameter>();
                List<float> parametersInitValues = new List<float>();
                
                //Parameter Initial Values
                for (int j = 0; j < numberOfParameters; j++)
                {
                    float parameterValue = BitConverter.ToSingle(SwapBytes(bytes.Skip(index), 4));

                    parametersInitValues.Add(parameterValue);
                    index += 4;

                }
                //Number of Parameters Names
                int numberOfParametersNames = BitConverter.ToInt32(SwapBytes(bytes.Skip(index), 4));
                index += 4;
                Console.WriteLine("Number of parameters Names found: {0}", numberOfParametersNames);
                for (int j = 0; j < numberOfParametersNames; j++)
                {
                    int parameterNameLength;
                    string parameterName = FromPString(bytes.Skip(index), out parameterNameLength);
                    index += parameterNameLength;

                    int parameterIndex = BitConverter.ToInt32(SwapBytes(bytes.Skip(index), 4));
                    index += 4;

                    var parameter = new Parameter(parameterName, parametersInitValues[j]);
                    parameter.index = j;
                    parameters.Add(parameter);

                }

                

                int numberOfUGens = BitConverter.ToInt32(SwapBytes(bytes.Skip(index), 4));
                index += 4;



                for (int j = 0; j < numberOfUGens; j++)
                {

                    int prevLength;
                    DecompileUGen(bytes.Skip(index).ToArray(), out prevLength);
                    index += prevLength;
                }


                int numberOfVariants = BitConverter.ToInt16(SwapBytes(bytes.Skip(index), 2));
                index += 2;


                for (int j = 0; j < numberOfVariants; j++)
                {
                    int variantsLength;
                    DecompileVariants(bytes.Skip(index).ToArray(), numberOfParameters, out variantsLength);
                    index += variantsLength;
                }


                //Add to Dictionary
                SynthDefs.TryAdd(synthDefName, parameters);

                Console.WriteLine(synthDefName);
            }
            Console.WriteLine("Decompiled Synthdef SUCCESFUL");
            return SynthDefs;


        }

        
        internal static void DecompileUGen(byte[] bytes, out int length)
        {
            length = 0;
            int index = 0;
            int classNameLength = 0;
            string className = FromPString(bytes, out classNameLength);
            index += classNameLength;

            //Calculation Rate
            index += 1;

            int numberOfInputs = BitConverter.ToInt32(SwapBytes(bytes.Skip(index), 4));
            index += 4;

            int numberOfOutputs = BitConverter.ToInt32(SwapBytes(bytes.Skip(index), 4));
            index += 4;

            //Special Index
            index += 2;


            for (int i = 0; i < numberOfInputs; i++)
            {
                index += 8;
            }

            for (int i = 0; i < numberOfOutputs; i++)
            {
                index += 1;
            }

            length = index;
        }
        internal static void DecompileVariants(byte[] bytes, int numberOfParameters, out int variantsLength)
        {
            variantsLength = 0;
            int index = 0;
            int variantNameLength;
            string variantName = FromPString(bytes, out variantNameLength);
            index += variantNameLength;
            for (int i = 0; i < numberOfParameters; i++)
            {
                float variantValue = BitConverter.ToSingle(SwapBytes(bytes.Skip(index), 4));
                index += 4;
            }
            variantsLength = index;
        }
        public static byte[] SwapBytes(IEnumerable<byte> bytes)
        {
            return bytes.Reverse().ToArray();
        }

        public static byte[] SwapBytes(IEnumerable<byte> bytes, int count)
        {
            return bytes.Take(count).Reverse().ToArray();
        }

        public static byte[] SwapBytes(IEnumerable<byte> bytes, int offset, int count)
        {
            return bytes.Skip(offset).Take(count).Reverse().ToArray();
        }

        public static string FromPString(IEnumerable<byte> bytes, out int stringLength)
        {

            string stringValue = "";
            stringLength = bytes.ToArray()[0];
            stringValue = Encoding.ASCII.GetString(bytes.Skip(1).Take(stringLength).ToArray());
            stringLength += 1;
            return stringValue;
        }
    }

    
}
