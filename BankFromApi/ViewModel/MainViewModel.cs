using BankFromApi.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using LiveCharts;
using LiveCharts.Wpf;

namespace BankFromApi.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        private DateTime _dateFrom = DateTime.UtcNow.AddDays(-1);
        private DateTime _dateTo = DateTime.UtcNow;
        private ObservableCollection<CurrencyValue> _rates;
        private ObservableCollection<string> _symbols;
        private string _selectedSymbol;
        private SeriesCollection _series;

        public SeriesCollection Series { get => _series; set { Set(nameof(Series), ref _series, value); } }


        public string SelectedSymbol { get => _selectedSymbol; set { Set(nameof(SelectedSymbol), ref _selectedSymbol, value); } }

        public ObservableCollection<string> Symbols { get => _symbols; set { Set(nameof(Symbols), ref _symbols, value); } }

        public ObservableCollection<CurrencyValue> Rates { get => _rates; set { Set(nameof(Rates), ref _rates, value); } }

        public DateTime DateFrom
        {
            get => _dateFrom;
            set
            {
                Set(nameof(DateFrom), ref _dateFrom, value);
            }
        }
        public DateTime DateTo
        {
            get => _dateTo;
            set
            {
                Set(nameof(DateTo), ref _dateTo, value);
            }
        }

        public ICommand GetDataCommand { get; set; }
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            GetDataCommand = new RelayCommand(GetRate, () => true);
            GetSymbols();
        }

        private async void GetSymbols()
        {
            var symbols = await GetSymbol();
            Symbols = new ObservableCollection<string>(symbols[0].rates.Select(s => s.code));
        }

        private async Task<List<RootObject>> GetSymbol()
        {
            var apiUrl = "http://api.nbp.pl/api/exchangerates/tables/A/";

            try
            {
                using (var client = new HttpClient())
                {
                    var currencyJson = await client.GetStringAsync(apiUrl);
                    return JsonConvert.DeserializeObject<List<RootObject>>(currencyJson);
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private async void GetRate()
        {
            var root = await GetData();

            var rates = new List<CurrencyValue>();

            var lineSeries = new List<double>();
            var columnSeries = new List<DateTimePoint>();

            foreach (var item in root)
            {
                var result = item.rates.FirstOrDefault(s => s.code == SelectedSymbol);
                lineSeries.Add(result.mid);
                rates.Add(new CurrencyValue(DateTime.Parse(item.effectiveDate), result.mid));
            }

            Series = new SeriesCollection
            {
                new LineSeries {Values = new ChartValues<double>(lineSeries) }
            };

            Rates = new ObservableCollection<CurrencyValue>(rates);
        }

        private async Task<List<RootObject>> GetData()
        {
            List<RootObject> ro_root = new List<RootObject>();
            var apiUrl = "http://api.nbp.pl/api/exchangerates/tables/A/";
            apiUrl += DateFrom.ToString("yyyy-MM-dd");
            apiUrl += "/";
            apiUrl += DateTo.ToString("yyyy-MM-dd") + "/?format=json";

            try
            {
                using (var client = new HttpClient())
                {
                    var currencyJson = await client.GetStringAsync(apiUrl);
                    ro_root = JsonConvert.DeserializeObject<List<RootObject>>(currencyJson);
                }
                return ro_root;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}