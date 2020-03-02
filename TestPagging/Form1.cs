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

namespace TestPagging
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        DataTable dataTable = new DataTable();
        SqlDataAdapter SqlDataAdapter = new SqlDataAdapter();
        SqlCommand SqlCommand;
        SqlConnection sqlConnection;
        int offset = 0;
        private void button1_Click(object sender, EventArgs e)
        {            
            if (offset>0)
            {
                offset -= 50;
                GetData(offset, 50);
                button1.Enabled = true;
            }                       
        }
        public bool DbConnection()
        {
            try
            {
                sqlConnection = new SqlConnection(@"Data Source=DESKTOP-3UFDSE4\ANURAGSQL;Initial Catalog=PaggingDb;Persist Security Info=True;User ID=sa;Password=8299156008");
                //SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlConnection.Close();
                return true;
            }
            catch (Exception ex)
            {
                return false;               
            }
        }

        public DataTable GetData()
        {
            sqlConnection.Open();            
            SqlDataAdapter = new SqlDataAdapter("select Top 50 * from tbl_data",sqlConnection);
            SqlDataAdapter.Fill(dataTable);
            sqlConnection.Close();
            return dataTable;
        }
        public DataTable GetData(int offset,int rows)
        {
            sqlConnection.Open();
            dataTable.Clear();
            SqlDataAdapter = new SqlDataAdapter("select * from tbl_data order by Id offset "+offset+" rows fetch next "+rows+" rows only", sqlConnection);
            SqlDataAdapter.Fill(dataTable);
            sqlConnection.Close();
            return dataTable;
        }
        public DataTable GetAll()
        {
            sqlConnection.Open();
            dataTable.Clear();
            SqlDataAdapter = new SqlDataAdapter("select * from tbl_data", sqlConnection);
            DataTable data = new DataTable();
            SqlDataAdapter.Fill(data);
            sqlConnection.Close();
            return data;
        }
        public void FillData()
        {
            int count = 1;
            sqlConnection.Open();

            for (int i = count; i <=50; i++)
            {
                SqlCommand = new SqlCommand("insert into tbl_data values('Gaurav','Himanchal')", sqlConnection);
                SqlCommand.ExecuteNonQuery();
                count++;
            }
            sqlConnection.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (DbConnection())
            {
                //FillData();
                dataGridView1.DataSource = GetData();
                button1.Enabled = false;
            }
            else
            {

            }
        }

           
        private void button2_Click(object sender, EventArgs e)
        {
            if (offset == 0)
            {
                offset = 50;
                button1.Enabled = true;
            }
            DataTable table = new DataTable();
            table = GetAll();
            
            if (dataGridView1.Rows.Count< table.Rows.Count)
            {
                GetData(offset, 50);
                offset += 50;
            }
            else
            {
                button2.Enabled = false;
            }
            
        }
    }
}
