using FrikiMarvelApi.Domain.Entities;

namespace FrikiMarvelApi.Domain.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetByIdentificationAsync(string identification);
    Task<bool> ExistsByEmailAsync(string email);
    Task<bool> ExistsByIdentificationAsync(string identification);
    Task SaveChangesAsync();
}
