using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Opyum.Structures.Global
{
    public class UndoRedoStack
    {
        protected Stack<IUndoRedoMethodCapsule> UndoStack { get; set; } = new Stack<IUndoRedoMethodCapsule>();
        protected Stack<IUndoRedoMethodCapsule> RedoStack { get; set; } = new Stack<IUndoRedoMethodCapsule>();

        //returns the number of elements in the undo or redo stack
        public int UndoCount { get => UndoStack.Count; }
        public int RedoCount { get => RedoStack.Count; }


        //event called upon a change in the undo stack
        public event EventHandler UndoRedoStackChanged;


        /// <summary>
        /// Undo an <see cref="Action{T1}"/> or <see cref="Func{T, TResult}"/>
        /// </summary>
        public void Undo()
        {
            if (UndoStack.Count > 0)
            {
                var last = UndoStack.Pop();
                last.Invoke();
                RedoStack.Push(last);
                UndoRedoStackChanged?.Invoke(this, new EventArgs());
            }

        }

        /// <summary>
        /// Use to redo the function done through the <see cref="Do"/> method
        /// <para>It pulls the function call from the <see cref="RedoStack"/> and calls the method in the function call</para>
        /// </summary>
        public void Redo()
        {
            if (RedoStack.Count > 0)
            {
                var last = RedoStack.Pop();
                last.Invoke();
                UndoStack.Push(last);
                UndoRedoStackChanged?.Invoke(this, new EventArgs());
            }
        }

        public void Do<T>(UndoRedoMethodCapsule<T> capsule)
        {
            capsule.Invoke();
            UndoStack.Push(capsule);
            if (RedoStack.Count > 0)
            {
                RedoStack.Clear();
            }
            UndoRedoStackChanged?.Invoke(this, new EventArgs());
        }

        public void Do<T>(Func<object, T, T>func, object target, T val, object caller = null)
        {
            Do(new UndoRedoMethodCapsule<T>(func, target, val, caller));
        }

        public void Do<Tobj, Tval>(Func<Tobj, Tval, Tval> func, object target, Tval val, object caller = null)
        {

            Do(new UndoRedoMethodCapsule<Tval>((object a, Tval b) => func((Tobj)a, (Tval)b), target, val, caller));
        }


        public void Do<T>(Action<T, T> action, T oldVal, T newVal, object caller = null)
        {
            Do(new UndoRedoMethodCapsule<T>(action, oldVal, newVal, caller));
        }

    }
}
