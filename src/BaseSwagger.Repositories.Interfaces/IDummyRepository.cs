using BaseSwagger.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BaseSwagger.Repositories.Interfaces
{
    public interface IDummyRepository
    {
        IEnumerable<Dummy> Get();

        Dummy Get(string id);

        Dummy GetWithTracking(string id);

        Task Create(Dummy dummy);

        Task Update(Dummy dummy);

        Task Delete(Dummy dummy);
    }
}
