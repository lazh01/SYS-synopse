using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataProcessing.Processing.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAverageToSensorAggregate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Average",
                table: "SensorAggregates",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Average",
                table: "SensorAggregates");
        }
    }
}
