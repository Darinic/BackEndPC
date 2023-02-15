using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Thoughts.Infrastructure.Migrations
{
    public partial class updatedCreationDateForCommentst : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ModificationDate",
                table: "ThoughtComments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 2, 12, 23, 18, 55, 596, DateTimeKind.Local).AddTicks(1742),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 2, 12, 23, 12, 19, 813, DateTimeKind.Local).AddTicks(1803));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "ThoughtComments",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDATE()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "ModificationDate",
                table: "ThoughtComments",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2023, 2, 12, 23, 12, 19, 813, DateTimeKind.Local).AddTicks(1803),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2023, 2, 12, 23, 18, 55, 596, DateTimeKind.Local).AddTicks(1742));

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreationDate",
                table: "ThoughtComments",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDATE()");
        }
    }
}
