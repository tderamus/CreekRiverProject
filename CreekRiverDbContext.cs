using Microsoft.EntityFrameworkCore;
using CreekRiver.Models;

public class CreekRiverDbContext : DbContext
{

    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<Campsite> Campsites { get; set; }
    public DbSet<CampsiteType> CampsiteTypes { get; set; }

    public CreekRiverDbContext(DbContextOptions<CreekRiverDbContext> context) : base(context)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    
{
    // seed data with campsite types
    modelBuilder.Entity<CampsiteType>().HasData(new CampsiteType[]
    {
        new CampsiteType {Id = 1, CampsiteTypeName = "Tent", FeePerNight = 15.99M, MaxReservationDays = 7},
        new CampsiteType {Id = 2, CampsiteTypeName = "RV", FeePerNight = 26.50M, MaxReservationDays = 14},
        new CampsiteType {Id = 3, CampsiteTypeName = "Primitive", FeePerNight = 10.00M, MaxReservationDays = 3},
        new CampsiteType {Id = 4, CampsiteTypeName = "Hammock", FeePerNight = 12M, MaxReservationDays = 7}
    });

        // seed data with campsites
        modelBuilder.Entity<Campsite>().HasData(new Campsite[]
        {
            new Campsite {Id = 1, CampsiteTypeId = 1, Nickname = "Barred Owl", ImageUrl="https://tnstateparks.com/assets/images/content-images/campgrounds/249/colsp-area2-site73.jpg"},
            new Campsite {Id = 2, CampsiteTypeId = 2, Nickname = "Couple Camps",  ImageUrl = "https://www.istockphoto.com/photo/camping-in-the-wild-gm2190552397-608924047?searchscope=image%2Cfilm"},
            new Campsite {Id = 3, CampsiteTypeId = 3, Nickname = "Family Camps",  ImageUrl = "https://www.istockphoto.com/photo/happy-black-family-roasting-marshmallows-while-camping-in-the-woods-gm1773349643-546062640?searchscope=image%2Cfilm"},
            new Campsite {Id = 4, CampsiteTypeId = 4, Nickname = "Kid Camps",  ImageUrl = "https://media.istockphoto.com/id/2185613260/photo/portrait-of-a-child-boys-embracing-in-a-summer-camp.jpg?s=612x612&w=0&k=20&c=rznIJ8-fUOWVvP4L_e6CUrJeHU5bPPOTt1L57P8TPu0="},
            new Campsite {Id = 5, CampsiteTypeId = 1, Nickname = "Group Camps",  ImageUrl = "https://media.istockphoto.com/id/1943831013/photo/diverse-group-of-friends-enjoying-a-campfire-outdoors.jpg?s=612x612&w=0&k=20&c=I9Lj2arcBL3xdykbJ1RYsv6Qgv4yVkSALAhLxrxFwqQ="},
            new Campsite {Id = 6, CampsiteTypeId = 2, Nickname = "Couple Camps",  ImageUrl = "https://www.istockphoto.com/video/a-silhouette-of-two-hikers-and-their-dog-around-a-camp-fire-gm1925122589-555625349?searchscope=image%2Cfilm"},
            new Campsite {Id = 7, CampsiteTypeId = 3, Nickname = "Family Camps",  ImageUrl = "https://media.istockphoto.com/id/2161607196/photo/multigenerational-mixed-race-family-enjoying-a-campfire-outdoors-at-dusk.jpg?s=612x612&w=0&k=20&c=1vsNgnVQbNMaIY-Wp91CgpqQL-Gsoh_WTBuPnxwouNU="},
            new Campsite {Id = 8, CampsiteTypeId = 4, Nickname = "Kid Camps",  ImageUrl = "https://via.placeholder.com/150"},
        });
    }
}
