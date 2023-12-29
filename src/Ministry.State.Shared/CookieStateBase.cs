using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Ministry.State
{
    /// <summary>
    /// Wrapper for a state storage mechanism that uses persistent cookies.
    /// </summary>
    /// <inheritdoc cref="IStateStorage"/>
    [SuppressMessage("ReSharper", "UnusedType.Global", Justification = "Library")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "Library")]
    public abstract class CookieStateBase : IStateStorage
    {
        private readonly DateTime? persistenceDate;

        #region | Construction |

        /// <summary>
        /// Initializes a new instance of the <see cref="CookieStateBase" /> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="persistenceDate">The persistence date.</param>
        [SuppressMessage("ReSharper", "UnusedMember.Global")]
        protected CookieStateBase(IHttpContextAccessor httpContextAccessor, DateTime? persistenceDate = null)
        {
            Context = httpContextAccessor.HttpContext;
            this.persistenceDate = persistenceDate;
        }

        #endregion

        /// <summary>
        /// Gets the context.
        /// </summary>
        protected HttpContext Context { get; }

        /// <inheritdoc/>
        public void Clear()
        {
            foreach (var cookie in Context.Request.Cookies.Keys)
                Context.Response.Cookies.Delete(cookie);
        }

        /// <inheritdoc/>
        public object GetValue(string key)
        {
            var item = Context.Request.Cookies[key];
            return item == null
                ? null
                : JsonConvert.DeserializeObject(item);
        }

        /// <inheritdoc/>
        public T GetValue<T>(string key)
        {
            var item = Context.Request.Cookies[key];
            return item == null
                ? default(T)
                : JsonConvert.DeserializeObject<T>(item);
        }

        /// <inheritdoc/>
        public void Remove(string key)
        {
            if (Context.Request.Cookies[key] != null)
                Context.Response.Cookies.Delete(key);
        }

        /// <inheritdoc/>
        public void SetValue(string key, object value) => SetCookie(key, value);

        /// <inheritdoc/>
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
                #pragma warning disable CA2201
                throw new NullReferenceException("The Cookies element of the context is null.");
                #pragma warning restore CA2201

            var existingCookie = Context.Request.Cookies[key];

            if (existingCookie != null)
            {
                if (persistenceDate.HasValue)
                {
                    Context.Response.Cookies.Delete(key);
                    Context.Response.Cookies.Append(key, JsonConvert.SerializeObject(value),
                        new CookieOptions { Expires = persistenceDate.Value });
                }
            }
            else
            {
                if (persistenceDate.HasValue)
                    Context.Response.Cookies.Append(key, JsonConvert.SerializeObject(value), 
                        new CookieOptions { Expires = persistenceDate.Value });
                else
                    Context.Response.Cookies.Append(key, JsonConvert.SerializeObject(value));
            }
        }

        #endregion
    }
}