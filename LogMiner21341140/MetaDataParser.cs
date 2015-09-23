using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogMiner
{
    class MetaDataParser
    {
        private readonly string _connectionString;
        public MetaDataParser()
        {
            _connectionString = "Server=CARLOSV;Database=ventas;Trusted_Connection=True;MultipleActiveResultSets=True;";
        }

        public List<MetaData> GetMetadata(string database, string table)
        {
            var primaryKey = GetPrimaryKey(database, table);
            var result = new List<MetaData>();
            var sql =
                "USE " + database + " SELECT [name], [xtype], [length] FROM syscolumns WHERE id = (SELECT id FROM sysobjects WHERE xtype = 'u' and name = '" +
                table + "');";
            var conn = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(sql, conn);
            conn.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var toAdd = new MetaData(Convert.ToInt32(reader[1]), Convert.ToInt32(reader[2]),
                    reader[0].ToString(), primaryKey.Contains(reader[0].ToString()));
                result.Add(toAdd);
            }
            reader.Close();
            conn.Close();
            return result;
        }

        public List<string> GetRowLogContentsCero(string database, string table, string tipo,string transid)
        {
            var result = new List<string>(); ;
            var sql = "";
            if (tipo == "LOP_MODIFY_ROW")
                sql = "USE " + database + " SELECT [RowLog Contents 0] FROM fn_dblog(null, null) WHERE Operation = '" +
                      tipo + "'" + " and Context = 'LCX_HEAP' AND AllocUnitName = 'dbo." + table + "' AND [Transaction ID] = " + "'" + transid +
                      "'";
            else
                sql = "USE " + database + " SELECT [RowLog Contents 0] FROM fn_dblog(null, null) WHERE Operation = '" +
                      tipo + "'" + " and Context = 'LCX_HEAP' AND AllocUnitName = 'dbo." + table + "' AND [Transaction ID] = " + "'" + transid +
                      "'";

            var conn = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(sql, conn);
            conn.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var bytes = reader[0] as byte[];

                if (bytes == null)
                    continue;
                var hex = BitConverter.ToString(bytes).Replace("-", string.Empty);
                if (!String.IsNullOrEmpty(hex))
                {
                   result.Add(hex);
                }
            }
            if (!reader.HasRows)
            {
                sql = "USE " + database + " SELECT [RowLog Contents 0] FROM fn_dblog(null, null) WHERE Operation = '" +
                     tipo + "'" + " and AllocUnitName like'dbo." + table + ".PK%' AND [Transaction ID] = " + "'" + transid +
                      "'";
                var cmd1 = new SqlCommand(sql, conn);
                var reader1 = cmd1.ExecuteReader();
                while (reader1.Read())
                {
                    var bytes = reader1[0] as byte[];

                    if (bytes == null)
                        continue;
                    var hex = BitConverter.ToString(bytes).Replace("-", string.Empty);
                    if (!String.IsNullOrEmpty(hex))
                    {
                        result.Add(hex);
                    }
                }

            }
            reader.Close();
           
            conn.Close();
            return result;
        }
        public List<string> GetRowLogContentsOne(string database, string table, string tipo, string transid)
        {
            var result = new List<string>(); ;
            var sql = "";
            if (tipo == "LOP_MODIFY_ROW")
                sql = "USE " + database + " SELECT [RowLog Contents 1] FROM fn_dblog(null, null) WHERE Operation = '" +
                      tipo + "'" + " and Context = 'LCX_HEAP' AND AllocUnitName = 'dbo." + table + "' AND [Transaction ID] = " + "'" + transid +
                      "'";
            else
                sql = "USE " + database + " SELECT [RowLog Contents 1] FROM fn_dblog(null, null) WHERE Operation = '" +
                      tipo + "'" + " and Context = 'LCX_HEAP' AND AllocUnitName = 'dbo." + table + "'";

            var conn = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(sql, conn);
            conn.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                var bytes = reader[0] as byte[];

                if (bytes == null)
                    continue;
                var hex = BitConverter.ToString(bytes).Replace("-", string.Empty);
                if (!String.IsNullOrEmpty(hex))
                {
                    result.Add(hex);
                }
            }
            reader.Close();
            conn.Close();
            return result;
        }
        public string GetPrimaryKey(string database, string table)
        {
            var query = "USE " + database + " SELECT column_name FROM INFORMATION_SCHEMA.KEY_COLUMN_USAGE WHERE OBJECTPROPERTY(OBJECT_ID(constraint_name), 'IsPrimaryKey') = 1 AND table_name = '" + table + "'";
            var conn = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(query, conn);
            conn.Open();
            var reader = cmd.ExecuteReader();
            var toReturn = "";
            if (reader.Read())
                toReturn = reader[0].ToString();
            reader.Close();
            conn.Close();
            return toReturn;
        }

      
    }
}
