using ChildrenLeisure.DAL.Entities;
using ChildrenLeisure.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChildrenLeisure.BLL.Services
{
    public class EntertainmentService
    {
        private readonly IRepository<EntertainmentZone> _zoneRepository;
        private readonly IRepository<Attraction> _attractionRepository;
        private readonly IRepository<FairyCharacter> _fairyCharacterRepository;

        public EntertainmentService(
            IRepository<EntertainmentZone> zoneRepository,
            IRepository<Attraction> attractionRepository,
            IRepository<FairyCharacter> fairyCharacterRepository)
        {
            _zoneRepository = zoneRepository;
            _attractionRepository = attractionRepository;
            _fairyCharacterRepository = fairyCharacterRepository;
        }

        // Отримання всіх зон розваг
        public async Task<IQueryable<EntertainmentZone>> GetAllZonesAsync()
        {
            return await _zoneRepository.GetAllAsync();
        }

        // Отримання всіх атракціонів
        public async Task<IQueryable<Attraction>> GetAllAttractionsAsync()
        {
            return await _attractionRepository.GetAllAsync();
        }

        // Отримання всіх казкових героїв
        public async Task<IQueryable<FairyCharacter>> GetAllFairyCharactersAsync()
        {
            return await _fairyCharacterRepository.GetAllAsync();
        }

    }
}
