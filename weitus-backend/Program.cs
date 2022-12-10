using Microsoft.EntityFrameworkCore;
using weitus_backend.Data;
using weitus_backend.Data.Models;

namespace weitus_backend
{
	public class Program
	{
		public static void Main(string[] args)
		{
			CreateHostBuilder(args).Build().Run();
		}

		public static IHostBuilder CreateHostBuilder(string[] args)
		{
			return Host.CreateDefaultBuilder(args).ConfigureWebHostDefaults(webBuilder =>
			{
				webBuilder.UseStartup<Startup>();
			});
		}
	}
}