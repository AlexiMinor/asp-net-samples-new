﻿using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using AspNetSamples.Abstractions;
using AspNetSamples.Abstractions.Services;
using AspNetSamples.Core.DTOs;
using AspNetSamples.Data.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AspNetSamples.Business
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        private readonly IRoleService _roleService;
        private readonly IMapper _mapper;

        public UserService(IUnitOfWork unitOfWork,
            IMapper mapper,
            IRoleService roleService, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _roleService = roleService;
            _configuration = configuration;
        }

        public async Task<bool> IsUserExistsAsync(string email)
        {
            var user = await _unitOfWork.Users.FindBy(user => user.Email.Equals(email)).FirstOrDefaultAsync();

            return user != null;
        }

        public async Task<bool> IsPasswordCorrectAsync(string email, string password)
        {
            if (await IsUserExistsAsync(email))
            {
                var passwordHash = GetPasswordHash(password);
                var user = await _unitOfWork.Users
                    .GetAsQueryable()
                    .AsNoTracking()
                    .FirstOrDefaultAsync(user
                        => user.Email.Equals(email)
                           && user.PasswordHash.Equals(passwordHash));

                if (user != null)
                {
                    return true;
                }

                return false;
            }
            else
            {
                throw new ArgumentException("User with that email does not exist", nameof(email));
            }
        }

        public async Task<UserDto?> RegisterAsync(string email, string password)
        {
            if (!await IsUserExistsAsync(email))
            {
                var userRoleId = await _roleService.GetRoleIdByName("User");
                if (userRoleId == 0)
                {
                    //throw new DataException("Role User does not exist");
                    await _roleService.InitiateDefaultRolesAsync();
                }

                var user = new User()
                {
                    Email = email,
                    PasswordHash = GetPasswordHash(password),
                    RoleId = userRoleId
                };

                var userEntry = await _unitOfWork.Users.AddAsync(user);
                await _unitOfWork.SaveChangesAsync();

                return _mapper.Map<UserDto>(await _unitOfWork.Users.GetByIdAsync(userEntry.Entity.Id));
            }

            return null;
        }

        public async Task<UserDto?> GetUserByEmailAsync(string modelEmail)
        {
            var user = await _unitOfWork.Users
                .FindBy(user => user.Email.Equals(modelEmail))
                .FirstOrDefaultAsync();
            return
                user != null 
                    ? _mapper.Map<UserDto>(user) 
                    : null;
        }

        public async Task<List<UserDto>> GetUsersAsync()
        {
            return await _unitOfWork.Users.GetAsQueryable().
                Select(user => _mapper.Map<UserDto>(user)).ToListAsync();
        }

        public async Task<List<Claim>> GetUserClamsAsync(UserDto user)
        {
            var role = await _roleService.GetUserRole(user.Id);

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
            };
            if (string.IsNullOrEmpty(role))
            {
                throw new ArgumentException("Incorrect user or role", nameof(user));
            }
            claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, role));


            return claims;
        }


        private string GetPasswordHash(string password)
        {
            var sb = new StringBuilder();

            using (var hash = SHA256.Create())
            {
                var encoding = Encoding.UTF8;
                var result = hash
                    .ComputeHash(
                        encoding
                            .GetBytes($"{password}{_configuration["Secrets:Salt"]}"));

                foreach (var b in result)
                {
                    sb.Append(b.ToString("x2"));
                }
            }
            return sb.ToString();
        }
    }
}