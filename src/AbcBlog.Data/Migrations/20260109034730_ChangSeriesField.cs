using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AbcBlog.Core.Migrations
{
    /// <inheritdoc />
    public partial class ChangSeriesField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "name",
                table: "Series",
                newName: "Name");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Series",
                newName: "name");
        }
    }
}
