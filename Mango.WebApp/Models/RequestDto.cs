using static Mango.WebApp.Types.ApiTypes;

namespace Mango.WebApp.Models
{
	public class RequestDto<T>
	{
		public ApiType ApiType { get; set; } = ApiType.GET;

		public string Url { get; set; } = string.Empty;

		public T Data { get; set; } = default!;

		public string Token { get; set; } = string.Empty;
	}
}