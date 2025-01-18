using Pawgress.Repositories;
using Pawgress.Models;

namespace Pawgress.Services
{
    public class DogProfileService
    {
        private readonly IDogProfileRepository _dogProfileRepository;

        public DogProfileService(IDogProfileRepository dogProfileRepository)
        {
            _dogProfileRepository = dogProfileRepository ?? throw new ArgumentNullException(nameof(dogProfileRepository));
        }

        public List<DogProfile> GetAll()
        {
            return _dogProfileRepository.GetAll() ?? new List<DogProfile>();
        }

        public DogProfile? GetById(Guid id)
        {
            return _dogProfileRepository.GetById(id);
        }

        public DogProfile Create(DogProfile dogProfile)
        {
            if (dogProfile == null) throw new ArgumentNullException(nameof(dogProfile));

            dogProfile.DogProfileId = Guid.NewGuid();
            dogProfile.CreationDate = DateTime.UtcNow;
            dogProfile.UpdateDate = DateTime.UtcNow;
            return _dogProfileRepository.Create(dogProfile);
        }

        public DogProfile? Update(Guid id, DogProfile dogProfile)
        {
            if (dogProfile == null) throw new ArgumentNullException(nameof(dogProfile));

            var existingProfile = GetById(id);
            if (existingProfile == null) return null;

            dogProfile.CreationDate = existingProfile.CreationDate;
            dogProfile.UpdateDate = DateTime.UtcNow;
            return _dogProfileRepository.Update(id, dogProfile);
        }

        public bool Delete(Guid id)
        {
            return _dogProfileRepository.Delete(id);
        }
    }
}
