using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ITSupportBE.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateDbContextChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CannedResponse_Categorie_CategoryId",
                table: "CannedResponse");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Categorie_CategoryId",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_TicketStatus_StatusId",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketAttachment_TicketReplie_ReplyId",
                table: "TicketAttachment");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketReplie_Ticket_TicketId",
                table: "TicketReplie");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketReplie_User_UserId",
                table: "TicketReplie");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketReplie",
                table: "TicketReplie");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categorie",
                table: "Categorie");

            migrationBuilder.RenameTable(
                name: "TicketReplie",
                newName: "TicketReply");

            migrationBuilder.RenameTable(
                name: "Categorie",
                newName: "Category");

            migrationBuilder.RenameIndex(
                name: "IX_TicketReplie_UserId",
                table: "TicketReply",
                newName: "IX_TicketReply_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_TicketReplie_TicketId",
                table: "TicketReply",
                newName: "IX_TicketReply_TicketId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketReply",
                table: "TicketReply",
                column: "ReplyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Category",
                table: "Category",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_CannedResponse_Category_CategoryId",
                table: "CannedResponse",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_Category_CategoryId",
                table: "Ticket",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_TicketStatus_StatusId",
                table: "Ticket",
                column: "StatusId",
                principalTable: "TicketStatus",
                principalColumn: "StatusId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketAttachment_TicketReply_ReplyId",
                table: "TicketAttachment",
                column: "ReplyId",
                principalTable: "TicketReply",
                principalColumn: "ReplyId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketReply_Ticket_TicketId",
                table: "TicketReply",
                column: "TicketId",
                principalTable: "Ticket",
                principalColumn: "TicketId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketReply_User_UserId",
                table: "TicketReply",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CannedResponse_Category_CategoryId",
                table: "CannedResponse");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_Category_CategoryId",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "FK_Ticket_TicketStatus_StatusId",
                table: "Ticket");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketAttachment_TicketReply_ReplyId",
                table: "TicketAttachment");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketReply_Ticket_TicketId",
                table: "TicketReply");

            migrationBuilder.DropForeignKey(
                name: "FK_TicketReply_User_UserId",
                table: "TicketReply");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TicketReply",
                table: "TicketReply");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Category",
                table: "Category");

            migrationBuilder.RenameTable(
                name: "TicketReply",
                newName: "TicketReplie");

            migrationBuilder.RenameTable(
                name: "Category",
                newName: "Categorie");

            migrationBuilder.RenameIndex(
                name: "IX_TicketReply_UserId",
                table: "TicketReplie",
                newName: "IX_TicketReplie_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_TicketReply_TicketId",
                table: "TicketReplie",
                newName: "IX_TicketReplie_TicketId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TicketReplie",
                table: "TicketReplie",
                column: "ReplyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categorie",
                table: "Categorie",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_CannedResponse_Categorie_CategoryId",
                table: "CannedResponse",
                column: "CategoryId",
                principalTable: "Categorie",
                principalColumn: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_Categorie_CategoryId",
                table: "Ticket",
                column: "CategoryId",
                principalTable: "Categorie",
                principalColumn: "CategoryId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Ticket_TicketStatus_StatusId",
                table: "Ticket",
                column: "StatusId",
                principalTable: "TicketStatus",
                principalColumn: "StatusId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketAttachment_TicketReplie_ReplyId",
                table: "TicketAttachment",
                column: "ReplyId",
                principalTable: "TicketReplie",
                principalColumn: "ReplyId");

            migrationBuilder.AddForeignKey(
                name: "FK_TicketReplie_Ticket_TicketId",
                table: "TicketReplie",
                column: "TicketId",
                principalTable: "Ticket",
                principalColumn: "TicketId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TicketReplie_User_UserId",
                table: "TicketReplie",
                column: "UserId",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
