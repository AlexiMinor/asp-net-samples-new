﻿using AspNetSamples.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AspNetSamples.Data
{
    public class NewsAggregatorContext : DbContext
    {
        public DbSet<Article> Articles { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<Role> Roles { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<User> Users { get; set; }

        public NewsAggregatorContext(DbContextOptions<NewsAggregatorContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}