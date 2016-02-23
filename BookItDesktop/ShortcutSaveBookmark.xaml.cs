using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Management;
using Microsoft.Win32;

namespace BookItDesktop
{
    /// <summary>
    /// Interaction logic for ShortcutSaveBookmark.xaml
    /// </summary>
    public partial class ShortcutSaveBookmark : Window
    {
        private string defaultBrowser = "";
        protected AspNetUsers aspNetUser;
        protected string userId;
        List<ModelBookmarTag> mbt;
        RegisterKeyboardInput registerKeyboard;
        public ShortcutSaveBookmark(string a1)
        {
            InitializeComponent();
            this.userId = a1;
            DisplayBookmarkList dbl = new DisplayBookmarkList();
            mbt = dbl.getBookmarks();
           // registerKeyboard = new RegisterKeyboardInput(this);
          //  registerKeyboard.KeyBoardKeyPressed += keyboard_KeyBoardKeyPressed;
            foreach (ModelBookmarTag vara in mbt)
            {
                DisplaySearchUrl dsu = new DisplaySearchUrl();
                dsu.displayUrl = vara.bookmarkedPage.Url;
                listViewDisplayBookmark.Items.Add(dsu);
            }
            defaultBrowser = getDefaultBrowser();
            if (!this.IsVisible)
            {
                this.Show();
            }
            if (this.WindowState == WindowState.Minimized)
            {
                this.WindowState = WindowState.Normal;
            }
            //this.WindowStyle = System.Windows.WindowStyle.None;
            this.Topmost = true;
            this.Activate();
            

        }

        public class DisplaySearchUrl
        {
            public string displayUrl { get; set; }
            public int hit { get; set; }
        }

        private void txtBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            string textBoxSearch = txtBoxSearch.Text;
            if (textBoxSearch.Length > 0)
            {
                if (textBoxSearch[textBoxSearch.Length - 1] == ',')
                {
                    string[] splitSearchTerms = textBoxSearch.Split(',');
                    RefreshBookmarkList(splitSearchTerms);
                }
            }
        }

        private void RefreshBookmarkList(string[] searchTerms)
        {
            listViewDisplayBookmark.Items.Clear();
            List<DisplaySearchUrl> displayList = new List<DisplaySearchUrl>();
            foreach (ModelBookmarTag m1 in mbt)
            {
                if (m1.tagList.Count > 0)
                {
                    foreach (string term in searchTerms)
                    {
                        bool found = false;
                        DisplaySearchUrl d1 = null;
                        foreach (Tags t1 in m1.tagList)
                        {
                            if (t1.TagName == term && !found && term != "")
                            {
                                found = true;
                                d1 = new DisplaySearchUrl();
                                d1.displayUrl = m1.bookmarkedPage.Url;
                                d1.hit += 1;

                            }
                            if (found)
                            {
                                d1.hit += 1;
                            }
                        }
                        if (d1 != null)
                            displayList.Add(d1);
                    }
                }
            }
            foreach (DisplaySearchUrl dd in displayList)
            {
                listViewDisplayBookmark.Items.Add(dd);
            }
        }

        void keyboard_KeyBoardKeyPressed(object sender, EventArgs e)
        {
            //registerKeyboard.Dispose();
             
           
            
        }

        private void listViewDisplayBookmark_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (listViewDisplayBookmark.SelectedItems.Count == 0)
                return;
            DisplaySearchUrl d1 = (DisplaySearchUrl)listViewDisplayBookmark.SelectedItem;
            if (defaultBrowser != "other")
                Process.Start(defaultBrowser + ".exe", d1.displayUrl);
            else
            {
                Clipboard.SetText(d1.displayUrl);
                MessageBox.Show("Default browser not found, Url was pasted into clipboard");
            }
                
            //getDefaultBrowser();
            //getProceses();
            //MessageBox.Show(d1.displayUrl);


        }

        private void getProceses()
        {
            bool processActive = false;
            var wmiQueryString = "SELECT ProcessId, ExecutablePath, CommandLine FROM Win32_Process";
            using (var searcher = new ManagementObjectSearcher(wmiQueryString))
            using (var results = searcher.Get())
            {
                var query = from p in Process.GetProcesses()
                            join mo in results.Cast<ManagementObject>()
                            on p.Id equals (int)(uint)mo["ProcessId"]
                            select new
                            {
                                Process = p,
                                Path = (string)mo["ExecutablePath"],
                                CommandLine = (string)mo["CommandLine"],
                            };
                foreach (var item in query)
                {
                    if(item.Process.ToString() == defaultBrowser)
                    {
                        processActive = true;
                    }
                }
            }
        }

        private string  getDefaultBrowser()
        {
            const string userChoice = @"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http\UserChoice";
            string progId;
            string browser;
            using (RegistryKey userChoiceKey = Registry.CurrentUser.OpenSubKey(userChoice))
            {
                if (userChoiceKey == null)
                {
                    browser = "";
                    
                }
                object progIdValue = userChoiceKey.GetValue("Progid");
                if (progIdValue == null)
                {
                    browser = "ds";
                    
                }
                progId = progIdValue.ToString();
                switch (progId)
                {
                    case "IE.HTTP":
                        browser = "iexplorer";
                        break;
                    case "FirefoxURL":
                        browser = "firefox";
                        break;
                    case "ChromeHTML":
                        browser = "chrom";
                        break;

                    case "OperaStable":
                        browser = "opera";
                        break;
                    default:
                        browser = "other";
                        break;
                }
            }

            return browser;
        }
    }
}
