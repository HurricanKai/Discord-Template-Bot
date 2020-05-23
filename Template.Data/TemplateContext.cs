﻿using Microsoft.EntityFrameworkCore;

namespace Template.Data
{
    public class TemplateContext : DbContext 
    {
        public TemplateContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(TemplateContext).Assembly);
        }
    }
}