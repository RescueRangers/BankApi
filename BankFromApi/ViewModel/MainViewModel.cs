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
using System.Windows.Data;
using System.Windows.Media;

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
        private static readonly List<string> _colours = new List<string> { "#ff0000", "#a3a67c", "#99c2cc", "#8500a6", "#4c0000", "#c2f200", "#13394d", 
            "#80407b", "#bf3030", "#5c7300", "#0081f2", "#ff00cc", "#cc9999", "#40ff40", "#466c8c", "#4c0029", "#d95700", "#60bf6c", "#404880", "#ff4073", 
            "#f2aa79", "#264d36", "#281d73", "#994d61", "#593a16", "#30bfa3", "#4100f2", "#8c5e00", "#1a6166", "#9979f2", "#e5c339", "#00b8e6", "#b499cc" };

        private DateTime _dateFrom = DateTime.Today.AddDays(-1);
        private DateTime _dateTo = DateTime.Today;
        private ObservableCollection<Rate> _symbols;
        private Rate _selectedSymbol;
        private string _appStatus;
        private ObservableCollection<SeriesWithLabels> _seriesCollections = new ObservableCollection<SeriesWithLabels>();
        private SeriesWithLabels _goldPriceSeries;

        public SeriesWithLabels GoldPriceSeries
        {
            get => _goldPriceSeries;
            set => Set(nameof(GoldPriceSeries), ref _goldPriceSeries, value);
        }

        public ObservableCollection<SeriesWithLabels> SeriesCollections
        {
            get => _seriesCollections;
            set => Set(nameof(SeriesCollections), ref _seriesCollections, value);
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
        public ICommand GetGoldDataCommand { get; set; }
        public ICommand ClearGoldChartCommand { get; set; }
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            GetDataCommand = new RelayCommand(GetRate, CanGetData);
            ClearChartCommand = new RelayCommand(ClearChart, true);
            GetGoldDataCommand = new RelayCommand(GetGoldData, CanGetGoldData);
            ClearGoldChartCommand = new RelayCommand(ClearGoldChart, true);
            GetSymbols();
        }

        private void ClearGoldChart()
        {
            GoldPriceSeries.Clear();
        }

        private bool CanGetGoldData()
        {
            return DateFrom <= DateTo && DateTo <= DateTime.Today;
        }

        private async void GetGoldData()
        {
            var goldPrices = await GetFromNBPApi.GoldPrice(DateFrom, DateTo);

            var labels = goldPrices.Select(g => g.data);

            var series = new SeriesCollection
            {
                new LineSeries
                {
                    Values = new ChartValues<double>(goldPrices.Select(g => g.cena)),
                    Title = "Cena złota"
                },
            };

            GoldPriceSeries = new SeriesWithLabels(series, labels.ToList());
        }

        private void ClearChart()
        {
            SeriesCollections.Clear();
        }

        private bool CanGetData()
        {
            return DateFrom <= DateTo && SelectedSymbol != null && DateTo <= DateTime.Today;
        }

        private async void GetSymbols()
        {
            var symbols = await GetFromNBPApi.Symbols();
            Symbols = new ObservableCollection<Rate>(symbols);
        }

        private async void GetRate()
        {
            var root = await GetFromNBPApi.CurrencyRate(DateFrom, DateTo, SelectedSymbol);
            if (root == null) return;

            var lineSeries = new List<double>();
            var labels = new List<string>();

            foreach (var item in root)
            {
                lineSeries.AddRange(item.rates.Select(s => s.mid));
                labels.AddRange(item.rates.Select(s => s.effectiveDate));
            }

            var brush = (Brush)new BrushConverter().ConvertFrom(_colours[SeriesCollections.Count % (_colours.Count + 1)]);
            var brushWithOpacity = brush.Clone();
            brushWithOpacity.Opacity = 0.3;

            var series = new SeriesCollection
            {
                new LineSeries
                {
                    Values = new ChartValues<double>(lineSeries),
                    Title = SelectedSymbol.ToString(),
                    Fill = brushWithOpacity,
                    Stroke = brush
                },
            };

            SeriesCollections.Add(new SeriesWithLabels(series, labels));

            if(SeriesCollections.Count == 2)
                CollectionViewSource.GetDefaultView(SeriesCollections).Refresh();
            if (SeriesCollections.Count == 5)
                CollectionViewSource.GetDefaultView(SeriesCollections).Refresh();
        }
    }
}