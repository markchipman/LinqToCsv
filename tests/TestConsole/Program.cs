﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using LINQtoCSV;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            // ------------------------------------
            // Reading files, no erros
            // The input files are meant to test the library code, so have lots of weird cases.

            // ----
            // Read comma delimited file with names in first line, US-English culture. 
            // Fields do not have to have CsvColumn attribute.

            CsvContext cc = new CsvContext();

            IEnumerable<ProductData_MissingFieldIndex> dataRows_namesUs = null;
            IEnumerable<TestDataRow> dataRows_namesUsRaw = null;
            CsvMetaData metaDataNamesUs = new CsvMetaData
            {
                SeparatorChar = ',', // default is ','
                FirstLineHasColumnNames = true,
                EnforceCsvColumnAttribute = false, // default is false
                FileCultureName = "en-US" // default is the current culture
            };

            try
            {
                dataRows_namesUs =
                    cc.Read<ProductData>("TestFiles/goodfile_us.csv", metaDataNamesUs) as IEnumerable<ProductData_MissingFieldIndex>;

                Utils.OutputData<ProductData>(dataRows_namesUs, "Good file, English US culture");

                // -----------
                // Manually change contents of file, to see whether file is read again with new values.

                Utils.OutputData<ProductData>(dataRows_namesUs, "Good file, English US culture, second read");

                // ------------
                // Partial read - read just one record from the file

                foreach (ProductData_MissingFieldIndex row in dataRows_namesUs)
                {
                    break;
                }

                // -----------
                // Read raw data rows

                dataRows_namesUsRaw =
                    cc.Read<TestDataRow>("TestFiles/goodfile_us.csv", metaDataNamesUs) as IEnumerable<TestDataRow>;

                Utils.OutputData<TestDataRow>(dataRows_namesUsRaw, "Good file, English US culture, Raw data rows");
            }
            catch (Exception e)
            {
                Utils.OutputException(e);
            }

            // ----
            // Read file without names, Dutch culture, tab delimited.

            // EnforceCsvColumnAttribute is not set, because it is implicitly true
            // when there are no names in the first line. This is because only
            // fields that have a FieldIndex can be used, which means having
            // a CsvColumn attribute.

            CsvMetaData metaDataNonamesNl = new CsvMetaData
            {
                SeparatorChar = '\t', // tab character
                FirstLineHasColumnNames = false,
                EnforceCsvColumnAttribute = true,
                FileCultureName = "nl-NL" // default is the current culture
            };

            try
            {
                var dataRowsNonamesNl =
                    cc.Read<ProductData>("TestFiles/goodfile_nl.csv", metaDataNonamesNl);

                Utils.OutputData<ProductData>(dataRowsNonamesNl, "Good file, Dutch culture");
            }
            catch (Exception e)
            {
                Utils.OutputException(e);
            }

            // ----
            // Read a stream instead of a file, without names, Dutch culture, tab delimited.

            // EnforceCsvColumnAttribute is not set, because it is implicitly true
            // when there are no names in the first line. This is because only
            // fields that have a FieldIndex can be used, which means having
            // a CsvColumn attribute.

            CsvMetaData metaDataNonamesNlStream = new CsvMetaData
            {
                SeparatorChar = '\t', // tab character
                FirstLineHasColumnNames = false,
                EnforceCsvColumnAttribute = true,
                FileCultureName = "nl-NL" // default is the current culture
            };

            try
            {
                using (StreamReader sr =
                    new StreamReader("TestFiles/goodfile_nl.csv", Encoding.UTF8))
                {
                   var dataRowsNonamesNl =
                        cc.Read<ProductData>(sr, metaDataNonamesNlStream);

                    Utils.OutputData(dataRowsNonamesNl, "Good file, Dutch culture, using stream");
                }
            }
            catch (Exception e)
            {
                Utils.OutputException(e);
            }

            // ------------------------------------
            // Reading files, with errors

            // Type has duplicate FileIndices

            try
            {
                IEnumerable<ProductData_DuplicateIndices> dataRows2 =
                    cc.Read<ProductData_DuplicateIndices>("TestFiles/goodfile_nl.csv", metaDataNonamesNl);

                Utils.OutputData(dataRows2, "Good file, Dutch culture");
            }
            catch (Exception e)
            {
                Utils.OutputException(e);
            }

            // Type has required fields that do not have a FieldIndex.

            try
            {
                IEnumerable<ProductData_MissingFieldIndex> dataRows2 =
                    cc.Read<ProductData_MissingFieldIndex>("TestFiles/goodfile_nl.csv", metaDataNonamesNl);

                Utils.OutputData<ProductData_MissingFieldIndex>(dataRows2, "Good file, Dutch culture");
            }
            catch (Exception e)
            {
                Utils.OutputException(e);
            }

            // CsvFileDescription.EnforceCsvColumnAttribute is false, but needs to be true because
            // CsvFileDescription.FirstLineHasColumnNames is false.

            CsvMetaData metaDataBad = new CsvMetaData
            {
                SeparatorChar = '\t', // tab character
                FirstLineHasColumnNames = false,
                EnforceCsvColumnAttribute = false,
                FileCultureName = "nl-NL" // default is the current culture
            };

            try
            {
                IEnumerable<ProductData> dataRows_nonamesNl_bad =
                    cc.Read<ProductData>("TestFiles/goodfile_nl.csv", metaDataBad);

                Utils.OutputData<ProductData>(dataRows_nonamesNl_bad, "Good file, Dutch culture");
            }
            catch (Exception e)
            {
                Utils.OutputException(e);
            }

            // ----
            // Read file with names, but one name not declared in type

            try
            {
                IEnumerable<ProductData> dataRows_namesUs_3 =
                    cc.Read<ProductData>("TestFiles/badfile_unknownname.csv", metaDataNamesUs);

                Utils.OutputData<ProductData>(dataRows_namesUs_3, "Bad file, English US culture, unknown name");
            }
            catch (Exception e)
            {
                Utils.OutputException(e);
            }


            // ----
            // Read file with names, only columns with CsvColumn attribute participate.
            // But one name matches a column without CsvColumn attribute.

           var fileDescriptionNamesUsEnforceCsvColumn = new CsvMetaData
            {
                SeparatorChar = ',', // default is ','
                FirstLineHasColumnNames = true,
                EnforceCsvColumnAttribute = true, // default is false
                FileCultureName = "en-US" // default is the current culture
            };

            try
            {
               var dataRowsNamesUs2 =
                    cc.Read<ProductData>("TestFiles/goodfile_us.csv", fileDescriptionNamesUsEnforceCsvColumn);

                Utils.OutputData(dataRowsNamesUs2, "Good file, English US culture");
            }
            catch (Exception e)
            {
                Utils.OutputException(e);
            }


            // ----
            // Various errors in data fields - all captured in AggregatedException
            // * A row with too many fields
            // * Rows with badly formatted data - letters in numeric fields, bad dates

            CsvMetaData metaDataNonamesUs = new CsvMetaData
            {
                SeparatorChar = ',', // default is ','
                FirstLineHasColumnNames = true,
                EnforceCsvColumnAttribute = true,
                FileCultureName = "en-US" // default is the current culture
            };

            try
            {
               var dataRowsNamesUsDataerrors =
                    cc.Read<ProductData>("TestFiles/badfile_us_dataerrors.csv", metaDataNonamesUs);

                Utils.OutputData(
                        dataRowsNamesUsDataerrors,
                        "Bad file, English US culture, various data errors");
            }
            catch (Exception e)
            {
                Utils.OutputException(e);
            }


            // ------------------------------------
            // Writing files

            // ---------------
            // Create own IEnumarable, rather then using one created by reading a file.
            // Dutch, names in first line, don't limit writing to fields with CsvColumn attribute.

           var dataRowsTest = new List<ProductData>();
            dataRowsTest.Add(new ProductData { retailPrice = 4.59M, name = "Wooden toy", startDate = DateTime.Parse("1/2/2008"), nbrAvailable = 67 });
            dataRowsTest.Add(new ProductData { onsale = true, weight = 4.03, shopsAvailable = "Ashfield", description = "" });
            dataRowsTest.Add(new ProductData { name = "Metal box", launchTime = DateTime.Parse("5/11/2009 4:50"), description = "Great\nproduct" });

            var fileDescriptionNamesNl2 = new CsvMetaData
            {
                SeparatorChar = ',',
                FirstLineHasColumnNames = true,
                EnforceCsvColumnAttribute = false,
                TextEncoding = Encoding.Unicode,
                FileCultureName = "nl-Nl" // default is the current culture
            };

            try
            {
                cc.Write(
                            dataRowsTest,
                            "TestFiles/output_newdata_names_nl.csv",
                            fileDescriptionNamesNl2);
            }
            catch (Exception e)
            {
                Utils.OutputException(e);
            }

            // ---------------
            // Write tab delimited file, US-English, no names in first line
            // using FieldIndices provided in CsvColumn attributes

            var fileDescription_nonamesUs_output = new CsvMetaData
            {
                SeparatorChar = '\t', 
                FirstLineHasColumnNames = false,
                EnforceCsvColumnAttribute = true, 
                FileCultureName = "en-US" // default is the current culture
            };

            try
            {
                cc.Write<ProductData>(
                            dataRows_namesUs, 
                            "TestFiles/output_nonames_us.csv",
                            fileDescription_nonamesUs_output);
            }
            catch (Exception e)
            {
                Utils.OutputException(e);
            }

            // ---------------
            // Write comma delimited file, Dutch, names in first line using 
            // CsvColumn attributes

            var fileDescriptionNamesNl = new CsvMetaData
            {
                SeparatorChar = ',', 
                FirstLineHasColumnNames = true,
                EnforceCsvColumnAttribute = true,
                TextEncoding = Encoding.Unicode,
                FileCultureName = "nl-NL" // default is the current culture
            };

            try
            {
                cc.Write<ProductData>(
                            dataRows_namesUs, 
                            "TestFiles/output_names_nl.csv",
                            fileDescriptionNamesNl);
            }
            catch (Exception e)
            {
                Utils.OutputException(e);
            }


            // Write comma delimited file, column names in first record,
            // using anonymous type. Because there are no FieldIndices,
            // the order of the fields on each line in the file is not guaranteed.
            //
            // FileCultureName is not set, so the current culture is used
            // (so if you are Canadian, the system uses Canadian dates, etc.)

            var fileDescriptionAnon = new CsvMetaData
            {
                SeparatorChar = ',',
                FirstLineHasColumnNames = true,
                EnforceCsvColumnAttribute = false
                // use current culture
            };

            try
            {
                var query = from row in dataRows_namesUs
                            orderby row.weight
                            select new { 
                                ProductName = row.name,
                                InShops = row.startDate,
                                Markup = row.retailPrice * (decimal)0.5 };

                cc.Write(
                    query,
                    "TestFiles/output_anon.csv",
                    fileDescriptionAnon);
            }
            catch (Exception e)
            {
                Utils.OutputException(e);
            }

            // ------------------------------------
            // Writing files, with errors

            // If not writing field names to the first line, then you have to only
            // use fields with the CsvColumn attribute, and give them all a FieldIndex.
            // Otherwise, there is no reliable way to read back the data, because the order
            // of the fields is not guaranteed.

            try
            {
                cc.Write<ProductData>(
                            dataRows_namesUs, 
                            "TestFiles/output_bad.csv",
                            metaDataBad);
            }
            catch (Exception e)
            {
                Utils.OutputException(e);
            }


            // CsvFileDescription settings are good, but not all fields with CsvColumn attribute
            // have a FieldIndex.

            CsvMetaData metaDataNonamesNl2 = new CsvMetaData
            {
                SeparatorChar = ',',
                FirstLineHasColumnNames = false,
                EnforceCsvColumnAttribute = true,
                TextEncoding = Encoding.Unicode,
                FileCultureName = "nl-NL" // default is the current culture
            };

            try
            {
                List<ProductData_MissingFieldIndex> emptyData = new List<ProductData_MissingFieldIndex>();

                cc.Write<ProductData_MissingFieldIndex>(
                            emptyData,
                            "TestFiles/output_bad.csv",
                            metaDataNonamesNl2);
            }
            catch (Exception e)
            {
                Utils.OutputException(e);
            }

        }
    }
}
