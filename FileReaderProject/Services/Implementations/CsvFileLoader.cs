using CsvHelper;
using CsvHelper.Configuration;
using FileReaderProject.Models;
using FileReaderProject.Services.Interfaces;
using System.Globalization;
using System.IO;
using System.Windows;

namespace FileReaderProject.Services.Implementations;

public class CsvFileLoader : IFileLoader
{

    public List<TradeData> Load(string filePath)
    {

        try
        {
            var trades = new List<TradeData>();

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                MissingFieldFound = null,
            };

            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, config))
            {
                csv.Read();
                csv.ReadHeader();

                var fileName = Path.GetFileName(filePath);

                while (csv.Read())
                {
                    trades.Add(new TradeData
                    {
                        Date = csv.GetField<DateTime>("Date"),
                        Open = csv.GetField<decimal>("Open"),
                        High = csv.GetField<decimal>("High"),
                        Low = csv.GetField<decimal>("Low"),
                        Close = csv.GetField<decimal>("Close"),
                        Volume = csv.GetField<decimal>("Volume"),
                        FileName = fileName
                    });
                }
            }

            return trades;

        }
        catch (Exception ex)
        {
            MessageBox.Show($"Something went wrong! {ex.Message}");

            return new List<TradeData>();

        }

    }

}
