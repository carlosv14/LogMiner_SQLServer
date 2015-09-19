using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogMiner
{

    public class Hexadecimals
    {
        public string backwards(string hexadecimal)
        {
            string reverse = "";
            for (int i = hexadecimal.Length - 1; i >= 0; i -= 2)

            {
                if (i > 0)
                    reverse += hexadecimal.Substring(i - 1, 1);

                reverse += hexadecimal.Substring(i, 1);
            }
            return reverse;
        }

        public Byte[] FromHex(string hexadecimal)
        {
            Byte[] data = new Byte[hexadecimal.Length / 2];

            for (int i = 0; i < data.Length; i++)
            {
                data[i] = Convert.ToByte(hexadecimal.Substring(i * 2, 2), 16);
            }
            return data;

        }
    }

}
