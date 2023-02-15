using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thoughts.Infrastructure.Migrations
{
    public partial class updatedCreationDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "Thoughts",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModificationDate",
                table: "ThoughtComments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 2, 12, 23, 5, 5, 440, DateTimeKind.Local).AddTicks(5884),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 2, 12, 22, 39, 11, 182, DateTimeKind.Local).AddTicks(8387));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "ThoughtComments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 2, 12, 23, 5, 5, 440, DateTimeKind.Local).AddTicks(5728),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 2, 12, 22, 39, 11, 182, DateTimeKind.Local).AddTicks(8203));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "Thoughts",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");

            migrationBuilder.AlterColumn<DateTime>(
                name: "ModificationDate",
                table: "ThoughtComments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 2, 12, 22, 39, 11, 182, DateTimeKind.Local).AddTicks(8387),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 2, 12, 23, 5, 5, 440, DateTimeKind.Local).AddTicks(5884));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "ThoughtComments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 2, 12, 22, 39, 11, 182, DateTimeKind.Local).AddTicks(8203),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 2, 12, 23, 5, 5, 440, DateTimeKind.Local).AddTicks(5728));
        }
    }
}
