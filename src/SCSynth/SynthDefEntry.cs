using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSynth
{
    public class SynthDefEntry
    {
        public string Name { get; set; }
        public byte[] Bytes;
        public Guid Id { get; }
        public string FilePath;
        public List<Synth> AssociatedSynths;

        public SynthDefEntry(string Name, string FilePath =  "")
        {
            this.Name = Name;
            Bytes = new byte[0];
            Id = new Guid();
            this.FilePath = FilePath;
        }

        public void SetFilePath(VL.Lib.IO.Path FilePath)
        {
            this.FilePath = FilePath.ToString();
        }

        public void SetFilePath(string FilePath)
        {
            this.FilePath = FilePath;
        }

        public void SetBytes(IEnumerable<byte> Bytes)
        {
            this.Bytes = new byte[Bytes.Count()];
            Bytes.ToArray().CopyTo(this.Bytes, 0);
        }


        public IEnumerable<byte> GetBytes() 
        { 
            return this.Bytes;
        }




    }
}
