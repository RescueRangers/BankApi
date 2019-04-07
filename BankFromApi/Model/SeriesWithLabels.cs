using LiveCharts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankFromApi.Model
{
    public class SeriesWithLabels
    {
        public SeriesCollection SeriesCollection { get; set; }
        public List<string> Labels { get; set; }

        public SeriesWithLabels(SeriesCollection series, List<string> labels)
        {
            SeriesCollection = series;
            Labels = labels;
        }
    }
}
