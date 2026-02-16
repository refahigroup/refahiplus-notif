using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Refahi.Notif.Infrastructure.Persistence.Postgres.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    Owner = table.Column<long>(type: "bigint", nullable: true),
                    AppName = table.Column<int>(type: "integer", nullable: true),
                    DueTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ValidatorUrl = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    JobId = table.Column<string>(type: "text", nullable: true),
                    Sms_PhoneNumbers = table.Column<string>(type: "text", nullable: true),
                    Sms_Body = table.Column<string>(type: "text", nullable: true),
                    Sms_Status = table.Column<int>(type: "integer", nullable: true),
                    Sms_SendTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Sms_DeliverTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Sms_RetryCount = table.Column<int>(type: "integer", nullable: true),
                    Sms_IdInProvider = table.Column<string>(type: "text", nullable: true),
                    Sms_Sender = table.Column<string>(type: "text", nullable: true),
                    Sms_Gateway = table.Column<int>(type: "integer", nullable: true),
                    Sms_DueTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Sms_ValidatorUrl = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    Email_Addresses = table.Column<string>(type: "text", nullable: true),
                    Email_Subject = table.Column<string>(type: "text", nullable: true),
                    Email_Body = table.Column<string>(type: "text", nullable: true),
                    Email_IsHtml = table.Column<bool>(type: "boolean", nullable: true),
                    Email_RetryCount = table.Column<int>(type: "integer", nullable: true),
                    Email_SendTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Email_Status = table.Column<int>(type: "integer", nullable: true),
                    Email_DueTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    PushNotification_Subject = table.Column<string>(type: "text", nullable: true),
                    PushNotification_Body = table.Column<string>(type: "text", nullable: true),
                    PushNotification_Url = table.Column<string>(type: "text", nullable: true),
                    PushNotification_Data = table.Column<string>(type: "text", nullable: true),
                    PushNotification_Status = table.Column<int>(type: "integer", nullable: true),
                    PushNotification_Jobs = table.Column<string>(type: "text", nullable: true),
                    PushNotification_DueTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    PushNotification_ValidatorUrl = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    Notification_Subject = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    Notification_Body = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    Notification_Link = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    Notification_Icon = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    Notification_DueTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Notification_ExpiredDate = table.Column<DateTime>(type: "timestamp", nullable: true),
                    Notification_ValidatorUrl = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    Notification_Status = table.Column<int>(type: "integer", nullable: true),
                    Telegram_ChatId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    Telegram_Body = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: true),
                    Telegram_FileName = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    Telegram_FileId = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: true),
                    Telegram_SendResult = table.Column<string>(type: "text", nullable: true),
                    Telegram_DueTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Telegram_ValidatorUrl = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    Telegram_Status = table.Column<int>(type: "integer", nullable: true),
                    Telegram_RetryCount = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationEvents",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MessageId = table.Column<Guid>(type: "uuid", nullable: true),
                    FCMMessageId = table.Column<string>(type: "character varying(36)", maxLength: 36, nullable: true),
                    UserAgent = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    EventName = table.Column<string>(type: "character varying(64)", maxLength: 64, nullable: true),
                    EventDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "NOW()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Key = table.Column<int>(type: "integer", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false),
                    ValueType = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Key);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VerifyMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    PhoneNumber = table.Column<string>(type: "text", nullable: false),
                    Template = table.Column<int>(type: "integer", nullable: false),
                    SendTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IdInProvider = table.Column<string>(type: "text", nullable: false),
                    IsAudio = table.Column<bool>(type: "boolean", nullable: false),
                    DeliverTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VerifyMessages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tag",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    MessageId = table.Column<Guid>(type: "uuid", nullable: false),
                    Value = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tag_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    NotificationToken = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    App = table.Column<int>(type: "integer", nullable: true),
                    CreateDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "NOW()"),
                    ModifiedDateTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Devices_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Inbox",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    AppName = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inbox", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inbox_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InboxMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: false),
                    Description = table.Column<string>(type: "character varying(1024)", maxLength: 1024, nullable: false),
                    Link = table.Column<string>(type: "character varying(512)", maxLength: 512, nullable: true),
                    Icon = table.Column<string>(type: "character varying(32)", maxLength: 32, nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    DeliveredTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ReadTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ExpiredDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    InboxId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InboxMessages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InboxMessages_Inbox_InboxId",
                        column: x => x.InboxId,
                        principalTable: "Inbox",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Devices_NotificationToken",
                table: "Devices",
                column: "NotificationToken",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Devices_UserId",
                table: "Devices",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Inbox_UserId_AppName",
                table: "Inbox",
                columns: new[] { "UserId", "AppName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InboxMessages_InboxId",
                table: "InboxMessages",
                column: "InboxId");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_MessageId",
                table: "Tag",
                column: "MessageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "InboxMessages");

            migrationBuilder.DropTable(
                name: "NotificationEvents");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "Tag");

            migrationBuilder.DropTable(
                name: "VerifyMessages");

            migrationBuilder.DropTable(
                name: "Inbox");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
