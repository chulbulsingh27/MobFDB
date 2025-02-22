﻿using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using MobFDB.Interface;
using MobFDB.Models;
using System.Security.Cryptography;

namespace MobFDB.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly MobDbContext _context;

        public UserRepository(MobDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUser(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task PutUser(User user)
        {
            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task<User> PostUser(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }

}
