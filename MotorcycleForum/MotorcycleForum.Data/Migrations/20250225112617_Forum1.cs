using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MotorcycleForum.Data.Migrations
{
    /// <inheritdoc />
    public partial class Forum1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForumVotes_AspNetUsers_UserId",
                table: "ForumVotes");

            migrationBuilder.DropForeignKey(
                name: "FK_ForumVotes_Comments_CommentId",
                table: "ForumVotes");

            migrationBuilder.DropForeignKey(
                name: "FK_ForumVotes_ForumPosts_PostId",
                table: "ForumVotes");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "ForumVotes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "PostId",
                table: "ForumVotes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "CommentId",
                table: "ForumVotes",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ForumVotes_AspNetUsers_UserId",
                table: "ForumVotes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ForumVotes_Comments_CommentId",
                table: "ForumVotes",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "CommentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ForumVotes_ForumPosts_PostId",
                table: "ForumVotes",
                column: "PostId",
                principalTable: "ForumPosts",
                principalColumn: "ForumPostId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ForumVotes_AspNetUsers_UserId",
                table: "ForumVotes");

            migrationBuilder.DropForeignKey(
                name: "FK_ForumVotes_Comments_CommentId",
                table: "ForumVotes");

            migrationBuilder.DropForeignKey(
                name: "FK_ForumVotes_ForumPosts_PostId",
                table: "ForumVotes");

            migrationBuilder.AlterColumn<Guid>(
                name: "UserId",
                table: "ForumVotes",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "PostId",
                table: "ForumVotes",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "CommentId",
                table: "ForumVotes",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumVotes_AspNetUsers_UserId",
                table: "ForumVotes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumVotes_Comments_CommentId",
                table: "ForumVotes",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "CommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ForumVotes_ForumPosts_PostId",
                table: "ForumVotes",
                column: "PostId",
                principalTable: "ForumPosts",
                principalColumn: "ForumPostId");
        }
    }
}
