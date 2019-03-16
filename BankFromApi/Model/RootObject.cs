using System.Collections.Generic;

namespace BankFromApi.Model
{
    public class RootObject
    {
        public string table { get; set; }
        public string no { get; set; }
        public string effectiveDate { get; set; }
        public List<Rate> rates { get; set; }
    }
}
