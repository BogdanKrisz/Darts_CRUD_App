﻿using EUDBLD_HFT_2023242.Models;
using EUDBLD_HFT_2023242.Repository.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EUDBLD_HFT_2023242.Repository
{
    public class Repository<T> :  IRepository<T> where T : Entity
    {
        protected DartsDbContext ctx;

        public Repository(DartsDbContext ctx)
        {
            this.ctx = ctx ?? throw  new ArgumentNullException(nameof(ctx));
        }

        public void Create(T item)
        {
            ctx.Set<T>().Add(item);
            ctx.SaveChanges();
        }

        public void Delete(int id)
        {
            ctx.Set<T>().Remove(Read(id));
            ctx.SaveChanges();
        }

        public virtual T Read(int id)
        {
            return ctx.Set<T>().FirstOrDefault(item => item.Id == id);
        }

        public IQueryable<T> ReadAll()
        {
            return ctx.Set<T>();
        }

        public virtual void Update(T item)
        {
            var old = Read(item.Id);
            foreach (var prop in old.GetType().GetProperties())
            {
                if (prop.GetAccessors().FirstOrDefault(t => t.IsVirtual) == null)
                    prop.SetValue(old, prop.GetValue(item));
            }
            ctx.SaveChanges();
        }
    }
}
