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
        private string _appStatus;
        private List<string> _labels;

        public List<string> Labels
        {
            get => _labels;
            set
            {
                Set(nameof(Labels), ref _labels, value);
            }
        }

        public string AppStatus
        {
            get => _appStatus;
            set
            {
                Set(nameof(AppStatus), ref _appStatus, value);
            }
        }

        public SeriesCollection Series
        {
            get => _series;
            set
            {
                Set(nameof(Series), ref _series, value);
            }
        }

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
            var apiUrl = "http://api.nbp.pl/api/exchangerates/tables/A/";

            try
            {
                using (var client = new HttpClient())
                {
                    var currencyJson = await client.GetStringAsync(apiUrl);
                    var symbols = JsonConvert.DeserializeObject<List<RootObject>>(currencyJson);
                    Symbols = new ObservableCollection<string>(symbols[0].rates.Select(s => s.code));
                }
            }
            catch (Exception ex)
            {

            }

        }

        private async void GetRate()
        {
            var root = await GetData();

            var lineSeries = new List<double>();
            var labels = new List<string>();

            foreach (var item in root)
            {
                var result = item.rates.FirstOrDefault(s => s.code == SelectedSymbol);
                lineSeries.Add(result.mid);
                labels.Add(item.effectiveDate);
            }

            Series = new SeriesCollection
            {
                new LineSeries {Values = new ChartValues<double>(lineSeries) }
            };

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