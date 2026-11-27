using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Bison.Core.Migrations
{
    /// <inheritdoc />
    public partial class AddUpvotes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ObservationUpvotes",
                columns: table => new
                {
                    ObservationPostId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpVotersId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ObservationUpvotes", x => new { x.ObservationPostId, x.UpVotersId });
                    table.ForeignKey(
                        name: "FK_ObservationUpvotes_AspNetUsers_UpVotersId",
                        column: x => x.UpVotersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ObservationUpvotes_Posts_ObservationPostId",
                        column: x => x.ObservationPostId,
                        principalTable: "Posts",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProposalUpvotes",
                columns: table => new
                {
                    ProposalPostId = table.Column<int>(type: "INTEGER", nullable: false),
                    UpVotersId = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProposalUpvotes", x => new { x.ProposalPostId, x.UpVotersId });
                    table.ForeignKey(
                        name: "FK_ProposalUpvotes_AspNetUsers_UpVotersId",
                        column: x => x.UpVotersId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProposalUpvotes_Posts_ProposalPostId",
                        column: x => x.ProposalPostId,
                        principalTable: "Posts",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ObservationUpvotes_UpVotersId",
                table: "ObservationUpvotes",
                column: "UpVotersId");

            migrationBuilder.CreateIndex(
                name: "IX_ProposalUpvotes_UpVotersId",
                table: "ProposalUpvotes",
                column: "UpVotersId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ObservationUpvotes");

            migrationBuilder.DropTable(
                name: "ProposalUpvotes");
        }
    }
}
