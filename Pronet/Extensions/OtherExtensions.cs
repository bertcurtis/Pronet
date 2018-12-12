using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pronet.Extensions
{
    public static class OtherExtensions
    {
        public static int GetNumbers(this string input)
        {
            if (input == "")
            {
                return 0;
            }
            else
            {
                string toParse = new string(input.Where(c => char.IsDigit(c)).ToArray());
                return int.Parse(toParse);
            }
        }
    }
}
