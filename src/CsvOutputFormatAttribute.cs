﻿using System;

namespace LINQtoCSV
{
    /// <summary>
    /// Summary description for FieldFormat
    /// </summary>
    [AttributeUsage(AttributeTargets.Field |
                           AttributeTargets.Property)
    ]
    public class CsvOutputFormatAttribute : Attribute
    {
        private string m_Format = "";
        public string Format
        {
            get { return m_Format; }
            set { m_Format = value; }
        }

        public CsvOutputFormatAttribute(string format)
        {
            m_Format = format;
        }
    }
}
