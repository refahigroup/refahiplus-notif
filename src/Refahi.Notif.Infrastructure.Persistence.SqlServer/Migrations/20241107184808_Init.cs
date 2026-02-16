using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Refahi.Notif.Infrastructure.Persistence.SqlServer.Migrations
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
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: true),
                    Owner = table.Column<long>(type: "bigint", nullable: true),
                    AppName = table.Column<int>(type: "int", nullable: true),
                    DueTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ValidatorUrl = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    JobId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sms_PhoneNumbers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sms_Body = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sms_Status = table.Column<int>(type: "int", nullable: true),
                    Sms_SendTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Sms_DeliverTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Sms_RetryCount = table.Column<int>(type: "int", nullable: true),
                    Sms_IdInProvider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sms_Sender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sms_Gateway = table.Column<int>(type: "int", nullable: true),
                    Sms_DueTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Sms_ValidatorUrl = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Email_Addresses = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email_Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email_Body = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email_IsHtml = table.Column<bool>(type: "bit", nullable: true),
                    Email_RetryCount = table.Column<int>(type: "int", nullable: true),
                    Email_SendTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Email_Status = table.Column<int>(type: "int", nullable: true),
                    Email_DueTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PushNotification_Subject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PushNotification_Body = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PushNotification_Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PushNotification_Data = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PushNotification_Status = table.Column<int>(type: "int", nullable: true),
                    PushNotification_Jobs = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PushNotification_DueTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PushNotification_ValidatorUrl = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Notification_Subject = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Notification_Body = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Notification_Link = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Notification_Icon = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    Notification_DueTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Notification_ExpiredDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Notification_ValidatorUrl = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Notification_Status = table.Column<int>(type: "int", nullable: true),
                    Telegram_ChatId = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Telegram_Body = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: true),
                    Telegram_FileName = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Telegram_FileId = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    Telegram_SendResult = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Telegram_DueTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Telegram_ValidatorUrl = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Telegram_Status = table.Column<int>(type: "int", nullable: true),
                    Telegram_RetryCount = table.Column<int>(type: "int", nullable: true)
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
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FCMMessageId = table.Column<string>(type: "nvarchar(36)", maxLength: 36, nullable: true),
                    UserAgent = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EventName = table.Column<string>(type: "nvarchar(64)", maxLength: 64, nullable: true),
                    EventDateTime = table.Column<DateTime>(type: "DateTime", nullable: true, defaultValueSql: "GetDate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Key = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ValueType = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "VerifyMessages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Template = table.Column<int>(type: "int", nullable: false),
                    SendTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IdInProvider = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsAudio = table.Column<bool>(type: "bit", nullable: false),
                    DeliverTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
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
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false)
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
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NotificationToken = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    App = table.Column<int>(type: "int", nullable: true),
                    CreateDateTime = table.Column<DateTime>(type: "DateTime", nullable: false, defaultValueSql: "GetDate()"),
                    ModifiedDateTime = table.Column<DateTime>(type: "DateTime", nullable: true)
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
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    AppName = table.Column<int>(type: "int", nullable: false)
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
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1024)", maxLength: 1024, nullable: false),
                    Link = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    Icon = table.Column<string>(type: "nvarchar(32)", maxLength: 32, nullable: true),
                    CreatedTime = table.Column<DateTime>(type: "DateTime", nullable: true),
                    DeliveredTime = table.Column<DateTime>(type: "DateTime", nullable: true),
                    ReadTime = table.Column<DateTime>(type: "DateTime", nullable: true),
                    ExpiredDate = table.Column<DateTime>(type: "DateTime", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    InboxId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
