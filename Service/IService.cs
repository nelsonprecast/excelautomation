using Core;

namespace Service
{
    public interface IService<TEntity> where TEntity : BaseEntity
    {
        #region Methods
        /// <summary>
        /// Get All records
        /// </summary>
        /// <returns></returns>
        ICollection<TEntity> Get();
        /// <summary>
        /// Get By Id
        /// </summary>
        /// <param name="id">Primary Key</param>
        /// <returns></returns>
        TEntity Get(int id);

        /// <summary>
        /// Get By Id
        /// </summary>
        /// <param name="id">Primary Key</param>
        /// <returns></returns>
        ICollection<TEntity> Get(int[] id);

        /// <summary>
        /// Insert record
        /// </summary>
        /// <param name="entity">Entity</param>
        void Create(TEntity entity);
        /// <summary>
        /// Insert bulk records
        /// </summary>
        /// <param name="entities">Entities</param>
        void Create(ICollection<TEntity> entities);
        /// <summary>
        /// Update record
        /// </summary>
        /// <param name="entity">Entity</param>
        bool Update(TEntity entity);
        /// <summary>
        /// Update Bulk records
        /// </summary>
        /// <param name="entities">Entities</param>
        bool Update(ICollection<TEntity> entities);
        /// <summary>
        /// Delete record
        /// </summary>
        /// <param name="entity">Entity</param>
        void Remove(TEntity entity);
        /// <summary>
        /// Delete Bulk records
        /// </summary>
        /// <param name="entities">Entities</param>
        void Remove(List<TEntity> entities);
        /// <summary>
        /// Check if entity exist
        /// </summary>
        /// <param name="id">entity id</param>
        /// <returns></returns>
        bool Exist(int id);

        #endregion

    }
}
