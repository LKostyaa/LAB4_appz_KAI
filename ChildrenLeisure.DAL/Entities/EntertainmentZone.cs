using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildrenLeisure.DAL.Entities
{
    public class EntertainmentZone
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal BasePrice { get; set; }
        public virtual ICollection<Attraction> Attractions { get; set; }
    }
}