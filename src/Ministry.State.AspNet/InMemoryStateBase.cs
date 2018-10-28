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

namespace Ministry.State
{
    /// <summary>
    /// Fake state implementation that stores values in memory.
    /// </summary>
    /// <remarks>
    /// This is a useful swap out for a concrete state. You can inherit a custom version for session storage checking if needed.
    /// </remarks>
    public class InMemoryState : IStateStorage
    {
        #region | Construction |

        /// <summary>
        /// Initializes a new instance of the <see cref="Items"/> class.
        /// </summary>
        public InMemoryState()
        {
            Items = new List<StateItem>();
        }

        #endregion

        /// <summary>
        /// Gets the in memory session.
        /// </summary>
        protected List<StateItem> Items { get; }

        /// <summary>
        /// Clears the state.
        /// </summary>
        public void Clear() => Items.Clear();

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The value.</returns>
        public object GetValue(string key) 
            => Items.All(o => o.Key != key) ? null : Items.FirstOrDefault(o => o.Key == key)?.Value;

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <typeparam name="T">The type of the object to get.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>The value.</returns>
        public T GetValue<T>(string key) 
            => (T)Items.FirstOrDefault(o => o.Key == key)?.Value;

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void SetValue(string key, object value)
        {
            if (Items.All(o => o.Key != key))
                Items.Add(new StateItem(key, value));
            else
                Items.First(o => o.Key == key).Value = value;
        }

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <typeparam name="T">The type of the value to set.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void SetValue<T>(string key, T value)
        {
            if (Items.All(o => o.Key != key))
                Items.Add(new StateItem(key, value));
            else
                Items.First(o => o.Key == key).Value = value;
        }

        #region | Nested Classes |

        /// <summary>
        /// A State Item
        /// </summary>
        protected class StateItem
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="StateItem"/> class.
            /// </summary>
            /// <param name="key">The key.</param>
            /// <param name="value">The value.</param>
            public StateItem(string key, object value)
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
            public string Key { get; }

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