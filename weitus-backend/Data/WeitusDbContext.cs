﻿#nullable disable

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using weitus_backend.Data.Models;

namespace weitus_backend.Data
{
	public class WeitusDbContext : IdentityUserContext<WeitusUser>
	{
		public DbSet<ChatMessage> ChatMessages { get; set; }

		public DbSet<ChatBot> ChatBots { get; set; }

		public WeitusDbContext(DbContextOptions<WeitusDbContext> options) : base(options) { }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.Entity<WeitusUser>(b =>
			{
				b.HasMany(e => e.ChatMessages)
					.WithOne(e => e.Chatter)
					.HasForeignKey(ut => ut.ChatterId)
					.IsRequired();
			});

			builder.Entity<ChatMessage>(b =>
			{
				b.HasOne(e => e.Chatter)
					.WithMany(e => e.ChatMessages)
					.HasForeignKey(ut => ut.ChatterId)
					.IsRequired();

				b.HasOne(e => e.Bot)
					.WithMany(e => e.ChatMessages)
					.HasForeignKey(ut => ut.BotId)
					.IsRequired(false);
			});

			builder.Entity<ChatBot>(b =>
			{
				b.HasMany(e => e.ChatMessages)
					.WithOne(e => e.Bot)
					.HasForeignKey(ut => ut.BotId)
					.IsRequired(false);
			});

			builder.Entity<ChatMessage>()
				.HasIndex(e => e.TimeStamp);

			builder.Entity<ChatMessage>().ToTable("CHAT_MESSAGES");

			builder.Entity<ChatBot>().ToTable("CHAT_BOTS");

			builder.Entity<WeitusUser>().ToTable("ASP_IDENTITY_USERS");

			builder.Entity<IdentityUserLogin<string>>().ToTable("ASP_IDENTITY_LOGINS");

			builder.Entity<IdentityUserClaim<string>>().ToTable("ASP_IDENTITY_CLAIMS");

			builder.Entity<IdentityUserToken<string>>().ToTable("ASP_IDENTITY_TOKENS");

			// Seed

			builder.Entity<ChatBot>().HasData(new ChatBot { ChatBotId = 1, Name = "Weituś" });
		}
	}
}
