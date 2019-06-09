using System;
using System.Collections;
using System.Collections.Generic;
using LINQtoCSV;

namespace TestConsole
{
    class Utils
    {
        /// ///////////////////////////////////////////////////////////////////////
        /// OutputException
        /// 
        public static void OutputException(Exception e)
        {
            Console.WriteLine("#################### Exception");
            Console.WriteLine(e.GetType().ToString());
            Console.WriteLine(e.Message);
            if (e.Data != null)
            {
                foreach (DictionaryEntry de in e.Data)
                {
                    Console.WriteLine(de.Key + "=" + de.Value);
                }
            }

            Console.WriteLine("");

            if (e is AggregatedException)
            {
                foreach (Exception e2 in ((AggregatedException)e).m_InnerExceptionsList)
                {
                    Console.WriteLine("#################### Inner Exception");
                    OutputException(e2);
                }
            }
        }

        /// ///////////////////////////////////////////////////////////////////////
        /// OutputException
        /// 
        public static void OutputData<T>(IEnumerable<ProductData_MissingFieldIndex> dataRows, string title)
        {
            Console.WriteLine("--------------- " + title);
            foreach (ProductData_MissingFieldIndex row in dataRows)
            {
                Console.WriteLine(row);
                Console.WriteLine("");
            }
        }

        public static void OutputData<T>(IEnumerable<T> dataRowsNamesUsRaw, string goodFileEnglishUsCultureRawDataRows)
        {
            throw new NotImplementedException();
        }
    }
}
