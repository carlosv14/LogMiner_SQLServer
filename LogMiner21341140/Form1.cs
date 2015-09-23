using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevComponents.DotNetBar;
using LogMiner;
using ScintillaNET;

namespace LogMiner21341140
{
    public partial class Form1 : Office2007Form
    {
        private string tipo;
        private string database;
        private string table;
        private DatabaseConnection dbc;
        private string complete;

        private string undoupdate = "";
        private string redoupdate = "";
        private string anyinsert;
        public Form1()
        {
            InitializeComponent();
            tipo = "LOP_DELETE_ROWS";
            this.database = "";
            this.table = "";
            anyinsert = "";
            dbc = new DatabaseConnection();
            dbc.ShowDialog();
            database = dbc.database;
            table = dbc.table;
            FillList();
            deleteOps();
        
            richTextBox1.StyleResetDefault();
            richTextBox1.Styles[Style.Default].Font = "Consolas";
            richTextBox1.Styles[Style.Default].Size = 10;
            richTextBox1.StyleClearAll();
            richTextBox1.Styles[Style.Cpp.Default].ForeColor = Color.Silver;
            richTextBox1.Styles[Style.Cpp.Comment].ForeColor = Color.FromArgb(0, 128, 0); // Green
            richTextBox1.Styles[Style.Cpp.CommentLine].ForeColor = Color.FromArgb(0, 128, 0); // Green
            richTextBox1.Styles[Style.Cpp.CommentLineDoc].ForeColor = Color.FromArgb(128, 128, 128); // Gray
            richTextBox1.Styles[Style.Cpp.Number].ForeColor = Color.Red;
            richTextBox1.Styles[Style.Cpp.Word].ForeColor = Color.Blue;
            richTextBox1.Styles[Style.Cpp.Word2].ForeColor = Color.Fuchsia;
            richTextBox1.Styles[Style.Cpp.String].ForeColor = Color.FromArgb(163, 21, 21); // Red
            richTextBox1.Styles[Style.Cpp.Character].ForeColor = Color.FromArgb(163, 21, 21); // Red
            richTextBox1.Styles[Style.Cpp.Verbatim].ForeColor = Color.FromArgb(163, 21, 21); // Red
            richTextBox1.Styles[Style.Cpp.Operator].ForeColor = Color.Silver;
            richTextBox1.Styles[Style.Cpp.Preprocessor].ForeColor = Color.Purple;
            richTextBox1.Lexer = Lexer.Cpp;
            richTextBox1.Margins[0].Width = 16;

            richTextBox1.SetKeywords(0, "abort absolute access action add admin after aggregate all also alter always analyse analyze and any array as asc assertion assignment asymmetric at attribute authorization backward before begin between bigint binary bit boolean both by cache called cascade cascaded case cast catalog chain char character characteristics check checkpoint class close cluster coalesce collate collation column comment comments commit committed concurrently configuration connection constraint constraints content continue conversion copy cost create cross csv current current_catalog current_date current_role current_schema current_time current_timestamp current_user cursor cycle data database day deallocate dec decimal declare default defaults deferrable deferred definer delete delimiter delimiters desc dictionary disable discard distinct do document domain double drop each else enable encoding encrypted end enum escape event except exclude excluding exclusive execute exists explain extension external extract false family fetch filter first float following for force foreign forward freeze from full function functions global grant granted greatest group handler having header hold hour identity if ilike immediate immutable implicit in including increment index indexes inherit inherits initially inline inner inout input insensitive insert instead int integer intersect interval into invoker is isnull isolation join key label language large last lateral lc_collate lc_ctype leading leakproof least left level like limit listen load local localtime localtimestamp location lock mapping match materialized maxvalue minute minvalue mode month move name names national natural nchar next no none not nothing notify notnull nowait null nullif nulls numeric object of off offset oids on only operator option options or order ordinality out outer over overlaps overlay owned owner parser partial partition passing password placing plans position preceding precision prepare prepared preserve primary prior privileges procedural procedure program quote range read real reassign recheck recursive ref references refresh reindex relative release rename repeatable replace replica reset restart restrict returning returns revoke right role rollback row rows rule savepoint schema scroll search second security select sequence sequences serializable server session session_user set setof share show similar simple smallint snapshot some stable standalone start statement statistics stdin stdout storage strict strip substring symmetric sysid system table tables tablespace temp template temporary text then time timestamp to trailing transaction treat trigger trim true truncate trusted type types unbounded uncommitted unencrypted union unique unknown unlisten unlogged until user using vacuum valid validate validator value values varchar variadic varying verbose version view views volatile when where whitespace window with within without work wrapper write xml xmlattributes xmlconcat xmlelement xmlexists xmlforest xmlparse xmlpi xmlroot xmlserialize year yes zone ABORT ABSOLUTE ACCESS ACTION ADD ADMIN AFTER AGGREGATE ALL ALSO ALTER ALWAYS ANALYSE ANALYZE AND ANY ARRAY AS ASC ASSERTION ASSIGNMENT ASYMMETRIC AT ATTRIBUTE AUTHORIZATION BACKWARD BEFORE BEGIN BETWEEN BIGINT BINARY BIT BOOLEAN BOTH BY CACHE CALLED CASCADE CASCADED CASE CAST CATALOG CHAIN CHAR CHARACTER CHARACTERISTICS CHECK CHECKPOINT CLASS CLOSE CLUSTER COALESCE COLLATE COLLATION COLUMN COMMENT COMMENTS COMMIT COMMITTED CONCURRENTLY CONFIGURATION CONNECTION CONSTRAINT CONSTRAINTS CONTENT CONTINUE CONVERSION COPY COST CREATE CROSS CSV CURRENT CURRENT_CATALOG CURRENT_DATE CURRENT_ROLE CURRENT_SCHEMA CURRENT_TIME CURRENT_TIMESTAMP CURRENT_USER CURSOR CYCLE DATA DATABASE DAY DEALLOCATE DEC DECIMAL DECLARE DEFAULT DEFAULTS DEFERRABLE DEFERRED DEFINER DELETE DELIMITER DELIMITERS DESC DICTIONARY DISABLE DISCARD DISTINCT DO DOCUMENT DOMAIN DOUBLE DROP EACH ELSE ENABLE ENCODING ENCRYPTED END ENUM ESCAPE EVENT EXCEPT EXCLUDE EXCLUDING EXCLUSIVE EXECUTE EXISTS EXPLAIN EXTENSION EXTERNAL EXTRACT FALSE FAMILY FETCH FILTER FIRST FLOAT FOLLOWING FOR FORCE FOREIGN FORWARD FREEZE FROM FULL FUNCTION FUNCTIONS GLOBAL GRANT GRANTED GREATEST GROUP HANDLER HAVING HEADER HOLD HOUR IDENTITY IF ILIKE IMMEDIATE IMMUTABLE IMPLICIT IN INCLUDING INCREMENT INDEX INDEXES INHERIT INHERITS INITIALLY INLINE INNER INOUT INPUT INSENSITIVE INSERT INSTEAD INT INTEGER INTERSECT INTERVAL INTO INVOKER IS ISNULL ISOLATION JOIN KEY LABEL LANGUAGE LARGE LAST LATERAL LC_COLLATE LC_CTYPE LEADING LEAKPROOF LEAST LEFT LEVEL LIKE LIMIT LISTEN LOAD LOCAL LOCALTIME LOCALTIMESTAMP LOCATION LOCK MAPPING MATCH MATERIALIZED MAXVALUE MINUTE MINVALUE MODE MONTH MOVE NAME NAMES NATIONAL NATURAL NCHAR NEXT NO NONE NOT NOTHING NOTIFY NOTNULL NOWAIT NULL NULLIF NULLS NUMERIC OBJECT OF OFF OFFSET OIDS ON ONLY OPERATOR OPTION OPTIONS OR ORDER ORDINALITY OUT OUTER OVER OVERLAPS OVERLAY OWNED OWNER PARSER PARTIAL PARTITION PASSING PASSWORD PLACING PLANS POSITION PRECEDING PRECISION PREPARE PREPARED PRESERVE PRIMARY PRIOR PRIVILEGES PROCEDURAL PROCEDURE PROGRAM QUOTE RANGE READ REAL REASSIGN RECHECK RECURSIVE REF REFERENCES REFRESH REINDEX RELATIVE RELEASE RENAME REPEATABLE REPLACE REPLICA RESET RESTART RESTRICT RETURNING RETURNS REVOKE RIGHT ROLE ROLLBACK ROW ROWS RULE SAVEPOINT SCHEMA SCROLL SEARCH SECOND SECURITY SELECT SEQUENCE SEQUENCES SERIALIZABLE SERVER SESSION SESSION_USER SET SETOF SHARE SHOW SIMILAR SIMPLE SMALLINT SNAPSHOT SOME STABLE STANDALONE START STATEMENT STATISTICS STDIN STDOUT STORAGE STRICT STRIP SUBSTRING SYMMETRIC SYSID SYSTEM TABLE TABLES TABLESPACE TEMP TEMPLATE TEMPORARY TEXT THEN TIME TIMESTAMP TO TRAILING TRANSACTION TREAT TRIGGER TRIM TRUE TRUNCATE TRUSTED TYPE TYPES UNBOUNDED UNCOMMITTED UNENCRYPTED UNION UNIQUE UNKNOWN UNLISTEN UNLOGGED UNTIL USER USING VACUUM VALID VALIDATE VALIDATOR VALUE VALUES VARCHAR VARIADIC VARYING VERBOSE VERSION VIEW VIEWS VOLATILE WHEN WHERE WHITESPACE WINDOW WITH WITHIN WITHOUT WORK WRAPPER WRITE XML XMLATTRIBUTES XMLCONCAT XMLELEMENT XMLEXISTS XMLFOREST XMLPARSE XMLPI XMLROOT XMLSERIALIZE YEAR YES ZONE");
            richTextBox1.SetKeywords(1, "update UPDATE");

            richTextBox2.StyleResetDefault();
            richTextBox2.Styles[Style.Default].Font = "Consolas";
            richTextBox2.Styles[Style.Default].Size = 10;
            richTextBox2.StyleClearAll();
            richTextBox2.Styles[Style.Cpp.Default].ForeColor = Color.Silver;
            richTextBox2.Styles[Style.Cpp.Comment].ForeColor = Color.FromArgb(0, 128, 0); // Green
            richTextBox2.Styles[Style.Cpp.CommentLine].ForeColor = Color.FromArgb(0, 128, 0); // Green
            richTextBox2.Styles[Style.Cpp.CommentLineDoc].ForeColor = Color.FromArgb(128, 128, 128); // Gray
            richTextBox2.Styles[Style.Cpp.Number].ForeColor = Color.Red;
            richTextBox2.Styles[Style.Cpp.Word].ForeColor = Color.Blue;
            richTextBox2.Styles[Style.Cpp.Word2].ForeColor = Color.Fuchsia;
            richTextBox2.Styles[Style.Cpp.String].ForeColor = Color.FromArgb(163, 21, 21); // Red
            richTextBox2.Styles[Style.Cpp.Character].ForeColor = Color.FromArgb(163, 21, 21); // Red
            richTextBox2.Styles[Style.Cpp.Verbatim].ForeColor = Color.FromArgb(163, 21, 21); // Red
            richTextBox2.Styles[Style.Cpp.Operator].ForeColor = Color.Silver;
            richTextBox2.Styles[Style.Cpp.Preprocessor].ForeColor = Color.Purple;
            richTextBox2.Lexer = Lexer.Cpp;
            richTextBox2.Margins[0].Width = 16;
        
            richTextBox2.SetKeywords(0, "abort absolute access action add admin after aggregate all also alter always analyse analyze and any array as asc assertion assignment asymmetric at attribute authorization backward before begin between bigint binary bit boolean both by cache called cascade cascaded case cast catalog chain char character characteristics check checkpoint class close cluster coalesce collate collation column comment comments commit committed concurrently configuration connection constraint constraints content continue conversion copy cost create cross csv current current_catalog current_date current_role current_schema current_time current_timestamp current_user cursor cycle data database day deallocate dec decimal declare default defaults deferrable deferred definer delete delimiter delimiters desc dictionary disable discard distinct do document domain double drop each else enable encoding encrypted end enum escape event except exclude excluding exclusive execute exists explain extension external extract false family fetch filter first float following for force foreign forward freeze from full function functions global grant granted greatest group handler having header hold hour identity if ilike immediate immutable implicit in including increment index indexes inherit inherits initially inline inner inout input insensitive insert instead int integer intersect interval into invoker is isnull isolation join key label language large last lateral lc_collate lc_ctype leading leakproof least left level like limit listen load local localtime localtimestamp location lock mapping match materialized maxvalue minute minvalue mode month move name names national natural nchar next no none not nothing notify notnull nowait null nullif nulls numeric object of off offset oids on only operator option options or order ordinality out outer over overlaps overlay owned owner parser partial partition passing password placing plans position preceding precision prepare prepared preserve primary prior privileges procedural procedure program quote range read real reassign recheck recursive ref references refresh reindex relative release rename repeatable replace replica reset restart restrict returning returns revoke right role rollback row rows rule savepoint schema scroll search second security select sequence sequences serializable server session session_user set setof share show similar simple smallint snapshot some stable standalone start statement statistics stdin stdout storage strict strip substring symmetric sysid system table tables tablespace temp template temporary text then time timestamp to trailing transaction treat trigger trim true truncate trusted type types unbounded uncommitted unencrypted union unique unknown unlisten unlogged until user using vacuum valid validate validator value values varchar variadic varying verbose version view views volatile when where whitespace window with within without work wrapper write xml xmlattributes xmlconcat xmlelement xmlexists xmlforest xmlparse xmlpi xmlroot xmlserialize year yes zone ABORT ABSOLUTE ACCESS ACTION ADD ADMIN AFTER AGGREGATE ALL ALSO ALTER ALWAYS ANALYSE ANALYZE AND ANY ARRAY AS ASC ASSERTION ASSIGNMENT ASYMMETRIC AT ATTRIBUTE AUTHORIZATION BACKWARD BEFORE BEGIN BETWEEN BIGINT BINARY BIT BOOLEAN BOTH BY CACHE CALLED CASCADE CASCADED CASE CAST CATALOG CHAIN CHAR CHARACTER CHARACTERISTICS CHECK CHECKPOINT CLASS CLOSE CLUSTER COALESCE COLLATE COLLATION COLUMN COMMENT COMMENTS COMMIT COMMITTED CONCURRENTLY CONFIGURATION CONNECTION CONSTRAINT CONSTRAINTS CONTENT CONTINUE CONVERSION COPY COST CREATE CROSS CSV CURRENT CURRENT_CATALOG CURRENT_DATE CURRENT_ROLE CURRENT_SCHEMA CURRENT_TIME CURRENT_TIMESTAMP CURRENT_USER CURSOR CYCLE DATA DATABASE DAY DEALLOCATE DEC DECIMAL DECLARE DEFAULT DEFAULTS DEFERRABLE DEFERRED DEFINER DELETE DELIMITER DELIMITERS DESC DICTIONARY DISABLE DISCARD DISTINCT DO DOCUMENT DOMAIN DOUBLE DROP EACH ELSE ENABLE ENCODING ENCRYPTED END ENUM ESCAPE EVENT EXCEPT EXCLUDE EXCLUDING EXCLUSIVE EXECUTE EXISTS EXPLAIN EXTENSION EXTERNAL EXTRACT FALSE FAMILY FETCH FILTER FIRST FLOAT FOLLOWING FOR FORCE FOREIGN FORWARD FREEZE FROM FULL FUNCTION FUNCTIONS GLOBAL GRANT GRANTED GREATEST GROUP HANDLER HAVING HEADER HOLD HOUR IDENTITY IF ILIKE IMMEDIATE IMMUTABLE IMPLICIT IN INCLUDING INCREMENT INDEX INDEXES INHERIT INHERITS INITIALLY INLINE INNER INOUT INPUT INSENSITIVE INSERT INSTEAD INT INTEGER INTERSECT INTERVAL INTO INVOKER IS ISNULL ISOLATION JOIN KEY LABEL LANGUAGE LARGE LAST LATERAL LC_COLLATE LC_CTYPE LEADING LEAKPROOF LEAST LEFT LEVEL LIKE LIMIT LISTEN LOAD LOCAL LOCALTIME LOCALTIMESTAMP LOCATION LOCK MAPPING MATCH MATERIALIZED MAXVALUE MINUTE MINVALUE MODE MONTH MOVE NAME NAMES NATIONAL NATURAL NCHAR NEXT NO NONE NOT NOTHING NOTIFY NOTNULL NOWAIT NULL NULLIF NULLS NUMERIC OBJECT OF OFF OFFSET OIDS ON ONLY OPERATOR OPTION OPTIONS OR ORDER ORDINALITY OUT OUTER OVER OVERLAPS OVERLAY OWNED OWNER PARSER PARTIAL PARTITION PASSING PASSWORD PLACING PLANS POSITION PRECEDING PRECISION PREPARE PREPARED PRESERVE PRIMARY PRIOR PRIVILEGES PROCEDURAL PROCEDURE PROGRAM QUOTE RANGE READ REAL REASSIGN RECHECK RECURSIVE REF REFERENCES REFRESH REINDEX RELATIVE RELEASE RENAME REPEATABLE REPLACE REPLICA RESET RESTART RESTRICT RETURNING RETURNS REVOKE RIGHT ROLE ROLLBACK ROW ROWS RULE SAVEPOINT SCHEMA SCROLL SEARCH SECOND SECURITY SELECT SEQUENCE SEQUENCES SERIALIZABLE SERVER SESSION SESSION_USER SET SETOF SHARE SHOW SIMILAR SIMPLE SMALLINT SNAPSHOT SOME STABLE STANDALONE START STATEMENT STATISTICS STDIN STDOUT STORAGE STRICT STRIP SUBSTRING SYMMETRIC SYSID SYSTEM TABLE TABLES TABLESPACE TEMP TEMPLATE TEMPORARY TEXT THEN TIME TIMESTAMP TO TRAILING TRANSACTION TREAT TRIGGER TRIM TRUE TRUNCATE TRUSTED TYPE TYPES UNBOUNDED UNCOMMITTED UNENCRYPTED UNION UNIQUE UNKNOWN UNLISTEN UNLOGGED UNTIL USER USING VACUUM VALID VALIDATE VALIDATOR VALUE VALUES VARCHAR VARIADIC VARYING VERBOSE VERSION VIEW VIEWS VOLATILE WHEN WHERE WHITESPACE WINDOW WITH WITHIN WITHOUT WORK WRAPPER WRITE XML XMLATTRIBUTES XMLCONCAT XMLELEMENT XMLEXISTS XMLFOREST XMLPARSE XMLPI XMLROOT XMLSERIALIZE YEAR YES ZONE");
            richTextBox2.SetKeywords(1, "update UPDATE");
            listView3.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);
            listView3.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

        }


        public void AddMetaData(string table, string transid)
        {
           
            var metadata = new MetaDataParser().GetMetadata(database, table);

            for (int i = 0, x = 0; i < metadata.Count - x; i++)
            {
                if (metadata.ElementAt(i).Type == ColumnType.VarChar)
                {
                    var value = metadata.ElementAt(i);
                    metadata.RemoveAt(i);
                    metadata.Add(value);
                    i = -1;
                    x++;
                }
            }

            List<string>values = new List<string>();
            for (var i = 0; i < metadata.Count; i++)
            {
                
                values.Add(metadata[i].ColumnName);
                values.Add(metadata[i].Type.ToString());
                values.Add("");
                if(tipo=="LOP_MODIFY_ROW")
                    values.Add("");
                ListViewItem lvi = new ListViewItem(values.ToArray());
                listView1.Items.Add(lvi);
                values.Clear();
            }
           
            AddValues(metadata,table,transid);
        }

        public void deleteOps()
        {
            for (int i = 0; i < listView3.Items.Count; i++)
            {

                if (listView3.Items[i].SubItems[0].Text != "LOP_DELETE_ROWS" && listView3.Items[i].SubItems[0].Text != "LOP_INSERT_ROWS" && listView3.Items[i].SubItems[0].Text != "LOP_MODIFY_ROW")
                    listView3.Items.RemoveAt(i);


            }
        }

        public void CargarPrimeraParte(string sql)
        {
            string _connectionString = "Server=CARLOSV;Database= " + dbc.database + ";Trusted_Connection=True;MultipleActiveResultSets=True;";
            var conn = new SqlConnection(_connectionString);
            var cmd = new SqlCommand(sql, conn);
            conn.Open();
            var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
              
                List<string> info = new List<string>();
                info.Add("");
                info.Add(reader[1].ToString());
                info.Add(table);
                info.Add(reader[2].ToString());
                info.Add(reader[3].ToString());
                info.Add("");
                ListViewItem lvi = new ListViewItem(info.ToArray());
                listView3.Items.Add(lvi);
                info.Clear();
            }
        }

        public void CargarParteDos(string sql1)
        {
            string _connectionString = "Server=CARLOSV;Database= " + dbc.database + ";Trusted_Connection=True;MultipleActiveResultSets=True;";

            var conn1 = new SqlConnection(_connectionString);
            var cmd1 = new SqlCommand(sql1, conn1);
            conn1.Open();
            var reader1 = cmd1.ExecuteReader();
            while (reader1.Read())
            {
                for (int j = 0; j < listView3.Items.Count; j++)
                    listView3.Items[j].SubItems[listView3.Items[j].SubItems.Count - 1].Text = reader1[0].ToString();
            }

        }

        public void CargarParteTres(string sql2)
        {
            string _connectionString = "Server=CARLOSV;Database= " + dbc.database + ";Trusted_Connection=True;MultipleActiveResultSets=True;";
            var conn2 = new SqlConnection(_connectionString);
            var cmd2 = new SqlCommand(sql2, conn2);
            conn2.Open();
            var reader2 = cmd2.ExecuteReader();
            int contador = 0;
            while (reader2.Read())
            {
                listView3.Items[contador].SubItems[0].Text = reader2[0].ToString();
                if (contador < listView3.Items.Count - 1)
                    contador++;
            }

            if (!reader2.HasRows)
            {
                int contador1 = 0;
              
                sql2 = "USE " + dbc.database +
                  " SELECT [Operation],[Transaction ID] FROM sys.fn_dblog(NULL, NULL) WHERE AllocUnitName like 'dbo."+table+".PK%';";
                var cmd1 = new SqlCommand(sql2, conn2);

                var reader1 = cmd1.ExecuteReader();

                while (reader1.Read())
                {
                   
                        listView3.Items[contador1].SubItems[0].Text = reader1[0].ToString();
                    
                    if (contador1 < listView3.Items.Count-1)
                    {
                        contador1++;
                    }
                }

            }

        }
        public void FillList()
        {
            listView3.Columns.Clear();
            listView3.Items.Clear();
            List<string>info   = new List<string>();
            listView3.Columns.Add("OPERATION");
            listView3.Columns.Add("TRANSACTION ID");
            listView3.Columns.Add("OBJECT");
            listView3.Columns.Add("USER");
            listView3.Columns.Add("BEGIN TIME");
            listView3.Columns.Add("END TIME");
            List<string> toCompare = null;
            string _connectionString ="Server=CARLOSV;Database= " + dbc.database + ";Trusted_Connection=True;MultipleActiveResultSets=True;";

            if (!dbc.alltables)
            {
                toCompare = new List<string>();
               var query = "USE " + dbc.database +
                    " SELECT [Transaction ID] FROM sys.fn_dblog(NULL, NULL) WHERE AllocUnitName like'dbo." + table + ".PK%';";
              
                var con = new SqlConnection(_connectionString);
                var cmd3 = new SqlCommand(query, con);
                con.Open();
                var reader3 = cmd3.ExecuteReader();
              
                while(reader3.Read())
                {
                toCompare.Add(reader3[0].ToString());   
                }

                if (!reader3.HasRows)
                {
                     query = "USE " + dbc.database +
                   " SELECT [Transaction ID] FROM sys.fn_dblog(NULL, NULL) WHERE AllocUnitName = 'dbo." + table + "';";
                    var cmd1 = new SqlCommand(query, con);

                    var reader1 = cmd1.ExecuteReader();

                    while (reader1.Read())
                    {
                        toCompare.Add(reader1[0].ToString());
                    }

                }
            }

           
            var sql = "";
           
            if(!dbc.alltables)
            for(int i =0; i<toCompare.Count;i++)
                CargarPrimeraParte("USE " + dbc.database + " SELECT [Operation], [Transaction ID],  SUSER_SNAME ([Transaction SID]) AS [USER], [Begin Time] FROM sys.fn_dblog(NULL, NULL) WHERE [Operation] = 'LOP_BEGIN_XACT' AND [Transaction ID] =" + "'" + toCompare[i] + "'");


            if (!dbc.alltables)     
                    CargarParteTres("USE " + dbc.database + " SELECT [Operation] FROM sys.fn_dblog(NULL, NULL) WHERE AllocUnitName = 'dbo." + table + "'; ");


        }

        public void FirstTab(string thistb,string transid)
        {
            listView1.Columns.Clear();
            listView1.Items.Clear();
            listView1.Columns.Add("FIELD");
            listView1.Columns.Add("TYPE");
            if (tipo == "LOP_MODIFY_ROW")
            {
                listView1.Columns.Add("BEFORE VALUE");
                listView1.Columns.Add("AFTER VALUE");
            }
            else
            {
                listView1.Columns.Add("VALUE");
            }
            AddMetaData(thistb,transid);
        }


        public void AddAfter(List<MetaData> metadata, string tab, string transid)
        {
            string _connectionString = "Server=CARLOSV;Database= " + dbc.database + ";Trusted_Connection=True;MultipleActiveResultSets=True;";
            int cantidad = 0;

            var rowlog = new MetaDataParser().GetRowLogContentsOne(dbc.database, tab, tipo, transid);



            var converter = new RowLogConversions();

            List<string> valores;

            for (int i = 0; i < listView3.Items.Count; i++)
            {
                if (listView3.Items[i].SubItems[0].Text == "LOP_INSERT_ROWS")
                    anyinsert = listView3.Items[i].SubItems[1].Text;
            }
            for (int i = 0; i < rowlog.Count; i++)
            {
                valores = new List<string>();
                if (tipo == "LOP_MODIFY_ROW")
                {
                    var rowlog1 = new MetaDataParser().GetRowLogContentsCero(dbc.database, tab, "LOP_INSERT_ROWS", anyinsert);
                    cantidad = RowLogConversions.CantidadLongitudFija(rowlog1[0], metadata);
                }



                var values = converter.ParseRowLogContents(rowlog[i], metadata, tipo, cantidad);

                if (values.Count > 0)
                {
                    for (int j = 0; j < values.Count; j++)
                        valores.Add(values[j]);


                    for (int j = 0; j < values.Count; j++)
                    {
                        listView1.Items[j].SubItems[3].Text = values.ElementAt(j);
                    }
                  
                    if (tipo == "LOP_MODIFY_ROW")
                    {


                        undoupdate = "";
                        complete = "";

                        complete += "UPDATE " + dbc.database + " SET ";
                        undoupdate += " WHERE ";
                        for (int k = 0; k < values.Count; k++)
                        {

                            if (metadata[k].Type == ColumnType.Char || metadata[k].Type == ColumnType.VarChar)
                            {

                                undoupdate += metadata.ElementAt(k).ColumnName + " = '" + values.ElementAt(k) + "'";
                                complete += metadata.ElementAt(k).ColumnName + " = '" + values.ElementAt(k) + "'";
                            }
                            else
                            {

                                undoupdate += metadata.ElementAt(k).ColumnName + " = " + values.ElementAt(k);
                                complete += metadata.ElementAt(k).ColumnName + " = " + values.ElementAt(k);
                            }
                            if (k < values.Count - 1)
                            {

                                undoupdate += " AND ";
                                complete += " AND ";
                            }
                            if (k == values.Count - 1)
                            {

                                undoupdate += ";";
                               
                            }
                        }

                        richTextBox1.Text += undoupdate;
                        richTextBox2.Text += complete + redoupdate;
                    }
                }
            }


        }
        public void AddValues(List<MetaData> metadata,string tab,string transid)
        {
            string redo = "";
            var cantidad = 0;
            string undo = "";
             string _connectionString = "Server=CARLOSV;Database= "+dbc.database+";Trusted_Connection=True;MultipleActiveResultSets=True;";


            var rowlog = new MetaDataParser().GetRowLogContentsCero(dbc.database, tab, tipo, transid);
         

           
            var converter = new RowLogConversions();

            List<string> valores;
            for (int i = 0; i < listView3.Items.Count; i++)
            {
                if (listView3.Items[i].SubItems[0].Text == "LOP_INSERT_ROWS")
                    anyinsert = listView3.Items[i].SubItems[1].Text;
            }
            for (int i = 0; i < rowlog.Count; i++)
            {
                valores = new List<string>();

                if (tipo == "LOP_MODIFY_ROW")
                {
                    var rowlog1 = new MetaDataParser().GetRowLogContentsCero(dbc.database, tab, "LOP_INSERT_ROWS",anyinsert);
                    cantidad = RowLogConversions.CantidadLongitudFija(rowlog1[0], metadata);
                }


                var values = converter.ParseRowLogContents(rowlog[i], metadata,tipo,cantidad);
                
                if (values.Count > 0)
                {
                    for (int j = 0; j < values.Count; j++)
                        valores.Add(values[j]);


                    for (int j = 0; j < values.Count; j++)
                    {
                        listView1.Items[j].SubItems[2].Text = values.ElementAt(j);
                       
                    }

                    if (tipo == "LOP_DELETE_ROWS")
                    {
                        redo = "";
                        undo = "";

                        redo += "INSERT INTO " + dbc.database + " VALUES(";
                        undo += "DELETE FROM " + dbc.database + " WHERE ";
                        for (int k = 0; k < values.Count; k++)
                        {
                            if (metadata[k].Type == ColumnType.Char || metadata[k].Type == ColumnType.VarChar)
                            {
                                redo += " '" + values.ElementAt(k) + "'";
                                undo += metadata.ElementAt(k).ColumnName + " = '" + values.ElementAt(k) + "'";
                            }
                            else
                            {
                                redo += " " + values.ElementAt(k);
                                undo += metadata.ElementAt(k).ColumnName + " = " + values.ElementAt(k);
                            }
                            if (k < values.Count - 1)
                            {
                                redo += ",";
                                undo += " AND ";
                            }
                            if (k == values.Count - 1)
                            {
                                redo += ");";
                                undo += ";";
                            }
                        }


                        richTextBox1.Text = redo;
                        richTextBox2.Text = undo;


                    }
                    else if (tipo == "LOP_INSERT_ROWS")
                    {
                        redo = "";
                        undo = "";
                        

                        redo += "INSERT INTO " + dbc.database + " VALUES(";
                        undo += "DELETE FROM " + dbc.database + " WHERE ";
                        for (int k = 0; k < values.Count; k++)
                        {
                            if (metadata[k].Type == ColumnType.Char || metadata[k].Type == ColumnType.VarChar)
                            {
                                redo += " '" + values.ElementAt(k) + "'";
                                undo += metadata.ElementAt(k).ColumnName + " = '" + values.ElementAt(k) + "'";
                            }
                            else
                            {
                                redo += " " + values.ElementAt(k);
                                undo += metadata.ElementAt(k).ColumnName + " = " + values.ElementAt(k);
                            }
                            if (k < values.Count - 1)
                            {
                                redo += ",";
                                undo += " AND ";
                            }
                            if (k == values.Count - 1)
                            {
                                redo += ");";
                                undo += ";";
                            }
                        }

                        richTextBox1.Text = undo;
                        richTextBox2.Text = redo;
                    }
                    else if (tipo == "LOP_MODIFY_ROW")
                    {
                      
                        undoupdate = "";
                        redoupdate = " WHERE ";

                      
                        undoupdate += "UPDATE " + dbc.database + " SET ";
                        for (int k = 0; k < values.Count; k++)
                        {
                            if (metadata[k].Type == ColumnType.Char || metadata[k].Type == ColumnType.VarChar)
                            {
                
                                undoupdate += metadata.ElementAt(k).ColumnName + " = '" + values.ElementAt(k) + "'";
                                redoupdate += metadata.ElementAt(k).ColumnName + " = '" + values.ElementAt(k) + "'";

                            }
                            else
                            {
                                
                                undoupdate += metadata.ElementAt(k).ColumnName + " = " + values.ElementAt(k);
                                redoupdate += metadata.ElementAt(k).ColumnName + " = " + values.ElementAt(k);
                            }
                            if (k < values.Count - 1)
                            {
                             
                                undoupdate += " AND ";
                                redoupdate += " AND ";
                            }
                            if (k == values.Count - 1)
                            {
                              
                               redoupdate += ";";
                            }
                        }

                        richTextBox1.Text = undoupdate;
                      
                    }
                }
            }

            if (tipo == "LOP_MODIFY_ROW")
            {
               AddAfter(metadata,tab,transid);
            }

        }


       

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
          
        }

        private void ribbonControl1_Click(object sender, EventArgs e)
        {

        }


        private void buttonItem14_Click(object sender, EventArgs e)
        {
            DatabaseConnection dtbc = new DatabaseConnection();
            dtbc.ShowDialog();
            dbc = dtbc;
            table = dbc.table;
            database = dbc.database;
            FillList();
            deleteOps();
            listView1.Clear();
            richTextBox1.Clear();
            richTextBox2.Clear();
        }

        private void buttonItem15_Click(object sender, EventArgs e)
        {
            dbc.comboBox1.Enabled = false;
            dbc.ShowDialog();       
            database = dbc.database;
            table = dbc.table;
            FillList();
            deleteOps();
        }

        private void listView3_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void listView1_DoubleClick(object sender, EventArgs e)
        {
            
        }

        private void listView3_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                tipo = listView3.SelectedItems[0].SubItems[0].Text;
                FirstTab(listView3.SelectedItems[0].SubItems[2].Text, listView3.SelectedItems[0].SubItems[1].Text);
            }
            catch (Exception x)
            {
                MessageBox.Show("Modify is From Insert");
            }
        }

        private void listView3_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }
    }
}
