using CommonLibrary.Dtos;
using EssentialLayers.Helpers.Extension;
using EssentialLayers.Request.Helpers;
using EssentialLayers.Request.Services.Factory;
using Mango.ConsoleApp.Services.Api;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace Mango.ConsoleApp.App
{
	public class MainProduct(
		AuthService authService,
		ProductService productService,
		IFactoryTokenProvider tokenProvider
	)
	{
		public async Task InitAsync()
		{
			HttpResponse<LoginResponseDto> loginResponse = await authService.LoginAsync(
				new LoginRequestDto
				{
					Password = "Admin123_",
					UserName = "MSoto"
				}
			);

			if (loginResponse.Ok && loginResponse.Data.User != null)
			{
				string token = loginResponse.Data.Token;
				int size = 1000;
				int userId = loginResponse.Data.User.Id;

				tokenProvider.SetToken(token);

				await CheckTimeAsync(
					() => ForEachAsync(size, userId), size
				);
			}
		}

		private async Task<HttpResponse<QueryProductResultDto>[]> WhenAllAsync(
			int size, int userId, int maxConcurrency = 100
		)
		{
			using SemaphoreSlim semaphore = new(maxConcurrency);
			List<Task<HttpResponse<QueryProductResultDto>>> tasks = [];

			foreach (int number in Enumerable.Range(0, size))
			{
				await semaphore.WaitAsync();

				tasks.Add(Task.Run(async () =>
				{
					try
					{
						return await NewProductAsync(number, userId);
					}
					finally
					{
						semaphore.Release();
					}
				}));
			}

			return await Task.WhenAll(tasks);
		}

		private async Task<List<HttpResponse<QueryProductResultDto>>> ForEachAsync(
			int size, int userId
		)
		{
			IEnumerable<Task<HttpResponse<QueryProductResultDto>>> tasks = NewProductsAsync(
				2000, size, userId
			);

			List<HttpResponse<QueryProductResultDto>> responses = [];

			foreach (Task<HttpResponse<QueryProductResultDto>> task in tasks)
			{
				HttpResponse<QueryProductResultDto> response = await task;

				responses.Add(response);
			}

			return responses;
		}

		private async Task<List<HttpResponse<QueryProductResultDto>>> ParallelLimitedAsync(
			int size, int userId, int maxConcurrency = 50
		)
		{
			ConcurrentBag<HttpResponse<QueryProductResultDto>> results = [];

			await Parallel.ForEachAsync(
				Enumerable.Range(0, size),
				new ParallelOptions
				{
					MaxDegreeOfParallelism = maxConcurrency
				},
				async (number, _) =>
				{
					HttpResponse<QueryProductResultDto> response = await NewProductAsync(number, userId);

					results.Add(response);
				});

			return [.. results];
		}

		private IEnumerable<Task<HttpResponse<QueryProductResultDto>>> NewProductsAsync(
			int start, int size, int userId
		)
		{
			IEnumerable<Task<HttpResponse<QueryProductResultDto>>> results = Enumerable.Range(start, size).ToList().Select(
				(number) => NewProductAsync(number, userId)
			);

			return results;
		}

		private Task<HttpResponse<QueryProductResultDto>> NewProductAsync(
			int number, int userId
		)
		{
			return productService.NewAsync(
				new NewProductRequestDto
				{
					CategoryId = Random.Shared.Next(1, 3),
					ImageUrl = string.Empty,
					Name = $"Product Name {number + 1}",
					Price = (Random.Shared.NextDouble() * 50).ToDecimal(),
					UserId = userId
				}
			);
		}

		private static async Task<T> CheckTimeAsync<T>(Func<Task<T>> function, int size)
		{
			Stopwatch sw = Stopwatch.StartNew();

			T result = await function();

			sw.Stop();

			Console.WriteLine($"Size: {size}, Time: {sw.ElapsedMilliseconds} ms");

			return result;
		}
	}
}