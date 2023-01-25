using System.Reflection.Emit;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using RekrutacjaApp.Entities;

namespace RekrutacjaApp.Data
{
    public class DbInitializer
    {
        private readonly ModelBuilder modelBuilder;
        public DbInitializer(ModelBuilder modelBuilder)
        {
            this.modelBuilder = modelBuilder;
        }
        public void Seed()
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = Guid.NewGuid(),
                    Name = "Dawid",
                    Surname = "Mroczkowski",
                    BirthDate = new DateTime(1996, 12, 10),
                    Gender = Gender.Male,

                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Name = "Adam",
                    Surname = "Nowak",
                    BirthDate = new DateTime(1997, 01, 14),
                    Gender = Gender.Male,
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Name = "Jan",
                    Surname = "Kowalski",
                    BirthDate = new DateTime(1986, 7, 21),
                    Gender = Gender.Male,
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Name = "Karolina",
                    Surname = "Szpak",
                    BirthDate = new DateTime(1979, 3, 13),
                    Gender = Gender.Famale,
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Name = "Wiktoria",
                    Surname = "Kowalska",
                    BirthDate = new DateTime(1944, 12, 21),
                    Gender = Gender.Famale,
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Name = "Zbigniew",
                    Surname = "Stawik",
                    BirthDate = new DateTime(1990, 4, 13),
                    Gender = Gender.Male,
                }
             );
        }
    }
}
