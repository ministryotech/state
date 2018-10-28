// Copyright (c) 2018 Minotech Ltd.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files
// (the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge,
// publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do
// so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE
// FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Ministry.State
{
    /// <summary>
    /// Wrapper for a state storage mechanism that uses persistent cookies.
    /// </summary>
    public abstract class CookieStateBase : IStateStorage
    {
        private readonly DateTime? persistanceDate;

        #region | Construction |

        /// <summary>
        /// Initializes a new instance of the <see cref="CookieStateBase" /> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="persistanceDate">The persistance date.</param>
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        protected CookieStateBase(IHttpContextAccessor httpContextAccessor, DateTime? persistanceDate = null)
        {
            Context = httpContextAccessor.HttpContext;
            this.persistanceDate = persistanceDate;
        }

        #endregion

        /// <summary>
        /// Gets the context.
        /// </summary>
        protected HttpContext Context { get; }

        /// <summary>
        /// Clears the state.
        /// </summary>
        public void Clear()
        {
            foreach (var cookie in Context.Request.Cookies.Keys)
                Context.Response.Cookies.Delete(cookie);
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public object GetValue(string key)
        {
            var item = Context.Request.Cookies[key];
            return item == null
                ? null
                : JsonConvert.DeserializeObject(item);
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <typeparam name="T">The type of the object to get.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public T GetValue<T>(string key)
        {
            var item = Context.Request.Cookies[key];
            return item == null
                ? default(T)
                : JsonConvert.DeserializeObject<T>(item);
        }

        /// <summary>
        /// Removes the specified iten from state.
        /// </summary>
        /// <param name="key">The key.</param>
        public void Remove(string key)
        {
            if (Context.Request.Cookies[key] != null)
                Context.Response.Cookies.Delete(key);
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="NullReferenceException">The Session element of the context is null.</exception>
        public void SetValue(string key, object value) => SetCookie(key, value);

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <typeparam name="T">The type of the value to set.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="NullReferenceException">The Session element of the context is null.</exception>
        public void SetValue<T>(string key, T value) => SetCookie(key, value);

        #region | Private Methods |

        /// <summary>
        /// Sets the cookie.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="NullReferenceException">The Cookies element of the context is null.</exception>
        private void SetCookie(string key, object value)
        {
            if (Context.Request.Cookies == null)
                throw new NullReferenceException("The Cookies element of the context is null.");

            var existingCookie = Context.Request.Cookies[key];

            if (existingCookie != null)
            {
                if (persistanceDate.HasValue)
                {
                    Context.Response.Cookies.Delete(key);
                    Context.Response.Cookies.Append(key, JsonConvert.SerializeObject(value),
                        new CookieOptions { Expires = persistanceDate.Value });
                }
            }
            else
            {
                if (persistanceDate.HasValue)
                    Context.Response.Cookies.Append(key, JsonConvert.SerializeObject(value), 
                        new CookieOptions { Expires = persistanceDate.Value });
                else
                    Context.Response.Cookies.Append(key, JsonConvert.SerializeObject(value));
             }
        }

        #endregion
    }
}