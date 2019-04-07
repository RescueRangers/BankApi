using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using LiveCharts;
using LiveCharts.Wpf;
using ApiLibrary;
using ApiLibrary.DataModel;
using BankFromApi.Model;

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
        private string _appStatus;
        private bool _canEditDates = true;
        private ObservableCollection<SeriesWithLabels> _seriesCollections = new ObservableCollection<SeriesWithLabels>();

        public ObservableCollection<SeriesWithLabels> SeriesCollections
        {
            get => _seriesCollections;
            set => Set(nameof(SeriesCollections), ref _seriesCollections, value);
        }

        public bool CanEditDates
        {
            get => _canEditDates;
            set
            {
                Set(nameof(CanEditDates), ref _canEditDates, value);
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
        public ICommand ClearChartCommand { get; set; }
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            GetDataCommand = new RelayCommand(GetRate, CanGetData);
            ClearChartCommand = new RelayCommand(ClearChart, !CanEditDates);
            GetSymbols();
        }

        private void ClearChart()
        {
            SeriesCollections.Clear();
            CanEditDates = true;

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
            var root = await GetFromApi.CurrencyRate(DateFrom, DateTo, SelectedSymbol);
            if (root == null) return;

            var lineSeries = new List<double>();
            var labels = new List<string>();

            foreach (var item in root)
            {
                lineSeries.AddRange(item.rates.Select(s => s.mid));
                labels.AddRange(item.rates.Select(s => s.effectiveDate));
            }

            var series = new SeriesCollection
            {
                new LineSeries
                {
                    Values = new ChartValues<double>(lineSeries),
                    Title = SelectedSymbol.ToString()
                },
            };

            SeriesCollections.Add(new SeriesWithLabels(series, labels));

            CanEditDates = false;
        }
    }
}