using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TestSystem.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTableNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AskAnswer_TestAsk_TestAskId",
                table: "AskAnswer");

            migrationBuilder.DropTable(
                name: "TestAsk");

            migrationBuilder.DropTable(
                name: "AskAnswer");

            migrationBuilder.DropTable(
                name: "AskType");

            migrationBuilder.CreateTable(
                name: "QueryType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueryType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "QueryTest",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Ask = table.Column<string>(type: "text", nullable: false),
                    TypeId = table.Column<int>(type: "integer", nullable: false),
                    TestId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueryTest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QueryTest_QueryType_TypeId",
                        column: x => x.TypeId,
                        principalTable: "QueryType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_QueryTest_Test_TestId",
                        column: x => x.TestId,
                        principalTable: "Test",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "QueryAnswer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AnswerValue = table.Column<string>(type: "text", nullable: false),
                    IsCorrect = table.Column<bool>(type: "boolean", nullable: false),
                    QueryTestId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QueryAnswer", x => x.Id);
                    table.ForeignKey(
                        name: "FK_QueryAnswer_QueryTest_QueryTestId",
                        column: x => x.QueryTestId,
                        principalTable: "QueryTest",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_QueryAnswer_QueryTestId",
                table: "QueryAnswer",
                column: "QueryTestId");

            migrationBuilder.CreateIndex(
                name: "IX_QueryTest_TestId",
                table: "QueryTest",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_QueryTest_TypeId",
                table: "QueryTest",
                column: "TypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QueryAnswer");

            migrationBuilder.DropTable(
                name: "QueryTest");

            migrationBuilder.DropTable(
                name: "QueryType");

            migrationBuilder.CreateTable(
                name: "AskType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Type = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AskType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AskAnswer",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    AnswerValue = table.Column<string>(type: "text", nullable: false),
                    TestAskId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AskAnswer", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TestAsk",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    RightAnswerId = table.Column<int>(type: "integer", nullable: false),
                    TypeId = table.Column<int>(type: "integer", nullable: false),
                    Ask = table.Column<string>(type: "text", nullable: false),
                    TestAskId = table.Column<int>(type: "integer", nullable: true),
                    TestId = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestAsk", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestAsk_AskAnswer_RightAnswerId",
                        column: x => x.RightAnswerId,
                        principalTable: "AskAnswer",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TestAsk_AskType_TypeId",
                        column: x => x.TypeId,
                        principalTable: "AskType",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TestAsk_TestAsk_TestAskId",
                        column: x => x.TestAskId,
                        principalTable: "TestAsk",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TestAsk_Test_TestId",
                        column: x => x.TestId,
                        principalTable: "Test",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_AskAnswer_TestAskId",
                table: "AskAnswer",
                column: "TestAskId");

            migrationBuilder.CreateIndex(
                name: "IX_TestAsk_RightAnswerId",
                table: "TestAsk",
                column: "RightAnswerId");

            migrationBuilder.CreateIndex(
                name: "IX_TestAsk_TestAskId",
                table: "TestAsk",
                column: "TestAskId");

            migrationBuilder.CreateIndex(
                name: "IX_TestAsk_TestId",
                table: "TestAsk",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_TestAsk_TypeId",
                table: "TestAsk",
                column: "TypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AskAnswer_TestAsk_TestAskId",
                table: "AskAnswer",
                column: "TestAskId",
                principalTable: "TestAsk",
                principalColumn: "Id");
        }
    }
}
