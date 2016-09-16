using RepositoryLogic.BaseEntity;
using RepositoryLogic.Repository;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using static RepositoryLogic.BaseEntity.Entity;

namespace RepositoryLogic.BaseLogic
{
    public interface ILogic<E> where E : Entity
    {
        int? byUserId { get; set; }
        CommonResponse Add(E entity);
        CommonResponse GetAll();
        CommonResponse GetByID(int ID);
        CommonResponse Remove(int id);
        CommonResponse Activate(int id);
        CommonResponse Update(E entity);
        CommonResponse AddToParent<ParentType>(int parentID, E entity) where ParentType : Entity;
        CommonResponse GetAllByParent<ParentType>(int parentID) where ParentType : Entity;
        CommonResponse RemoveFromParent<Parent>(int parentID, E entity) where Parent : Entity;
        CommonResponse CreateInstance();
        CommonResponse GetAvailableFor<ForEntity>(int id) where ForEntity : Entity;
        CommonResponse GetCatalogs();
    }

    public abstract class Logic<E> : ILogic<E> where E : Entity, new()
    {
        public int? byUserId { get; set; }

        protected DbContext context;
        protected IRepository<E> repository;

        public Logic(DbContext context, IRepository<E> repository)//, int? byUserId)
        {
            this.context = context;
            this.repository = repository;
            //this.byUserId = byUserId;
        }

        protected abstract void loadNavigationProperties(DbContext context, IList<E> entities);

        protected static EntityState GetEntityState(EF_EntityState state)
        {
            switch (state)
            {
                case EF_EntityState.Unchanged:
                    return EntityState.Unchanged;
                case EF_EntityState.Added:
                    return EntityState.Added;
                case EF_EntityState.Modified:
                    return EntityState.Modified;
                case EF_EntityState.Deleted:
                    return EntityState.Deleted;
                default:
                    return EntityState.Detached;
            }
        }

        protected virtual void onSaving(DbContext context, E entity, int? parentId = null) { }
        protected virtual void onCreate(E entity) { }

        public virtual CommonResponse Add(E entity)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        //var repository = RepositoryFactory.Create<Entity>(context, byUserId);

                        repository.byUserId = byUserId;
                        repository.Add(entity);
                        onSaving(context, entity);

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        return response.Error(ex.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }

            return response.Success(entity);
        }

        public virtual CommonResponse GetAll()
        {
            CommonResponse response = new CommonResponse();
            IList<E> entities;
            try
            {
                //var repository = RepositoryFactory.Create<Entity>(context, byUserId);

                repository.byUserId = byUserId;
                entities = repository.GetAll();

                //loadNavigationProperties(context, entities);
            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }

            return response.Success(entities);
        }

        public virtual CommonResponse GetByID(int ID)
        {
            CommonResponse response = new CommonResponse();
            List<E> entities = new List<E>();
            try
            {

                //var repository = RepositoryFactory.Create<Entity>(context, byUserId);

                E entity = repository.GetByID(ID);
                if (entity != null)
                {
                    repository.byUserId = byUserId;
                    entities.Add(entity);
                    loadNavigationProperties(context, entities);
                    return response.Success(entity);
                }
                else
                {
                    return response.Error("Entity not found.");
                }
            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }
        }

        public virtual CommonResponse Remove(int id)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        //var repository = RepositoryFactory.Create<Entity>(context, byUserId);
                        repository.byUserId = byUserId;
                        repository.Delete(id);

                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return response.Error("ERROR: " + e.ToString());
                    }
                }

            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }
            return response.Success(id, repository.EntityName + " removed successfully.");
        }

        public virtual CommonResponse Activate(int id)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        repository.byUserId = byUserId;
                        repository.Activate(id);

                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return response.Error("ERROR: " + e.ToString());
                    }
                }

            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }
            return response.Success(id);
        }

        public virtual CommonResponse Update(E entity)
        {
            CommonResponse response = new CommonResponse();
            try
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        //var repository = RepositoryFactory.Create<Entity>(context, byUserId);

                        repository.byUserId = byUserId;
                        repository.Update(entity);
                        onSaving(context, entity);

                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return response.Error("ERROR: " + e.ToString());
                    }
                }

            }
            catch (Exception e)
            {

                return response.Error("ERROR: " + e.ToString());
            }

            return response.Success(entity);
        }

        public virtual CommonResponse AddToParent<ParentType>(int parentID, E entity) where ParentType : Entity
        {
            CommonResponse response = new CommonResponse();
            try
            {

                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        //var repository = RepositoryFactory.Create<Entity>(context, byUserId);

                        //var parentRepoType = typeof(BaseEntityRepository<>);
                        //Type[] parentRepositoryArgs = { typeof(ParentType) };
                        //var makeme = parentRepoType.MakeGenericType(parentRepositoryArgs);
                        //object parentRepository = Activator.CreateInstance(makeme);

                        //PropertyInfo propContext = parentRepository.GetType().GetProperty("context", BindingFlags.Public | BindingFlags.Instance);
                        //propContext.SetValue(parentRepository, context);

                        //PropertyInfo propByUser = parentRepository.GetType().GetProperty("byUserID", BindingFlags.Public | BindingFlags.Instance);
                        //propByUser.SetValue(parentRepository, byUserID);

                        //MethodInfo method = parentRepository.GetType().GetMethod("GetByID");
                        //BaseEntity parent = (Entity)method.Invoke(parentRepository, new object[] { parentID });
                        //if (parent == null)
                        //{
                        //    return response.Error("Non-existent Parent Entity.");
                        //}

                        repository.byUserId = byUserId;
                        repository.AddToParent<ParentType>(parentID, entity);
                        onSaving(context, entity, parentID);

                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return response.Error("ERROR: " + e.ToString());
                    }
                }

            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }

            return response.Success(entity);
        }

        public virtual CommonResponse GetAllByParent<ParentType>(int parentID) where ParentType : Entity
        {
            CommonResponse response = new CommonResponse();
            IList<E> entities;

            try
            {

                //var repository = RepositoryFactory.Create<Entity>(context, byUserId);

                repository.byUserId = byUserId;
                entities = repository.GetListByParent<ParentType>(parentID);
                loadNavigationProperties(context, entities);
                //MethodInfo method = repository.GetType().GetMethod("GetListByParent");
                //MethodInfo genericMethod = method.MakeGenericMethod(new Type[] { typeof(ParentType) });
                //entities = (IList<Entity>) genericMethod.Invoke(repository, new object[] { parentID });

            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }

            return response.Success(entities);
        }

        public virtual CommonResponse GetAvailableFor<ForEntity>(int id) where ForEntity : Entity
        {
            CommonResponse response = new CommonResponse();
            IEnumerable<E> availableEntities;
            try
            {
                repository.byUserId = byUserId;

                IRepository<ForEntity> oRepository = new Repository<ForEntity>(context);
                oRepository.byUserId = byUserId;


                ForEntity forEntity = oRepository.GetByID(id);
                if (forEntity == null)
                {
                    throw new Exception("Entity " + forEntity.AAA_EntityName + " not found.");
                }

                IList<E> childrenInForEntity = repository.GetListByParent<ForEntity>(id);

                IList<E> allEntities = repository.GetAll();

                availableEntities = allEntities.Where(e => !childrenInForEntity.Any(o => o.id == e.id));

                loadNavigationProperties(context, availableEntities.ToList());
            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }

            return response.Success(availableEntities);
        }

        public virtual CommonResponse RemoveFromParent<Parent>(int parentID, E entity) where Parent : Entity
        {
            CommonResponse response = new CommonResponse();
            try
            {
                using (var transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        repository.byUserId = byUserId;
                        repository.RemoveFromParent<Parent>(parentID, entity);

                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        return response.Error("ERROR: " + e.ToString());
                    }
                }
            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }

            return response.Success();
        }

        public CommonResponse CreateInstance()
        {
            CommonResponse response = new CommonResponse();
            E entity = new E();
            onCreate(entity);
            return response.Success(entity);
        }

        protected virtual ICatalogContainer LoadCatalogs() { return null; }

        public virtual CommonResponse GetCatalogs()
        {
            CommonResponse response = new CommonResponse();
            try
            {
                response.Success(LoadCatalogs());
            }
            catch (Exception e)
            {
                return response.Error("ERROR: " + e.ToString());
            }
            return response.Success();
        }
    }
}
