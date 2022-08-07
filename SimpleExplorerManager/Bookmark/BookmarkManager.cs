using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace SimpleExplorerManager.Bookmark
{
    public class BookmarkManager
    {

        public static string BookmarkConfigPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)
            + Path.DirectorySeparatorChar + "SimpleExplorerManager" + Path.DirectorySeparatorChar + "config";
        public static string BookmarkConfigFile = BookmarkConfigPath + Path.DirectorySeparatorChar + "bookmark.json";

        public static void createNewFile()
        {
            if (!Directory.Exists(BookmarkConfigPath))
            {
                Directory.CreateDirectory(BookmarkConfigPath);
            }

            if (!File.Exists(BookmarkConfigFile))
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                using (StreamWriter sw = new StreamWriter(BookmarkConfigFile, false, Encoding.GetEncoding("shift_jis")))
                {
                    BookmarkData data = new BookmarkData();
                    data.DisplayName = "sample";
                    data.Path = "file://C:/sample";
                    BookmarkGroup group = new BookmarkGroup();
                    group.GroupName = "new group";
                    group.Data = new List<BookmarkData>() { data };
                    BookmarkGroupSuite suite = new BookmarkGroupSuite();
                    suite.Suite = new List<BookmarkGroup>() { group };
                    sw.Write(JsonSerializer.Serialize(suite));
                }
            }
        }

        public static void openEditor()
        {
            if (!File.Exists(BookmarkConfigFile))
            {
                createNewFile();
            }
            try
            {
                var p = new Process();
                p.StartInfo = new ProcessStartInfo(BookmarkConfigFile)
                {
                    UseShellExecute = true
                };
                p.Start();
            }catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public static BookmarkGroupSuite readBookmark()
        {
            if (!File.Exists(BookmarkConfigFile))
            {
                createNewFile();
            }
            string json = File.ReadAllText(BookmarkConfigFile);
            BookmarkGroupSuite suite = JsonSerializer.Deserialize<BookmarkGroupSuite>(json);
            return suite;
        }
    }
}
