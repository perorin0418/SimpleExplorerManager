using SimpleExplorerManager.Bookmark;
using SimpleExplorerManager.Explorer;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;

namespace SimpleExplorerManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<ExplorerComData> CurrentExpList { get; set; }
        public ObservableCollection<ExplorerComData> BookmarkExpList { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            CurrentExpList = new ObservableCollection<ExplorerComData>();
            ICollectionView currentView = CollectionViewSource.GetDefaultView(CurrentExpList);
            currentView.GroupDescriptions.Add(new PropertyGroupDescription("Kind"));
            currentView.SortDescriptions.Add(new SortDescription("Kind", ListSortDirection.Ascending));
            currentExpListbox.ItemsSource = currentView;

            BookmarkExpList = new ObservableCollection<ExplorerComData>();
            ICollectionView bookmarkView = CollectionViewSource.GetDefaultView(BookmarkExpList);
            bookmarkView.GroupDescriptions.Add(new PropertyGroupDescription("Kind"));
            bookmarkView.SortDescriptions.Add(new SortDescription("Kind", ListSortDirection.Ascending));
            bookmarkExpListBox.ItemsSource = bookmarkView;

            DispatcherTimer currentExpSearchTimer = new DispatcherTimer();
            currentExpSearchTimer.Interval = new TimeSpan(0, 0, 1);
            currentExpSearchTimer.Tick += new EventHandler(CurrentExpSearchTimer);
            currentExpSearchTimer.Start();

            DispatcherTimer bookmarkTimer = new DispatcherTimer();
            bookmarkTimer.Interval = new TimeSpan(0, 0, 3);
            bookmarkTimer.Tick += new EventHandler(BookmarkTimer);
            bookmarkTimer.Start();
        }

        public void CurrentExpSearchTimer(object sender, EventArgs e)
        {
            int selectedIndex = currentExpListbox.SelectedIndex;
            CurrentExpList.Clear();
            foreach(ExplorerComData exp in ExplorerUtil.GetExplorerWindows())
            {
                CurrentExpList.Add(exp);
            }
            if(selectedIndex >= 0 && selectedIndex < CurrentExpList.Count)
            {
                currentExpListbox.SelectedIndex = selectedIndex;
            }
        }

        public void BookmarkTimer(object sender, EventArgs e)
        {
            try
            {
                int selectedIndex = bookmarkExpListBox.SelectedIndex;
                BookmarkExpList.Clear();
                BookmarkGroupSuite suite = BookmarkManager.readBookmark();
                foreach(BookmarkGroup group in suite.Suite)
                {
                    foreach(BookmarkData data in group.Data)
                    {
                        ExplorerComData com = new ExplorerComData();
                        com.LocationName = data.DisplayName;
                        com.LocationURL = data.Path;
                        com.Kind = group.GroupName;
                        BookmarkExpList.Add(com);
                    }
                }
                if (selectedIndex >= 0 && selectedIndex < BookmarkExpList.Count)
                {
                    bookmarkExpListBox.SelectedIndex = selectedIndex;
                }
            }
            catch(Exception ex)
            {
                // NOP
            }
        }

        private void listbox_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            ListBox listBox = sender as ListBox;

            // 選択中のアイテムの ListBoxItem を取得
            var listBoxItem = (ListBoxItem)listBox.ItemContainerGenerator.ContainerFromItem(listBox.SelectedItem);
            if (listBoxItem != null)
            {
                // ヒットテストでアイテム上
                if (listBoxItem.InputHitTest(e.GetPosition(listBoxItem)) != null)
                {
                    // 選択中のアイテム上でダブルクリックされたとき
                    ExplorerComData com = (ExplorerComData)listBox.SelectedItem;
                    long hWnd = com.HWND;
                    if(hWnd == 0)
                    {
                        ExplorerUtil.ShowExplorerWindow(com.GetUrlAsPath());
                    }
                    else
                    {
                        ExplorerUtil.SetExplorerForegroud(hWnd);
                    }
                    listBox.SelectedIndex = -1;
                }
                else
                {
                    // アイテム以外でダブルクリックされたとき選択解除
                    listBox.SelectedIndex = -1;
                }
            }
        }

        private void listbox_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            ListBox listBox = sender as ListBox;

            // 選択中のアイテムの ListBoxItem を取得
            var listBoxItem = (ListBoxItem)listBox.ItemContainerGenerator.ContainerFromItem(listBox.SelectedItem);
            if(listBoxItem != null)
            {
                // ヒットテストでアイテム上
                if (listBoxItem.InputHitTest(e.GetPosition(listBoxItem)) != null)
                {
                    // 選択中のアイテム上でクリックされたとき
                }
                else
                {
                    // アイテム以外でクリックされたとき選択解除
                    listBox.SelectedIndex = -1;
                }
            }
        }

        private void listbox_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            ListBox listBox = sender as ListBox;

            // 選択中のアイテムの ListBoxItem を取得
            var listBoxItem = (ListBoxItem)listBox.ItemContainerGenerator.ContainerFromItem(listBox.SelectedItem);
            if (listBoxItem != null)
            {
                // ヒットテストでアイテム上
                if (listBoxItem.InputHitTest(e.GetPosition(listBoxItem)) != null)
                {
                    // 選択中のアイテム上で右クリックされたとき
                    ExplorerUtil.ShowExplorerContextMenu(((ExplorerComData)listBox.SelectedItem).GetUrlAsPath());
                }
                else
                {
                    // アイテム以外で右クリックされたとき選択解除
                    listBox.SelectedIndex = -1;
                }
            }
        }

        private void listbox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void kindButton_Click(object sender, RoutedEventArgs e)
        {
        }

        private void editBookmarkButton_Click(object sender, RoutedEventArgs e)
        {
            BookmarkManager.openEditor();
        }
    }
}
