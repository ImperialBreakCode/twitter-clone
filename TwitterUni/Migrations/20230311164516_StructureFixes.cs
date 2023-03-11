using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TwitterUni.Migrations
{
    /// <inheritdoc />
    public partial class StructureFixes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Comments_CommentsId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_CommentsId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CommentsId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "CommentsId",
                table: "Comments",
                newName: "CommentId");

            migrationBuilder.CreateTable(
                name: "CommentUser",
                columns: table => new
                {
                    LikedCommentsCommentId = table.Column<int>(type: "int", nullable: false),
                    LikesId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentUser", x => new { x.LikedCommentsCommentId, x.LikesId });
                    table.ForeignKey(
                        name: "FK_CommentUser_AspNetUsers_LikesId",
                        column: x => x.LikesId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CommentUser_Comments_LikedCommentsCommentId",
                        column: x => x.LikedCommentsCommentId,
                        principalTable: "Comments",
                        principalColumn: "CommentId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommentUser_LikesId",
                table: "CommentUser",
                column: "LikesId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentUser");

            migrationBuilder.RenameColumn(
                name: "CommentId",
                table: "Comments",
                newName: "CommentsId");

            migrationBuilder.AddColumn<int>(
                name: "CommentsId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_CommentsId",
                table: "AspNetUsers",
                column: "CommentsId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Comments_CommentsId",
                table: "AspNetUsers",
                column: "CommentsId",
                principalTable: "Comments",
                principalColumn: "CommentsId");
        }
    }
}
