using Models;

namespace Data
{
    using System.Data.Entity;

    public class NobelPrizeWinners : DbContext
    {
        public NobelPrizeWinners()
            : base("NobelPrizeWinners")
        {
        }
        public virtual IDbSet<NobelPrizeWinner> PrizeWinners { get; set; }
    }

}