using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Util
{
    public static class Helpers
    {
        public static List<string> LoadStrings(string path)
        {
            var strings = new List<string>();

            using (var file = new System.IO.StreamReader(path))
            {
                string line;

                while ((line = file.ReadLine()) != null)
                {
                    strings.Add(line);
                }
            }

            return strings;
        }
    }
}
