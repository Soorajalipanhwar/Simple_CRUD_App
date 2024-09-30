using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace Simple_CRUD_App
{
    public partial class Form1 : Form
    {

        SqlConnection conn = new SqlConnection("Data Source=DESKTOP-HFSKL45;Initial Catalog=SchoolDatabase;Integrated Security=True");
        object name;

        public Form1()
        {
            InitializeComponent();

            dataGridView2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            LoadID();
            LoadSales();            
        }
        public void LoadID()
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("Select Item_id From Stock", conn);
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                comboBox1.Items.Add(reader["item_id"].ToString());
            }   
            conn.Close();
        }
        public void LoadSales()
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("Select * From Sales", conn);
            SqlDataReader reader = cmd.ExecuteReader();
            DataTable dataTable = new DataTable();
            dataTable.Load(reader);
            dataGridView2.DataSource = dataTable;            
            conn.Close();
        }
        
        
        private void button1_Click(object sender, EventArgs e)//Add sales btn
        {
            if (comboBox1.SelectedItem != null)
            {
                SqlCommand cmd = new SqlCommand($"SELECT item_name FROM Stock WHERE item_id = {comboBox1.SelectedItem}", conn);
                conn.Open();
                name = cmd.ExecuteScalar();
                conn.Close();

                conn.Open();
                SqlCommand cmd2 = new SqlCommand("INSERT INTO Sales (item_id, item_name, no_of_items, unit_price) VALUES (" + comboBox1.SelectedItem.ToString() + ",'" + name.ToString() + "'," + textBox1.Text + "," + textBox2.Text + ")", conn);
                int result = cmd2.ExecuteNonQuery();
                if (result > 0)
                {
                    MessageBox.Show("Added Sales record");
                }
                else { MessageBox.Show("Error Adding the record"); }
                conn.Close();

                conn.Open();
                SqlCommand cmd3 = new SqlCommand("Update Stock " +
                    "SET item_quantity = item_quantity - " + textBox1.Text + "" +
                    "Where item_id = " + comboBox1.SelectedItem.ToString() + "", conn);
                cmd3.ExecuteNonQuery();
                conn.Close();

                //to refresh the sales table
                LoadSales();
            }
            textBox1.Clear();
            textBox2.Clear();
        }

        private void button2_Click(object sender, EventArgs e)//Load sales table
        {
            LoadSales();
        } 

        private void button3_Click(object sender, EventArgs e)//Load Stock DAta btn
        {
            conn.Open();
            SqlCommand cmd = new SqlCommand("Select * From Stock", conn);
            SqlDataReader reader = cmd.ExecuteReader();
            DataTable table = new DataTable();
            table.Load(reader);
            dataGridView2.DataSource = table;
            conn.Close();

        }

        private void button4_Click(object sender, EventArgs e)//Delete Sales btn
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                int salesId = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells["sales_id"].Value);
                int itemId = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells["item_id"].Value.ToString());
                int itemQuantity = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells["no_of_items"].Value.ToString());

                conn.Open();
                SqlCommand cmd2 = new SqlCommand("DELETE FROM Sales WHERE sales_id = @salesId", conn);
                cmd2.Parameters.AddWithValue("@salesId", salesId);
                int result = cmd2.ExecuteNonQuery();

                conn.Close();

                if (result > 0)
                {
                    LoadSales(); // Reload the DataGridView after deletion

                    if (dataGridView2.SelectedRows.Count > 0)
                    { 
                        // Update the stock table
                        conn.Open();
                        SqlCommand cmd = new SqlCommand("UPDATE Stock SET item_quantity = item_quantity + " + itemQuantity+ " Where item_id = " + itemId+"", conn);
                        int results = cmd.ExecuteNonQuery();
                        conn.Close();

                        if (results > 0)
                        {
                            MessageBox.Show("Stock quantity restored successfully.");
                        }
                        else
                        {
                            MessageBox.Show("Error restoring stock quantity.");
                        }
                    }

                    LoadSales();
                }
                else
                {
                    MessageBox.Show("Error deleting record.");
                }
            }
            else
            {
                MessageBox.Show("Please select a row to delete.");
            }
        }
    }
}
