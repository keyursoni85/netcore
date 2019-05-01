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

namespace Application.Framework.Caching
{
	public static class CacheManagerExtensions
	{
		public static T Get<T>(this ICacheManager cacheManager, string key, Func<T> acquire)
		{
			return Get(cacheManager, key, 60, acquire);
		}

        public static T Get<T>(this ICacheManager cacheManager, string key, int cacheTime, Func<T> acquire)
		{
			if (cacheTime <= 0)
				return acquire();

            var data = cacheManager.Get<T>(key);

            if (data != null)
                return data;

			var result = acquire();

			cacheManager.Set(key, result, cacheTime);

			return result;
		}

        public static async Task<T> GetAsync<T>(this ICacheManager cacheManager, string key, Func<Task<T>> acquire)
        {
            return await GetAsync(cacheManager, key, 60, acquire);
        }

        public static async Task<T> GetAsync<T>(this ICacheManager cacheManager, string key, int cacheTime, Func<Task<T>> acquire)
        {
            if (cacheTime <= 0)
                return await acquire();

            var data = await cacheManager.GetAsync<T>(key);

            if (data != null)
                return data;

            var result = await acquire();

            cacheManager.Set(key, result, cacheTime);

            return result;
        }
    }
}