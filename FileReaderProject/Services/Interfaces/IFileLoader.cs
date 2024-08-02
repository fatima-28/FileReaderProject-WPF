using FileReaderProject.Models;

namespace FileReaderProject.Services.Interfaces;

public interface IFileLoader
{
    List<TradeData> Load(string path);
}
