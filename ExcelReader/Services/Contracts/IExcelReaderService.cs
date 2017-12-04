using System.Collections.Generic;

namespace Services.Contracts
{
    public interface IExcelReaderService
    {
        IEnumerable<T> ReadFromExcel<T>(string pathToExcelFile);
    }
}
