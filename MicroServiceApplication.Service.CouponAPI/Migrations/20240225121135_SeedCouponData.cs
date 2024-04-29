using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MicroServiceApplication.Service.CouponAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedCouponData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Coupons",
                columns: new[] { "CouponId", "CouponCode", "CreationDate", "DiscountAmount", "LastUpdatedDate", "MinAmount" },
                values: new object[,]
                {
                    { 1, "OO100", new DateTime(2024, 2, 25, 14, 11, 35, 17, DateTimeKind.Local).AddTicks(4620), 10.0, null, 30 },
                    { 2, "FF100", new DateTime(2024, 2, 25, 14, 11, 35, 17, DateTimeKind.Local).AddTicks(4698), 20.0, null, 50 },
                    { 3, "AA105", new DateTime(2024, 2, 25, 14, 11, 35, 17, DateTimeKind.Local).AddTicks(4708), 25.0, null, 80 },
                    { 4, "BB105", new DateTime(2024, 2, 25, 14, 11, 35, 17, DateTimeKind.Local).AddTicks(4716), 30.0, null, 100 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Coupons",
                keyColumn: "CouponId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Coupons",
                keyColumn: "CouponId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Coupons",
                keyColumn: "CouponId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Coupons",
                keyColumn: "CouponId",
                keyValue: 4);
        }
    }
}
