using bookStore.Infrastructure.Formatters;

namespace bookStore.Infrastructure.Extensions
{
	public static class IMvcBuilderExtensions
	{
		public static IMvcBuilder AddCustomCsvFormatter(this IMvcBuilder builder) =>
			builder.AddMvcOptions(options =>
			{
				options.OutputFormatters.Add(new CsvOutputFormatter());
			});
	}
}
