using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NextHire_API.Domains;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace NextHire_API.Migrations
{
    /// <inheritdoc />
    public partial class AfterUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "contato",
                schema: "public",
                table: "informacao_candidato",
                type: "jsonb",
                nullable: false,
                defaultValueSql: "'{}'::jsonb");

            migrationBuilder.AddColumn<string>(
                name: "endereco",
                schema: "public",
                table: "informacao_candidato",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "contato",
                schema: "public",
                table: "informacao_candidato");

            migrationBuilder.DropColumn(
                name: "endereco",
                schema: "public",
                table: "informacao_candidato");
        }
    }
}
