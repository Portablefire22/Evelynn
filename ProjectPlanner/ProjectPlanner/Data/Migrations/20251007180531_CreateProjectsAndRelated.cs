using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ProjectPlanner.Migrations
{
    /// <inheritdoc />
    public partial class CreateProjectsAndRelated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string?>(type: "TEXT", nullable: true),
                    Created = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Modified = table.Column<DateTime?>(type: "TEXT", nullable: true),
                    ModifiedBy = table.Column<string?>(type: "TEXT", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("Projects_pk", x => x.Id);
                    table.ForeignKey(
                        name: "Projects_AspNetUsers_Id_fk",
                        column: x => x.ModifiedBy,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull,
                        onUpdate: ReferentialAction.Cascade
                    ); 
                }
            );
            
            // migrationBuilder.CreateTable(
            //     name: "Gantts",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "INTEGER", nullable: false),
            //         ProjectId = table.Column<int>(type: "INTEGER", nullable: false),
            //         Name = table.Column<string>(type: "TEXT", nullable: false),
            //         Created = table.Column<DateTime>(type: "TEXT", nullable: false),
            //         Modified = table.Column<DateTime>(type: "TEXT", nullable: true),
            //         ModifiedBy = table.Column<string>(type: "TEXT", nullable: true),
            //         XmlPath = table.Column<string>(type: "TEXT", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("Projects_pk", x => x.Id);
            //         table.ForeignKey(
            //             name: "Projects_AspNetUsers_Id_fk",
            //             column: x => x.ModifiedBy,
            //             principalTable: "AspNetUsers",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.SetNull,
            //             onUpdate: ReferentialAction.Cascade
            //         ); 
            //         table.ForeignKey(
            //             name: "Gantts_Projects_Id_fk",
            //             column: x => x.ProjectId,
            //             principalTable: "Projects",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade,
            //             onUpdate: ReferentialAction.Cascade
            //         ); 
            //     }
            // ); 
            //
            // migrationBuilder.CreateTable(
            //     name: "UserProjects",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "INTEGER", nullable: false),
            //         UserId = table.Column<int>(type: "TEXT", nullable: false),
            //         ProjectId = table.Column<int>(type: "INTEGER", nullable: false),
            //         Capabilities = table.Column<int>(type: "INTEGER", nullable: false)
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("Projects_pk", x => x.Id);
            //         table.ForeignKey(
            //             name: "UserProjects_AspNetUsers_Id_fk",
            //             column: x => x.UserId,
            //             principalTable: "AspNetUsers",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.SetNull,
            //             onUpdate: ReferentialAction.Cascade
            //         ); 
            //         table.ForeignKey(
            //             name: "UserProjects_Projects_Id_fk",
            //             column: x => x.ProjectId,
            //             principalTable: "Projects",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade,
            //             onUpdate: ReferentialAction.Cascade
            //         ); 
            //     }
            // ); 
            //
            // migrationBuilder.CreateTable(
            //     name: "Notes",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "INTEGER", nullable: false),
            //         ProjectId = table.Column<int>(type: "INTEGER", nullable: false),
            //         Created = table.Column<DateTime>(type: "TEXT", nullable: false),
            //         Modified = table.Column<DateTime>(type: "TEXT", nullable: true),
            //         UploadedBy = table.Column<string>(type: "TEXT", nullable: true),
            //         NotePath = table.Column<string>(type: "TEXT", nullable: false),
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("Projects_pk", x => x.Id);
            //         table.ForeignKey(
            //             name: "Notes_AspNetUsers_Id_fk",
            //             column: x => x.UploadedBy,
            //             principalTable: "AspNetUsers",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.SetNull,
            //             onUpdate: ReferentialAction.Cascade
            //         ); 
            //         table.ForeignKey(
            //             name: "Notes_Projects_Id_fk",
            //             column: x => x.ProjectId,
            //             principalTable: "Projects",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade,
            //             onUpdate: ReferentialAction.Cascade
            //         ); 
            //     }
            // );
            // migrationBuilder.CreateTable(
            //     name: "NoteAttachments",
            //     columns: table => new
            //     {
            //         Id = table.Column<int>(type: "INTEGER", nullable: false),
            //         NoteId = table.Column<int>(type: "INTEGER", nullable: false),
            //         Created = table.Column<DateTime>(type: "TEXT", nullable: false),
            //         UploadedBy = table.Column<string>(type: "TEXT", nullable: true),
            //         FilePath = table.Column<string>(type: "TEXT", nullable: false),
            //     },
            //     constraints: table =>
            //     {
            //         table.PrimaryKey("Projects_pk", x => x.Id );
            //         table.ForeignKey(
            //             name: "NoteAttachments_AspNetUsers_Id_fk",
            //             column: x => x.UploadedBy,
            //             principalTable: "AspNetUsers",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.SetNull,
            //             onUpdate: ReferentialAction.Cascade
            //         ); 
            //         table.ForeignKey(
            //             name: "NoteAttachments_NoteId_Id_fk",
            //             column: x => x.NoteId,
            //             principalTable: "Notes",
            //             principalColumn: "Id",
            //             onDelete: ReferentialAction.Cascade,
            //             onUpdate: ReferentialAction.Cascade
            //         ); 
            //     }
            // );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Projects");
            migrationBuilder.DropTable(name: "UserProjects");
            migrationBuilder.DropTable(name: "Gantts");
            migrationBuilder.DropTable(name: "Notes");
            migrationBuilder.DropTable(name: "NoteAttachments");
        }
    }
}
