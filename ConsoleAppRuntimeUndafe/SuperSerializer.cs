using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppRuntimeUndafe
{
    // В классе представлены 2 функции:
    // Serializer - для сериализации произвольной структуры в массив байт
    // DeSerializer - десериализации массива байт в исходную структуру

    internal class SuperSerializer
    {
        public void Serializer<T>(Stream stream, T[] toSerialize) where T : struct
        {
            ThreadRuntime tr = new ThreadRuntime();
            var bytes = ThreadRuntime.ChangeType(toSerialize, Array.Empty<byte>());
            try
            {
                var writer = new BinaryWriter(stream);
                writer.Write(bytes.LongLength);
                writer.Write(bytes);
            }finally
            {
                ThreadRuntime.ChangeType(bytes, Array.Empty<T>());
            }            
        }

        public T[] DeSerializer<T>(Stream stream) where T : struct
        {
            var reader = new BinaryReader(stream);
            var length = reader.ReadInt64();
            var arr = reader.ReadBytes((int)length);
            return ThreadRuntime.ChangeType(arr, Array.Empty<T>());
            
        }

        SampleAlignment[] SampleDeserialize()
        {
            var fs = File.OpenRead("File.bin");
            //var sr = new SuperSerializer();
            return DeSerializer<SampleAlignment>(fs);
        }

        public void SampeSerialize()
        {
            var fs = File.OpenWrite("File.bin");
            var arr = new[]
            {
                new SampleAlignment() { B1= 0xAA, B2 = 0xBB, I = 0xCCDDEEFF, L = 0x001122334455667788U  },
                new SampleAlignment() { B1= 0xAA, B2 = 0xBB, I = 0xCCDDEEFF, L = 0x001122334455667788U  },
                new SampleAlignment() { B1= 0xAA, B2 = 0xBB, I = 0xCCDDEEFF, L = 0x001122334455667788U  }
            };
            var ss = new SuperSerializer();
            ss.Serializer(fs,arr);

            var d = SampleDeserialize();
            Console.WriteLine(d);
        }


        struct SampleAlignment
        {
            public byte B1;
            public byte B2;
            public ulong L;
            public uint I;

        }
    }
}
