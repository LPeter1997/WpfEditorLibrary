using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace Nfh.EditorLibrary.Converters
{
    /// <summary>
    /// A simple converter for negating a boolean value.
    /// </summary>
    [ValueConversion(typeof(bool), typeof(bool))]
    public class NegateBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (ReferenceEquals(value, DependencyProperty.UnsetValue))
            {
                return DependencyProperty.UnsetValue;
            }
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new InvalidOperationException($"{nameof(NegateBooleanConverter)} is a one-way converter!");
        }
    }
}
