using BankFromApi.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace BankFromApi.Converters
{
    public class HalfTheValueConverter : IMultiValueConverter
    {
        /// <summary>
        /// Sprawdza ile wykresow znajduje sie na liscie wykresow 
        /// i na tej podstawie ustala wartosc szerokosci i wysokosci wykresu
        /// </summary>
        /// <param name="values"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if(values.Length != 2)
                throw new ArgumentException("Incorrect number of arguments");
            try
            {
                var series = (IList<SeriesWithLabels>)values[1];
                var actualValue = (double)values[0];


                if (series.Count > 1 && series.Count <= 4)
                    return (actualValue - 30) / 2;
                else if(series.Count > 4)
                    return (actualValue - 50) / 2;
                return actualValue - 10;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
