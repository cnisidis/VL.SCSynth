using SCSynth.SCNodes;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VL.Lib.Collections;

namespace SCSynth.OSC
{
    public static class OscEncoder
    {
        public static Spread<byte> EncodeMessage(string address, params object?[]? args)
        {
            // 1. Handle the null case
            if (args == null) args = Array.Empty<object>();

            using (MemoryStream ms = new MemoryStream())
            {
                // 1. Address Pattern (e.g., "/s_new")
                WriteOscString(ms, address);

                // 2. Type Tag String (starts with ',')
                StringBuilder typeTags = new StringBuilder(",");
                foreach (var arg in args)
                {
                    if (arg is int) typeTags.Append("i");
                    else if (arg is AddAction) typeTags.Append('i');
                    else if (arg is float || arg is double) typeTags.Append("f");
                    else if (arg is string) typeTags.Append("s");
                    else if (arg is byte[]) typeTags.Append("b");
                    else if (arg is Spread<ControlParameter>)
                    {
                        Console.WriteLine("Encode OSC Control Parameters stage 1");
                        var count = ((Spread<ControlParameter>)arg).Count();
                        for (int i = 0; i < count; i++) { typeTags.Append("s"); typeTags.Append("f"); }
                        
                    }
                    else if (arg is IEnumerable<Tuple<int, int, int>>)
                    {
                        typeTags.Append("iii");
                    }
                    // Add more types as needed (h for long, d for double, etc.)
                }
                WriteOscString(ms, typeTags.ToString());

                // 3. Arguments
                foreach (var arg in args)
                {
                    if (arg is int i)
                    {
                        byte[] data = BitConverter.GetBytes(i);
                        if (BitConverter.IsLittleEndian) Array.Reverse(data);
                        ms.Write(data, 0, 4);
                    }
                    else if (arg is float f)
                    {
                        byte[] data = BitConverter.GetBytes(f);
                        if (BitConverter.IsLittleEndian) Array.Reverse(data);
                        ms.Write(data, 0, 4);
                    }
                    else if (arg is double d) // SC usually expects floats, but we convert if needed
                    {
                        byte[] data = BitConverter.GetBytes((float)d);
                        if (BitConverter.IsLittleEndian) Array.Reverse(data);
                        ms.Write(data, 0, 4);
                    }
                    else if (arg is string s)
                    {
                        WriteOscString(ms, s);
                    }
                    else if (arg is byte[] b)
                    {
                        // Blob: Size (int32) followed by data
                        byte[] size = BitConverter.GetBytes(b.Length);
                        if (BitConverter.IsLittleEndian) Array.Reverse(size);
                        ms.Write(size, 0, 4);
                        ms.Write(b, 0, b.Length);
                        PadStream(ms, b.Length);
                    }
                    else if (arg is Spread<ControlParameter> controlParams)
                    {
                        foreach (var control in controlParams)
                        {
                            // 1. Write the Name (The helper MUST handle null-termination and 4-byte padding)
                            WriteOscString(ms, control.Name);

                            // 2. Convert and Write the Float
                            byte[] data = BitConverter.GetBytes((float)control.Value);
                            if (BitConverter.IsLittleEndian) Array.Reverse(data);

                            // Offset is 0 because we want the whole 4-byte 'data' array
                            ms.Write(data, 0, 4);
                        }
                    }

                }

                return ms.ToArray().ToSpread();
            }
        }

        private static void WriteOscString(Stream ms, string s)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(s);
            ms.Write(bytes, 0, bytes.Length);
            ms.WriteByte(0); // At least one null terminator
            PadStream(ms, bytes.Length + 1);
        }

        private static void PadStream(Stream ms, int currentLength)
        {
            int pad = (4 - (currentLength % 4)) % 4;
            for (int i = 0; i < pad; i++) ms.WriteByte(0);
        }

        public static Spread<byte> EncodeBundle(Spread<Spread<byte>> OSCEncodedArgs, byte[] timetag)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // 1. Write Header (8 bytes)
                byte[] header = Encoding.ASCII.GetBytes("#bundle\0");
                ms.Write(header, 0, header.Length);

                // 2. Write Timetag (8 bytes)
                ms.Write(timetag, 0, 8);

                foreach (var msg in OSCEncodedArgs)
                {
                    // 3. Write the SIZE of this specific message (4 bytes)
                    byte[] size = BitConverter.GetBytes(msg.Count);

                    // IMPORTANT: OSC is Big-Endian. C# is usually Little-Endian.
                    if (BitConverter.IsLittleEndian) Array.Reverse(size);

                    ms.Write(size, 0, 4);

                    // 4. Write the ACTUAL message bytes
                    ms.Write(msg.ToArray(), 0, msg.Count);
                }

                return ms.ToArray().ToSpread();
            }
        }
        public static byte[] GetNtpTimestamp(float Offset)
        {
            var offset = TimeSpan.FromMilliseconds(Offset);
            // 1. Calculate the target time in the future (or now)
            DateTime targetDate = DateTime.UtcNow + offset;

            // 2. NTP Era Start
            DateTime eraStart = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            TimeSpan totalDelta = targetDate - eraStart;

            // 3. Split into Seconds and Fractions
            uint seconds = (uint)totalDelta.TotalSeconds;
            double fractionsDouble = (totalDelta.TotalSeconds - Math.Floor(totalDelta.TotalSeconds)) * 4294967296.0;
            uint fractions = (uint)Math.Round(fractionsDouble);

            // 4. Pack for OSC (Big-Endian)
            byte[] timestamp = new byte[8];
            byte[] sBytes = BitConverter.GetBytes(seconds);
            byte[] fBytes = BitConverter.GetBytes(fractions);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(sBytes);
                Array.Reverse(fBytes);
            }

            Buffer.BlockCopy(sBytes, 0, timestamp, 0, 4);
            Buffer.BlockCopy(fBytes, 0, timestamp, 4, 4);

            return timestamp;
        }

        public static byte[] GetNtpTimestamp(DateTime date)
        {
            // 1. The NTP Era starts at 1900-01-01
            DateTime eraStart = new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            // 2. Calculate the total span since 1900
            TimeSpan delta = date.ToUniversalTime() - eraStart;

            // 3. The high 32 bits are the whole seconds
            uint seconds = (uint)delta.TotalSeconds;

            // 4. The low 32 bits are the fractional seconds scaled to 2^32
            // We take the remainder of the total seconds and multiply
            double fractionsDouble = (delta.TotalSeconds - Math.Floor(delta.TotalSeconds)) * 4294967296.0;
            uint fractions = (uint)Math.Round(fractionsDouble);

            // 5. Pack into 8 bytes (OSC requires Big-Endian)
            byte[] timestamp = new byte[8];
            byte[] sBytes = BitConverter.GetBytes(seconds);
            byte[] fBytes = BitConverter.GetBytes(fractions);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(sBytes);
                Array.Reverse(fBytes);
            }

            // Copy seconds to first 4 bytes, fractions to last 4 bytes
            Buffer.BlockCopy(sBytes, 0, timestamp, 0, 4);
            Buffer.BlockCopy(fBytes, 0, timestamp, 4, 4);

            return timestamp;
        }
    }
}
