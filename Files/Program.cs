using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Files
{
    class Program
    {
        static void Main(string[] args)
        {
                /*foreach (var e in CsvReader.ReadCsv1(@"..\..\airquality.csv"))
                    foreach (var item in e)
                    {
                        Console.WriteLine(item);
                    }*/
                foreach (var e in CsvReader.ReadCsv4(@"..\..\airquality.csv").Where(z => z.Ozone > 10).Select(z => z.Wind))
                    Console.WriteLine(e);
            }
    }
}
