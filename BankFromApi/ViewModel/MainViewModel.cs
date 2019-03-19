using BankFromApi.Model;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Input;
using Flurl;
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
        private DateTime _dateFrom = DateTime.Today.AddDays(-1);
        private DateTime _dateTo = DateTime.Today;
        private ObservableCollection<string> _symbols;
        private string _selectedSymbol;
        private SeriesCollection _series;
        private string _appStatus;
        private List<string> _labels;
        private List<RootObject> _root;
        private bool _canEditDates = true;

        public bool CanEditDates
        {
            get => _canEditDates;
            set
            {
                Set(nameof(CanEditDates), ref _canEditDates, value);
            }
        }

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

        public string SelectedSymbol
        {
            get => _selectedSymbol;
            set
            {
                Set(nameof(SelectedSymbol), ref _selectedSymbol, value);
            }
        }

        public ObservableCollection<string> Symbols
        {
            get => _symbols;
            set
            {
                Set(nameof(Symbols), ref _symbols, value);
            }
        }

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
        public ICommand AddLineSeriesCommand { get; set; }
        public ICommand ClearChartCommand { get; set; }
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            GetDataCommand = new RelayCommand(GetRate, CanGetData);
            AddLineSeriesCommand = new RelayCommand(AddLineSeries, () => _root != null);
            ClearChartCommand = new RelayCommand(ClearChart, () => Series != null);
            GetSymbols();
        }

        private void ClearChart()
        {
            Series.Clear();
            CanEditDates = true;

        }

        private void AddLineSeries()
        {
            var values = new List<double>();

            foreach (var item in _root)
            {
                var result = item.rates.FirstOrDefault(s => s.code == SelectedSymbol);
                values.Add(result.mid);
            }

            var lineSeries = new LineSeries
            {
                Values = new ChartValues<double>(values),
                Title = SelectedSymbol,
            };

            Series.Add(lineSeries);
        }

        private bool CanGetData()
        {
            return DateFrom <= DateTo && !string.IsNullOrWhiteSpace(SelectedSymbol);
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
                    AppStatus = null;
                }
            }
            catch (Exception ex)
            {
                AppStatus = ex.Message;
            }
        }

        private async void GetRate()
        {
            _root = await GetData();
            if (_root == null) return;

            var lineSeries = new List<double>();
            var labels = new List<string>();

            foreach (var item in _root)
            {
                var result = item.rates.FirstOrDefault(s => s.code == SelectedSymbol);
                lineSeries.Add(result.mid);
                labels.Add(item.effectiveDate);
            }

            Labels = labels;

            Series = new SeriesCollection
            {
                new LineSeries
                {
                    Values = new ChartValues<double>(lineSeries),
                    Title = SelectedSymbol
                },
            };

            CanEditDates = false;
        }

        private async Task<List<RootObject>> GetData()
        {
            var root = new List<RootObject>();
            var dates = new List<Tuple<DateTime, DateTime>>();
            var apiDayLimit = 93;

            var dateFrom = DateFrom;
            var dateTo = DateTo;

            if ((dateFrom - dateTo).TotalDays < -apiDayLimit)
            {
                while ((dateFrom - dateTo).TotalDays < -apiDayLimit)
                {
                    dates.Add(new Tuple<DateTime, DateTime>(dateFrom, dateFrom.AddDays(apiDayLimit)));

                    dateFrom = dateFrom.AddDays(apiDayLimit+1);
                }

                dates.Add(new Tuple<DateTime, DateTime>(dateFrom, dateTo));
            }
            else
            {
                dates.Add(new Tuple<DateTime, DateTime>(dateFrom, dateTo));
            }

            foreach (var date in dates)
            {
                var url = Url.Combine("http://api.nbp.pl/api/exchangerates/tables/A/", date.Item1.ToString("yyyy-MM-dd"), date.Item2.ToString("yyyy-MM-dd"));

                try
                {
                    using (var client = new HttpClient())
                    {
                        var currencyJson = await client.GetStringAsync(url);
                        root.AddRange(JsonConvert.DeserializeObject<List<RootObject>>(currencyJson));
                    }
                    AppStatus = null;
                }
                catch (Exception ex)
                {
                    AppStatus = ex.Message;
                    return null;
                }
            }

            return root;
        }
    }
}