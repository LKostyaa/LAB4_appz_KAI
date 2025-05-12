using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildrenLeisure.DAL.Entities
{
    public class FairyCharacter
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Costume { get; set; }
        public decimal PricePerHour { get; set; }
        public string Description { get; set; }
    }
}
