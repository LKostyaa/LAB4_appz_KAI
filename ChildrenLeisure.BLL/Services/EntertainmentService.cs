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
        private readonly IRepository<Attraction> _attractionRepository;
        private readonly IRepository<FairyCharacter> _fairyCharacterRepository;

        public EntertainmentService(
            IRepository<Attraction> attractionRepository,
            IRepository<FairyCharacter> fairyCharacterRepository)
        {
            _attractionRepository = attractionRepository;
            _fairyCharacterRepository = fairyCharacterRepository;
        }
        // Отримання всіх атракціонів
        public IQueryable<Attraction> GetAllAttractions()
        {
            return _attractionRepository.GetAll();
        }

        // Отримання всіх казкових героїв
        public IQueryable<FairyCharacter> GetAllFairyCharacters()
        {
            return _fairyCharacterRepository.GetAll();
        }
    }
}
