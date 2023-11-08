using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace TestSystem.Migrations
{
    /// <inheritdoc />
    public partial class addTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                name: "Test",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatorUserId = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Test", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Test_User_CreatorUserId",
                        column: x => x.CreatorUserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TestUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TestId = table.Column<int>(type: "integer", nullable: false),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    RightAnswersCount = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TestUser_Test_TestId",
                        column: x => x.TestId,
                        principalTable: "Test",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TestUser_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
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
                    Ask = table.Column<string>(type: "text", nullable: false),
                    TypeId = table.Column<int>(type: "integer", nullable: false),
                    RightAnswerId = table.Column<int>(type: "integer", nullable: false),
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
                name: "IX_Test_CreatorUserId",
                table: "Test",
                column: "CreatorUserId");

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

            migrationBuilder.CreateIndex(
                name: "IX_TestUser_TestId",
                table: "TestUser",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_TestUser_UserId",
                table: "TestUser",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AskAnswer_TestAsk_TestAskId",
                table: "AskAnswer",
                column: "TestAskId",
                principalTable: "TestAsk",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AskAnswer_TestAsk_TestAskId",
                table: "AskAnswer");

            migrationBuilder.DropTable(
                name: "TestUser");

            migrationBuilder.DropTable(
                name: "TestAsk");

            migrationBuilder.DropTable(
                name: "AskAnswer");

            migrationBuilder.DropTable(
                name: "AskType");

            migrationBuilder.DropTable(
                name: "Test");
        }
    }
}
