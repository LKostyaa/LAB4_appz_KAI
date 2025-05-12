using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChildrenLeisure.DAL.Entities;
using ChildrenLeisure.DAL.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ChildrenLeisure.BLL.Services
{
    public class OrderService
    {
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<Attraction> _attractionRepository;
        private readonly IRepository<FairyCharacter> _fairyCharacterRepository;
        private readonly PricingService _pricingService;

        public OrderService(
            IRepository<Order> orderRepository,
            IRepository<Attraction> attractionRepository,
            IRepository<FairyCharacter> fairyCharacterRepository,
            PricingService pricingService)
        {
            _orderRepository = orderRepository;
            _attractionRepository = attractionRepository;
            _fairyCharacterRepository = fairyCharacterRepository;
            _pricingService = pricingService;
        }

        public Order CreateOrder(
            string customerName,
            string phoneNumber,
            bool isBirthdayParty,
            int? fairyCharacterId = null,
            int[]? attractionIds = null,
            int[]? zoneIds = null)
        {
            var order = new Order
            {
                CustomerName = customerName,
                PhoneNumber = phoneNumber,
                OrderDate = DateTime.Now,
                IsBirthdayParty = isBirthdayParty,
                Status = OrderStatus.Pending
            };

            // Додавання казкового героя
            if (fairyCharacterId.HasValue)
            {
                order.FairyCharacter = _fairyCharacterRepository.GetById(fairyCharacterId.Value);
            }

            // Додавання атракціонів
            if (attractionIds != null && attractionIds.Length > 0)
            {
                order.SelectedAttractions = _attractionRepository
                    .GetAll() 
                    .Where(a => attractionIds.Contains(a.Id))
                    .ToList();
            }

            // Розрахунок ціни
            order.TotalPrice = _pricingService.CalculateOrderPrice(order);

            return _orderRepository.Add(order);
        }

        // Підтвердження замовлення
        public Order ConfirmOrder(int orderId)
        {
            var order = _orderRepository.GetById(orderId);
            if (order == null)
                throw new ArgumentException("Замовлення не знайдено");

            order.Status = OrderStatus.Confirmed;
            return _orderRepository.Update(order);
        }
    }
}