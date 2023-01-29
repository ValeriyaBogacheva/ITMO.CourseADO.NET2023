using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace zachet
{
    public partial class main : Form
    {
        private string Table;
        private SqlConnection NorthwindConnection;
        private SqlDataAdapter SqlDataAdapter1;
        private DataSet NorthwindDataSet;
        DataTable dataTable;

        public main()
        {
            InitializeComponent();
            closing.Enabled = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        }

        private void connecting_Click(object sender, EventArgs e)
        {
            connecting con = new connecting();
            if (con.ShowDialog() != DialogResult.OK)
                return;
            try
            {
                string param = $"Server=(local);Initial Catalog=Northwind;Password={con.Password};User Id={con.Login};Integrated Security=False;";
                NorthwindConnection = new SqlConnection(param);
                NorthwindConnection.Open();
                DataSet ds = new DataSet();
                ToolStripMenuItem menuItem = new ToolStripMenuItem("Таблицы");
                SqlCommand smd = new SqlCommand("SELECT TABLE_NAME FROM Northwind.INFORMATION_SCHEMA.TABLES WHERE TABLE_TYPE = 'BASE TABLE'", NorthwindConnection);
                SqlDataReader sdr = smd.ExecuteReader();
                while (sdr.Read())
                {
                    menuItem.DropDownItems.Add(sdr["TABLE_NAME"].ToString());
                    menuItem.DropDownItems[menuItem.DropDownItems.Count - 1].Click += new EventHandler(clickOnTable);
                }

                menuStrip1.Items.Add(menuItem);
                NorthwindConnection.Close();
                NorthwindDataSet = new DataSet("Northwind");

                connecting.Enabled = false;
                closing.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void closing_Click(object sender, EventArgs e)
        {
            SqlDataAdapter1 = new SqlDataAdapter();
            NorthwindDataSet = new DataSet();
            NorthwindConnection = new SqlConnection();
            dataGridView1.Columns.Clear();
            menuStrip1.Items.RemoveAt(1);

            connecting.Enabled = true;
            closing.Enabled = false;

        }
        private void clickOnTable(object sender,System.EventArgs e) 
        {
            
            SqlDataAdapter1 = new SqlDataAdapter($"SELECT * FROM [{sender}]", NorthwindConnection);

            dataTable = new DataTable(sender.ToString());
            NorthwindDataSet = new DataSet();
            NorthwindDataSet.Tables.Add(dataTable);

            SqlDataAdapter1.Fill(NorthwindDataSet.Tables[sender.ToString()]);
            dataGridView1.DataSource = NorthwindDataSet.Tables[$"{sender}"];
            Table = sender.ToString();
            SqlCommandBuilder commands = new SqlCommandBuilder(SqlDataAdapter1);


        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                NorthwindDataSet.EndInit();
                SqlDataAdapter1.Update(NorthwindDataSet.Tables[Table]);
            }catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        

        private void addRow_Click(object sender, EventArgs e)
        {
            DataRow row = dataTable.NewRow();
            dataTable.Rows.Add(row);
        }

        private void deleteRow_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                try
                {
                    dataGridView1.Rows.Remove(dataGridView1.SelectedRows[0]);
                }catch(Exception er)
                {
                    MessageBox.Show(er.Message);
                }
            }
        }
    }
}
