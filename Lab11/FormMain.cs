using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Lab11.Models;
using Npgsql;
namespace Lab11
{
    public partial class FormMain : Form
    {
        private readonly NpgsqlConnection _connection;
        public FormMain()
        {
            InitializeComponent();
            Continent continent = new Continent();
            
            _connection = new NpgsqlConnection("Host=localhost;Port=5433;Database=dotnet12;Username=postgres;Password=********");
        }

        private void toolStripButton1_Click(object sender, System.EventArgs e)
        {
            NpgsqlConnection con = new NpgsqlConnection("Host=localhost;Port=5433;Database=dotnet12;Username=postgres;Password=********");
            con.Open();
            string sql = ("SELECT * FROM sport_clubs");
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, con);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];
            dataGridView2.DataSource = dt;
            con.Close();

        }

        private void toolStripButton2_Click(object sender, System.EventArgs e)
        {
            FormUser formUser = new FormUser
            {
                User = new Country()
            };
            if (formUser.ShowDialog() == DialogResult.OK)
            {
                Country.Insert(_connection, formUser.User);
            }
        }

        private void toolStripButton3_Click(object sender, System.EventArgs e)
        {
            Country temp= new Country();    
            DataGridViewRow selectedRow = dataGridView2.SelectedRows[0];
            if (selectedRow != null)
            {
                temp.Id = (int)selectedRow.Cells["id"].Value;
                temp.Name = selectedRow.Cells["name"].Value.ToString();
                
                temp.ContinentId = selectedRow.Cells["kind_name"].Value.ToString();

                if (selectedRow.Cells["photo"].Value != DBNull.Value)
                {
                    temp.Photo = (byte[])selectedRow.Cells["photo"].Value;
                }
            }
            FormUser formUser = new FormUser
            {
                User = temp
            };
          
            if (formUser.ShowDialog() == DialogResult.OK)
            {
                Country.Update(_connection, formUser.User);
            }

        }

        private void toolStripButton4_Click(object sender, System.EventArgs e)
        {
            if (dataGridView2.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView2.SelectedRows[0];

                int Id = Convert.ToInt32(selectedRow.Cells["id"].Value);
                    
                Country.Delete(_connection,Id);
            }
        }

        private void listViewUsers_SelectedIndexChanged(object sender, System.EventArgs e)
        {

        }

        private void tabPage2_Click(object sender, System.EventArgs e)
        {

        }



        private DataSet ds = new DataSet();
        private DataTable dt = new DataTable();
        private void buttonSave_Click(object sender, System.EventArgs e)

        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                propertyGrid1.SelectedObject = dataGridView1.SelectedRows[0];
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                if (selectedRow.Cells["name"].Value.ToString().Count() == 0)
                {
                    errorProvider1.SetError(dataGridView1, "Не указано название!");
                    return;
                }
                Continent selectedcont = new Continent
                {
                    
                    Name = selectedRow.Cells["name"].Value.ToString(),
                    
                };
                Continent.Insert(selectedcont);
            }
        }
        private void buttonUpdate_Click(object sender, System.EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                if (Convert.ToInt32(selectedRow.Cells["id"].Value) == 0 || selectedRow.Cells["name"].Value.ToString().Count() == 0)
                { 
                    
                  errorProvider1.SetError(dataGridView1, "Не указано название или уникальный индефикатор!"); 
                    return;
                }
                Continent selectedcont = new Continent
                {
                    Id = Convert.ToInt32(selectedRow.Cells["id"].Value),
                    Name = selectedRow.Cells["name"].Value.ToString(),

                };
                Continent.Update(selectedcont);
            }
        }

        private void buttonRefresh_Click(object sender, System.EventArgs e)
        {
            NpgsqlConnection con = new NpgsqlConnection("Host=localhost;Port=5433;Database=dotnet12;Username=postgres;Password=********");
            con.Open();
            string sql = ("SELECT * FROM sport_kinds");
            NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, con);
            ds.Reset();
            da.Fill(ds);
            dt = ds.Tables[0];
            dataGridView1.DataSource = dt;
            con.Close();

        }

        private void buttonDelete_Click(object sender, System.EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
                Continent selectedcont = new Continent
                {
                    Id = Convert.ToInt32(selectedRow.Cells["id"].Value),
                    Name = selectedRow.Cells["name"].Value.ToString(),

                };
                Continent.Delete(selectedcont);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Print_Click(object sender, EventArgs e)
        {
            // Проверка, выбрана ли какая-то строка
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите строку для печати.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += new PrintPageEventHandler(Print_Click1);

            PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
            printPreviewDialog.Document = printDocument;

            // Открывает диалог предварительного просмотра
            if (printPreviewDialog.ShowDialog() == DialogResult.OK)
            {
                // Если пользователь нажал "ОК" в предварительном просмотре, тогда печатаем
                printDocument.Print();
            }
        }
        private void Print_Click1(object sender, PrintPageEventArgs e)
        {
            Graphics graphics = e.Graphics;
            Font font = new Font("Arial", 12);
            string reportContent = "Содержимое текущей записи:\n";
            DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];
            reportContent += "Поле 1: " + selectedRow.Cells["name"].Value.ToString() + "\n";
            reportContent += "Поле 2: " + selectedRow.Cells["id"].Value.ToString() + "\n";
            graphics.DrawString(reportContent, font, Brushes.Black, 100, 100);
            e.HasMorePages = false;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (!IsRowSelected())
            {
                MessageBox.Show("Не выбрана строка.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            PrintDocument printDocument = new PrintDocument();
            printDocument.PrintPage += new PrintPageEventHandler(Print_Click2);

            PrintPreviewDialog printPreviewDialog = new PrintPreviewDialog();
            printPreviewDialog.Document = printDocument;

            // Открывает диалог предварительного просмотра
            if (printPreviewDialog.ShowDialog() == DialogResult.OK)
            {
                // Если пользователь нажал "ОК" в предварительном просмотре, тогда печатаем
                printDocument.Print();
            }
        }

        private void Print_Click2(object sender, PrintPageEventArgs e)
        {
            Graphics graphics = e.Graphics;
            Font font = new Font("Arial", 12);
            string reportContent = "Содержимое текущей записи:\n";
            DataGridViewRow selectedRow = dataGridView2.SelectedRows[0];
            reportContent += "Поле 1: " + selectedRow.Cells["name"].Value.ToString() + "\n";

            byte[] photoByteArray = selectedRow.Cells["photo"].Value as byte[];
            if (photoByteArray != null)
            {
                using (MemoryStream stream = new MemoryStream(photoByteArray))
                {
                    Image photoImage = Image.FromStream(stream);

                    // Отобразите изображение на странице
                    graphics.DrawImage(photoImage, 100, 200); // Разместите изображение в нужном месте на странице
                }
            }

            reportContent += "Поле 2: (изображение)\n";
            graphics.DrawString(reportContent, font, Brushes.Black, 100, 100);
            e.HasMorePages = false;
        }

        private bool IsRowSelected()
        {
            if (dataGridView2.SelectedRows.Count == 0)
            {
                return false;
            }
            return true;
        }


        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            // Вызывает проверку выбора строки при изменении выбора в DataGridView
            IsRowSelected();
        }



        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }
    }
}
