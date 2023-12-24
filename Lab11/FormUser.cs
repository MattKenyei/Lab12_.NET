using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Lab11.Models;
using Npgsql;

namespace Lab11
{
    public partial class FormUser : Form
    {
        private Country _user;
        public Country User
        {
            get { return _user; }
            set
            {
                _user = value;
                textBoxLastName.Text = _user.Name;
                if(_user.ContinentId !=null)
                    comboBox1.Items.Add(_user.ContinentId);
                comboBox1.SelectedItem = _user.ContinentId;
                byte[] byteArray = _user.Photo;

               
                if (byteArray != null && byteArray.Length > 0)
                {
                    using (MemoryStream stream = new MemoryStream(byteArray))
                    {
                        pictureBox1.Image = Image.FromStream(stream);
                    }
                }
            }
        }
        public FormUser()
        {
            
            InitializeComponent();
            NpgsqlConnection con = new NpgsqlConnection("Host=localhost;Port=5433;Database=dotnet12;Username=postgres;Password=********");
            con.Open();

            string sql = "SELECT name FROM sport_kinds";

            NpgsqlCommand cmd = new NpgsqlCommand(sql, con);
            NpgsqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string id = reader.GetString(0);
                comboBox1.Items.Add(id);
            }

            reader.Close();
            con.Close();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            User.Name = textBoxLastName.Text;
            Image image = pictureBox1.Image;
            User.ContinentId = (string)comboBox1.SelectedItem;
            
            if (image != null)
            {
                
                using (MemoryStream stream = new MemoryStream())
                {
                    image.Save(stream, System.Drawing.Imaging.ImageFormat.Jpeg); 
                    byte[] byteArray = stream.ToArray();

                    
                    User.Photo = byteArray;
                }
            }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click(object sender, EventArgs e)
        {

            openFileDialog1.Filter = "Изображения (*.jpg; *.png; *.bmp)|*.jpg;*.png;*.bmp|Все файлы (*.*)|*.*";

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string selectedFileName = openFileDialog1.FileName;
                pictureBox1.Image = new Bitmap(selectedFileName);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "Изображения (*.jpg)|*.jpg|Изображения (*.png)|*.png|Изображения (*.bmp)|*.bmp|Все файлы (*.*)|*.*";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string saveFileName = saveFileDialog.FileName;
                    pictureBox1.Image.Save(saveFileName);
                }
            }
            else
            {
                // Вывести сообщение об ошибке, если в PictureBox нет изображения.
                MessageBox.Show("Нет изображения для сохранения.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void labelLastName_Click(object sender, EventArgs e)
        {

        }
    }
}
