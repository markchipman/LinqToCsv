﻿using System;
using System.Globalization;

namespace LINQtoCSV
{
    /// <summary>
    /// Summary description for CsvInputFormat
    /// </summary>
    [AttributeUsage(AttributeTargets.Field |
                           AttributeTargets.Property)
    ]
    public class CsvInputFormatAttribute : Attribute
    {
        private NumberStyles m_NumberStyle = NumberStyles.Any;
        public NumberStyles NumberStyle
        {
            get { return m_NumberStyle; }
            set { m_NumberStyle = value; }
        }

        public CsvInputFormatAttribute(NumberStyles numberStyle)
        {
            m_NumberStyle = numberStyle;
        }
    }
}
