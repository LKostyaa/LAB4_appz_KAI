using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildrenLeisure.DAL.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsBirthdayParty { get; set; }
        public int? FairyCharacterId { get; set; }
        public virtual FairyCharacter FairyCharacter { get; set; }
        public virtual ICollection<Attraction> SelectedAttractions { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderStatus Status { get; set; }
    }

    public enum OrderStatus
    {
        Pending, Confirmed, InProgress, Completed, Cancelled
    }
}
