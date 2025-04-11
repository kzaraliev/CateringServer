using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Catering.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(21)", maxLength: 21, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "User's first name."),
                    LastName = table.Column<string>(type: "nvarchar(60)", maxLength: 60, nullable: true, comment: "User's last name."),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                },
                comment: "Represents an application user in the system.");

            migrationBuilder.CreateTable(
                name: "WorkingTimes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Working Time Identifier")
                        .Annotation("SqlServer:Identity", "1, 1")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingTimes", x => x.Id);
                },
                comment: "Represents the overall working time schedule for a restaurant.");

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Addresses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Address Identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AddressName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "Friendly name for this address"),
                    Street = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "Street address"),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "City"),
                    ZipCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "ZIP/Postal code"),
                    Country = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Country"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "User Identifier")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Addresses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Addresses_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Represents a user address in the system for delivery or pickup.");

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Restaurants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Restaurant Identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, comment: "Restaurant Name"),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Restaurant Description"),
                    ContactEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Restaurant Email Address"),
                    PhoneNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Restaurant Phone Number"),
                    Address = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, comment: "Restaurant Address"),
                    ImageUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true, comment: "Restaurant Image URL Address"),
                    SupportedDeliveryMethods = table.Column<int>(type: "int", nullable: false, comment: "Delivery methods supported by the restaurant"),
                    OwnerId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "Owner Identifier"),
                    WorkingTimeId = table.Column<int>(type: "int", nullable: false, comment: "Working Time Schedule Identifier")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Restaurants_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Restaurants_WorkingTimes_WorkingTimeId",
                        column: x => x.WorkingTimeId,
                        principalTable: "WorkingTimes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Represents a restaurant in the catering platform.");

            migrationBuilder.CreateTable(
                name: "WorkingDays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Working Day Identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Day = table.Column<int>(type: "int", nullable: false, comment: "Day of the week."),
                    OpenTime = table.Column<TimeSpan>(type: "time", nullable: false, comment: "Opening time for the day."),
                    CloseTime = table.Column<TimeSpan>(type: "time", nullable: false, comment: "Closing time for the day."),
                    IsClosed = table.Column<bool>(type: "bit", nullable: false, comment: "Indicates whether the restaurant is closed on this day."),
                    WorkingTimeId = table.Column<int>(type: "int", nullable: false, comment: "Foreign key to the related WorkingTime schedule.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingDays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkingDays_WorkingTimes_WorkingTimeId",
                        column: x => x.WorkingTimeId,
                        principalTable: "WorkingTimes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Represents a specific day within a restaurant's working time schedule.");

            migrationBuilder.CreateTable(
                name: "Coupons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Coupon Identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Coupon code"),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true, comment: "Coupon description"),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true, comment: "Discount amount in currency"),
                    DiscountPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: true, comment: "Discount percentage"),
                    MinimumOrderAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true, comment: "Minimum order amount required"),
                    MaximumDiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: true, comment: "Maximum discount amount for percentage-based coupons"),
                    ValidFrom = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Start date of the coupon validity period"),
                    ValidTo = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "End date of the coupon validity period"),
                    MaxUsageCount = table.Column<int>(type: "int", nullable: true, comment: "Maximum usage count"),
                    UsageCount = table.Column<int>(type: "int", nullable: false, comment: "Current usage count"),
                    RestaurantId = table.Column<int>(type: "int", nullable: false, comment: "Restaurant Identifier")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coupons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Coupons_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id");
                },
                comment: "Represents a discount coupon that can be applied to orders.");

            migrationBuilder.CreateTable(
                name: "MenuCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Menu Category Identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Name of the menu category"),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Optional description for the menu category"),
                    RestaurantId = table.Column<int>(type: "int", nullable: false, comment: "Foreign key to the restaurant.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuCategories_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Represents a category of menu items for a restaurant.");

            migrationBuilder.CreateTable(
                name: "Reviews",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Review Identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RestaurantId = table.Column<int>(type: "int", nullable: false, comment: "Foreign key to the reviewed restaurant."),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "Identifier of the user who submitted the review."),
                    Rating = table.Column<int>(type: "int", nullable: false, comment: "Rating provided in the review."),
                    Comment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Optional comment provided in the review."),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date and time when the review was created.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reviews", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reviews_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Reviews_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id");
                },
                comment: "Represents a review left by a user for a restaurant.");

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Order Identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date and time when the order was placed"),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "Customer Identifier"),
                    RestaurantId = table.Column<int>(type: "int", nullable: false, comment: "Restaurant Identifier"),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "Order status"),
                    DeliveryMethod = table.Column<int>(type: "int", nullable: false, comment: "Delivery method"),
                    DeliveryAddressId = table.Column<int>(type: "int", nullable: true, comment: "Delivery Address Identifier"),
                    RequestedDeliveryTime = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Requested delivery or pickup time"),
                    ActualDeliveryTime = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Actual delivery or pickup time"),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "Subtotal amount before tax, discount, and delivery fee"),
                    TaxAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "Tax amount"),
                    DeliveryFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "Delivery fee"),
                    DiscountAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "Discount amount"),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "Total amount after tax, discount, and delivery fee"),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Additional notes or instructions"),
                    CouponId = table.Column<int>(type: "int", nullable: true, comment: "Coupon Identifier")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Orders_Addresses_DeliveryAddressId",
                        column: x => x.DeliveryAddressId,
                        principalTable: "Addresses",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Orders_AspNetUsers_CustomerId",
                        column: x => x.CustomerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Orders_Coupons_CouponId",
                        column: x => x.CouponId,
                        principalTable: "Coupons",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Orders_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id");
                },
                comment: "Represents an order in the catering system.");

            migrationBuilder.CreateTable(
                name: "MenuItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Menu Item Identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Name of the menu item"),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Description of the menu item"),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "Price of the menu item"),
                    ImageUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true, comment: "Image URL for the menu item"),
                    MenuCategoryId = table.Column<int>(type: "int", nullable: false, comment: "Foreign key to the associated menu category.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MenuItems_MenuCategories_MenuCategoryId",
                        column: x => x.MenuCategoryId,
                        principalTable: "MenuCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Represents an individual menu item or product offered by the restaurant.");

            migrationBuilder.CreateTable(
                name: "Payments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Payment Identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false, comment: "Order Identifier"),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "Payment amount"),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false, comment: "Payment method used (Card, BankTransfer, or OnDelivery)"),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "Payment status"),
                    TransactionId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "Transaction identifier from the payment processor"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date and time when the payment was created"),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Date and time when the payment was processed")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Payments_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Represents a payment for an order in the catering system.");

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Order Item Identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false, comment: "Order Identifier"),
                    MenuItemId = table.Column<int>(type: "int", nullable: false, comment: "Menu Item Identifier"),
                    Quantity = table.Column<int>(type: "int", nullable: false, comment: "Quantity of the menu item"),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "Unit price at the time of ordering"),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "Total price for this order item"),
                    SpecialInstructions = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Special instructions for this menu item")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_MenuItems_MenuItemId",
                        column: x => x.MenuItemId,
                        principalTable: "MenuItems",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Represents an item within an order in the catering system.");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_UserId",
                table: "Addresses",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Coupons_RestaurantId",
                table: "Coupons",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuCategories_RestaurantId",
                table: "MenuCategories",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_MenuCategoryId",
                table: "MenuItems",
                column: "MenuCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_MenuItemId",
                table: "OrderItems",
                column: "MenuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_OrderId",
                table: "OrderItems",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CouponId",
                table: "Orders",
                column: "CouponId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_CustomerId",
                table: "Orders",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_DeliveryAddressId",
                table: "Orders",
                column: "DeliveryAddressId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_RestaurantId",
                table: "Orders",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_OrderId",
                table: "Payments",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_OwnerId",
                table: "Restaurants",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_WorkingTimeId",
                table: "Restaurants",
                column: "WorkingTimeId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_RestaurantId",
                table: "Reviews",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingDays_WorkingTimeId",
                table: "WorkingDays",
                column: "WorkingTimeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "WorkingDays");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "MenuItems");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "MenuCategories");

            migrationBuilder.DropTable(
                name: "Addresses");

            migrationBuilder.DropTable(
                name: "Coupons");

            migrationBuilder.DropTable(
                name: "Restaurants");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "WorkingTimes");
        }
    }
}
