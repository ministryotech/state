using System.Diagnostics.CodeAnalysis;

namespace Ministry.State
{
    /// <summary>
    /// Wrapper for a state storage mechanism such as Session state, Application state or Cookie state
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global", Justification = "Library")]
    public interface IStateStorage
    {
        /// <summary>
        /// Clears the state.
        /// </summary>
        void Clear();

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>The value.</returns>
        object GetValue(string key);

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <returns>The value.</returns>
        T GetValue<T>(string key);

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        void SetValue(string key, object value);

        /// <summary>
        /// Sets the value.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        void SetValue<T>(string key, T value);

        /// <summary>
        /// Removes the specified item from state.
        /// </summary>
        /// <param name="key">The key.</param>
        void Remove(string key);
    }
}