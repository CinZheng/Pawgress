using Pawgress.Repositories;
using Pawgress.Models;

namespace Pawgress.Services
{
    public class DogProfileService
    {
        private readonly IDogProfileRepository _dogProfileRepository;

        public DogProfileService(IDogProfileRepository dogProfileRepository)
        {
            _dogProfileRepository = dogProfileRepository;
        }

        public List<DogProfile> GetAll()
        {
            return _dogProfileRepository.GetAll();
        }

        public DogProfile? GetById(Guid id)
        {
            return _dogProfileRepository.GetById(id);
        }

        public DogProfile Create(DogProfile dogProfile)
        {
            dogProfile.DogProfileId = Guid.NewGuid();
            dogProfile.CreationDate = DateTime.Now;
            dogProfile.UpdateDate = DateTime.Now;
            return _dogProfileRepository.Create(dogProfile);
        }

        public DogProfile? Update(Guid id, DogProfile dogProfile)
        {
            return _dogProfileRepository.Update(id, dogProfile);
        }

        public bool Delete(Guid id)
        {
            return _dogProfileRepository.Delete(id);
        }
    }
}
