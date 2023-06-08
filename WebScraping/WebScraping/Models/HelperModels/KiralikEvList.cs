using System;

namespace WebScraping.Models.HelperModels
{
    public class KiralikEvList
    {
        public int ID { get; set; }
        public Nullable<int> price { get; set; }
        public string city { get; set; }
        public string county { get; set; }
        public Nullable<int> age { get; set; }
        public Nullable<int> floor { get; set; }
        public string room { get; set; }
        public Nullable<int> metre { get; set; }
        public string district { get; set; }
        public Nullable<int> totalFloor { get; set; }
        public string heating { get; set; }
        public string isFurnished { get; set; }
        public string formHousing { get; set; }
        public Nullable<int> bathroom { get; set; }
        public string link { get; set; }
        public string image { get; set; }
    }
}
