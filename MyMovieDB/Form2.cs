using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyMovieDB
{
    public partial class Form2 : Form
    {
        private string year, publisher, title, type, movieID;

        private void Form2_Load(object sender, EventArgs e)
        {
            textBox1.Text = title;
            textBox2.Text = publisher;
            textBox3.Text = year;
            comboBox1.Text = type;
        }

        public Form2(string Title, string Publisher, string Year, string Type, string MovieID)
        {
            InitializeComponent();
            this.title = Title;
            this.publisher = Publisher;
            this.year = Year;
            this.type = Type;
            this.movieID = MovieID;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 f1 = new Form1();
            string SQLUpdateString;
            string typeString;
            title = textBox1.Text.ToString();
            publisher = textBox2.Text.ToString();
            year = textBox3.Text.ToString();
            int yr = 0;
            if (year != "")
            {
                yr = f1.CheckYear(year);
            }
            try
            {
                typeString = f1.convertType(comboBox1.SelectedItem.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("You need to select movie type! \nError: " + ex.Message + "");
                return;
            }

            if (yr != 1)
            {
                if (year == "")
                {
                    SQLUpdateString = "UPDATE MyMovieDB2.dbo.movie SET Title ='" + title.Replace("'", "''") + "', MovieYear=NULL, Publisher='" + publisher + "', typeID='" + typeString + "' WHERE movieID ='" + movieID + "'";
                }
                else
                {
                    SQLUpdateString = "UPDATE MyMovieDB2.dbo.movie SET Title ='" + title.Replace("'", "''") + "', MovieYear=" + yr + ", Publisher='" + publisher + "', typeID='" + typeString + "' WHERE movieID='" + movieID + "'";
                }

                f1.dbInsertDelUpdate(SQLUpdateString);
                f1.SelectAllQuerry();
                this.Close();
            }
            else
            {
                MessageBox.Show("The year format is not correct!\nPlease try to pick a valid year.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox3.Clear();
                textBox3.Focus();
            }

        }
    }
}
