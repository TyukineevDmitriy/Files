using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Files
{
    class CsvReader
    {
        public static IEnumerable<dynamic> ReadCsv4(string filename)
        {
            var names = new string[0];
            foreach (var strArr in ReadCsv(filename))
            {
                if (names.Length == 0)
                    names = ParseNames(strArr);
                else
                {
                    dynamic dynamicRes = new ExpandoObject();
                    IDictionary<string, object> res = dynamicRes;
                    for (int i = 0; i < strArr.Length; i++)
                        res.Add(names[i], ParseValue(strArr[i]));
                    yield return res;
                }
            }
        }

        private static string[] ParseNames(string[] strArr)
        {
            for (int i = 0; i < strArr.Length; i++)
                strArr[i] = strArr[i].Replace("\"", "").Replace(".", "");
            return strArr;
        }

        private static object ParseValue(string value)
        {
            int intValue;
            if (int.TryParse(value, out intValue))
            {
                return intValue;
            }
            double doubleValue;
            if (double.TryParse(value, out doubleValue))
            {
                return doubleValue;
            }
            if (value == "NA")
            {
                return null;
            }
            return value;
        }

        public static IEnumerable<Dictionary<string, object>> ReadCsv3(string filename)
        {
            var names = new string[0];
            foreach (var strArr in ReadCsv(filename))
            {
                if (names.Length == 0)
                    names = ParseNames(strArr);
                else
                {
                    Dictionary<string, object> res = new Dictionary<string, object>();
                    for (int i = 0; i < strArr.Length; i++)
                    {
                        res.Add(names[i], ParseValue(strArr[i]));
                    }
                    yield return res;
                }
            }
        }

        public static IEnumerable<T> ReadCsv2<T>(string filename)
            where T : new()
        {
            var names = new string[0];
            foreach (var strArr in ReadCsv(filename))
            {
                if (names.Length == 0)
                    names = ParseNames(strArr);
                else
                {
                    T res = new T();
                    for (int i = 0; i < strArr.Length; i++)
                    {
                        var prop = res.GetType().GetProperty(names[i]);
                        if (prop == null)
                            throw new Exception(string.Format("Incorrect column {0}", names[i]));
                        else
                        {
                            if (strArr[i] == "NA")
                            {
                                if (Nullable.GetUnderlyingType(prop.GetType()) == null)
                                    throw new Exception("This column cannot be null");
                                else
                                    prop.SetValue(res, null);
                            }
                            else
                                prop.SetValue(res, strArr[i]);
                        }
                    }
                    yield return res;
                }
            }
        }

        public static IEnumerable<string[]> ReadCsv1(string filename)
        {
            foreach (var strArr in ReadCsv(filename))
            {
                for (int i = 0; i < strArr.Length; i++)
                {
                    if (strArr[i] == "NA")
                    {
                        strArr[i] = null;
                    }
                }
                yield return strArr;
            }
        }

        private static IEnumerable<string[]> ReadCsv(string filename)
        {
            using (var stream = new StreamReader(filename))
                while (true)
                {
                    var str = stream.ReadLine();
                    if (str == null)
                    {
                        stream.Close();
                        yield break;
                    }
                    var strArr = str.Split(',');
                    yield return strArr;
                }
        }
    }
}
