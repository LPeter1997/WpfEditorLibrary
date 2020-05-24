using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Nfh.EditorLibrary.Converters
{
    /// <summary>
    /// A value converter for conjunction of multiple boolean values.
    /// </summary>
    public class AndBooleanConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            foreach (var value in values)
            {
                if (ReferenceEquals(value, DependencyProperty.UnsetValue))
                {
                    return DependencyProperty.UnsetValue;
                }
                if (!(bool)value)
                {
                    return false;
                }
            }
            return true;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException($"{nameof(AndBooleanConverter)} is a one-way converter!");
        }
    }
}
