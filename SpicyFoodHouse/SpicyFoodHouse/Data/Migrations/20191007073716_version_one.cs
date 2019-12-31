using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace SpicyFoodHouse.Data.Migrations
{
    public partial class version_one : Migration
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
                name: "Address",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "NidOrBith",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Phone",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Comment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CommentText = table.Column<string>(maxLength: 1500, nullable: false),
                    CommentTime = table.Column<DateTime>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Username = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FoodQuarter",
                columns: table => new
                {
                    QuarterId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EntryDate = table.Column<DateTime>(nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    ManagerSignature = table.Column<string>(nullable: true),
                    QuarterName = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodQuarter", x => x.QuarterId);
                });

            migrationBuilder.CreateTable(
                name: "FoodType",
                columns: table => new
                {
                    TypeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EntryDate = table.Column<DateTime>(nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    ManagerSignature = table.Column<string>(nullable: true),
                    TypeName = table.Column<string>(maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodType", x => x.TypeId);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethod",
                columns: table => new
                {
                    PaymentMethodId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EntryDate = table.Column<DateTime>(nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    ManagerSignature = table.Column<string>(nullable: true),
                    PaymentMethodName = table.Column<string>(maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethod", x => x.PaymentMethodId);
                });
            
            migrationBuilder.CreateTable(
                name: "Food",
                columns: table => new
                {
                    FoodId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AvailableInStock = table.Column<string>(nullable: false),
                    Description = table.Column<string>(maxLength: 1000, nullable: false),
                    Discount = table.Column<float>(nullable: false),
                    EntryDate = table.Column<DateTime>(nullable: false),
                    FoodName = table.Column<string>(maxLength: 200, nullable: false),
                    Image = table.Column<byte[]>(nullable: false),
                    IsDiscounted = table.Column<bool>(nullable: false),
                    IsPopular = table.Column<bool>(nullable: false),
                    IsTranding = table.Column<bool>(nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    ManagerSignature = table.Column<string>(nullable: true),
                    Price = table.Column<float>(nullable: false),
                    TypeId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Food", x => x.FoodId);
                    table.ForeignKey(
                        name: "FK_Food_FoodType_TypeId",
                        column: x => x.TypeId,
                        principalTable: "FoodType",
                        principalColumn: "TypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FoodOrder",
                columns: table => new
                {
                    OrderId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CustomerEmail = table.Column<string>(nullable: true),
                    IsAccepted = table.Column<bool>(nullable: false),
                    FoodName = table.Column<string>(maxLength: 200, nullable: false),
                    FoodName2 = table.Column<string>(maxLength: 200, nullable: false),
                    FoodName3 = table.Column<string>(maxLength: 200, nullable: false),
                    IsPaid = table.Column<bool>(nullable: false),
                    IsSeen = table.Column<bool>(nullable: false),
                    LastFiveDigit = table.Column<int>(nullable: false),
                    LastUpdatedDate = table.Column<DateTime>(nullable: false),
                    NumberOfFood = table.Column<int>(nullable: false),
                    NumberOfFood2 = table.Column<int>(nullable: false),
                    NumberOfFood3 = table.Column<int>(nullable: false),
                    OrderDate = table.Column<DateTime>(nullable: false),
                    PaymentMethodId = table.Column<int>(nullable: false),
                    Price = table.Column<float>(nullable: false),
                    Price2 = table.Column<float>(nullable: false),
                    Price3 = table.Column<float>(nullable: false),
                    QuarterId = table.Column<int>(nullable: false),
                    QuarterId2 = table.Column<int>(nullable: false),
                    QuarterId3 = table.Column<int>(nullable: false),
                    IsRejected = table.Column<bool>(nullable: false),
                    SubTotalPrice = table.Column<float>(nullable: false),
                    TotalPrice = table.Column<float>(nullable: false),
                    TotalPrice2 = table.Column<float>(nullable: false),
                    TotalPrice3 = table.Column<float>(nullable: false),
                    TypeId = table.Column<int>(nullable: false),
                    TypeId2 = table.Column<int>(nullable: false),
                    TypeId3 = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodOrder", x => x.OrderId);
                    table.ForeignKey(
                        name: "FK_FoodOrder_PaymentMethod_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalTable: "PaymentMethod",
                        principalColumn: "PaymentMethodId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoodOrder_FoodQuarter_QuarterId",
                        column: x => x.QuarterId,
                        principalTable: "FoodQuarter",
                        principalColumn: "QuarterId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoodOrder_FoodType_TypeId",
                        column: x => x.TypeId,
                        principalTable: "FoodType",
                        principalColumn: "TypeId",
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
                name: "IX_Food_TypeId",
                table: "Food",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FoodOrder_PaymentMethodId",
                table: "FoodOrder",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_FoodOrder_QuarterId",
                table: "FoodOrder",
                column: "QuarterId");

            migrationBuilder.CreateIndex(
                name: "IX_FoodOrder_TypeId",
                table: "FoodOrder",
                column: "TypeId");

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
                name: "Comment");

            migrationBuilder.DropTable(
                name: "Food");

            migrationBuilder.DropTable(
                name: "FoodOrder");

            migrationBuilder.DropTable(
                name: "PaymentMethod");

            migrationBuilder.DropTable(
                name: "DeliveryCharge");

            migrationBuilder.DropTable(
                name: "FoodQuarter");

            migrationBuilder.DropTable(
                name: "FoodType");

            migrationBuilder.DropIndex(
                name: "UserNameIndex",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NidOrBith",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Phone",
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
