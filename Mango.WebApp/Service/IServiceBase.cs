using EssentialLayers.Helpers.Result;
using Mango.WebApp.Models;

namespace Mango.WebApp.Service
{
	public interface IServiceBase
	{
		Task<ResultHelper<TResult>> Send<TResult, TRequest>(
			RequestDto<TRequest> request
		);
	}
}
