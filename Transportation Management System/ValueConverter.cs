﻿
/* -- FILEHEADER COMMENT --
    FILE		:	ValueConverter.cs
    PROJECT		:	Transportation Management System
    PROGRAMMER	:  * Ana De Oliveira
                   * Icaro Ryan Oliveira Souza
                   * Lazir Pascual
                   * Rohullah Noory
    DATE		:	2021-12-07
    DESCRIPTION	:	This file contains the source for the StatusConverter class.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Globalization;

namespace Transportation_Management_System
{
    public class StatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, 
                System.Globalization.CultureInfo culture)
        {
            switch (value)
            {
                case 0:
                    return "Active";
                case 1:
                    return "Completed";
                default:
                    return "Active";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, 
                System.Globalization.CultureInfo culture)
        {
            switch (value.ToString().ToLower())
            {
                case "Active":
                    return 0;
                case "Completed":
                    return 1;
                default:
                    return 0;
            }
        }
    }

    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
        {
            switch (value.ToString())
            {
                case "1/1/0001 12:00:00 AM":
                    return "N/A";
                default:
                    return value.ToString();
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter,
                System.Globalization.CultureInfo culture)
        {
            switch (value)
            {
                case "N/A":
                    return null;
                default:
                    return value;
            }
        }
    }
}
