﻿using System.Collections.Generic;
using System.Linq;
using Thinktecture.IdentityServer3.Admin.Models.Persistence;
using Thinktecture.IdentityServer3.Admin.Models.Storage;
using Thinktecture.IdentityServer3.Admin.Persistence;
using Thinktecture.IdentityServer3.Admin.Persistence.Models;

namespace Thinktecture.IdentityServer3.Admin.Storage
{
    /// <summary>
    /// Provides basic storage capabilities for in memory scope storage
    /// </summary>
    public class InMemoryScopeStorage : IPersistence<Scope>
    {
        private readonly IList<Scope> _scopes = new List<Scope>();
        private int _internalScopeCount = 1;

        public PageResult<Scope> List(PagingInformation pagingInformation)
        {
            return new PageResult<Scope>()
            {
                Items = _scopes.Skip(pagingInformation.Skip).Take(pagingInformation.Take).ToList(),
                TotalCount = _scopes.Count
            };
        }

        public Scope Get(int key)
        {
            return _scopes.SingleOrDefault(c => c.Id == key);
        }

        public void Delete(int key)
        {
            _scopes.Remove(_scopes.SingleOrDefault(c => c.Id == key));
        }

        public object Add(Scope entity)
        {
            entity.Id = _internalScopeCount++;

            _scopes.Add(entity);

            return entity.Id;
        }

        public void Update(Scope entity)
        {
            var oldId = entity.Id;
            Delete(entity.Id);
            Add(entity);

            _scopes.Last().Id = oldId;
        }

        public int TotalCount()
        {
            return _scopes.Count;
        }

        public bool IsNameAvailable(Scope entity)
        {
            return !_scopes.Any(s => s.Name == entity.Name && s.Id != entity.Id);
        }
    }
}