using ChildrenLeisure.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildrenLeisure.BLL.Services
{
    public class PricingService
    {
        public decimal CalculateOrderPrice(Order order)
        {
            decimal totalPrice = 0;

            // Базова вартість днів народження вища
            if (order.IsBirthdayParty)
            {
                totalPrice += 500; // Базова вартість організації дня народження
            }

            // Додавання вартості атракціонів
            if (order.SelectedAttractions != null)
            {
                totalPrice += order.SelectedAttractions.Sum(a => a.Price);
            }

            // Додавання вартості казкового героя
            if (order.FairyCharacter != null)
            {
                totalPrice += order.FairyCharacter.PricePerHour;
            }

            return totalPrice;
        }
    }
}
