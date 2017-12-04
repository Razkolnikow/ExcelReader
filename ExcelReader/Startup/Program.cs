using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Models;
using Services;

namespace Startup
{
    class Program
    {
        static void Main(string[] args)
        {
            string filePath = "..\\..\\DataToRead\\NobelPrizeWinners.xlsx";

            var excelService = new ExcelDataReaderService();

            var winners = excelService.ReadFromExcel<NobelPrizeWinner>(filePath);
            using (var context = new NobelPrizeWinners())
            {
                var names = context.PrizeWinners.Select(w => w.Name).ToList();
                HashSet<string> winnersNames = new HashSet<string>();
                foreach (var name in names)
                {
                    winnersNames.Add(name);
                }

                foreach (var winner in winners)
                {
                    if (winnersNames.Contains(winner.Name))
                    {
                        var prizeWinner = context.PrizeWinners.First(w => w.Name == winner.Name);
                        prizeWinner.BirthPlace = winner.BirthPlace;
                        prizeWinner.County = winner.County;
                        prizeWinner.FieldLanguage = winner.FieldLanguage;
                        prizeWinner.Motivation = winner.Motivation;
                        prizeWinner.PrizeName = winner.PrizeName;
                        prizeWinner.Residence = winner.Residence;
                        prizeWinner.Year = winner.Year;
                        prizeWinner.Birthdate = winner.Birthdate;
                        prizeWinner.Category = winner.Category;
                    }
                    else
                    {
                        context.PrizeWinners.Add(winner);
                    }
                }
                
                context.SaveChanges();
            }
        }
    }
}
