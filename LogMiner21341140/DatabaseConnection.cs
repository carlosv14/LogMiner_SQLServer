using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LogMiner21341140
{
    public partial class DatabaseConnection : Form
    {
        public string database;
        public string table;
        public bool alltables;
        public DatabaseConnection()
        {
            InitializeComponent();
            this.database = "";
            this.table = "";
            alltables = false;
            AddDatabases();

        }

        public void AddDatabases()
        {

            string connectionString = "Server=CARLOSV;Database=ventas;" + "Trusted_Connection=True;MultipleActiveResultSets=True;";
            const string sql = "SELECT name FROM Sys.Databases";
            var conn = new SqlConnection(connectionString);
            var cmd = new SqlCommand(sql, conn);
            conn.Open();
            var reader = cmd.ExecuteReader();
            var datasource = new List<string>();
            while (reader.Read())
            {
                datasource.Add(reader[0].ToString());

            }
            reader.Close();
            conn.Close();

            this.comboBox1.DataSource = datasource;
            this.comboBox1.SelectedIndex = 0;
        }
        public void AddTableNames(string db)
        {
             string connectionString = "Server=CARLOSV;Database= "+db+";Trusted_Connection=True;MultipleActiveResultSets=True;";
            string sql = "USE " + this.comboBox1.SelectedItem.ToString() +
                            " SELECT TABLE_NAME FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'";
            var conn = new SqlConnection(connectionString);
            var cmd = new SqlCommand(sql, conn);
            conn.Open();
            var reader = cmd.ExecuteReader();
            var datasource = new List<string>();
            while (reader.Read())
            {
                datasource.Add(reader[0].ToString());

            }
            reader.Close();
            conn.Close();

            this.comboBox2.DataSource = datasource;
            this.comboBox2.SelectedIndex = 0;
        }

        private void DatabaseConnection_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.database = comboBox1.SelectedItem.ToString();
            this.table = comboBox2.SelectedItem.ToString();
            if (checkBox1.Checked)
                this.alltables = true;
            this.Close();
           
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            AddTableNames(comboBox1.SelectedItem.ToString());
        }

        private void ribbonControl1_Click(object sender, EventArgs e)
        {

        }
    }
}
