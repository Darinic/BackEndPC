using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thoughts.Infrastructure.Migrations
{
    public partial class updatedCreationDateForCommentstg : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ModificationDate",
                table: "ThoughtComments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 2, 12, 23, 26, 24, 641, DateTimeKind.Local).AddTicks(9403),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 2, 12, 23, 18, 55, 596, DateTimeKind.Local).AddTicks(1742));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "ThoughtComments",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETUTCDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ModificationDate",
                table: "ThoughtComments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 2, 12, 23, 18, 55, 596, DateTimeKind.Local).AddTicks(1742),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 2, 12, 23, 26, 24, 641, DateTimeKind.Local).AddTicks(9403));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "ThoughtComments",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETUTCDATE()");
        }
    }
}
