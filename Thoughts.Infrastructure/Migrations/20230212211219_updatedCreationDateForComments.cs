using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thoughts.Infrastructure.Migrations
{
    public partial class updatedCreationDateForComments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ModificationDate",
                table: "ThoughtComments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 2, 12, 23, 12, 19, 813, DateTimeKind.Local).AddTicks(1803),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 2, 12, 23, 5, 5, 440, DateTimeKind.Local).AddTicks(5884));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "ThoughtComments",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 2, 12, 23, 5, 5, 440, DateTimeKind.Local).AddTicks(5728));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ModificationDate",
                table: "ThoughtComments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 2, 12, 23, 5, 5, 440, DateTimeKind.Local).AddTicks(5884),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 2, 12, 23, 12, 19, 813, DateTimeKind.Local).AddTicks(1803));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "ThoughtComments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 2, 12, 23, 5, 5, 440, DateTimeKind.Local).AddTicks(5728),
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
