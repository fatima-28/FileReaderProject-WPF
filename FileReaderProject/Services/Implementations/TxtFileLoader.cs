using FileReaderProject.Models;
using FileReaderProject.Services.Interfaces;
using System.IO;
using System.Windows;

namespace FileReaderProject.Services.Implementations
{
    public class TxtFileLoader : IFileLoader
    {
        public List<TradeData> Load(string filePath)
        {

            try
            {
                List<TradeData> tradeDataList = new List<TradeData>();
                var lines = File.ReadAllLines(filePath);

                foreach (var line in File.ReadLines(filePath))
                {
                    var parts = line.Split(',');
                    if (parts.Length == 6)
                    {
                        tradeDataList.Add(new TradeData
                        {
                            FileName = Path.GetFileName(filePath),
                            Date = DateTime.Parse(parts[0]),
                            Open = decimal.Parse(parts[1]),
                            High = decimal.Parse(parts[2]),
                            Low = decimal.Parse(parts[3]),
                            Close = decimal.Parse(parts[4]),
                            Volume = decimal.Parse(parts[5]),

                        });
                    }
                }

                return tradeDataList;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Something went wrong! {ex.Message}");

                return new List<TradeData>();

            }

        }
    }
}
