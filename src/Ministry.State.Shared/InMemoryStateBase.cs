using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Ministry.State
{
    /// <summary>
    /// Fake state implementation that stores values in memory.
    /// </summary>
    /// <remarks>
    /// This is a useful swap out for a concrete state. You can inherit a custom version for session storage checking if needed.
    /// </remarks>
    /// <inheritdoc cref="IStateStorage"/>
    [SuppressMessage("ReSharper", "UnusedType.Global", Justification = "Library")]
    [SuppressMessage("ReSharper", "MemberCanBePrivate.Global", Justification = "Library")]
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

        /// <inheritdoc/>
        public void Clear() => Items.Clear();

        /// <inheritdoc/>
        public object GetValue(string key) 
            => Items.All(o => o.Key != key) ? null : Items.FirstOrDefault(o => o.Key == key)?.Value;

        /// <inheritdoc/>
        public T GetValue<T>(string key) 
            => (T)Items.FirstOrDefault(o => o.Key == key)?.Value;

        /// <inheritdoc/>
        public void Remove(string key)
        {
            var item = Items.FirstOrDefault(o => o.Key == key);
            if (item != null) Items.Remove(item);
        }

        /// <inheritdoc/>
        public void SetValue(string key, object value)
        {
            if (Items.All(o => o.Key != key))
                Items.Add(new StateItem(key, value));
            else
                Items.First(o => o.Key == key).Value = value;
        }

        /// <inheritdoc/>
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