using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace ConsoleAppRuntimeUndafe
{
    
    internal class Program
    {
        static void Main(string[] args)
        {
            //ThreadRuntime tr = new ThreadRuntime();
            //tr.Run();

            SuperSerializer sr = new SuperSerializer();
            sr.SampeSerialize();

            Console.ReadKey();
        }
    }
}
