using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
                case ColumnType.Numeric:
                  return "" + ToDecimal(value);
                case ColumnType.Bit:
                    return "" + ConvertToBit(value);
                case ColumnType.Binary:
                    return "" + ToBinary(value);
                case ColumnType.Char:
                    return ToChar(value);
                case ColumnType.VarChar:
                    return ToVarchar(value);
                case ColumnType.DateTime:
                    return ToDateTime(value);
                case ColumnType.SmallDateTime:
                    return ToSmallDateTime(value);
                default:
                    return null;
            }
        }

        public static string ToSmallDateTime(string hexadecimal)
        {
            Hexadecimals h = new Hexadecimals();

            var fechabase = "1900-01-01 00:00";
            var date = DateTime.ParseExact(fechabase, "yyyy-MM-dd HH:mm", null);
            date = date.AddDays(BitConverter.ToUInt16(h.FromHex(hexadecimal), 2));
            date = date.AddSeconds(BitConverter.ToUInt16(h.FromHex(hexadecimal), 0) / 300);
            return date.ToString();



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
        public static byte ToTinyInt(string hex)
        {
            return Byte.Parse(hex, NumberStyles.HexNumber);
        }
        public static string ToChar(string hex)
        {
            var result = "";
            for (var i = 0; i < hex.Length; i = i + 2)
            {
                result += (char)Int16.Parse(hex.Substring(i, 2), NumberStyles.AllowHexSpecifier);
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
       

   

        public static decimal ToDecimal(string input)
        {
           
            var stringlist = new List<string>();
            for (int i = 0; i < input.Length; i += 2)
            {
                stringlist.Add(input.Substring(i, 2));
            }
            stringlist.Reverse();
            stringlist.RemoveAt(stringlist.Count - 1);
            string numero = String.Join("", stringlist);
            
            return Convert.ToInt64(numero, 16);
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
            var numero = String.Join("", stringlist);

            return Int16.Parse(numero, NumberStyles.HexNumber); ;
        }

        public static Int64 ToBigInt(string input)
        {
            var stringlist = new List<string>();
            for (int i = 0; i < 16; i += 2)
            {
                stringlist.Add(input.Substring(i, 2));
            }
            stringlist.Reverse();
            string numero = String.Join("", stringlist);
            return Int64.Parse(numero, NumberStyles.HexNumber);
        }

        public static double ToFloat(string input)
        {
            return BitConverter.Int64BitsToDouble(ToBigInt(input));
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
        public static string ToDateTime(string hexadecimal)
        {
            Hexadecimals h = new Hexadecimals();
           
            var fechabase= "1900-01-01 00:00";
            var date = DateTime.ParseExact(fechabase, "yyyy-MM-dd HH:mm", null);
            date = date.AddDays(BitConverter.ToUInt32(h.FromHex(hexadecimal), 4));
            date = date.AddSeconds(BitConverter.ToUInt32(h.FromHex(hexadecimal), 0) / 300);
            return date.ToString("yyyy/MM/dd HH:mm:ss");      
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
