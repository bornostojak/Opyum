using System;

namespace Opyum.Structures.Global
{
    public class UndoRedoMethodCapsule<T> : IUndoRedoMethodCapsule
    {
        /// <summary>
        /// Create a <see cref="UndoRedoMethodCapsule"/> with the info needed to execute the operation.
        /// </summary>
        /// <param name="method">The method to execute a certain operation.</param>
        /// <param name="victim">The object being edited.</param>
        /// <param name="args">The the new value of the <paramref name="victim"/>.</param>
        /// <param name="caller">The object used to Invoke the method.</param>
        //public UndoRedoMethodCapsule(UndoRedoDelegate method, object victim, object args, object caller)
        //{
        //    Method = method;
        //    Victim = victim;
        //    Args = args;
        //    CallerObject = caller;
        //}

        /// <summary>
        /// Create a <see cref="UndoRedoMethodCapsule"/> with the info needed to execute the operation.
        /// </summary>
        /// <param name="action">The action to execute a certain operation.</param>
        /// <param name="victim">The object being edited.</param>
        /// <param name="args">The the new value of the <paramref name="victim"/>.</param>
        /// <param name="caller">The object used to Invoke the method.</param>
        public UndoRedoMethodCapsule(Action<T, T> action, T oldValue, T newValue, object caller)
        {
            Action = action;
            Target = oldValue;
            MemValue = newValue;

            CallerObject = caller;
        }



        /// <summary>
        /// Create a <see cref="UndoRedoMethodCapsule"/> with the info needed to execute the operation.
        /// </summary>
        /// <param name="func">The function to execute a certain operation NEEDS TO RETURN OLD VALUE.</param>
        /// <param name="victim">The object being edited.</param>
        /// <param name="args">The the new value of the <paramref name="victim"/>.</param>
        /// <param name="caller">The object used to Invoke the method.</param>
        public UndoRedoMethodCapsule(Func<object, T, T> func, object target, T val, object caller)
        {
            Function = func;
            Target = target;
            MemValue = val;
            CallerObject = caller;
        }

        T MemValue { get; set; }
        object Target { get; set; }
        internal Func<object, T, T> Function { get; set; } = null;

        internal Action<T, T> Action { get; set; } = null;

        public object CallerObject { get; set; }

        /// <summary>
        /// Apply saved args to target, and save the old value of target to args
        /// </summary>
        public void Invoke()
        {
            if (Action != null)
            {
                InvokeAction();
            }
            else if (Function != null)
            {
                InvokeFunc();
            }
        }

        public void InvokeFunc()
        {
            MemValue = Function(Target, MemValue);
        }

        public void InvokeAction()
        {
            T temp = (T)Target;
            Action((T)Target, MemValue);
            MemValue = temp;
        }
    }


}
