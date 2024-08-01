using FileReaderProject.Models;
using FileReaderProject.Services.Interfaces;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Xml.Linq;

namespace FileReaderProject.Services.Implementations;


public class XmlFileLoader : IFileLoader
{
    public List<TradeData> Load(string filePath)
    {

        try
        {
            var tradeDataList = new List<TradeData>();

            var doc = XDocument.Load(filePath);
            var values = doc.Root.Elements("value");


            foreach (var value in values)
            {
                var tradeData = new TradeData
                {
                    Date = DateTime.Parse(value.Attribute("date").Value),
                    Open = decimal.Parse(value.Attribute("open").Value, CultureInfo.InvariantCulture),
                    High = decimal.Parse(value.Attribute("high").Value, CultureInfo.InvariantCulture),
                    Low = decimal.Parse(value.Attribute("low").Value, CultureInfo.InvariantCulture),
                    Close = decimal.Parse(value.Attribute("close").Value, CultureInfo.InvariantCulture),
                    Volume = decimal.Parse(value.Attribute("volume").Value, CultureInfo.InvariantCulture),
                    FileName = Path.GetFileName(filePath)
                };

                tradeDataList.Add(tradeData);
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
