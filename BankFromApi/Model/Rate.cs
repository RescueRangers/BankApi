using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankFromApi.Model
{
    public class Rate
    {
        public string currency { get; set; }
        public string code { get; set; }
        public double mid { get; set; }
    }
}
