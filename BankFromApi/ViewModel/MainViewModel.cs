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
using ApiLibrary;
using ApiLibrary.DataModel;

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
        private ObservableCollection<Rate> _symbols;
        private Rate _selectedSymbol;
        private SeriesCollection _series;
        private string _appStatus;
        private List<string> _labels;
        private List<CurrencyRoot> _root;
        private bool _canEditDates = true;

        private readonly int ApiDayLimit = 93;

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

        public Rate SelectedSymbol
        {
            get => _selectedSymbol;
            set
            {
                Set(nameof(SelectedSymbol), ref _selectedSymbol, value);
            }
        }

        public ObservableCollection<Rate> Symbols
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

            foreach(var root in _root)
            {
                values.AddRange(root.rates.Select(s => s.mid));
            }
            

            var lineSeries = new LineSeries
            {
                Values = new ChartValues<double>(values),
                Title = SelectedSymbol.ToString(),
            };

            Series.Add(lineSeries);
        }

        private bool CanGetData()
        {
            return DateFrom <= DateTo && SelectedSymbol != null && DateTo <= DateTime.Today;
        }

        private async void GetSymbols()
        {
            var symbols = await GetFromApi.Symbols();
            Symbols = new ObservableCollection<Rate>(symbols);
        }

        private async void GetRate()
        {
            _root = await GetFromApi.CurrencyRate(DateFrom, DateTo, SelectedSymbol);
            if (_root == null) return;

            var lineSeries = new List<double>();
            var labels = new List<string>();

            foreach (var item in _root)
            {
                lineSeries.AddRange(item.rates.Select(s => s.mid));
                labels.AddRange(item.rates.Select(s => s.effectiveDate));
            }

            Labels = labels;

            Series = new SeriesCollection
            {
                new LineSeries
                {
                    Values = new ChartValues<double>(lineSeries),
                    Title = SelectedSymbol.ToString()
                },
            };

            CanEditDates = false;
        }
    }
}