using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Cryptocop.Software.API.Migrations
{
    public partial class init5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartShoppingCartItem_ShoppingCartItems_ShoppingCart~",
                table: "ShoppingCartShoppingCartItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoppingCartShoppingCartItem",
                table: "ShoppingCartShoppingCartItem");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCartShoppingCartItem_ShoppingCartItemsId_ShoppingCa~",
                table: "ShoppingCartShoppingCartItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoppingCartItems",
                table: "ShoppingCartItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentCards",
                table: "PaymentCards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderItems",
                table: "OrderItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Address",
                table: "Address");

            migrationBuilder.DropColumn(
                name: "ShoppingCartItemsShoppingCartId",
                table: "ShoppingCartShoppingCartItem");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ShoppingCartItems",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "PaymentCards",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "OrderItems",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Address",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoppingCartShoppingCartItem",
                table: "ShoppingCartShoppingCartItem",
                columns: new[] { "ShoppingCartId", "ShoppingCartItemsId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoppingCartItems",
                table: "ShoppingCartItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentCards",
                table: "PaymentCards",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderItems",
                table: "OrderItems",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Address",
                table: "Address",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartShoppingCartItem_ShoppingCartItemsId",
                table: "ShoppingCartShoppingCartItem",
                column: "ShoppingCartItemsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartShoppingCartItem_ShoppingCartItems_ShoppingCart~",
                table: "ShoppingCartShoppingCartItem",
                column: "ShoppingCartItemsId",
                principalTable: "ShoppingCartItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ShoppingCartShoppingCartItem_ShoppingCartItems_ShoppingCart~",
                table: "ShoppingCartShoppingCartItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoppingCartShoppingCartItem",
                table: "ShoppingCartShoppingCartItem");

            migrationBuilder.DropIndex(
                name: "IX_ShoppingCartShoppingCartItem_ShoppingCartItemsId",
                table: "ShoppingCartShoppingCartItem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShoppingCartItems",
                table: "ShoppingCartItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PaymentCards",
                table: "PaymentCards");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderItems",
                table: "OrderItems");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Address",
                table: "Address");

            migrationBuilder.AddColumn<int>(
                name: "ShoppingCartItemsShoppingCartId",
                table: "ShoppingCartShoppingCartItem",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "ShoppingCartItems",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "PaymentCards",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "OrderItems",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "Address",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer")
                .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoppingCartShoppingCartItem",
                table: "ShoppingCartShoppingCartItem",
                columns: new[] { "ShoppingCartId", "ShoppingCartItemsId", "ShoppingCartItemsShoppingCartId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShoppingCartItems",
                table: "ShoppingCartItems",
                columns: new[] { "Id", "ShoppingCartId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_PaymentCards",
                table: "PaymentCards",
                columns: new[] { "Id", "UserId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderItems",
                table: "OrderItems",
                columns: new[] { "Id", "OrderId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Address",
                table: "Address",
                columns: new[] { "Id", "UserId" });

            migrationBuilder.CreateIndex(
                name: "IX_ShoppingCartShoppingCartItem_ShoppingCartItemsId_ShoppingCa~",
                table: "ShoppingCartShoppingCartItem",
                columns: new[] { "ShoppingCartItemsId", "ShoppingCartItemsShoppingCartId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ShoppingCartShoppingCartItem_ShoppingCartItems_ShoppingCart~",
                table: "ShoppingCartShoppingCartItem",
                columns: new[] { "ShoppingCartItemsId", "ShoppingCartItemsShoppingCartId" },
                principalTable: "ShoppingCartItems",
                principalColumns: new[] { "Id", "ShoppingCartId" },
                onDelete: ReferentialAction.Cascade);
        }
    }
}
