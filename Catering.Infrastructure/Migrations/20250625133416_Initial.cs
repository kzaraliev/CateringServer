using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Catering.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
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
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
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
                name: "Carts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Unique identifier for the cart."),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true, comment: "ID of the authenticated user who owns this cart. Null for guest carts."),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "UTC date and time when the cart was created."),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "UTC date and time when the cart was last modified.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Carts_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                },
                comment: "Represents a shopping cart.");

            migrationBuilder.CreateTable(
                name: "LoginCodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Unique identifier for the login code.")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(6)", maxLength: 6, nullable: false, comment: "The actual numeric or alphanumeric code sent to the user."),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "UTC date and time when the login code was created."),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "UTC date and time when the login code expires."),
                    UsedAt = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "UTC date and time when the login code was used. Null if not yet used."),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "ID of the user for whom this login code was generated.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginCodes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoginCodes_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Represents a one-time login code (OTP) generated for passwordless authentication.");

            migrationBuilder.CreateTable(
                name: "RefreshTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Refresh token identifier.")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false, comment: "The token string."),
                    Expires = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "The expiration date and time of the token."),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "The date and time when the token was created."),
                    Revoked = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "The date and time when the token was revoked, if any."),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false, comment: "Identifier of the user the token belongs to.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefreshTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Represents a refresh token used for renewing JWT tokens.");

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
                    IsPublic = table.Column<bool>(type: "bit", nullable: false, comment: "Is restaurant public"),
                    ImageUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true, comment: "Restaurant Image URL Address"),
                    SupportedDeliveryMethods = table.Column<int>(type: "int", nullable: false, comment: "Delivery methods supported by the restaurant"),
                    OwnerId = table.Column<string>(type: "nvarchar(450)", nullable: true, comment: "Owner Identifier")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Restaurants_AspNetUsers_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                },
                comment: "Represents a restaurant in the catering platform.");

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
                name: "PartnershipRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Partnership Request Identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RestaurantName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Name of the restaurant in the request"),
                    ContactEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Email of the person requesting the partnership"),
                    PhoneNumber = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Restaurant Phone Number"),
                    Address = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false, comment: "Restaurant Address"),
                    Message = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true, comment: "Optional message from the requester"),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "The status of the partner request."),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Timestamp when the request was created"),
                    ProcessedAt = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Timestamp of request processing"),
                    RestaurantId = table.Column<int>(type: "int", nullable: true, comment: "Foreign key to the related restaurant created from this request")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartnershipRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartnershipRequests_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id");
                },
                comment: "Represents a request for a restaurant partnership in the platform.");

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
                name: "WorkingDays",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Working Day Identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Day = table.Column<int>(type: "int", nullable: false, comment: "Day of the week."),
                    OpenTime = table.Column<TimeSpan>(type: "time", nullable: true, comment: "Opening time for the day."),
                    CloseTime = table.Column<TimeSpan>(type: "time", nullable: true, comment: "Closing time for the day."),
                    IsClosed = table.Column<bool>(type: "bit", nullable: false, comment: "Indicates whether the restaurant is closed on this day."),
                    RestaurantId = table.Column<int>(type: "int", nullable: false, comment: "Foreign key to the related restaurant.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingDays", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkingDays_Restaurants_RestaurantId",
                        column: x => x.RestaurantId,
                        principalTable: "Restaurants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Represents a specific day within a restaurant's working time schedule.");

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Order Identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderDate = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Date and time when the order was placed"),
                    CustomerId = table.Column<string>(type: "nvarchar(450)", nullable: true, comment: "Customer Identifier (nullable for guest orders)"),
                    GuestEmail = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true, comment: "Email for guest customer"),
                    GuestPhoneNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true, comment: "Phone number for guest customer"),
                    GuestName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true, comment: "First name for guest customer"),
                    RestaurantId = table.Column<int>(type: "int", nullable: false, comment: "Restaurant Identifier"),
                    Status = table.Column<int>(type: "int", nullable: false, comment: "Order status"),
                    OrderType = table.Column<int>(type: "int", nullable: false, comment: "Type of order: Delivery or Pickup"),
                    Street = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true, comment: "Street address"),
                    City = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true, comment: "City"),
                    PostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true, comment: "ZIP/Postal code"),
                    RequestedDeliveryTime = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Requested delivery or pickup time"),
                    ActualDeliveryTime = table.Column<DateTime>(type: "datetime2", nullable: true, comment: "Actual delivery or pickup time"),
                    Subtotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "Subtotal amount before tax, discount, and delivery fee"),
                    DeliveryFee = table.Column<decimal>(type: "decimal(18,2)", nullable: true, comment: "Delivery fee"),
                    OrderTotal = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "Total amount after tax, discount, and delivery fee"),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false, comment: "Payment method used for the order"),
                    Notes = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true, comment: "Additional notes or instructions"),
                    CouponId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.Id);
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
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false, comment: "Indicates if the menu item is currently available for order."),
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
                name: "OrderItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Order Item Identifier")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderId = table.Column<int>(type: "int", nullable: false, comment: "Order Identifier"),
                    ItemName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Name of the menu item"),
                    ItemImageUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: true, comment: "Image of the menu item"),
                    Quantity = table.Column<int>(type: "int", nullable: false, comment: "Quantity of the menu item"),
                    UnitPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "Unit price at the time of ordering"),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false, comment: "Total price for this order item")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Represents an item within an order in the catering system.");

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
                name: "CartItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false, comment: "Unique identifier for the cart item.")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Quantity = table.Column<int>(type: "int", nullable: false, comment: "Quantity of the menu item in the cart."),
                    CartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false, comment: "Foreign key to the associated Cart."),
                    MenuItemId = table.Column<int>(type: "int", nullable: false, comment: "Foreign key to the associated MenuItem.")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CartItems_Carts_CartId",
                        column: x => x.CartId,
                        principalTable: "Carts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CartItems_MenuItems_MenuItemId",
                        column: x => x.MenuItemId,
                        principalTable: "MenuItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "Represents an individual item within a shopping cart.");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "32dbb61d-0b46-4c77-8449-f3a633b6a72b", "459d0654-cabb-4c9d-bc6d-b81d8d72cb51", "RestaurantOwner", "RESTAURANTOWNER" },
                    { "6d5800ce-d726-4fc8-83d9-d6b3ac1f591e", "cd76a1be-26ad-4f32-b29d-f0c0d5f7cd37", "Admin", "ADMIN" },
                    { "9054b99c-81ba-465d-bb62-606df48b58b9", "76a5f411-9a1a-4318-b2d0-c826fc86b32d", "Moderator", "MODERATOR" },
                    { "b9711b31-d6cf-4c89-b7c0-9634db87154d", "8d103fbd-2ede-4e27-ade3-3d0c959935b1", "User", "USER" }
                });

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
                name: "IX_CartItems_CartId",
                table: "CartItems",
                column: "CartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_MenuItemId",
                table: "CartItems",
                column: "MenuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_UserId",
                table: "Carts",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Coupons_RestaurantId",
                table: "Coupons",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_LoginCodes_UserId",
                table: "LoginCodes",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuCategories_RestaurantId",
                table: "MenuCategories",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_MenuCategoryId",
                table: "MenuItems",
                column: "MenuCategoryId");

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
                name: "IX_Orders_RestaurantId",
                table: "Orders",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_PartnershipRequests_RestaurantId",
                table: "PartnershipRequests",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_OrderId",
                table: "Payments",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshTokens_UserId",
                table: "RefreshTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Restaurants_OwnerId",
                table: "Restaurants",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_RestaurantId",
                table: "Reviews",
                column: "RestaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_Reviews_UserId",
                table: "Reviews",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingDays_RestaurantId",
                table: "WorkingDays",
                column: "RestaurantId");
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
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "LoginCodes");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "PartnershipRequests");

            migrationBuilder.DropTable(
                name: "Payments");

            migrationBuilder.DropTable(
                name: "RefreshTokens");

            migrationBuilder.DropTable(
                name: "Reviews");

            migrationBuilder.DropTable(
                name: "WorkingDays");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "MenuItems");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "MenuCategories");

            migrationBuilder.DropTable(
                name: "Coupons");

            migrationBuilder.DropTable(
                name: "Restaurants");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
