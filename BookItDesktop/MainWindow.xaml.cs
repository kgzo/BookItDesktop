using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookItDesktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Entities ent;
        static List<AspNetUsers> user1;
        MainWindow windowRef;
        bool credentialsLoaded = false;
        public MainWindow()
        {
            InitializeComponent();
            user1 = new List<AspNetUsers>();
            ent = new Entities();
            windowRef = this;
            loadCredentials();
            //this.Name = "Book IT";
        }

        void testThis()
        {
            user1 = ent.AspNetUsers.ToList();
        }

        void testOne()
        {
            string userName = txtBoxUserName.Text.ToString();
            string password = txtBoxPassword.Password;
            Thread t1 = new Thread(testThis);
            t1.IsBackground = true;
            t1.Start();
            //MessageBox.Show("s");
            bool userFound = false;
            //bool test = VerifyHashedPassword(hashedPassword, password);
            //MessageBox.Show(hashedPassword);
            //t1.Join();
            t1.Join();
            foreach (AspNetUsers au in user1)
            {
                string hashedPassword = HashPassword(password);
                bool test11 = VerifyHashedPassword(au.PasswordHash, password);
                if (test11 && au.UserName == userName)
                {
                    userFound = true;
                    var mainProgram = new BookIT(au);
                    mainProgram.Show();
                    this.Close();
                    break;
                }


            }
        }
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            /*testOne();
            string userName = txtBoxUserName.Text.ToString();
            string password = txtBoxPassword.Text.ToString();//"AFnnc1gK15QvF8iFmpWdL1xo68LkrJnAqAF+x6Nr3j0FPQz1WSox05iFXzfjdHYpHQ==";
            //List<AspNetUsers> users = ent.AspNetUsers.ToList();//test@dsa.foi
            //List<AspNetUsers> user1 = new List<AspNetUsers>();
            Thread t1 = new Thread(testThis);
            t1.IsBackground = true;
            t1.Start();
            //MessageBox.Show("s");
            bool userFound = false;
            //bool test = VerifyHashedPassword(hashedPassword, password);
            //MessageBox.Show(hashedPassword);
            //t1.Join();
            foreach(AspNetUsers au in user1)
            {
                string hashedPassword = HashPassword(password);
                bool test11 = VerifyHashedPassword(au.PasswordHash, password);
                if (test11 && au.UserName == userName)
                {
                    userFound = true;
                    //MessageBox.Show(au.UserName);
                    //MessageBox.Show(au.PasswordHash);
                    var mainProgram = new BookIT(au);
                    mainProgram.Show();
                    this.Close();
                    break;
                }

               
            }

            if (!userFound)
                MessageBox.Show("Wrong login information");*/

            BackgroundWorker bgWorker;
            bgWorker = new BackgroundWorker();
            bgWorker.DoWork += new DoWorkEventHandler(bgWorker_getUserInfo);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_userDataLoadComplete);

            // Launch background thread to do the work of reading the file.  This will
            // trigger BackgroundWorker.DoWork().  Note that we pass the filename to
            // process as a parameter.
            bgWorker.RunWorkerAsync();
        }

        void bgWorker_getUserInfo(object sender, DoWorkEventArgs e)
        {
            user1 = ent.AspNetUsers.ToList();
        }

        void bgWorker_userDataLoadComplete(object sender, RunWorkerCompletedEventArgs e)
        {
            string userName = txtBoxUserName.Text.ToString();
            string password = txtBoxPassword.Password;
            bool userFound = false;
            foreach (AspNetUsers au in user1)
            {
                string hashedPassword = "";
                if (credentialsLoaded)
                    hashedPassword = HashPassword(password);
                else
                     hashedPassword = password;
                bool test11 = VerifyHashedPassword(au.PasswordHash, password);
                if (test11 && au.UserName == userName)
                {
                    if (checkBoxRemember.IsChecked == true && credentialsLoaded == false)
                    {
                        saveCredentials(userName, hashedPassword);
                    }
                   // userFound = true;
                    //MessageBox.Show(au.UserName);
                    //MessageBox.Show(au.PasswordHash);
                    var mainProgram = new BookIT(au);
                    mainProgram.Show();
                    userFound = true;
                    this.Close();
                    break;
                }


            }
            if (!userFound)
            {
                credentialsLoaded = false;
                MessageBox.Show("Data not found");
            }
        }

        public string HashPassword1(string password, string passwordSalt)
        {
            int saltSize = 128 / 8;//128 bits = 16 bytes
            int PBKDF2SubkeyLength = 256 / 8; //256 bits = 32 bytes
            byte[] salt;
            byte[] subkey;
            using (var deriveBytes = new Rfc2898DeriveBytes(password, saltSize, 1000))
            {
                salt = deriveBytes.Salt;
                subkey = deriveBytes.GetBytes(PBKDF2SubkeyLength);
            }

            byte[] outputBytes = new byte[1 + saltSize + PBKDF2SubkeyLength];
            Buffer.BlockCopy(salt, 0, outputBytes, 1, saltSize);
            Buffer.BlockCopy(subkey, 0, outputBytes, 1 + saltSize, PBKDF2SubkeyLength);
            return Convert.ToBase64String(outputBytes);
        }
          public static string HashPassword(string password)
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

          public static bool VerifyHashedPassword(string hashedPassword, string password)
          {
              byte[] buffer4;
              if (hashedPassword == null)
              {
                  return false;
              }
              if (password == null)
              {
                  throw new ArgumentNullException("password");
              }
              byte[] src = Convert.FromBase64String(hashedPassword);
              if ((src.Length != 0x31) || (src[0] != 0))
              {
                  return false;
              }
              byte[] dst = new byte[0x10];
              Buffer.BlockCopy(src, 1, dst, 0, 0x10);
              byte[] buffer3 = new byte[0x20];
              Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
              using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
              {
                  buffer4 = bytes.GetBytes(0x20);
              }
              return ByteArraysEqual(buffer3, buffer4);
          }

          private static bool ByteArraysEqual(byte[] buffer3, byte[] buffer4)
          {
            bool lol = buffer3.SequenceEqual(buffer4);
            return lol;
          }
          
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            BookItDesktop.DataSet1 dataSet1 = ((BookItDesktop.DataSet1)(this.FindResource("dataSet1")));
            // Load data into the table BookmarkTags. You can modify this code as needed.
            BookItDesktop.DataSet1TableAdapters.BookmarkTagsTableAdapter dataSet1BookmarkTagsTableAdapter = new BookItDesktop.DataSet1TableAdapters.BookmarkTagsTableAdapter();
            dataSet1BookmarkTagsTableAdapter.Fill(dataSet1.BookmarkTags);
            System.Windows.Data.CollectionViewSource bookmarkTagsViewSource = ((System.Windows.Data.CollectionViewSource)(this.FindResource("bookmarkTagsViewSource")));
            bookmarkTagsViewSource.View.MoveCurrentToFirst();
        }

        private void labelRegister_MouseEnter(object sender, MouseEventArgs e)
        {
            labelRegister.Foreground = Brushes.AliceBlue;
        }

        private void labelRegister_MouseLeave(object sender, MouseEventArgs e)
        {
           
            labelRegister.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#D0D9DB"));
        }

        private void labelRegister_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            MessageBox.Show("dsadasadsads");
            var registerWindow = new Register(windowRef);
            registerWindow.Show();
        }

        private void saveCredentials(string fUsername, string fPassword)
        {
            string path = "credentials.txt";
            if (!File.Exists(path))
            {
                
                using (StreamWriter sw = File.CreateText(path))
                {
                    sw.WriteLine(fUsername);
                    sw.WriteLine(fPassword);
                   
                }
            }
        }

        private void loadCredentials()
        {
            try {
                string path = "credentials.txt";
                string[] cred = new string[2];
                using (StreamReader sr = File.OpenText(path))
                {
                    string s = "";
                    int i = 0;
                    while ((s = sr.ReadLine()) != null)
                    {
                        cred[i] = s;
                        i++;
                    }
                }

                txtBoxUserName.Text = cred[0];
                txtBoxPassword.Password = cred[1];
                credentialsLoaded = true;
            }
            catch
            {
                MessageBox.Show("Not found");
            }
        }
    }
}
