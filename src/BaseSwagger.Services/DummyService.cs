using BaseSwagger.Data.Models;
using BaseSwagger.Repositories.Interfaces;
using BaseSwagger.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BaseSwagger.Services
{
    public class DummyService : IDummyService
    {
        private readonly IDummyRepository _dummyRepository;

        public DummyService(IDummyRepository dummyRepository)
        {
            _dummyRepository = dummyRepository;
        }

        public IEnumerable<Dummy> Get()
        {
            return _dummyRepository.Get();
        }

        public Dummy Get(string id)
        {
            return _dummyRepository.Get(id);
        }

        public async Task Create(Dummy dummy)
        {
            await _dummyRepository.Create(dummy);
        }

        public async Task Update(string id, Dummy updateDummy)
        {
            var dummy = _dummyRepository.GetWithTracking(id);
            if (dummy.Name != updateDummy.Name)
            {
                dummy.Name = updateDummy.Name;
            }

            await _dummyRepository.Update(dummy);
        }

        public async Task Delete(string id)
        {
            var dummy = _dummyRepository.GetWithTracking(id);
            await _dummyRepository.Delete(dummy);
        }
    }
}
