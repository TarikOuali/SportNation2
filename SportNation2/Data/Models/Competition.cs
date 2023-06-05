using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SportNation2.Data.Models
{
    public class Competition: Entity
    {
        public DateTime Date { get; set; }

        public Sport Sport { get; set; }
        public int SportId { get; set; }

        public List<CompetitionEvent> CompetitionEvents { get; set; }
            = new List<CompetitionEvent>();
    }
}
