using CommonLibrary.Dtos;
using Domain.Entities;
using Mango.Services.CustomerApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Reflection;

namespace Mango.Services.CustomerApi.Repositories
{
	public abstract class BaseRepository<Entity>(
		AppDbContext appDbContext
	) where Entity : BaseEntity
	{
		protected DbSet<Entity> DbSet => appDbContext.Set<Entity>();

		public async Task<Entity> NewAsync(Entity entity)
		{
			EntityEntry<Entity> result = await DbSet.AddAsync(entity);

			await SaveChangesAsync();

			return result.Entity;
		}

		public async Task<Entity?> GetByIdAsync(int id)
		{
			Entity? entity = entity = await DbSet.FirstOrDefaultAsync(e => e.Id == id);

			return entity;
		}

		public async Task<Entity?> GetByIdAsync<TProperty>(
			int id, Expression<Func<Entity, TProperty>> include
		)
		{
			Entity? entity = await DbSet.Include(include).FirstOrDefaultAsync(e => e.Id == id);

			return entity;
		}

		public IQueryable<Entity> All => DbSet;

		public IQueryable<Entity> AllInclude<TProperty>(
			Expression<Func<Entity, TProperty>> include
		) => DbSet.Include(include);

		public virtual async Task<Entity> UpdateAsync<Dto>(Dto dto) where Dto : UpdateBaseDto
		{
			if (dto == null) throw new NullReferenceException($"The dto parameter is null");

			PropertyInfo? propertyInfo = dto.GetType().GetProperties().FirstOrDefault(
				p => Attribute.IsDefined(p, typeof(KeyAttribute))
			) ?? throw new Exception($"The {dto.GetType().Name} doesn't contain an 'Id' parameter");

			int id = (int)propertyInfo.GetValue(dto, null)!;

			Entity? entity = await DbSet.FirstOrDefaultAsync(x => x.Id == id);

			if (entity != null)
			{
				appDbContext?.Entry(entity).CurrentValues.SetValues(dto);

				await SaveChangesAsync();
			}

			return entity!;
		}

		public virtual async Task<bool> Remove(int id, bool softDelete = false)
		{
			Entity? entity = await GetByIdAsync(id);

			if (entity == null) return false;

			if (softDelete)
			{
				entity.Deleted = true;
				entity.DeletedAt = DateTime.UtcNow;

				DbSet.Update(entity);
			}
			else DbSet.Remove(entity);

			await SaveChangesAsync();

			return true;
		}

		private async Task<int> SaveChangesAsync()
		{
			return await appDbContext.SaveChangesAsync();
		}
	}
}