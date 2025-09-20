using Microsoft.EntityFrameworkCore;
using FrikiMarvelApi.Domain.Entities;
using FrikiMarvelApi.Domain.Interfaces;
using FrikiMarvelApi.Infrastructure.Persistence;

namespace FrikiMarvelApi.Infrastructure.Persistence.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email && u.IsActive);
    }

    public async Task<User?> GetByIdentificationAsync(string identification)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Identification == identification && u.IsActive);
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _context.Users
            .AnyAsync(u => u.Email == email);
    }

    public async Task<bool> ExistsByIdentificationAsync(string identification)
    {
        return await _context.Users
            .AnyAsync(u => u.Identification == identification);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
