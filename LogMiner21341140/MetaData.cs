using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogMiner
{
    public enum ColumnType
    {
        TinyInt = 48,
        Int = 56,
        Float = 62,
        Bit = 104,
        BigInt = 127,
        VarChar = 167,
        Binary = 173,
        Char = 175,
        SmallDateTime = 58,
        Real = 59,
        Money = 60,
        DateTime = 61,
        Decimal = 106,
        Numeric = 108

    }

    public class MetaData
    {
        public ColumnType Type { get; set; }
        public int Len { get; set; }
        public string ColumnName { get; set; }
        public bool IsPK { get; set; }

        public MetaData(int type, int length, string columnName, bool isPrimaryKey)
        {
            Type = (ColumnType)type;
            Len = length;
            ColumnName = columnName;
            IsPK = isPrimaryKey;
        }
    }
}
