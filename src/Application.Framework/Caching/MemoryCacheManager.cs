/*
Application - The Multi server multi operating system control panel © copyright 2018.

Application is free software; you can redistribute it and/or modify it under the terms of the GNU Lesser General Public License 3.0 as published by the Free Software Foundation.

This file is part of the Application project and is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the GNU Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public License along with this file; if not, please contact Application at https://www.Application.com/contact

https://www.gnu.org/licenses/lgpl-3.0.txt
*/

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace Application.Framework.Caching
{
    public class MemoryCacheManager : ICacheManager
	{
	    private readonly IMemoryCache _memoryCache;

        public MemoryCacheManager(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

	    public T Get<T>(string key)
		{
            return (T)_memoryCache.Get(key);
		}

        public async Task<T> GetAsync<T>(string key)
        {
            return (T)_memoryCache.Get(key);
        }

        public void Set(string key, object data, int cacheTime)
		{
			if (data == null)
				return;

            if (IsSet(key))
                return;

            var memoryCacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromSeconds(cacheTime));

            _memoryCache.Set(key, data, memoryCacheEntryOptions);
		}

        public bool IsSet(string key)
        {
            return _memoryCache.Get(key) != null;
        }

        public void Remove(string key)
		{
            _memoryCache.Remove(key);
		}

        public async Task RemoveAsync(string key)
        {
            _memoryCache.Remove(key);
        }

        public void RemoveByPattern(string pattern)
		{
            throw new NotImplementedException();
        }

		public void Clear()
		{
            throw new NotImplementedException();
        }
    }
}