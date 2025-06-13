using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CarRentalApp
{
    public partial class Login : Form
    {
        private readonly CarRentalEntities _db;
        public Login()
        {
            InitializeComponent();
            _db = new CarRentalEntities();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                SHA256 sha = SHA256.Create();

                var username = tbUsername.Text.Trim();
                var password = tbPassword.Text;

                byte[] data = sha.ComputeHash(Encoding.UTF8.GetBytes(password));

                StringBuilder sBuilder = new StringBuilder();

                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                var hashed_password = sBuilder.ToString();

                var user = _db.Users.FirstOrDefault(q => q.username == username && q.password == hashed_password);
                if(user == null)
                {
                    MessageBox.Show("Invalid username or password. Please try again.");
                }
                else
                {
                    var mainWindow = new MainWindow(this);
                    mainWindow.Show();
                    this.Hide(); // Hide the login form
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something went wrong. Please try again");
            }
        }
    }
}
