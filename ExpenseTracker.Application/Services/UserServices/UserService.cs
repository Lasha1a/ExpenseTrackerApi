using ExpenseTracker.Application.DTOs.UserDtos;
using ExpenseTracker.Application.Interfaces.Repositories;
using ExpenseTracker.Application.Interfaces.Security;
using ExpenseTracker.Application.Specifications;
using ExpenseTracker.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseTracker.Application.Services.UserServices;

public class UserService
{
    private readonly IRepository<User> _repository;
    private readonly IPasswordHasher _passwordHasher;

    public UserService(IRepository<User> repository, IPasswordHasher passwordHasher)
    {
        _repository = repository;
        _passwordHasher = passwordHasher;
    }

    //add new user
    public async Task<User> CreateAsync(CreateUserRequest request)
    {
        var user = new User
        {
            Email = request.Email,
            FullName = request.FullName,
            CurrencyCode = request.CurrencyCode,
            PasswordHash = _passwordHasher.Hash(request.Password),
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(user);
        return user;
    }

    //read
    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }

    //login
    public async Task<User?> LoginAsync(LoginRequest request)
    {
        var spec = new UserByEmailSpec(request.Email);

        var users = await _repository.ListAsync(spec);

        var user = users.FirstOrDefault();
        if (user is null) 
            return null;

        var isValid = _passwordHasher.Verify(request.Password, user.PasswordHash);

        return isValid ? user : null;
    }
}
