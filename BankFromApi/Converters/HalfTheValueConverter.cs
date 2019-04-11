using System;
using System.Globalization;
using System.Windows.Data;

namespace BankFromApi.Converters
{
    public class HalfTheValueConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var actualValue = (double)value;

                return (actualValue - 30) / 2;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
