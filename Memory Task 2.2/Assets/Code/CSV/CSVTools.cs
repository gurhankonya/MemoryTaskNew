using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CSVTools 
{
    public static List<string> GenerateCSV(List<List<string>> data, List<string> csvHeaders , char separator = ',')
    {
        List<string> convertedData = new List<string>();
        string convertedHeaders = "";
        // converted our headers to comma separated variables format
        foreach (string header in csvHeaders)
        {
            convertedHeaders = convertedHeaders + header + separator;
        }

        convertedData.Add(convertedHeaders);
        // converted our data to comma separated variables format
        foreach (List<string> rowData in data)
        {
            string convertedRowData = "";
            foreach (string dataItems in rowData)
            {
                convertedRowData = convertedRowData + dataItems + separator;
            }
            convertedData.Add(convertedRowData);
        }
        return convertedData;
    }

    public static bool SaveCSV(List<string> csvLines, string fileAddress, string extension = ".csv")
    {
        

        try
        {
            using (StreamWriter csvWriter = new StreamWriter(fileAddress + extension))
            {
                foreach (string csvLine in csvLines)
                {
                    csvWriter.WriteLine(csvLine);
                    
                }
                csvWriter.Close();
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e);
            throw;
        }
        return true;
    }
}
