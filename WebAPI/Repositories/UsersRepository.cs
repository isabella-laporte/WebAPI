﻿using WebAPI.Context;
using WebAPI.Interfaces;
using WebAPI.Models;

namespace WebAPI.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly MemoryContext _context;

        public UsersRepository(MemoryContext context)
        {
            _context = context;
        }

        public Task<List<Users>> Get(int page, int maxResults)
        {
            return Task.Run(() =>
            {
                var users = _context.Users.Skip((page - 1) * maxResults).Take(maxResults).ToList();
                return users.Any() ? users : new List<Users>();
            });
        }

        public Task<Users?> Get(string username, string password)
        {
            return Task.Run(() =>
            {
                var user = _context.Users.FirstOrDefault(item => item.Username.Equals(username) && item.Password.Equals(password));
                return user;
            });
        }

        public Task<Users> Insert(Users user)
        {
            return Task.Run(() =>
            {
                _context.Add(user);
                _context.SaveChanges();
                return user;
            });
        }
    }
}
