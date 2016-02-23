using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BookItDesktop
{
    /// <summary>
    /// Interaction logic for Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        DisplayBookmarkList dbl;
        MainWindow mainWin;
        public Register(MainWindow m1)
        {
            InitializeComponent();
            dbl = new DisplayBookmarkList();
            this.mainWin = m1;
        }

        private static string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string password = txtBoxPassword.Text;
            string confirmPassword = txtBoxConfirmPassword.Text;
            string email = txtBoxEmail.Text;
            if(password == confirmPassword)
            {
                string hashedPassword = HashPassword(password);
                bool userRegisterd = dbl.registerUser(email, hashedPassword);
                if (userRegisterd)
                {
                    MessageBox.Show("Registration Ok");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Error while creating user");
                }
                
            }
            else
            {
                MessageBox.Show("Password's dont match");
            }
        }
    }
}
