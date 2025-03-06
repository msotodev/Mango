using AutoMapper;

namespace Mango.Services.ProductApi.Helpers
{
	public class MappingConfig
	{
		public static MapperConfiguration RegisterMaps()
		{
			MapperConfiguration mapperConfiguration = new(
				confg =>
				{

				}
			);

			return mapperConfiguration;
		}
	}
}