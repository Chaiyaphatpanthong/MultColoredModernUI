using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace MultColoredModernUI.Forms
{
    public partial class FormProducts : Form
    {
        private MySqlConnection connection;
        private string server;
        private string database;
        private string user;
        private string password;
        private string port;
        private string connectionString;
        private string sslM;
        public FormProducts()
        {
            InitializeComponent();
            LoadTheme();

            server = "localhost";
            database = "db_product";
            user = "root";
            password = "";
            port = "3306";
            sslM = "none";

            connectionString = String.Format("server={0}; port={1}; user id={2}; password={3}; database={4}; SslMode={5}",
                server, port, user, password, database, sslM);
            connection = new MySqlConnection(connectionString);
        }

        private void LoadTheme()
        {
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is System.Windows.Forms.Button)
                {
                    System.Windows.Forms.Button btn = (System.Windows.Forms.Button)ctrl;
                    btn.BackColor = Color.FromArgb(51, 51, 76);
                    btn.ForeColor = Color.White;
                    btn.FlatAppearance.BorderColor = ThemeColor.SecondaryColor;
                }
                else if (ctrl is Label && (ctrl.Name == "label4" || ctrl.Name == "label5"))
                {
                    Label label = (Label)ctrl;
                    if (label.Name == "label4")
                        label.ForeColor = ThemeColor.SecondaryColor;
                    else if (label.Name == "label5")
                        label.ForeColor = ThemeColor.PrimaryColor;
                }
            }
        }



        private void FormProducts_Load(object sender, EventArgs e)
        {
            listView1.Columns.Add("ProductID", "Product_Id");
            listView1.Columns.Add("ProductName", "productName");
            listView1.Columns.Add("ProductDescription", "productDescription");
            listView1.Columns.Add("Price", "Price");
            listView1.Columns.Add("StockStart", "stockStart");
            listView1.Columns.Add("QuantityInStock", "quantityInStock");
            listView1.Columns.Add("UnitOfMeasure", "unitOfMeasure");
            listView1.Columns.Add("ProductCategory", "productCategory");
            listView1.Columns.Add("Brand", "Brand");
            listView1.Columns.Add("Image", "Image");
            listView1.Columns.Add("Manufacturer", "Manufacturer");
            listView1.Columns.Add("DataAdded", "dataAdded");
            listView1.Columns.Add("Availability", "availabilityStatus");
        }

        private void dgv_ShowData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void LoadToListView()
        {
            try
            {
                connection.Open();

                string query = "SELECT * FROM products_list";
                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    using (MySqlDataReader dataReader = cmd.ExecuteReader())
                    {
                        // Clear existing items in the ListView
                        listView1.Items.Clear();

                        while (dataReader.Read())
                        {
                            // Create ListViewItem for each row
                            ListViewItem item = new ListViewItem();

                            // Add columns to the ListViewItem
                            for (int i = 0; i < dataReader.FieldCount; i++)
                            {
                                item.SubItems.Add(dataReader[i].ToString());
                            }

                            // Add the ListViewItem to the ListView
                            listView1.Items.Add(item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }




        private void btn_ChooseFile_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files | *.jpg; *.jpeg; *.png; *.gif; *.bmp";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    // Update the label with the selected file name
                    lb_Nofilechoose.Text = openFileDialog.SafeFileName;
                }
            }
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            string productId = tb_ProductID.Text;
            string productName = tb_ProductName.Text;
            string productDescription = tb_ProductDesciption.Text;
            int price = Convert.ToInt32(tb_Price.Text);
            int stockStart = Convert.ToInt32(tb_StockStart.Text);
            int quantityInStock = Convert.ToInt32(tb_QuantityinStock.Text);
            string unitOfMeasure = tb_UnitofMeasure.Text;
            string productCategory = cb_ProductCategory.SelectedItem?.ToString() ?? "";
           
            string brand = cb_Brand.SelectedItem?.ToString() ?? "";
         

            string manufacturer = tb_Manufacturer.Text;
            DateTime dataAdded = dtp_DataAdded.Value;
            string availabilityStatus = rbtn_Availabiliy.Checked ? "Available" : "Not for Sale";
           


            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    string query = "INSERT INTO products_list (Product_Id, ProductName, ProductDescription, Price, StockStart, QuantityInStock, " +
                        "UnitOfMeasure, ProductCategory, Brand, Manufacturer, DataAdded, Availability) " +
                        "VALUES (@Product_Id, @ProductName, @ProductDescription, @Price, @StockStart, @QuantityInStock, " +
                        "@UnitOfMeasure, @ProductCategory, @Brand, @Manufacturer, @DataAdded, @Availability)";

                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@Product_Id", productId);
                        cmd.Parameters.AddWithValue("@ProductName", productName);
                        cmd.Parameters.AddWithValue("@ProductDescription", productDescription);
                        cmd.Parameters.AddWithValue("@Price", price);
                        cmd.Parameters.AddWithValue("@StockStart", stockStart);
                        cmd.Parameters.AddWithValue("@QuantityInStock", quantityInStock);
                        cmd.Parameters.AddWithValue("@UnitOfMeasure", unitOfMeasure);
                        cmd.Parameters.AddWithValue("@ProductCategory", productCategory);
                        cmd.Parameters.AddWithValue("@Brand", brand);
                        cmd.Parameters.AddWithValue("@Manufacturer", manufacturer);
                        cmd.Parameters.AddWithValue("@DataAdded", dataAdded);
                        cmd.Parameters.AddWithValue("@Availability", availabilityStatus);

                        cmd.ExecuteNonQuery();
                    }

                    MessageBox.Show("Product added successfully.");
                    LoadToListView(); // เปลี่ยนจาก LoadToDataGridView() เป็น LoadToListView()
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: " + ex.Message);
                }
            }
        }


        private void InsertProductData()
        {
            string productId = tb_ProductID.Text;
            string productName = tb_ProductName.Text;
            string productDescription = tb_ProductDesciption.Text;
            int price = Convert.ToInt32(tb_Price.Text);
            int stockStart = Convert.ToInt32(tb_StockStart.Text);
            int quantityInStock = Convert.ToInt32(tb_QuantityinStock.Text);
            string unitOfMeasure = tb_UnitofMeasure.Text;
            string manufacturer = tb_Manufacturer.Text;

            string productCategory = cb_ProductCategory.SelectedItem?.ToString() ?? "";
            string brand = cb_Brand.SelectedItem?.ToString() ?? "";
            DateTime dataAdded = dtp_DataAdded.Value;

            string availabilityStatus = rbtn_Availabiliy.Checked ? "Available" : "Not for Sale";

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    try
                    {
                        connection.Open();

                        string query = "INSERT INTO products_list (Product_Id, ProductName, ProductDescription, Price, StockStart, QuantityInStock, " +
                            "UnitOfMeasure, ProductCategory, Brand, Manufacturer, DataAdded, Availability) " +
                            "VALUES (@Product_Id, @ProductName, @ProductDescription, @Price, @StockStart, @QuantityInStock, " +
                            "@UnitOfMeasure, @ProductCategory, @Brand, @Manufacturer, @DataAdded, @Availability)";

                        using (MySqlCommand cmd = new MySqlCommand(query, connection))
                        {
                            cmd.Parameters.AddWithValue("@Product_Id", productId);
                            cmd.Parameters.AddWithValue("@ProductName", productName);
                            cmd.Parameters.AddWithValue("@ProductDescription", productDescription);
                            cmd.Parameters.AddWithValue("@Price", price);
                            cmd.Parameters.AddWithValue("@StockStart", stockStart);
                            cmd.Parameters.AddWithValue("@QuantityInStock", quantityInStock);
                            cmd.Parameters.AddWithValue("@UnitOfMeasure", unitOfMeasure);
                            cmd.Parameters.AddWithValue("@ProductCategory", productCategory);
                            cmd.Parameters.AddWithValue("@Brand", brand);
                            cmd.Parameters.AddWithValue("@Manufacturer", manufacturer);
                            cmd.Parameters.AddWithValue("@DataAdded", dataAdded);
                            cmd.Parameters.AddWithValue("@Availability", availabilityStatus);

                            cmd.ExecuteNonQuery();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error: " + ex.Message);
                    }
                }
        private void btn_Edit_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem selectedItem = listView1.SelectedItems[0];
                selectedItem.SubItems[1].Text = "EditedProductName";
                // แก้ไขข้อมูลสินค้าที่ถูกเลือกใน ListView ตามต้องการ
            }
        }

        private void btn_Refresh_Click(object sender, EventArgs e)
        {
            LoadToListView();
            // รีเฟรชข้อมูลใน ListView โดยเรียกใช้ LoadToListView()
        }


        private void FormProducts_Load_1(object sender, EventArgs e)
        {
            listView1.Columns.Add("Select", 100);
            listView1.Columns.Add("Id", 100);
            listView1.Columns.Add("Name", 100);
            listView1.Columns.Add("Descript", 100);
            listView1.Columns.Add("Price", 100);
            listView1.Columns.Add("StockStart", 100);
            listView1.Columns.Add("Quantity in Stock", 100);
            listView1.Columns.Add("Unit of Measure", 100);
            listView1.Columns.Add("Product Category", 100);
            listView1.Columns.Add("Brand", 100);
            listView1.Columns.Add("Image", 100);
            listView1.Columns.Add("Manfacturer", 100);
            listView1.Columns.Add("Date Added", 100);
            listView1.Columns.Add("Availability Status", 100);


            // เปิดใช้งาน MultiSelect และ FullRowSelect
            listView1.MultiSelect = true;
            listView1.FullRowSelect = true;




        }

        private void rbtn_Availabiliy_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void rbtn_Notforsale_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                tb_ProductID.Text = listView1.SelectedItems[0].SubItems[0].Text; // Id
                tb_ProductName.Text = listView1.SelectedItems[0].SubItems[1].Text; // Name
                tb_ProductDesciption.Text = listView1.SelectedItems[0].SubItems[2].Text; // Descript
                tb_Price.Text = listView1.SelectedItems[0].SubItems[3].Text;
                tb_StockStart.Text = listView1.SelectedItems[0].SubItems[4].Text;
                tb_QuantityinStock.Text = listView1.SelectedItems[0].SubItems[5].Text;
                tb_UnitofMeasure.Text = listView1.SelectedItems[0].SubItems[6].Text;
                cb_ProductCategory.Text = listView1.SelectedItems[0].SubItems[7].Text;
                cb_Brand.Text = listView1.SelectedItems[0].SubItems[8].Text;
                btn_ChooseFile.Text = listView1.SelectedItems[0].SubItems[9].Text;
                tb_Manufacturer.Text = listView1.SelectedItems[0].SubItems[10].Text;
                dtp_DataAdded.Text = listView1.SelectedItems[0].SubItems[11].Text;







            }
        }

    }
}
