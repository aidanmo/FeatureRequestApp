using System;
namespace FeatureRequestAppUI
{
	public static class RegisterServices
	{
		public static void ConfigureServices(this WebApplicationBuilder builder)
		{
			// Creating extension method instead of repeating code in program.cs
			// for each dependecy injection.
			builder.Services.AddRazorPages();
			builder.Services.AddServerSideBlazor();
			builder.Services.AddMemoryCache();

			//Refrences used for our blazor project in order to get the data.
			builder.Services.AddSingleton<IDbConnection, DbConnection>();
			builder.Services.AddSingleton<ICategoryData, MongoCategoreyData>();
			builder.Services.AddSingleton<IStatusData, MongoStatusData>();
			builder.Services.AddSingleton<IUserData, MongoUserData>();
        }
	}
}

