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
using System.Web;

namespace Ministry.State
{
    /// <summary>
    /// Wrapper for Session state
    /// </summary>
    public abstract class WebSessionBase : IStateStorage
    {
        #region | Construction |

        /// <summary>
        /// Initializes a new instance of the <see cref="WebSessionBase"/> class.
        /// </summary>
        /// <param name="webContext">The web context.</param>
        protected WebSessionBase(HttpContextBase webContext)
        {
            Context = webContext;
        }

        #endregion

        /// <summary>
        /// Gets the context.
        /// </summary>
        protected HttpContextBase Context { get; }

        /// <summary>
        /// Clears the session state.
        /// </summary>
        public void Clear() => Context.Session.Clear();

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public object GetValue(string key) => Context.Session[key];

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <typeparam name="T">The type of the object to get.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public T GetValue<T>(string key) 
            => Context.Session[key] == null 
                ? default(T) 
                : (T) Context.Session[key];

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="System.NullReferenceException">The Session element of the context is null.</exception>
        public void SetValue(string key, object value)
        {
            if (Context.Session == null)
                throw new NullReferenceException("The Session element of the context is null.");

            Context.Session[key] = value;
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <typeparam name="T">The type of the value to set.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="System.NullReferenceException">The Session element of the context is null.</exception>
        public void SetValue<T>(string key, T value)
        {
            if (Context.Session == null)
                throw new NullReferenceException("The Session element of the context is null.");

            Context.Session[key] = value;
        }
    }
}