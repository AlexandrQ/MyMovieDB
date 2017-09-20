using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;

using System.Data.Sql;
using System.Data.SqlClient;

namespace MyMovieDB
{
    public partial class Form1 : Form
    {
        SqlConnection conn = new SqlConnection(@"Data source = HOME\SQLEXPRESS; Initial Catalog = MyMovieDB2; Integrated Security = true;");
        private string querryStr;

        DataGridViewButtonColumn editButton;
        DataGridViewButtonColumn deleteButton;
        DataGridViewButtonColumn playButton;

        public Form1()
        {
            InitializeComponent();
            addButtonInDataGrid();            
        }

        public void dbInsertDelUpdate(string qStr)
        {
            dataGridView1.Rows.Clear();
            try
            {
                conn.Open();
                using (SqlCommand sql = conn.CreateCommand())
                {
                    SqlCommand com = new SqlCommand(qStr, conn);
                    SqlDataReader read = com.ExecuteReader();    
                }
                conn.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!");
                conn.Close();
            }
        }

        private void dbSelect(string qStr)
        {
            dataGridView1.Rows.Clear();
            try
            {
                conn.Open();
                using (SqlCommand sql = conn.CreateCommand())
                {                    
                    SqlCommand com = new SqlCommand(qStr, conn);
                    SqlDataReader read = com.ExecuteReader();
                    

                    if (read.HasRows)
                    {
                        while (read.Read())
                        {
                            dataGridView1.Rows.Add(read["movieID"].ToString(),
                        read["Title"].ToString(),
                        read["Publisher"].ToString(),                        
                        read["MovieYear"].ToString(),
                        read["type"].ToString());
                        }
                    }

                    else MessageBox.Show("Read was not rows", "Error");


                }
                conn.Close();
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!");
                conn.Close();
            }
        }

        public string convertType(string typeStr)
        {
            string type = "";
            if (typeStr == "Adventure") type = "1";
            if (typeStr == "Comedy") type = "2";
            if (typeStr == "Action") type = "3";
            if (typeStr == "Cartoon") type = "4";
            if (typeStr == "Romantic") type = "5";
            if (typeStr == "Fantasy") type = "6";
            if (typeStr == "Thriller") type = "7";
            if (typeStr == "Historic") type = "8";
            if (typeStr == "Drama") type = "9";
            if (typeStr == "Horor") type = "10";
            if (typeStr == "Sci-Fi") type = "11";
            if (typeStr == "Crime") type = "12";
            if (typeStr == "Biografy") type = "13";
            if (typeStr == "Documentary") type = "14";
            return type;
        }

        public int CheckYear(string year)
        {
            int yr = int.Parse(year);
            if (yr >= 2100 || yr <= 1900)
            {
                return 1;
            }
            else
            {
                return yr;
            }
        }

        public void SelectAllQuerry()
        {
            querryStr = "SELECT movieID ,Title ,Publisher ,MovieYear ,MyMovieDB2.dbo.movietype.Type FROM MyMovieDB2.dbo.movie, MyMovieDB2.dbo.movietype WHERE movie.typeID = movietype.typeID";
            dbSelect(querryStr);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            querryStr = string.Format("SELECT movieID ,Title ,Publisher ,MovieYear ,MyMovieDB2.dbo.movietype.Type FROM MyMovieDB2.dbo.movie, MyMovieDB2.dbo.movietype WHERE movie.typeID = movietype.typeID AND Title = '{0}'", textBox4.Text);
            dbSelect(querryStr);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            querryStr = string.Format("SELECT movieID ,Title ,Publisher ,MovieYear ,MyMovieDB2.dbo.movietype.Type FROM MyMovieDB2.dbo.movie, MyMovieDB2.dbo.movietype WHERE movie.typeID = movietype.typeID AND MyMovieDB2.dbo.movie.typeID = {0}", convertType(comboBox2.SelectedItem.ToString()));
            dbSelect(querryStr);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            string firstYear = textBox5.Text.ToString();
            string secondYear = textBox6.Text.ToString();
            int yr1 = CheckYear(firstYear);
            int yr2 = CheckYear(secondYear);
            if ((yr1 != 1 && yr2 != 1) && yr1 <= yr2)
            {
                querryStr = string.Format("SELECT movieID ,Title ,Publisher ,MovieYear ,MyMovieDB2.dbo.movietype.Type FROM MyMovieDB2.dbo.movie, MyMovieDB2.dbo.movietype WHERE movie.typeID = movietype.typeID AND MovieYear BETWEEN {0} AND {1}", yr1, yr2);
                dbSelect(querryStr);
            }                
        }

        private void tabPage2_Enter(object sender, EventArgs e)
        {            
            SelectAllQuerry();
        }

        private void tabPage3_Enter(object sender, EventArgs e)
        {            
            SelectAllQuerry();
        }        

        private void tabPage4_Enter(object sender, EventArgs e)
        {            
            SelectAllQuerry();
        }

        private void tabPage5_Enter(object sender, EventArgs e)
        {            
            SelectAllQuerry();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            textBox4.Clear();            
            SelectAllQuerry();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string typeString;
            try
            {
                typeString = comboBox1.SelectedItem.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("You must enter movie type\nError: " + ex.Message + "");
                return;
            }            
            string name = textBox1.Text.ToString();
            string publisher = textBox2.Text.ToString();
            string year = textBox3.Text.ToString();
            typeString = convertType(typeString);

            querryStr = string.Format("INSERT INTO MyMovieDB2.dbo.movie(Title, Publisher, Previewed, MovieYear, typeID) VALUES('{0}','{1}','No',{2},{3})", name.Replace("'", "''"), publisher, year, typeString);
            
            dbInsertDelUpdate(querryStr);

            textBox1.Clear();
            textBox2.Clear();
            textBox3.Clear();
            comboBox1.SelectedItem = null;
        }       

        private void addButtonInDataGrid()
        {
            //DataGridViewButtonColumn playButton;
            playButton = new DataGridViewButtonColumn();
            playButton.HeaderText = "";
            playButton.Text = "Воспроизвести";
            playButton.UseColumnTextForButtonValue = true;
            playButton.Width = 100;
            dataGridView1.Columns.Add(playButton);

            //DataGridViewButtonColumn editButton;
            editButton = new DataGridViewButtonColumn();
            editButton.HeaderText = "";
            editButton.Text = "Изменить";
            editButton.UseColumnTextForButtonValue = true;
            editButton.Width = 80;
            dataGridView1.Columns.Add(editButton);

            //DataGridViewButtonColumn deleteButton;
            deleteButton = new DataGridViewButtonColumn();
            deleteButton.HeaderText = "";
            deleteButton.Text = "Удалить";
            deleteButton.UseColumnTextForButtonValue = true;
            deleteButton.Width = 80;
            dataGridView1.Columns.Add(deleteButton);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int currentRow = int.Parse(e.RowIndex.ToString());
            string movieIDString = "";

            try
            {
                movieIDString = dataGridView1[0, currentRow].Value.ToString();                
            }
            catch (Exception ex)
            {
             //   MessageBox.Show(ex.Message);
            }

            if (dataGridView1.Columns[e.ColumnIndex] == editButton && currentRow >= 0)
            {
                string title = dataGridView1[1, currentRow].Value.ToString();
                string publisher = dataGridView1[2, currentRow].Value.ToString();                
                string year = dataGridView1[3, currentRow].Value.ToString();
                string type = dataGridView1[4, currentRow].Value.ToString();                
                Form2 f2 = new Form2(title, publisher, year, type, movieIDString);
                
                f2.Show();

            }

            if (dataGridView1.Columns[e.ColumnIndex] == deleteButton && currentRow >= 0)
            {
                querryStr = string.Format("DELETE FROM MyMovieDB2.dbo.movie WHERE movieID = {0}", movieIDString);                
                dbInsertDelUpdate(querryStr);
                querryStr = "SELECT movieID ,Title ,Publisher ,MovieYear ,MyMovieDB2.dbo.movietype.Type FROM MyMovieDB2.dbo.movie, MyMovieDB2.dbo.movietype WHERE movie.typeID = movietype.typeID";
                dbSelect(querryStr);
            }

            if (dataGridView1.Columns[e.ColumnIndex] == playButton && currentRow >= 0)
            {
                string movieName = dataGridView1[1, currentRow].Value.ToString();
                movieName = movieName.Replace(" ", string.Empty);

                try
                {
                    Process p = Process.Start("C:\\MyMovieFolder\\" + movieName + ".avi");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }                
            }
        }
    }
}
