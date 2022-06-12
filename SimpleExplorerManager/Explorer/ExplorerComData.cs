using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SimpleExplorerManager.Explorer
{
    public class ExplorerComData
    {
        public int Left { get; set; }
        public int Top { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public string LocationName { get; set; }
        public string LocationURL { get; set; }
        public long HWND { get; set; }
        public string Kind { get; set; }
        [JsonIgnore]
        public List<ExplorerComData> sibilingList { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }

        public string GetUrlAsPath()
        {
            Uri uri = new Uri(LocationURL);
            return uri.LocalPath + Uri.UnescapeDataString(uri.Fragment);
        }
    }
}
