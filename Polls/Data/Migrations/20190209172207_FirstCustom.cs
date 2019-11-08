using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Polls.Data.Migrations
{
    public partial class FirstCustom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUserRoles_UserId",
                table: "AspNetUserRoles");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Elections",
                columns: table => new
                {
                    ElectionId = table.Column<string>(nullable: false),
                    ElectionEnds = table.Column<DateTime>(nullable: false),
                    ElectionInProgress = table.Column<bool>(nullable: false),
                    ElectionName = table.Column<string>(nullable: true),
                    ElectionStarts = table.Column<DateTime>(nullable: false),
                    ExtraInformation = table.Column<string>(nullable: true),
                    HasEnded = table.Column<bool>(nullable: false),
                    HasStarted = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Elections", x => x.ElectionId);
                });

            migrationBuilder.CreateTable(
                name: "Candidates",
                columns: table => new
                {
                    CandidateId = table.Column<string>(nullable: false),
                    ElectionId = table.Column<string>(nullable: true),
                    MyImageUrl = table.Column<string>(nullable: true),
                    MyPromise = table.Column<string>(nullable: true),
                    TotalNumberOfVotes = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Candidates", x => x.CandidateId);
                    table.ForeignKey(
                        name: "FK_Candidates_Elections_ElectionId",
                        column: x => x.ElectionId,
                        principalTable: "Elections",
                        principalColumn: "ElectionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Candidates_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Ties",
                columns: table => new
                {
                    TieId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ElectionId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ties", x => x.TieId);
                    table.ForeignKey(
                        name: "FK_Ties_Elections_ElectionId",
                        column: x => x.ElectionId,
                        principalTable: "Elections",
                        principalColumn: "ElectionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ElectionWinners",
                columns: table => new
                {
                    ElectionWinnerId = table.Column<string>(nullable: false),
                    CandidateId = table.Column<string>(nullable: true),
                    ElectionId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ElectionWinners", x => x.ElectionWinnerId);
                    table.ForeignKey(
                        name: "FK_ElectionWinners_Candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidates",
                        principalColumn: "CandidateId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ElectionWinners_Elections_ElectionId",
                        column: x => x.ElectionId,
                        principalTable: "Elections",
                        principalColumn: "ElectionId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Voters",
                columns: table => new
                {
                    VoterId = table.Column<string>(nullable: false),
                    CandidateId = table.Column<string>(nullable: true),
                    ElectionId = table.Column<string>(nullable: true),
                    HasVoted = table.Column<bool>(nullable: false),
                    UserId = table.Column<string>(nullable: true),
                    WhenIFinished = table.Column<DateTime>(nullable: false),
                    WhenIStartedVoting = table.Column<DateTime>(nullable: false),
                    WillVoteAgainBecauseOfTie = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Voters", x => x.VoterId);
                    table.ForeignKey(
                        name: "FK_Voters_Candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidates",
                        principalColumn: "CandidateId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Voters_Elections_ElectionId",
                        column: x => x.ElectionId,
                        principalTable: "Elections",
                        principalColumn: "ElectionId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Voters_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Grieviances",
                columns: table => new
                {
                    GrievianceId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CandidateId = table.Column<string>(nullable: true),
                    HasBeenSeen = table.Column<bool>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    VoterId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Grieviances", x => x.GrievianceId);
                    table.ForeignKey(
                        name: "FK_Grieviances_Candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidates",
                        principalColumn: "CandidateId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Grieviances_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Grieviances_Voters_VoterId",
                        column: x => x.VoterId,
                        principalTable: "Voters",
                        principalColumn: "VoterId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Notifications",
                columns: table => new
                {
                    NotificationId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CandidateId = table.Column<string>(nullable: true),
                    HasBeenSeen = table.Column<bool>(nullable: false),
                    Message = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    VoterId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notifications", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_Notifications_Candidates_CandidateId",
                        column: x => x.CandidateId,
                        principalTable: "Candidates",
                        principalColumn: "CandidateId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notifications_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Notifications_Voters_VoterId",
                        column: x => x.VoterId,
                        principalTable: "Voters",
                        principalColumn: "VoterId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GrievianceReplies",
                columns: table => new
                {
                    GrievianceReplyId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    GrievianceId = table.Column<int>(nullable: false),
                    Message = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GrievianceReplies", x => x.GrievianceReplyId);
                    table.ForeignKey(
                        name: "FK_GrievianceReplies_Grieviances_GrievianceId",
                        column: x => x.GrievianceId,
                        principalTable: "Grieviances",
                        principalColumn: "GrievianceId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_ElectionId",
                table: "Candidates",
                column: "ElectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Candidates_UserId",
                table: "Candidates",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ElectionWinners_CandidateId",
                table: "ElectionWinners",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_ElectionWinners_ElectionId",
                table: "ElectionWinners",
                column: "ElectionId");

            migrationBuilder.CreateIndex(
                name: "IX_GrievianceReplies_GrievianceId",
                table: "GrievianceReplies",
                column: "GrievianceId");

            migrationBuilder.CreateIndex(
                name: "IX_Grieviances_CandidateId",
                table: "Grieviances",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_Grieviances_UserId",
                table: "Grieviances",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Grieviances_VoterId",
                table: "Grieviances",
                column: "VoterId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_CandidateId",
                table: "Notifications",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_VoterId",
                table: "Notifications",
                column: "VoterId");

            migrationBuilder.CreateIndex(
                name: "IX_Ties_ElectionId",
                table: "Ties",
                column: "ElectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Voters_CandidateId",
                table: "Voters",
                column: "CandidateId");

            migrationBuilder.CreateIndex(
                name: "IX_Voters_ElectionId",
                table: "Voters",
                column: "ElectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Voters_UserId",
                table: "Voters",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                table: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "ElectionWinners");

            migrationBuilder.DropTable(
                name: "GrievianceReplies");

            migrationBuilder.DropTable(
                name: "Notifications");

            migrationBuilder.DropTable(
                name: "Ties");

            migrationBuilder.DropTable(
                name: "Grieviances");

            migrationBuilder.DropTable(
                name: "Voters");

            migrationBuilder.DropTable(
                name: "Candidates");

            migrationBuilder.DropTable(
                name: "Elections");

            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Position",
                table: "AspNetUsers");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_UserId",
                table: "AspNetUserRoles",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName");
        }
    }
}
