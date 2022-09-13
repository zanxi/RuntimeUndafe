using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAppRuntimeUndafe
{
    internal class ThreadRuntime
    {
        Thread t = null;
        public void Run()
        {
            var syncObject = new object();
            int hashCode = syncObject.GetHashCode();
            var ptr = AddressOf(syncObject);
            
            t = new Thread(s =>
              {
                  lock (syncObject)
                  {
                      lock(syncObject)
                      {
                          syncObject.GetHashCode();
                          Console.WriteLine(t.ManagedThreadId);
                          Console.WriteLine(ptr);
                      }
                  }
              });

            t.Start();
            t.Join();

            byte[] buffer = { 0xAA, 0xBB, 0xCC, 0xDD, 0xAA, 0xBB, 0xCC, 0xDD };
            int[] intArr = ChangeType(buffer);// buffer стал int !!!!!!
            Console.WriteLine(buffer+"; " + intArr);
            //Console.WriteLine(buffer);
        }



        public static unsafe IntPtr AddressOf(object o)
        {
            TypedReference mk = __makeref(o);
            return **(IntPtr**) & mk;
        }

        // преобразуем произвольный поток байт в масиив int
        public unsafe int[] ChangeType(byte[] arr)
        {
            var sPtr = (long*)AddressOf(arr).ToPointer();
            var intArrTypeRef = (long*)AddressOf(Array.Empty<int>()).ToPointer();
            *sPtr = *intArrTypeRef;
            *(sPtr + 1) =  arr.LongLength/sizeof(int);
            return (int[])(object)arr;


        }

        public static unsafe byte[] ChangeType<T>(T[] sArr, byte[] v) where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));
            var arrPtr = (long*)AddressOf(sArr).ToPointer();
            var dstPtr = (long*)AddressOf(v).ToPointer();
            long newLength = size / sizeof(byte) * sArr.LongLength;
            *arrPtr = *dstPtr;
            *(arrPtr + 1) = newLength;

            return (byte[])(object)sArr;


        }

        public static unsafe T[] ChangeType<T>(byte[] sArr, T[] sample) where T : struct
        {
            int size = Marshal.SizeOf(typeof(T));
            var arrPtr = (long*)AddressOf(sArr).ToPointer();
            var dstPtr = (long*)AddressOf(sample).ToPointer();
            long newLength = sArr.Length/size;
            *arrPtr = *dstPtr;
            *(arrPtr + 1) = newLength;
            return (T[])(object)sArr;
        }        
    }

}
