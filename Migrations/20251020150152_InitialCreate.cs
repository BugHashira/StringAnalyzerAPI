using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StringAnalyzerAPI.Migrations;

/// <inheritdoc />
public partial class InitialCreate : Migration
{
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "AnalyzedStrings",
            columns: table => new
            {
                Id = table.Column<string>(type: "text", nullable: false),
                Value = table.Column<string>(type: "text", nullable: false),
                Length = table.Column<int>(type: "integer", nullable: false),
                IsPalindrome = table.Column<bool>(type: "boolean", nullable: false),
                UniqueCharacters = table.Column<int>(type: "integer", nullable: false),
                WordCount = table.Column<int>(type: "integer", nullable: false),
                Sha256Hash = table.Column<string>(type: "text", nullable: false),
                CharacterFrequencyMapJson = table.Column<string>(type: "text", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_AnalyzedStrings", x => x.Id);
            });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "AnalyzedStrings");
    }
}
