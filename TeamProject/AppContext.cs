using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TeamProject.Models;

namespace TeamProject
{
    public class AppContext : DbContext
    {
        public AppContext() : base("name=AppContext")
        {

        }

        public DbSet<User> Users { get; set; }
        public DbSet<Problem> Problems { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Spot> Spots { get; set; }

        //Fluent API
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Card>()
            //    .HasRequired(i => i.User)
            //    .WithRequiredPrincipal(i => i.Card);

            modelBuilder.Entity<User>()
                .ToTable("Users")
                .HasKey(i => i.UserId);

            //modelBuilder.Entity<User>()
            //    .HasRequired(i => i.Card)
            //    .WithRequiredPrincipal(i => i.User);

            modelBuilder.Entity<Message>()
                .HasRequired(i => i.Receiver).WithMany().Map(i => i.MapKey("ReceiverId"));

            modelBuilder.Entity<Message>()
                .HasRequired(i => i.Sender).WithMany().Map(i => i.MapKey("SenderId"));
        }
    }
}