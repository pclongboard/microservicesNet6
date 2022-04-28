﻿using GeekShopping.IdentityServer.Configuration;
using GeekShopping.IdentityServer.Data;
using GeekShopping.IdentityServer.Data.Context;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace GeekShopping.IdentityServer.Initializer
{
    public class DbInitializer : IDbinitializer
    {
        private readonly MySQLContext _context;
        private readonly UserManager<ApplicationUser> _user;
        private readonly RoleManager<IdentityRole> _role;

        public DbInitializer(MySQLContext context, 
                             UserManager<ApplicationUser> user, 
                             RoleManager<IdentityRole> role)
        {
            _context = context;
            _user = user;  
            _role = role;
        }

        public void Initialize()
        {
            if (_role.FindByNameAsync(IdentityConfiguration.Admin).Result != null)
                return;

            _role.CreateAsync(new IdentityRole(IdentityConfiguration.Admin)).GetAwaiter().GetResult();
            _role.CreateAsync(new IdentityRole(IdentityConfiguration.Client)).GetAwaiter().GetResult();

            var admin = new ApplicationUser()
            {
                UserName = "PedroAdmin",
                Email = "pclongboard@hotmail.com",
                EmailConfirmed = true,
                PhoneNumber = "+55 (11) 97660-6336",
                FirstName = "Pedro",
                LastName = "Cabreira"
            };

            _user.CreateAsync(admin, "Admin@123").GetAwaiter().GetResult();
            _user.AddToRoleAsync(admin, IdentityConfiguration.Admin).GetAwaiter().GetResult();

            var adminClaims = _user.AddClaimsAsync(admin, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, $"{admin.FirstName} {admin.LastName}"),
                new Claim(JwtClaimTypes.GivenName, admin.FirstName),
                new Claim(JwtClaimTypes.FamilyName, admin.LastName),
                new Claim(JwtClaimTypes.Role, IdentityConfiguration.Admin)
            }).Result;

            var client = new ApplicationUser()
            {
                UserName = "PedroClient",
                Email = "pclongboard@hotmail.com",
                EmailConfirmed = true,
                PhoneNumber = "+55 (11) 97660-6336",
                FirstName = "Pedro",
                LastName = "Cabreira"
            };

            _user.CreateAsync(client, "Admin@123").GetAwaiter().GetResult();
            _user.AddToRoleAsync(client, IdentityConfiguration.Client).GetAwaiter().GetResult();

            var clientClaims = _user.AddClaimsAsync(client, new Claim[]
            {
                new Claim(JwtClaimTypes.Name, $"{client.FirstName} {client.LastName}"),
                new Claim(JwtClaimTypes.GivenName, client.FirstName),
                new Claim(JwtClaimTypes.FamilyName, client.LastName),
                new Claim(JwtClaimTypes.Role, IdentityConfiguration.Client)
            }).Result;


        }
    }
}
