using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CreekRiverProject.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CampsiteTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CampsiteTypeName = table.Column<string>(type: "text", nullable: false),
                    MaxReservationDays = table.Column<int>(type: "integer", nullable: false),
                    FeePerNight = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CampsiteTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "text", nullable: false),
                    LastName = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Campsites",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Nickname = table.Column<string>(type: "text", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: false),
                    CampsiteTypeId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Campsites", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Campsites_CampsiteTypes_CampsiteTypeId",
                        column: x => x.CampsiteTypeId,
                        principalTable: "CampsiteTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CampsiteId = table.Column<int>(type: "integer", nullable: false),
                    UserProfileId = table.Column<int>(type: "integer", nullable: false),
                    CheckinDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CheckoutDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservations_Campsites_CampsiteId",
                        column: x => x.CampsiteId,
                        principalTable: "Campsites",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reservations_UserProfiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CampsiteTypes",
                columns: new[] { "Id", "CampsiteTypeName", "FeePerNight", "MaxReservationDays" },
                values: new object[,]
                {
                    { 1, "Tent", 15.99m, 7 },
                    { 2, "RV", 26.50m, 14 },
                    { 3, "Primitive", 10.00m, 3 },
                    { 4, "Hammock", 12m, 7 }
                });

            migrationBuilder.InsertData(
                table: "Campsites",
                columns: new[] { "Id", "CampsiteTypeId", "ImageUrl", "Nickname" },
                values: new object[,]
                {
                    { 1, 1, "https://tnstateparks.com/assets/images/content-images/campgrounds/249/colsp-area2-site73.jpg", "Barred Owl" },
                    { 2, 2, "https://www.istockphoto.com/photo/camping-in-the-wild-gm2190552397-608924047?searchscope=image%2Cfilm", "Couple Camps" },
                    { 3, 3, "https://www.istockphoto.com/photo/happy-black-family-roasting-marshmallows-while-camping-in-the-woods-gm1773349643-546062640?searchscope=image%2Cfilm", "Family Camps" },
                    { 4, 4, "https://media.istockphoto.com/id/2185613260/photo/portrait-of-a-child-boys-embracing-in-a-summer-camp.jpg?s=612x612&w=0&k=20&c=rznIJ8-fUOWVvP4L_e6CUrJeHU5bPPOTt1L57P8TPu0=", "Kid Camps" },
                    { 5, 1, "https://media.istockphoto.com/id/1943831013/photo/diverse-group-of-friends-enjoying-a-campfire-outdoors.jpg?s=612x612&w=0&k=20&c=I9Lj2arcBL3xdykbJ1RYsv6Qgv4yVkSALAhLxrxFwqQ=", "Group Camps" },
                    { 6, 2, "https://www.istockphoto.com/video/a-silhouette-of-two-hikers-and-their-dog-around-a-camp-fire-gm1925122589-555625349?searchscope=image%2Cfilm", "Couple Camps" },
                    { 7, 3, "https://media.istockphoto.com/id/2161607196/photo/multigenerational-mixed-race-family-enjoying-a-campfire-outdoors-at-dusk.jpg?s=612x612&w=0&k=20&c=1vsNgnVQbNMaIY-Wp91CgpqQL-Gsoh_WTBuPnxwouNU=", "Family Camps" },
                    { 8, 4, "https://via.placeholder.com/150", "Kid Camps" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Campsites_CampsiteTypeId",
                table: "Campsites",
                column: "CampsiteTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_CampsiteId",
                table: "Reservations",
                column: "CampsiteId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_UserProfileId",
                table: "Reservations",
                column: "UserProfileId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Campsites");

            migrationBuilder.DropTable(
                name: "UserProfiles");

            migrationBuilder.DropTable(
                name: "CampsiteTypes");
        }
    }
}
