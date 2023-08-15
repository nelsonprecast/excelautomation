using Core;
using Core.Exception;
using Infrastructure.Data.Interfaces;

namespace Service
{
    public class BaseService<TEntity> : IService<TEntity> where TEntity : BaseEntity
    {
        #region Fields
        public readonly IRepository<TEntity> Repository;
        #endregion

        #region ctor
        public BaseService(IRepository<TEntity> repository)
        {
            Repository = repository;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Insert record
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Create(TEntity entity)
        {
            Repository.Insert(entity);
        }
        /// <summary>
        /// Insert bulk records
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void Create(ICollection<TEntity> entities)
        {
            Repository.Insert(entities);
        }
        /// <summary>
        /// Get All records
        /// </summary>
        /// <returns></returns>
        public virtual ICollection<TEntity> Get()
        {
            return Repository.Table.ToList();
        }
        /// <summary>
        /// Get By Id
        /// </summary>
        /// <param name="id">Primary Key</param>
        /// <returns></returns>
        public virtual TEntity Get(int id)
        {
            if (id <= 0)
                throw new ValidationException($"Invalid Value of Id : {id}");

            return Repository.GetById(id);
        }

        public virtual ICollection<TEntity> Get(int[] id)
        {
            if (id == null)
                throw new ValidationException("Ids are null.");

            var response = Repository.Table.Where(x => id.Any(y => y == x.Id) ).ToList();

            return response;
        }

        /// <summary>
        /// Delete record
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Remove(TEntity entity)
        {
            Repository.SoftDelete(entity);
        }
        /// <summary>
        /// Delete Bulk records
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void Remove(List<TEntity> entities)
        {
            if (entities.Any())
                Repository.SoftDelete(entities.ToList());
        }
        /// <summary>
        /// Update record
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual bool Update(TEntity entity)
        {
            return Repository.Update(entity);
        }
        /// <summary>
        /// Update Bulk records
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual bool Update(ICollection<TEntity> entities)
        {
            return Repository.Update(entities);
        }

        /// <summary>
        /// Check if entity exist
        /// </summary>
        /// <param name="id">entity id</param>
        /// <returns></returns>
        public bool Exist(int id)
        {
            return Repository.Table.Any(x => x.Id == id);
        }
        #endregion
    }
}
