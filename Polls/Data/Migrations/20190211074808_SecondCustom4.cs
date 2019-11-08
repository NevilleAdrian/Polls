using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Polls.Data.Migrations
{
    public partial class SecondCustom4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ElectionInProgress",
                table: "Elections");

            migrationBuilder.DropColumn(
                name: "HasEnded",
                table: "Elections");

            migrationBuilder.DropColumn(
                name: "HasStarted",
                table: "Elections");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ElectionInProgress",
                table: "Elections",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasEnded",
                table: "Elections",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "HasStarted",
                table: "Elections",
                nullable: false,
                defaultValue: false);
        }
    }
}
