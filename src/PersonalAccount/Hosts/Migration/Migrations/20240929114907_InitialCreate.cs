using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace InsuranceGoSmoke.PersonalAccount.Hosts.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountStatuses",
                columns: table => new
                {
                    ClientId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsVisible = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    IsAccessible = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    IsBlocked = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsPremium = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountStatuses", x => x.ClientId);
                });

            migrationBuilder.CreateTable(
                name: "Avatars",
                columns: table => new
                {
                    ClientId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ImageData = table.Column<Guid>(type: "uuid", nullable: false),
                    IsVisible = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Avatars", x => x.ClientId);
                });

            migrationBuilder.CreateTable(
                name: "PrivacySettings",
                columns: table => new
                {
                    ClientId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ShowEmail = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    ShowBirthdate = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    ShowDescription = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PrivacySettings", x => x.ClientId);
                });

            migrationBuilder.CreateTable(
                name: "ProfileDesigns",
                columns: table => new
                {
                    ClientId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ThemeColor = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    BackgroundImage = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    FontStyle = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    EnableAnimations = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProfileDesigns", x => x.ClientId);
                });

            migrationBuilder.CreateTable(
                name: "SmokingDescriptions",
                columns: table => new
                {
                    ClientId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SmokingExperienceYears = table.Column<int>(type: "integer", nullable: true),
                    ReasonStartedSmoking = table.Column<string>(type: "text", nullable: true),
                    IsSmoked = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    IsVape = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    IsDrink = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    ReadyMeeting = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    About = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmokingDescriptions", x => x.ClientId);
                });

            migrationBuilder.CreateTable(
                name: "AvatarHistories",
                columns: table => new
                {
                    ClientId = table.Column<long>(type: "bigint", nullable: false),
                    UploadDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ImageData = table.Column<Guid>(type: "uuid", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AvatarHistories", x => new { x.ClientId, x.UploadDate });
                    table.ForeignKey(
                        name: "FK_AvatarHistories_Avatars_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Avatars",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ClientGuid = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    LastName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Users_AccountStatuses_Id",
                        column: x => x.Id,
                        principalTable: "AccountStatuses",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_Avatars_Id",
                        column: x => x.Id,
                        principalTable: "Avatars",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_PrivacySettings_Id",
                        column: x => x.Id,
                        principalTable: "PrivacySettings",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_ProfileDesigns_Id",
                        column: x => x.Id,
                        principalTable: "ProfileDesigns",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Users_SmokingDescriptions_Id",
                        column: x => x.Id,
                        principalTable: "SmokingDescriptions",
                        principalColumn: "ClientId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseHistories",
                columns: table => new
                {
                    ClientId = table.Column<long>(type: "bigint", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    StatusPurchased = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseHistories", x => x.ClientId);
                    table.ForeignKey(
                        name: "FK_PurchaseHistories_Users_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AvatarHistories");

            migrationBuilder.DropTable(
                name: "PurchaseHistories");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "AccountStatuses");

            migrationBuilder.DropTable(
                name: "Avatars");

            migrationBuilder.DropTable(
                name: "PrivacySettings");

            migrationBuilder.DropTable(
                name: "ProfileDesigns");

            migrationBuilder.DropTable(
                name: "SmokingDescriptions");
        }
    }
}
