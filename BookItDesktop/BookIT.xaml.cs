using Hardcodet.Wpf.TaskbarNotification;
using Microsoft.Win32;
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
    /// Interaction logic for BookIT.xaml
    /// </summary>
    public partial class BookIT : Window, ICommand
    {
        StackPanel[] stackPanels;
        List<ModelBookmarTag> mbt;
        protected static AspNetUsers anu;
        protected bool boolTabItemAdd = false;
        protected bool boolTabEditItem = false;
        private int keyTest = 0;
        RegisterKeyboardInput rgk;
        TaskbarIcon taskbarIcon;
       
        public event EventHandler CanExecuteChanged;

        public BookIT()
        {


        }

        public BookIT(AspNetUsers anu1)
        {
            InitializeComponent();
           /* taskbarIcon = new TaskbarIcon();
            taskbarIcon = (TaskbarIcon)FindResource("taskBarIconMain");
            taskbarIcon.Visibility = Visibility.Hidden;*/
            taskBarIconMain.Visibility = Visibility.Hidden;
            anu = anu1;
            stackPanels = new StackPanel[4];
            DisplayBookmarkList dbl = new DisplayBookmarkList();
            rgk = new RegisterKeyboardInput();
            rgk.KeyBoardKeyPressed += keyboard_KeyBoardKeyPressed;
            /*Label qw = new Label();
            qw.Content = "dadsdsaddsa";
            qw.Height = 299;
            qw.Width = 299;
            stackPanelMain.Children.Add(qw);
            StackPanel sPanel = new StackPanel();
            sPanel.Height = stackPanelMain.Height;
            sPanel.Width = stackPanelMain.Width;
            sPanel.VerticalAlignment = stackPanelMain.VerticalAlignment;
            sPanel.HorizontalAlignment = stackPanelMain.HorizontalAlignment;
            sPanel.Margin = stackPanelMain.Margin;
            Label qw = new Label();
            qw.Content = "cxycyxxcy";
            qw.Height = 299;
            qw.Width = 299;
            sPanel.Children.Add(qw);
            stackPanelMain.Children.Add(sPanel);*/
            mbt = dbl.getBookmarks();
            showBookmarks(mbt);
        }

        private void showBookmarks(List<ModelBookmarTag> mainList)
        {
            foreach (ModelBookmarTag mbs in mainList)
            {
                string url = mbs.bookmarkedPage.Url;
                string tag1 = "";
                foreach (Tags aaa in mbs.tagList)
                {
                    tag1 += aaa.TagName.ToString() + ",";
                }
                testIt t1 = new testIt();
                t1.url1 = url;
                t1.tag1 = tag1;
                t1.id = mbs.bookmarkedPage.BookmarkID.ToString();
                testView.Items.Add(t1);
            }


            /*double mainTopMargin = 0;
            foreach(var a in mainList)
            {
                StackPanel sPanel = new StackPanel();
                List<Label> test = new List<Label>();
                sPanel.Height = 50;
                sPanel.Width = stackPanelMain.Width;
                sPanel.VerticalAlignment = stackPanelMain.VerticalAlignment;
                sPanel.HorizontalAlignment = stackPanelMain.HorizontalAlignment;
                sPanel.Margin = new Thickness(0, mainTopMargin, 0, 0);
                Label l2 = new Label();
                l2.VerticalAlignment = VerticalAlignment.Center;
                l2.HorizontalAlignment = HorizontalAlignment.Left;
                l2.Margin = new Thickness(0, 0, 0, 0);
                l2.Content = a.bookmarkedPage.BookmarkTags;
                double marginaTop = 0;
                double marginRight = 0;
                int counter = 0;
                foreach (var s in a.tagList)
                {     
                    Label l1 = new Label();
                    l1.Content = s.TagName.ToString();
                    l1.HorizontalAlignment = HorizontalAlignment.Center;
                    l1.Margin = new Thickness(0, marginaTop, marginRight, 0);
                    counter++;
                    marginaTop += 10;
                    if(counter == 3)
                    {
                        counter = 0;
                        marginaTop = 0;
                        marginRight += 30;
                    }
                }
                foreach (Label l1 in test)
                {
                    sPanel.Children.Add(l1);
                }


                stackPanelMain.Children.Add(sPanel);
                mainTopMargin += 50;


            }

            
           */
        }
        public class testIt
        {
            public string url1 { get; set; }
            public string tag1 { get; set; }
            public string id { get; set; }
        }

        private void BtnYourButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditBookmarkClick(object sender, RoutedEventArgs e)
        {
            clearGrid();
            MessageBox.Show("das");
            var boundData = (testIt)((Button)sender).DataContext;
            DisplayBookmarkList dbl = new DisplayBookmarkList(Int32.Parse(boundData.id));
            ModelBookmarTag mb = dbl.getSingleBookmark();
            createEditBookmark(mb);
            boolTabEditItem = true;
            boolTabItemAdd = false;
           
           //createAddBookmark();
            tabControl.SelectedIndex = 1;
        }
        private void SaveBookmarkEdit(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("das");
            bool urlFound = false;
            ModelBookmarTag mdl = new ModelBookmarTag();
            mdl.tagList = new List<Tags>();
            foreach(object aa in gridAdd.Children)
            {
                if(aa is TextBox)
                {
                    TextBox helper = (TextBox)aa;
                    if (!urlFound)
                    {
                        urlFound = true;
                        string partId = extractId(helper.Name.ToString());
                        mdl.bookmarkedPage = new Bookmarks();
                        mdl.bookmarkedPage.BookmarkID = Int32.Parse(partId);
                        mdl.bookmarkedPage.Url = helper.Text.ToString();
                       
                    }
                    else
                    {
                        Tags t1 = new Tags();
                        string partId = extractId(helper.Name.ToString());
                        t1.TagID = Int32.Parse(partId);
                        t1.TagName = helper.Text.ToString();
                        mdl.tagList.Add(t1);
                    }
                }
            }
            DisplayBookmarkList dbc = new DisplayBookmarkList(mdl);
        }

        private string extractId(string s)
        {
            char[] l1 = "txtBox".ToCharArray();
            string[] splitIt = s.Split(l1);
            return splitIt[6];
        }
        private void TabSelected(object sender, RoutedEventArgs e)
        {
            var boundData = (TabItem)sender;
            if (boundData.Name == tabItemAdd.Name)
            {
                if (!boolTabEditItem)
                {
                    if (!boolTabItemAdd)
                    {
                        clearGrid();
                        createAddBookmark();
                        boolTabItemAdd = true;
                    }
                    else
                    {
                        MessageBox.Show("created");
                    }
                }
                else
                {
                    boolTabEditItem = false;
                }
            }
            if (boundData.Name == tabItemLogOut.Name)
                MessageBox.Show("LogOUt");
        }

        private void createEditBookmark(ModelBookmarTag displayTag)
        {
            double height = gridAdd.Height;
            int numberOfTags = (displayTag.tagList.Count / 2)  + 2;
            int elementHeight = (int)height / numberOfTags;
            int collumnWidth = (int)gridAdd.Width / 2;
            List <RowDefinition>  rd= new List<RowDefinition>();
            List<ColumnDefinition> cd = new List<ColumnDefinition>();
            for (int i = 0; i < numberOfTags; i++)
            {
                RowDefinition nRd = new RowDefinition();
                nRd.Height = new GridLength(elementHeight);
                rd.Add(nRd);
            }
            for (int i = 0; i < 2; i++)
            {
                ColumnDefinition nCd = new ColumnDefinition();
                nCd.Width = new GridLength(collumnWidth);
                cd.Add(nCd);
            }
            int xy = 0;
            for (int i = 0; i < numberOfTags; i++)
            {
                if( i == 0)
                {
                    Label l1 = new Label();
                    l1.Content = "Url";
                    l1.VerticalAlignment = VerticalAlignment.Center;
                    l1.HorizontalAlignment = HorizontalAlignment.Right;
                    l1.Margin = new Thickness(0, 0, 0, 0);
                    TextBox t1 = new TextBox();
                    t1.Width = 100;
                    t1.Name =  "txtBox" + displayTag.bookmarkedPage.BookmarkID.ToString();
                    t1.HorizontalAlignment = HorizontalAlignment.Left;
                    t1.VerticalAlignment = VerticalAlignment.Center;
                    t1.Margin = new Thickness(0, 0, 0, 0);
                    t1.Text = displayTag.bookmarkedPage.Url;
                    gridAdd.ColumnDefinitions.Add(cd[0]);
                    gridAdd.ColumnDefinitions.Add(cd[1]);
                    gridAdd.RowDefinitions.Add(rd[i]);
                    Grid.SetRow(l1, i);
                    Grid.SetColumn(l1, 0);
                    Grid.SetRow(t1, i);
                    Grid.SetColumn(t1, 1);
                    gridAdd.Children.Add(l1);
                    gridAdd.Children.Add(t1);

                }

                else if ( i == numberOfTags - 1)
                {
                    Button btnSaveBookmark = new Button();
                    btnSaveBookmark.HorizontalAlignment = HorizontalAlignment.Center;
                    btnSaveBookmark.VerticalAlignment = VerticalAlignment.Center;
                    btnSaveBookmark.Margin = new Thickness(0, 0, 0, 0);
                    btnSaveBookmark.Height = 20;
                    btnSaveBookmark.Width = 100;
                    btnSaveBookmark.Click += new RoutedEventHandler(SaveBookmarkEdit);
                    //gridAdd.ColumnDefinitions.Add(cd[0]);
                    gridAdd.RowDefinitions.Add(rd[i]);
                    Grid.SetRow(btnSaveBookmark, i);
                    Grid.SetColumn(btnSaveBookmark, 0);
                    gridAdd.Children.Add(btnSaveBookmark);
                }

                else
                {
                    TextBox t1 = new TextBox();
                    t1.Name = "txtBox" + displayTag.tagList[xy].TagID.ToString();
                    t1.Text = displayTag.tagList[xy].TagName;
                    t1.HorizontalAlignment = HorizontalAlignment.Center;
                    t1.VerticalAlignment = VerticalAlignment.Center;
                    t1.Margin = new Thickness(0, 0, 0, 0);
                    t1.Width = 100;
                    xy += 1;
                    TextBox t2 = new TextBox();
                    t2.Name = "txtBox" + displayTag.tagList[xy].TagID.ToString();
                    t2.Text = displayTag.tagList[xy].TagName;
                    t2.HorizontalAlignment = HorizontalAlignment.Center;
                    t2.VerticalAlignment = VerticalAlignment.Center;
                    t2.Margin = new Thickness(0, 0, 0, 0);
                    t2.Width = 100;
                    xy += 1;
                    //gridAdd.ColumnDefinitions.Add(cd[0]);
                    //gridAdd.ColumnDefinitions.Add(cd[1]);
                    gridAdd.RowDefinitions.Add(rd[i]);
                    Grid.SetRow(t1, i);
                    Grid.SetColumn(t1, 0);
                    Grid.SetRow(t2, i);
                    Grid.SetColumn(t2, 1);
                    gridAdd.Children.Add(t1);
                    gridAdd.Children.Add(t2);
                }
            }
        }
        private void createAddBookmark()
        {
            Label tabTitle = new Label();
            tabTitle.Content = "Create new Bookmark";
            tabTitle.HorizontalAlignment = HorizontalAlignment.Center;
            tabTitle.FontSize = 22;
            TextBox enterUrl = new TextBox();
            enterUrl.HorizontalAlignment = HorizontalAlignment.Center;
            enterUrl.VerticalAlignment = VerticalAlignment.Center;
            enterUrl.Margin = new Thickness(0, 0, 0, 0);
            enterUrl.Height = 30;
            enterUrl.Width = 200;         
            TextBox enterTags = new TextBox();
            enterTags.HorizontalAlignment = HorizontalAlignment.Center;
            enterTags.VerticalAlignment = VerticalAlignment.Center;
            enterTags.Margin = new Thickness(0, 0, 0, 0);
            enterTags.Height = 30;
            enterTags.Width = 200;
            Button btnSaveBookmark = new Button();
            btnSaveBookmark.HorizontalAlignment = HorizontalAlignment.Center;
            btnSaveBookmark.VerticalAlignment = VerticalAlignment.Center;
            btnSaveBookmark.Margin = new Thickness(0, 50, 0, 0);
            btnSaveBookmark.Height = 20;
            btnSaveBookmark.Width = 100;
            btnSaveBookmark.Content = "Save";
            btnSaveBookmark.Background = Brushes.AliceBlue;
            RowDefinition r1 = new RowDefinition();
            RowDefinition r2 = new RowDefinition();
            RowDefinition r3 = new RowDefinition();
            RowDefinition r4 = new RowDefinition();
            ColumnDefinition c = new ColumnDefinition();
            c.Width = new GridLength(796);
            gridAdd.ColumnDefinitions.Add(c);
            r1.Height = new GridLength(50);
            r2.Height = new GridLength(50);
            r3.Height = new GridLength(50);
            r4.Height = new GridLength(50);
            gridAdd.RowDefinitions.Add(r1);
            gridAdd.RowDefinitions.Add(r2);
            gridAdd.RowDefinitions.Add(r3);
            gridAdd.RowDefinitions.Add(r4);
            Grid.SetRow(tabTitle,1);
            Grid.SetColumn(tabTitle, 1);
            Grid.SetRow(enterUrl, 2);
            Grid.SetColumn(enterUrl, 1);
            Grid.SetRow(enterTags, 3);
            Grid.SetColumn(enterTags, 1);
            Grid.SetRow(btnSaveBookmark, 4);
            Grid.SetColumn(btnSaveBookmark, 1);
            gridAdd.Children.Add(tabTitle);
            gridAdd.Children.Add(enterUrl);
            gridAdd.Children.Add(enterTags);
            gridAdd.Children.Add(btnSaveBookmark);

        }

        private void clearGrid()
        {
            gridAdd.Children.Clear();
            gridAdd.RowDefinitions.Clear();
            gridAdd.ColumnDefinitions.Clear();
        }
      
        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
       
        }

        void keyboard_KeyBoardKeyPressed(object sender, EventArgs e)
        {
          
            keyTest++;
            Int32 aa = rgk.keyBoardHandle.ToInt32();
            int a1 = rgk.pressedKey;
            labelTest.Content = "Lalalal:" + keyTest.ToString();
            if (rgk.openSave)
            {

                var saveBookmarkShortcut = new ShortcutSaveBookmark(anu.Id);
                saveBookmarkShortcut.Show();
            }
            if(rgk.openSearch)
            {
                var searchBookmark = new SaveBookmarkWindow(anu.Id);
                searchBookmark.Show();
            }
            //rgk.Dispose();
           
        
            
        }
        void trayIconClickItem(Object sender, System.Windows.RoutedEventArgs e)
        {
            //MessageBox.Show(e.ToString());
        }

        void trayGetItemName (Object sender, RoutedEventArgs e)
        {
            MenuItem m1 = (MenuItem)sender;
            string iva = m1.Name.ToString();
            if(iva == iconItem1.Name)
            {
                this.Visibility = Visibility.Visible;
                taskBarIconMain.Visibility = Visibility.Hidden;

            }

            if(iva == iconItem2.Name)
            {
                this.Close();
                Application.Current.Shutdown();
            }
            //MessageBox.Show(m1.Name.ToString());
            //int sddsaadasddsadasda = 3432;
        }
        void trayIconHelper(Object sender, System.Windows.RoutedEventArgs e)
        {

        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            //MessageBox.Show(parameter.ToString());
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            taskBarIconMain.Visibility = Visibility.Visible;
            e.Cancel = true;
            this.ShowInTaskbar = false;
            this.Hide();
        }

        private void btnSaveOptions_Click(object sender, RoutedEventArgs e)
        {
            bool isChecked;
            if (checkBoxAddApplicationToStartUp.IsChecked == true)
                isChecked = true;
            else
                isChecked = false;
            saveApplicationToStartuo(isChecked);
        }

        private void saveApplicationToStartuo(bool isChecked)
        {
            string appName = "BookIT";
            string path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            string fullPath = path + "BookItDesktop.exe";
            RegistryKey regStartApplications = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            if (isChecked)
                regStartApplications.SetValue(appName, fullPath);
            else
                regStartApplications.DeleteValue(appName);

        }
    }
}


