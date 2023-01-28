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
                    UserId = 1,
                    Name = "Dawid",
                    Surname = "Mroczkowski",
                    BirthDate = new DateTime(1996, 12, 10),
                    Gender = Gender.Mężczyzna,
                    CarLicense = true

                },
                new User
                {
                    UserId = 2,
                    Name = "Adam",
                    Surname = "Nowak",
                    BirthDate = new DateTime(1997, 01, 14),
                    Gender = Gender.Mężczyzna,
                    CarLicense = true
                },
                new User
                {
                    UserId = 3,
                    Name = "Jan",
                    Surname = "Kowalski",
                    BirthDate = new DateTime(1986, 7, 21),
                    Gender = Gender.Mężczyzna,
                    CarLicense = false
                },
                new User
                {
                    UserId = 4,
                    Name = "Karolina",
                    Surname = "Szpak",
                    BirthDate = new DateTime(1979, 3, 13),
                    Gender = Gender.Kobieta,
                    CarLicense = false
                },
                new User
                {
                    UserId = 5,
                    Name = "Wiktoria",
                    Surname = "Kowalska",
                    BirthDate = new DateTime(1944, 12, 21),
                    Gender = Gender.Kobieta,
                    CarLicense = true
                },
                new User
                {
                    UserId = 6,
                    Name = "Zbigniew",
                    Surname = "Stawik",
                    BirthDate = new DateTime(1990, 4, 13),
                    Gender = Gender.Mężczyzna,
                    CarLicense = true
                }
             ); ;
            modelBuilder.Entity<CustomAttribute>().HasData(
                new CustomAttribute
                {
                    CustomAttributeId = 1,
                    UserId = 1,
                    Name = "Numer buta",
                    Value = "43",
                    
                },
                new CustomAttribute
                {
                    CustomAttributeId = 2,
                    UserId = 2,
                    Name = "Kolor włosów",
                    Value = "Czarne",
                },
                new CustomAttribute
                {
                    CustomAttributeId = 3,
                    UserId = 2,
                    Name = "Kolor kurtki",
                    Value = "Niebieski",
                }
             );
        }
    }
}
