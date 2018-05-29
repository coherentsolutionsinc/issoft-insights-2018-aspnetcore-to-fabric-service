using System;

using Microsoft.EntityFrameworkCore.Migrations;

namespace JokesApi.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(
            MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Jokes",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Author = table.Column<string>(nullable: true),
                    Category = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    Language = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    PublishDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Jokes", x => x.Id);
                });
        }

        protected override void Down(
            MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Jokes");
        }
    }
}