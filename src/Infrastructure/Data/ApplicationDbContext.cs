﻿using System.Reflection;
using Auth.Api.Application.Common.Interfaces;
using Auth.Api.Infrastructure.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Auth.Api.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>, IApplicationDbContext
{
    public ApplicationDbContext()
    {
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(builder);
    }
}
