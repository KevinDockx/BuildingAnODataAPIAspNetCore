using AirVinyl.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace AirVinyl.API.DbContexts
{
    public class AirVinylDbContext : DbContext
    {
        public DbSet<Person> People { get; set; }
        public DbSet<VinylRecord> VinylRecords { get; set; }
        public DbSet<RecordStore> RecordStores { get; set; }
        public DbSet<PressingDetail> PressingDetails { get; set; }

        public AirVinylDbContext(DbContextOptions<AirVinylDbContext> options)
           : base(options)
        { 
        }
         
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PressingDetail>().HasData(
                new PressingDetail()
                {
                    PressingDetailId = 1,
                    Description = "Audiophile LP",
                    Grams = 180,
                    Inches = 12
                },
                new PressingDetail()
                {
                    PressingDetailId = 2,
                    Description = "Regular LP",
                    Grams = 140,
                    Inches = 12
                },
                new PressingDetail()
                {
                    PressingDetailId = 3,
                    Description = "Audiophile Single",
                    Grams = 50,
                    Inches = 7
                },
                new PressingDetail()
                {
                    PressingDetailId = 4,
                    Description = "Regular Single",
                    Grams = 40,
                    Inches = 7
                });

            modelBuilder.Entity<Person>().Property(p => p.AmountOfCashToSpend).HasColumnType("decimal(8,2)");

            modelBuilder.Entity<Person>().HasData(
                new Person()
                {
                    PersonId = 1,
                    DateOfBirth = new DateTimeOffset(new DateTime(1981, 5, 5)),
                    Email = "kevin@kevindockx.com",
                    FirstName = "Kevin",
                    LastName = "Dockx",
                    Gender = Gender.Male,
                    NumberOfRecordsOnWishList = 10,
                    AmountOfCashToSpend = 300
                },
                new Person()
                {
                    PersonId = 2,
                    DateOfBirth = new DateTimeOffset(new DateTime(1986, 3, 6)),
                    Email = "sven@someemailprovider.com",
                    FirstName = "Sven",
                    LastName = "Vercauteren",
                    Gender = Gender.Male,
                    NumberOfRecordsOnWishList = 34,
                    AmountOfCashToSpend = 2000
                },
                new Person()
                {
                    PersonId = 3,
                    DateOfBirth = new DateTimeOffset(new DateTime(1977, 12, 27)),
                    Email = "nele@someemailprovider.com",
                    FirstName = "Nele",
                    LastName = "Verheyen",
                    Gender = Gender.Female,
                    NumberOfRecordsOnWishList = 120,
                    AmountOfCashToSpend = 100
                },
                new Person()
                {
                    PersonId = 4,
                    DateOfBirth = new DateTimeOffset(new DateTime(1983, 5, 18)),
                    Email = "nils@someemailprovider.com",
                    FirstName = "Nils",
                    LastName = "Missorten",
                    Gender = Gender.Male,
                    NumberOfRecordsOnWishList = 23,
                    AmountOfCashToSpend = 2500 
                },
                new Person()
                {
                    PersonId = 5,
                    DateOfBirth = new DateTimeOffset(new DateTime(1981, 10, 15)),
                    Email = "tim@someemailprovider.com",
                    FirstName = "Tim",
                    LastName = "Van den Broeck",
                    Gender = Gender.Male,
                    NumberOfRecordsOnWishList = 19,
                    AmountOfCashToSpend = 90
                },                
                new Person()
                {
                    PersonId = 6,
                    DateOfBirth = new DateTimeOffset(new DateTime(1981, 1, 16)),
                    Email = null,
                    FirstName = "Kenneth",
                    LastName = "Mills",
                    Gender = Gender.Male,
                    NumberOfRecordsOnWishList = 98,
                    AmountOfCashToSpend = 200 
                }
            );

            modelBuilder.Entity<VinylRecord>().HasData(
                new VinylRecord()
                {
                    VinylRecordId = 1,
                    PersonId = 1,
                    Artist = "Nirvana",
                    Title = "Nevermind",
                    CatalogNumber = "ABC/111",
                    PressingDetailId = 1,
                    Year = 1991
                },
                new VinylRecord()
                {
                    VinylRecordId = 2,
                    PersonId = 1,
                    Artist = "Arctic Monkeys",
                    Title = "AM",
                    CatalogNumber = "EUI/111",
                    PressingDetailId =  2,
                    Year = 2013
                },
                new VinylRecord()
                {
                    VinylRecordId = 3,
                    PersonId = 1,
                    Artist = "Beatles",
                    Title = "The White Album",
                    CatalogNumber = "DEI/113",
                    PressingDetailId = 2,
                    Year = 1968
                },
                new VinylRecord()
                {
                    VinylRecordId = 4,
                    PersonId = 1,
                    Artist = "Beatles",
                    Title = "Sergeant Pepper's Lonely Hearts Club Band",
                    CatalogNumber = "DPI/123",
                    PressingDetailId = 2,
                    Year = 1967
                },
                new VinylRecord()
                {
                    VinylRecordId = 5,
                    PersonId = 1,
                    Artist = "Nirvana",
                    Title = "Bleach",
                    CatalogNumber = "DPI/123",
                    PressingDetailId = 1,
                    Year = 1989
                },
                new VinylRecord()
                {
                    VinylRecordId = 6,
                    PersonId = 1,
                    Artist = "Leonard Cohen",
                    Title = "Suzanne",
                    CatalogNumber = "PPP/783",
                    PressingDetailId = 3,
                    Year = 1967
                },
                new VinylRecord()
                {
                    VinylRecordId = 7,
                    PersonId = 1,
                    Artist = "Marvin Gaye",
                    Title = "What's Going On",
                    CatalogNumber = "MVG/445",
                    PressingDetailId = 1,
                    Year = null
                },
                new VinylRecord()
                {
                    VinylRecordId = 8,
                    PersonId = 2,
                    Artist = "Nirvana",
                    Title = "Nevermind",
                    CatalogNumber = "ABC/111",
                    PressingDetailId = 1,
                    Year = 1991
                },
                new VinylRecord()
                {
                    VinylRecordId = 9,
                    PersonId = 2,
                    Artist = "Cher",
                    Title = "Closer to the Truth",
                    CatalogNumber = "CHE/190",
                    PressingDetailId = 2,
                    Year = 2013
                },
                new VinylRecord()
                {
                    VinylRecordId = 10,
                    PersonId = 3,
                    Artist = "The Dandy Warhols",
                    Title = "Thirteen Tales From Urban Bohemia",
                    CatalogNumber = "TDW/516",
                    PressingDetailId = 2
                },
                new VinylRecord()
                {
                    VinylRecordId = 11,
                    PersonId = 4,
                    Artist = "Justin Bieber",
                    Title = "Baby",
                    CatalogNumber = "OOP/098",
                    PressingDetailId = 3
                },
                new VinylRecord()
                {
                    VinylRecordId = 12,
                    PersonId = 4,
                    Artist = "The Prodigy",
                    Title = "Music for the Jilted Generation",
                    CatalogNumber = "NBE/864",
                    PressingDetailId = 2
                },
                new VinylRecord()
                {
                    VinylRecordId = 13,
                    PersonId = 5,
                    Artist = "Anne Clarke",
                    Title = "Our Darkness",
                    CatalogNumber = "TII/339",
                    PressingDetailId = 3
                },
                new VinylRecord()
                {
                    VinylRecordId = 14,
                    PersonId = 5,
                    Artist = "Dead Kennedys",
                    Title = "Give Me Convenience or Give Me Death",
                    CatalogNumber = "DKE/864",
                    PressingDetailId = 2
                },
                new VinylRecord()
                {
                    VinylRecordId = 15,
                    PersonId = 5,
                    Artist = "Sisters of Mercy",
                    Title = "Temple of Love",
                    CatalogNumber = "IIE/824",
                    PressingDetailId = 4
                },
                new VinylRecord()
                {
                    VinylRecordId = 16,
                    PersonId = 6,
                    Artist = "Abba",
                    Title = "Gimme Gimme Gimme",
                    CatalogNumber = "TDW/516",
                    PressingDetailId = 4
                });

            // comparer & converter for storing a list of strings
            var stringListValueComparer = new ValueComparer<List<string>>(
                  (v1, v2) => v1.SequenceEqual(v2),
                  v => v.Aggregate(0, (a, f) => HashCode.Combine(a, f.GetHashCode())),
                  v => v.ToList());

            modelBuilder.Entity<RecordStore>()
                .Property(r => r.Tags)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, null),
                    v => JsonSerializer.Deserialize<List<string>>(v, null));

            modelBuilder.Entity<RecordStore>()
               .Property(r => r.Tags)
               .Metadata
               .SetValueComparer(stringListValueComparer);

            // address is an owned type (= type without id)
            modelBuilder.Entity<RecordStore>().OwnsOne(p => p.StoreAddress);

            modelBuilder.Entity<SpecializedRecordStore>(c =>
            {
                c.HasData(new SpecializedRecordStore()
                {
                    RecordStoreId = 2,
                    Name = "Indie Records, Inc",
                    Tags = new List<string>() { "Rock", "Indie", "Alternative" },
                    Specialization = "Indie"
                },
                 new SpecializedRecordStore()
                 {
                     RecordStoreId = 3,
                     Name = "Rock Records, Inc",
                     Tags = new List<string>() { "Rock", "Pop" },
                     Specialization = "Rock"
                 });

                c.OwnsOne(r => r.StoreAddress).HasData(                  
                  new Address()
                  {
                      RecordStoreId = 2,
                      City = "Antwerp",
                      PostalCode = "2000",
                      Street = "1, Main Street",
                      Country = "Belgium"
                  },
                  new Address()
                  {
                      RecordStoreId = 3,
                      City = "Antwerp",
                      PostalCode = "2000",
                      Street = "5, Big Street",
                      Country = "Belgium"
                  }
              );
            });
             
            modelBuilder.Entity<RecordStore>(c =>
            {
                c.HasData(new RecordStore()
                {
                    RecordStoreId = 1,
                    Name = "All Your Music Needs",
                    Tags = new List<string>() { "Rock", "Pop", "Indie", "Alternative" }
                });

                c.OwnsOne(r => r.StoreAddress).HasData(
                   new Address()
                   {
                       RecordStoreId = 1,
                       City = "Antwerp",
                       PostalCode = "2000",
                       Street = "25, Fluffy Road",
                       Country = "Belgium"
                   } 
              );
            });              

            modelBuilder.Entity<Rating>().HasData(
                new Rating()
                {    
                    RatingId = 1,
                    RecordStoreId = 1, 
                    RatedByPersonId = 1,
                    Value = 4
                },
                new Rating()
                {
                    RatingId = 2,
                    RecordStoreId = 1,
                    RatedByPersonId = 2,
                    Value = 4
                },
                new Rating()
                {
                    RatingId = 3,
                    RecordStoreId = 1,
                    RatedByPersonId = 3,
                    Value = 4
                },
                new Rating()
                {
                    RatingId = 4,
                    RecordStoreId = 2,
                    RatedByPersonId = 1,
                    Value = 5
                },
                new Rating()
                {
                    RatingId = 5,
                    RecordStoreId = 2,
                    RatedByPersonId = 2,
                    Value = 4
                },
                new Rating()
                {
                    RatingId = 6,
                    RecordStoreId = 3,
                    RatedByPersonId = 3,
                    Value = 5
                },
                new Rating()
                {
                    RatingId = 7,
                    RecordStoreId = 3,
                    RatedByPersonId = 2,
                    Value = 4
                }
            );
        }    
    }
}
