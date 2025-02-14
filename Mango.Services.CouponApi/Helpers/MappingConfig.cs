using AutoMapper;

namespace Mango.Services.CouponApi.Helpers
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