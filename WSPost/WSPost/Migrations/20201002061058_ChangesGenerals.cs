using Microsoft.EntityFrameworkCore.Migrations;

namespace WSPost.Migrations
{
    public partial class ChangesGenerals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
               name: "Post",
               columns: table => new
               {
                   Id = table.Column<int>(nullable: false),
                   UserName = table.Column<string>(nullable: false),
                   Password = table.Column<string>(nullable: false)

               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_Posts", x => x.Id);
               });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
               name: "Posts");
        }
    }
}
