using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Diagnostics.CodeAnalysis;

namespace Ministry.State
{
    /// <summary>
    /// Wrapper for Session state
    /// </summary>
    /// <inheritdoc cref="IStateStorage"/>
    [SuppressMessage("ReSharper", "UnusedType.Global", Justification = "Library")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "Library")]
    public abstract class WebSessionBase : IStateStorage
    {
        #region | Construction |

        /// <summary>
        /// Initializes a new instance of the <see cref="WebSessionBase" /> class.
        /// </summary>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        protected WebSessionBase(IHttpContextAccessor httpContextAccessor)
        {
            InnerSession = httpContextAccessor.HttpContext?.Session;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebSessionBase" /> class.
        /// </summary>
        /// <param name="innerSession">The inner session.</param>
        protected WebSessionBase(ISession innerSession)
        {
            InnerSession = innerSession;
        }

        #endregion

        /// <summary>
        /// Gets the inner session.
        /// </summary>
        protected ISession InnerSession { get; }

        /// <inheritdoc/>
        public void Clear() => InnerSession.Clear();

        /// <inheritdoc/>
        public object GetValue(string key)
        {
            var item = InnerSession.GetString(key);
            return item == null
                ? null
                : JsonConvert.DeserializeObject(item);
        }

        /// <inheritdoc/>
        public T GetValue<T>(string key)
        {
            var item = InnerSession.GetString(key);
            return item == null
                ? default(T)
                : JsonConvert.DeserializeObject<T>(item);
        }

        /// <inheritdoc/>
        public void Remove(string key) => InnerSession.Remove(key);

        /// <inheritdoc/>
        public void SetValue(string key, object value)
            => InnerSession.SetString(key, JsonConvert.SerializeObject(value));

        /// <inheritdoc/>
        public void SetValue<T>(string key, T value)
            => InnerSession.SetString(key, JsonConvert.SerializeObject(value));
    }
}