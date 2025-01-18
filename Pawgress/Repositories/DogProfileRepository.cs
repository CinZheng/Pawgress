using Pawgress.Data;
using Pawgress.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Pawgress.Repositories
{
    public class DogProfileRepository : IDogProfileRepository
    {
        private readonly ApplicationDbContext _context;

        public DogProfileRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<DogProfile> GetAll()
        {
            return _context.DogProfiles.ToList();
        }

        public DogProfile? GetById(Guid id)
        {
            return _context.DogProfiles.FirstOrDefault(dp => dp.DogProfileId == id);
        }

        public DogProfile Create(DogProfile dogProfile)
        {
            _context.DogProfiles.Add(dogProfile);
            _context.SaveChanges();
            return dogProfile;
        }

        public DogProfile? Update(Guid id, DogProfile dogProfile)
        {
            var existingDogProfile = _context.DogProfiles.FirstOrDefault(dp => dp.DogProfileId == id);
            if (existingDogProfile == null) return null;

            existingDogProfile.Name = dogProfile.Name;
            existingDogProfile.Breed = dogProfile.Breed;
            existingDogProfile.Image = dogProfile.Image;
            existingDogProfile.DateOfBirth = dogProfile.DateOfBirth;
            existingDogProfile.UpdateDate = DateTime.Now;

            _context.SaveChanges();
            return existingDogProfile;
        }

        public bool Delete(Guid id)
        {
            var dogProfile = _context.DogProfiles.FirstOrDefault(dp => dp.DogProfileId == id);
            if (dogProfile == null) return false;

            _context.DogProfiles.Remove(dogProfile);
            _context.SaveChanges();
            return true;
        }
    }
} 