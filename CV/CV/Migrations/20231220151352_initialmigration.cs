﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CV.Migrations
{
    /// <inheritdoc />
    public partial class initialmigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CV_s",
                columns: table => new
                {
                    CID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Picture = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CV_s", x => x.CID);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                columns: table => new
                {
                    PID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BeginDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.PID);
                });

            migrationBuilder.CreateTable(
                name: "Competence",
                columns: table => new
                {
                    CompID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CV_CID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Competence", x => x.CompID);
                    table.ForeignKey(
                        name: "FK_Competence_CV_s_CV_CID",
                        column: x => x.CV_CID,
                        principalTable: "CV_s",
                        principalColumn: "CID");
                });

            migrationBuilder.CreateTable(
                name: "Education",
                columns: table => new
                {
                    EdID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BeginDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CV_CID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Education", x => x.EdID);
                    table.ForeignKey(
                        name: "FK_Education_CV_s_CV_CID",
                        column: x => x.CV_CID,
                        principalTable: "CV_s",
                        principalColumn: "CID");
                });

            migrationBuilder.CreateTable(
                name: "Experience",
                columns: table => new
                {
                    EID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Experience", x => x.EID);
                    table.ForeignKey(
                        name: "FK_Experience_CV_s_CID",
                        column: x => x.CID,
                        principalTable: "CV_s",
                        principalColumn: "CID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Firstname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lastname = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConfirmPassword = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Epost = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Adress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Privat = table.Column<bool>(type: "bit", nullable: false),
                    CID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UID);
                    table.ForeignKey(
                        name: "FK_Users_CV_s_CID",
                        column: x => x.CID,
                        principalTable: "CV_s",
                        principalColumn: "CID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CV_Competences",
                columns: table => new
                {
                    CID = table.Column<int>(type: "int", nullable: false),
                    CompID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CV_Competences", x => new { x.CID, x.CompID });
                    table.ForeignKey(
                        name: "FK_CV_Competences_CV_s_CID",
                        column: x => x.CID,
                        principalTable: "CV_s",
                        principalColumn: "CID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CV_Competences_Competence_CompID",
                        column: x => x.CompID,
                        principalTable: "Competence",
                        principalColumn: "CompID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CV_Educations",
                columns: table => new
                {
                    CID = table.Column<int>(type: "int", nullable: false),
                    EdID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CV_Educations", x => new { x.CID, x.EdID });
                    table.ForeignKey(
                        name: "FK_CV_Educations_CV_s_CID",
                        column: x => x.CID,
                        principalTable: "CV_s",
                        principalColumn: "CID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CV_Educations_Education_EdID",
                        column: x => x.EdID,
                        principalTable: "Education",
                        principalColumn: "EdID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Chats",
                columns: table => new
                {
                    MID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Text = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Read = table.Column<bool>(type: "bit", nullable: true),
                    UID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Chats", x => x.MID);
                    table.ForeignKey(
                        name: "FK_Chats_Users_UID",
                        column: x => x.UID,
                        principalTable: "Users",
                        principalColumn: "UID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserProjects",
                columns: table => new
                {
                    UID = table.Column<int>(type: "int", nullable: false),
                    PID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProjects", x => new { x.PID, x.UID });
                    table.ForeignKey(
                        name: "FK_UserProjects_Projects_PID",
                        column: x => x.PID,
                        principalTable: "Projects",
                        principalColumn: "PID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserProjects_Users_UID",
                        column: x => x.UID,
                        principalTable: "Users",
                        principalColumn: "UID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CV_s",
                columns: new[] { "CID", "Picture" },
                values: new object[,]
                {
                    { 1, "profile_picture1.jpg" },
                    { 2, "profile_picture2.jpg" },
                    { 3, "profile_picture3.jpg" },
                    { 4, "profile_picture4.jpg" },
                    { 5, "profile_picture5.jpg" }
                });

            migrationBuilder.InsertData(
                table: "Competence",
                columns: new[] { "CompID", "CV_CID", "Description" },
                values: new object[,]
                {
                    { 1, null, "C# Programming" },
                    { 2, null, "Web Development" },
                    { 3, null, "Data Analysis" },
                    { 4, null, "Project Management" },
                    { 5, null, "Communication Skills" }
                });

            migrationBuilder.InsertData(
                table: "Education",
                columns: new[] { "EdID", "BeginDate", "CV_CID", "Description", "EndDate" },
                values: new object[,]
                {
                    { 1, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Master's in Computer Science", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Bachelor's in Information Technology", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "High School Diploma", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Certification in Web Development", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, new DateTime(2020, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Online Course in Data Science", new DateTime(2023, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Projects",
                columns: new[] { "PID", "BeginDate", "Description", "EndDate", "Title" },
                values: new object[,]
                {
                    { 1, new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Developing a responsive web application.", new DateTime(2022, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Web Development Project" },
                    { 2, new DateTime(2022, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Creating a cross-platform mobile application.", new DateTime(2022, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mobile App Development" },
                    { 3, new DateTime(2022, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Designing and implementing a relational database.", new DateTime(2022, 11, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Database Management System" },
                    { 4, new DateTime(2022, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Applying machine learning algorithms to solve a specific problem.", new DateTime(2023, 1, 31, 0, 0, 0, 0, DateTimeKind.Unspecified), "Machine Learning Project" },
                    { 5, new DateTime(2022, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Working on a software project using agile methodologies.", new DateTime(2023, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Agile Software Development" }
                });

            migrationBuilder.InsertData(
                table: "CV_Competences",
                columns: new[] { "CID", "CompID" },
                values: new object[,]
                {
                    { 1, 2 },
                    { 1, 4 },
                    { 2, 3 },
                    { 4, 2 },
                    { 4, 5 }
                });

            migrationBuilder.InsertData(
                table: "CV_Educations",
                columns: new[] { "CID", "EdID" },
                values: new object[,]
                {
                    { 1, 2 },
                    { 1, 4 },
                    { 2, 3 },
                    { 4, 2 },
                    { 4, 5 }
                });

            migrationBuilder.InsertData(
                table: "Experience",
                columns: new[] { "EID", "CID", "Description" },
                values: new object[,]
                {
                    { 1, 1, "Software Developer at ABC Tech" },
                    { 2, 2, "Data Analyst at XYZ Analytics" },
                    { 3, 3, "Project Manager at Acme Projects" },
                    { 4, 4, "Internship at DEF Corporation" },
                    { 5, 5, "Marketing Coordinator at LMN Marketing" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UID", "Adress", "CID", "ConfirmPassword", "Epost", "Firstname", "Lastname", "Password", "Privat", "Username" },
                values: new object[,]
                {
                    { 1, "456 Oak Street", 1, "password1", "alice.johnson@example.com", "Alice", "Johnson", "password1", true, "user1" },
                    { 2, "789 Pine Avenue", 2, "password2", "bob.smith@example.com", "Bob", "Smith", "password2", false, "user2" },
                    { 3, "101 Elm Lane", 3, "password3", "charlie.brown@example.com", "Charlie", "Brown", "password3", true, "user3" },
                    { 4, "202 Maple Road", 4, "password4", "david.lee@example.com", "David", "Lee", "password4", false, "user4" },
                    { 5, "303 Cedar Street", 5, "password5", "eva.miller@example.com", "Eva", "Miller", "password5", true, "user5" }
                });

            migrationBuilder.InsertData(
                table: "Chats",
                columns: new[] { "MID", "Date", "Read", "Text", "UID" },
                values: new object[,]
                {
                    { 1, new DateTime(2022, 1, 10, 12, 30, 0, 0, DateTimeKind.Unspecified), true, "Hej, hur mår du?", 1 },
                    { 2, new DateTime(2022, 1, 10, 12, 35, 0, 0, DateTimeKind.Unspecified), true, "Jag mår bra, tack!", 2 },
                    { 3, new DateTime(2022, 1, 10, 13, 0, 0, 0, DateTimeKind.Unspecified), false, "Vad har du gjort idag?", 3 },
                    { 4, new DateTime(2022, 1, 10, 13, 15, 0, 0, DateTimeKind.Unspecified), false, "Jobbat och tränat lite.", 4 },
                    { 5, new DateTime(2022, 1, 10, 14, 0, 0, 0, DateTimeKind.Unspecified), false, "Låter bra! Vad har du för planer resten av dagen?", 5 }
                });

            migrationBuilder.InsertData(
                table: "UserProjects",
                columns: new[] { "PID", "UID" },
                values: new object[,]
                {
                    { 2, 1 },
                    { 2, 4 },
                    { 3, 2 },
                    { 4, 1 },
                    { 5, 4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Chats_UID",
                table: "Chats",
                column: "UID");

            migrationBuilder.CreateIndex(
                name: "IX_Competence_CV_CID",
                table: "Competence",
                column: "CV_CID");

            migrationBuilder.CreateIndex(
                name: "IX_CV_Competences_CompID",
                table: "CV_Competences",
                column: "CompID");

            migrationBuilder.CreateIndex(
                name: "IX_CV_Educations_EdID",
                table: "CV_Educations",
                column: "EdID");

            migrationBuilder.CreateIndex(
                name: "IX_Education_CV_CID",
                table: "Education",
                column: "CV_CID");

            migrationBuilder.CreateIndex(
                name: "IX_Experience_CID",
                table: "Experience",
                column: "CID");

            migrationBuilder.CreateIndex(
                name: "IX_UserProjects_UID",
                table: "UserProjects",
                column: "UID");

            migrationBuilder.CreateIndex(
                name: "IX_Users_CID",
                table: "Users",
                column: "CID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Chats");

            migrationBuilder.DropTable(
                name: "CV_Competences");

            migrationBuilder.DropTable(
                name: "CV_Educations");

            migrationBuilder.DropTable(
                name: "Experience");

            migrationBuilder.DropTable(
                name: "UserProjects");

            migrationBuilder.DropTable(
                name: "Competence");

            migrationBuilder.DropTable(
                name: "Education");

            migrationBuilder.DropTable(
                name: "Projects");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "CV_s");
        }
    }
}