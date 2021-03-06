using System;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using stonefw.Utility.EntityExpressions;
using stonefw.Dao.BaseModule;
using stonefw.Entity.BaseModule;

namespace stonefw.Biz.BaseModule
{
    public class BcLogErrorBiz
    {
        private BcLogErrorDao _dao;
        private BcLogErrorDao Dao
        {
            get { return _dao ?? (_dao = new BcLogErrorDao()); }
        }
        public List<BcLogErrorEntity> GetBcLogErrorList()
        { return EntityExecution.ReadEntityList2<BcLogErrorEntity>(null).OrderByDescending(n => n.OpTime).ToList(); }
        public void DeleteBcLogError(int id)
        {
            BcLogErrorEntity entity = new BcLogErrorEntity() { Id = id };
            EntityExecution.DeleteEntity(entity);
        }
        public void AddNewBcLogError(BcLogErrorEntity entity)
        {
            entity.Id = null;
            EntityExecution.InsertEntity(entity);
        }
        public void UpdateBcLogError(BcLogErrorEntity entity) { EntityExecution.UpdateEntity(entity); }
        public BcLogErrorEntity GetSingleBcLogError(int id) { return EntityExecution.ReadEntity2<BcLogErrorEntity>(n => n.Id == id); }
    }
}
