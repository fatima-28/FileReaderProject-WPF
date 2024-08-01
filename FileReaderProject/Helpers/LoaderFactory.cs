using FileReaderProject.Services.Implementations;
using FileReaderProject.Services.Interfaces;
using System.Windows;

namespace FileReaderProject.Helpers;


public class LoaderFactory
{
    public static IFileLoader GetLoader(string fileType)
    {
        switch (fileType.ToLower())
        {
            case ".txt":
                return new TxtFileLoader();
            case ".xml":
                return new XmlFileLoader();
            case ".csv":
                return new CsvFileLoader();
            default:
                MessageBox.Show($"File type '{fileType}' is not supported.");
                return null;
        }
    }
}
