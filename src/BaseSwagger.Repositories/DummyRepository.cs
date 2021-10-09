using BaseSwagger.Data;
using BaseSwagger.Data.Models;
using BaseSwagger.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BaseSwagger.Repositories
{
    public class DummyRepository : IDummyRepository
    {
        private readonly Context _context;

        public DummyRepository(Context context)
        {
            _context = context;
        }

        public IEnumerable<Dummy> Get()
        {
            return DummiesAsQueryable()
                .AsNoTracking();
        }

        public Dummy Get(string id)
        {
            return DummiesAsQueryable()
                .AsNoTracking()
                .SingleOrDefault(d => d.Id == id);
        }

        public Dummy GetWithTracking(string id)
        {
            return DummiesAsQueryable()
                .AsTracking()
                .SingleOrDefault(d => d.Id == id);
        }

        public async Task Create(Dummy dummy)
        {
            _context.Add(dummy);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Dummy dummy)
        {
            _context.Update(dummy);
            await _context.SaveChangesAsync();
        }

        public async Task Delete(Dummy dummy)
        {
            _context.Remove(dummy);
            await _context.SaveChangesAsync();
        }

        private IQueryable<Dummy> DummiesAsQueryable() =>
            _context.Dummies.AsQueryable();
    }
}
