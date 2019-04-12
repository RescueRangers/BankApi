using BankFromApi.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace BankFromApi.Converters
{
    public class HalfTheValueConverter : IMultiValueConverter
    {
        
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if(values.Length != 2)
                throw new ArgumentException("Incorrect number of arguments");
            try
            {
                var series = (IList<SeriesWithLabels>)values[1];
                var actualValue = (double)values[0];


                if (series.Count > 1)
                    return (actualValue - 30) / 2;
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
