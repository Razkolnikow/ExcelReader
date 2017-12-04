using System;

namespace Models
{
    public class NobelPrizeWinner
    {
        public int Id { get; set; }

        public int Year { get; set; }

        public string Category { get; set; }

        public string Name { get; set; }

        public string Birthdate { get; set; }

        public string BirthPlace { get; set; }

        public string County { get; set; }

        public string Residence { get; set; }

        public string FieldLanguage { get; set; }

        public string PrizeName { get; set; }

        public string Motivation { get; set; }
    }
}
