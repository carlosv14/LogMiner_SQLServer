using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogMiner
{
    public class Conversions
    {
        public static string Converter(string value, ColumnType Type)
        {
            switch (Type)
            {
                case ColumnType.Int:
                    return "" + ToInt(value);
                case ColumnType.BigInt:
                    return "" + ToBigInt(value);
                case ColumnType.TinyInt:
                    return "" + ToTinyInt(value);
                case ColumnType.Decimal:
                    return "" + ToDecimal(value);
                //case ColumnType.Money:
                //    return "" + ToMoney(value);
                case ColumnType.Float:
                    return "" + ToFloat(value);
                case ColumnType.Real:
                    return "" + ConvertToReal(value);
                //case ColumnType.Numeric:
                //    return "" + nu(value);
                case ColumnType.Bit:
                    return "" + ConvertToBit(value);
                case ColumnType.Binary:
                    return "" + ToBinary(value);
                case ColumnType.Char:
                    return ToChar(value);
                case ColumnType.VarChar:
                    return ToVarchar(value);
                case ColumnType.DateTime:
                    return ToDateTime(value).ToString(CultureInfo.InvariantCulture);
                case ColumnType.SmallDateTime:
                    return (value).ToString(CultureInfo.InvariantCulture);
                default:
                    return null;
            }
        }

        public DateTime ParseSmallDateTime(string input)
        {
            UInt32 secondsAfterEpoch = (uint)ToInt(input);
            DateTime epoch = new DateTime(1900, 1, 1);
            DateTime myDateTime = epoch.AddMinutes(secondsAfterEpoch);
            return myDateTime;
          
        }
        private  static readonly Dictionary<char, string> hexToBin = new Dictionary<char, string> {
            { '0', "0000" },
            { '1', "0001" },
            { '2', "0010" },
            { '3', "0011" },
            { '4', "0100" },
            { '5', "0101" },
            { '6', "0110" },
            { '7', "0111" },
            { '8', "1000" },
            { '9', "1001" },
            { 'a', "1010" },
            { 'b', "1011" },
            { 'c', "1100" },
            { 'd', "1101" },
            { 'e', "1110" },
            { 'f', "1111" }
        };

       
        public static int ToInt(string hexadecimal)
        {
            int x;
            Hexadecimals h = new Hexadecimals();;
            string hex = h.backwards(hexadecimal);
            x = int.Parse(hex,NumberStyles.HexNumber);
            return x;
        }
        public static byte ToTinyInt(string input)
        {
            return Byte.Parse(input, NumberStyles.HexNumber);
        }
        public static string ToChar(string input)
        {
            var result = "";
            for (var i = 0; i < input.Length; i = i + 2)
            {
                result += (char)Int16.Parse(input.Substring(i, 2), NumberStyles.AllowHexSpecifier);
            }
            result = result.Trim();
            return result;
        }

        public static string ToVarchar(string hexadecimal)
        {
            List<int> nums = new List<int>();
            string result = "";

            for (int i = 0; i < hexadecimal.Length; i += 2)
            {
                nums.Add((int) Convert.ToUInt32(hexadecimal.Substring(i,2), 16));
            }


            for (int i = 0; i < nums.Count; i++)
            {
                result += (char)nums[i];
            }

            return result;
        }

        public static string ToBinary(string c)
        {
            string result = "";
           

            char[] arr=  c.ToCharArray();
            for (int i = 0; i < c.Length; i++)
                result+=(hexToBin[(char.ToLower(arr[i]))]);

            return result;
        }
       

    public static float ToFloat(string hex)
        {
            Hexadecimals h = new Hexadecimals();
             byte[] bytes = h.FromHex(hex);
            if (BitConverter.IsLittleEndian)
            {
                bytes = bytes.Reverse().ToArray();
            }
            return BitConverter.ToSingle(bytes, 0);
        }

        public static double ToDecimal(string hex)
        {

            return Convert.ToInt32(hex, 16);
        }

        public static bool ConvertToBit(string hex)
        {
            Hexadecimals h = new Hexadecimals();
            var bytes = h.FromHex(hex);

            return (bytes[0] & 1) != 0;
        }



        public static Int16 ToSmallInt(string input)
        {
            var stringlist = new List<string>();
            for (var i = 0; i < 4; i += 2)
            {
                stringlist.Add(input.Substring(i, 2));
            }
            stringlist.Reverse();
            var bigEndian = String.Join("", stringlist);

            return Int16.Parse(bigEndian, NumberStyles.HexNumber); ;
        }
        public static Int64 ToBigInt(string hexadecimal)
        {
            Hexadecimals h = new Hexadecimals();
            return (Convert.ToInt64(hexadecimal, 16));
        }


        public static float ConvertToReal(string hex)
        {
            Hexadecimals h = new Hexadecimals();
            var bytes = h.FromHex(hex);

            if (BitConverter.IsLittleEndian)
            {
                bytes = bytes.Reverse().ToArray();
            }
            return BitConverter.ToSingle(bytes, 0);
        }
        public static string ToDateTime(string hex)
        {
            Hexadecimals h = new Hexadecimals();
            Byte[] bytes = h.FromHex(hex);
            if (BitConverter.IsLittleEndian)
            {
                bytes = bytes.Reverse().ToArray();
            }
            return new DateTime(1900, 1, 1).AddDays(BitConverter.ToInt32(bytes, 4)).
                           AddMilliseconds(10 / 3.0d * BitConverter.ToInt32(bytes, 0)).
                           ToString(CultureInfo.InvariantCulture);
        }


        public static double ConvertToMoney(string hex)
        {
            Hexadecimals h = new Hexadecimals();
            Byte[] bytes = h.FromHex(hex);
            if (BitConverter.IsLittleEndian)
            {
                bytes = bytes.Reverse().ToArray();
            }

            return BitConverter.ToInt64(bytes, 0) / 10000d;
        }

    }

}
