using System;
using System.Collections.Generic;

namespace Perenis.Core.Collections
{
    /// <summary>
    /// Untyped stack enhanced by optional object names
    /// </summary>
    public class StackWithOptionalNames
    {
        /// <summary>
        /// Checks if the object with given name exists in named object dictionary or in there is any object in the stack
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Exists(string name)
        {
            if ((name != null && String.IsNullOrEmpty(name))) throw new ArgumentException("name");
            return name != null ? namedAccess.ContainsKey(name) : stack.Count > 0;
        }

        /// <summary>
        /// Peeks the named object or first object on the stack if the 
        /// </summary>
        /// <param name="name">name of object to peek</param>
        /// <returns></returns>
        public object Peek(string name)
        {
            if (name == null)
            {
                return PeekNthFromTop(1);
            }
            else
            {
                return namedAccess.ContainsKey(name) ? namedAccess[name].Item1 : null;
            }
        }

        /// <summary>
        /// Gets the object from the 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        public T Peek<T>(string name) where T : class
        {
            return Cast<T>(Peek(name));
        }

        /// <summary>
        /// peeks the N-th object from the top of the stack
        /// </summary>
        ///<param name="n">Which object from the top of the stack shall be returned (1=first object)</param>
        public object PeekNthFromTop(int n)
        {
            return stack.Count >= n ? stack[stack.Count - n].Item1 : null;
        }

        /// <summary>
        /// Peeks the N-th object from the top
        /// </summary>
        /// <typeparam name="T">Type of object to peek</typeparam>
        ///<param name="n">Which object from the top of the stack shall be returned (1=first object)</param>
        /// <returns>object</returns>
        public T PeekNthFromTop<T>(int n) where T : class
        {
            return Cast<T>(PeekNthFromTop(n));
        }

        /// <summary>
        /// pops the object from the stack
        /// </summary>
        /// <param name="name">name of the object</param>
        /// <returns>object</returns>
        public object Pop(string name)
        {
            if (stack.Count == 0)
                return null;
            if ((name != null && String.IsNullOrEmpty(name))) throw new ArgumentException("name");
            Tuple<object, string> tuple;
            if (name == null)
            {
                tuple = stack[stack.Count - 1];
                stack.RemoveAt(stack.Count - 1);
            }
            else
            {
                tuple = namedAccess[name];
                namedAccess.Remove(name);
                stack.Remove(tuple);
            }
            return tuple.Item1;
        }

        /// <summary>
        /// Pops the object of given name
        /// </summary>
        /// <typeparam name="T">Type of object to pop</typeparam>
        /// <param name="name">Name of object to pop</param>
        /// <returns></returns>
        public T Pop<T>(string name) where T : class
        {
            return Cast<T>(Pop(name));
        }

        /// <summary>
        /// Pushs the object on the stack - if name is not null then the object is added to named objects dictionary too
        /// </summary>
        /// <param name="obj">object to push</param>
        /// <param name="name">object name or null</param>
        public void Push(object obj, string name)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            if ((name != null && String.IsNullOrEmpty(name))) throw new ArgumentException("name");
            var tuple = new Tuple<object, string>(obj, name);
            stack.Add(tuple);
            if (name != null) namedAccess.Add(name, tuple);
        }

        #region ------ Internals ------------------------------------------------------------------

        /// <summary>
        /// Dictionary used for named access
        /// </summary>
        private readonly Dictionary<string, Tuple<object, string>> namedAccess = new Dictionary<string, Tuple<object, string>>();

        /// <summary>
        /// Stack
        /// </summary>
        private readonly List<Tuple<object, string>> stack = new List<Tuple<object, string>>();

        /// <summary>
        /// Casts given object to type T
        /// </summary>
        private T Cast<T>(object result) where T : class
        {
            // TODO this method shall be moved to Perenis.Core somewhere
            if (result == null) return null;
            if (!(result is T))
            {
                // TODO Develop a standard way to propagate errors from operations.
                throw new InvalidOperationException("The object named {0} is not compatible with the current operation");
            }
            return (T) result;
        }

        #endregion
    }
}