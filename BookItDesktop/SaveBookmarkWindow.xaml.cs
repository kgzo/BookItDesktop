using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for SaveBookmarkWindow.xaml
    /// </summary>
    public partial class SaveBookmarkWindow : Window
    {
        private string url;
        private string tag;
        private bool enterTag = false;
        private bool saveUrl = false;
        private string id;
        public SaveBookmarkWindow()
        {
            InitializeComponent();
        }

        public SaveBookmarkWindow(string id)
        {
            this.id = id;
            InitializeComponent();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            saveUrl = false;
            if (enterTag)
            {
                enterTag = false;
                saveUrl = true;
                tag = txtBoxEnterInfo.Text;
                //MessageBox.Show(tag);
                DisplayBookmarkList ddd = new DisplayBookmarkList();
                ddd.SaveBookmark(url, tag,id);

                txtBoxEnterInfo.Clear();
                btnNext.Content = "Next";
            }
            if (!enterTag && !saveUrl)
            {
                url = txtBoxEnterInfo.Text;
                btnNext.Content = "Save";
                enterTag = true;
                txtBoxEnterInfo.Clear();
            }

            
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            enterTag = false;
            url = "";
            tag = "";
            txtBoxEnterInfo.Clear();
            btnNext.Content = "next";
        }
    }
}
