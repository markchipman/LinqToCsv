﻿using System;
using LINQtoCSV;

namespace TestConsole
{
// Because the fields in this type are used only indirectly, the compiler
// will warn they are unused or unassigned. Disable those warnings.
#pragma warning disable 0169, 0414, 0649

    class ProductData_DuplicateIndices
    {
        [CsvColumn(FieldIndex = 1)]
        public string name;

        // , OutputFormat uses the same codes as the standard ToString method (search MSDN).
        [CsvColumn(FieldIndex = 2, OutputFormat = "d")]
        public DateTime startDate;

        [CsvColumn(FieldIndex = 3, OutputFormat = "dd MMM HH:mm:ss")]
        public DateTime launchTime;

        // ########## Duplicate index below

        // Can use both fields and properties
        [CsvColumn(FieldIndex = 2, OutputFormat = "#,000.000")]
        public double weight { get; set; }

        // Following field has no CsvColumn attribute.
        // So will be ignored when library is told to only use data with CsvColumn attribute.
        public int nbrAvailable;

        // Ok to have gaps in FieldIndex order
        [CsvColumn(FieldIndex = 10)]
        public string shopsAvailable;
    }
#pragma warning restore 0169, 0414, 0649
}
