using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PogodaArchive.Migrations
{
    /// <inheritdoc />
    public partial class initdata : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PogodaModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Time = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Temp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Humidity = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DewPoint = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AtmPressure = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WindDirection = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WindSpeed = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CloudCover = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LowerCloudLimit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HorVisibility = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WeatherEvents = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PogodaModel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "YearMonthModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<int>(type: "int", nullable: false),
                    Month = table.Column<int>(type: "int", nullable: false),
                    Discription = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YearMonthModel", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PogodaModel");

            migrationBuilder.DropTable(
                name: "YearMonthModel");
        }
    }
}
