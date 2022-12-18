#nullable disable

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using weitus_backend.Data.Models;

namespace weitus_backend.Data
{
	public class WeitusDbContext : IdentityUserContext<WeitusUser>
	{
		public DbSet<ChatMessage> ChatMessages { get; set; }

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
		}
	}
}
