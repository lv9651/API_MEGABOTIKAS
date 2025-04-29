using SISLAB_API.Areas.Maestros.Models;

using System.Collections.Generic;
using System.Threading.Tasks;



namespace SISLAB_API.Areas.Maestros.Services
{
    public class BloqueAnuncioService
    {
        private readonly BloqueAnuncioRepository _repository;

        // Constructor to inject the repository
        public BloqueAnuncioService(BloqueAnuncioRepository repository)
        {
            _repository = repository;
        }

        // Get a block by ID
        public async Task<BloqueAnuncio> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        // Get all blocks
        public async Task<IEnumerable<BloqueAnuncio>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        // Create a new block
        public async Task<int> CreateAsync(BloqueAnuncio bloqueAnuncio)
        {
            return await _repository.CreateAsync(bloqueAnuncio);
        }

        // Update an existing block
        public async Task<int> UpdateAsync(BloqueAnuncio bloqueAnuncio)
        {
            return await _repository.UpdateAsync(bloqueAnuncio);
        }

        // Delete a block by ID
        public async Task<int> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}