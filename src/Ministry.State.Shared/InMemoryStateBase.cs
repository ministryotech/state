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

using System.Collections.Generic;
using System.Linq;

// TODO: Move to Ministry.State Libraries (One Solution)
// MInistry.State.Abstractions
// Ministry.State.WebSession
// Ministry.State.Cookies

// TODO: This should become 'InMemoryState' instead of Fake with docs to indicate you can use it as fakes
// NOTE: The Two Fake State classes are EXACTLY the same!

// ReSharper disable once CheckNamespace
namespace Ministry.State
{
    /// <summary>
    /// Fake WebSession implementation that stores values in memory.
    /// </summary>
    /// <remarks>
    /// This is a useful swap out for a concrete web session. You can inherit a custom version for session storage checking if needed.
    /// </remarks>
    public class FakeWebSession : IStateStorage
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FakeWebSession"/> class.
        /// </summary>
        public FakeWebSession()
        {
            InMemorySession = new List<FakeSessionItem>();
        }

        /// <summary>
        /// Gets the in memory session.
        /// </summary>
        // ReSharper disable once MemberCanBePrivate.Global
        protected List<FakeSessionItem> InMemorySession { get; private set; }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public object GetValue(string key)
        {
            if (InMemorySession.All(o => o.Key != key)) return null;
            var item = InMemorySession.FirstOrDefault(o => o.Key == key);

            return item == null ? null : item.Value;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <typeparam name="T">The type of the object to get.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public T GetValue<T>(string key)
        {
            var item = InMemorySession.FirstOrDefault(o => o.Key == key);

            return item == null ? default(T) : (T)item.Value;
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void SetValue(string key, object value)
        {
            if (InMemorySession.All(o => o.Key != key))
            {
                InMemorySession.Add(new FakeSessionItem(key, value));
            }
            else
            {
                InMemorySession.First(o => o.Key == key).Value = value;
            }
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <typeparam name="T">The type of the value to set.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void SetValue<T>(string key, T value)
        {
            if (InMemorySession.All(o => o.Key != key))
            {
                InMemorySession.Add(new FakeSessionItem(key, value));
            }
            else
            {
                InMemorySession.First(o => o.Key == key).Value = value;
            }
        }

        public void Clear() => InMemorySession.Clear();

        #region | Nested Classes |

        /// <summary>
        /// A Fake Session Item
        /// </summary>
        protected class FakeSessionItem
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="FakeSessionItem"/> class.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <param name="value">The value.</param>
            public FakeSessionItem(string key, object value)
            {
                Key = key;
                Value = value;
            }

            /// <summary>
            /// Gets or sets the key.
            /// </summary>
            /// <value>
            /// The key.
            /// </value>
            public string Key { get; private set; }

            /// <summary>
            /// Gets or sets the value.
            /// </summary>
            /// <value>
            /// The value.
            /// </value>
            public object Value { get; set; }
        }

        #endregion
    }
}