using System;
using System.Collections.Generic;
using System.Linq;

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
        /// Use to undo the function done through the <see cref="Do"/> method
        /// <para>It pulls the function call from the <see cref="UndoStack"/> and calls the method in the function call</para>
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

        public void Do<T>(Func<object, T, T>func, object target, T val, object caller = null)
        {
            var capsule = new UndoRedoMethodCapsule<T>(func, target, val, caller);
            capsule.Invoke();
            UndoStack.Push(capsule);
            UndoRedoStackChanged?.Invoke(this, new EventArgs());
        }

        public void Do<Tobj, Tval>(Func<Tobj, Tval, Tval> func, object target, Tval val, object caller = null)
        {

            var capsule = new UndoRedoMethodCapsule<Tval>((object a, Tval b) => func((Tobj)a, (Tval)b), target, val, caller);
            capsule.Invoke();
            UndoStack.Push(capsule);
            UndoRedoStackChanged?.Invoke(this, new EventArgs());
        }


        public void Do<T>(Action<T, T> action, T oldVal, T newVal, object caller = null)
        {
            var capsule = new UndoRedoMethodCapsule<T>(action, oldVal, newVal, caller);
            capsule.Invoke();
            UndoStack.Push(capsule);
            UndoRedoStackChanged?.Invoke(this, new EventArgs());
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        ///// <summary>
        ///// Use to undo the function done through the <see cref="Do"/> method
        ///// <para>It pulls the function call from the <see cref="UndoStack"/> and calls the method in the function call</para>
        ///// </summary>
        //public void Undo()
        //{
        //    if (UndoStack.Count > 0)
        //    {
        //        var last = UndoStack.Pop();
        //        if (last.Count > 0)
        //        {
        //            //pulls the last element in the undo stack and if it has a list of oerations to exectu, does them in reverse order
        //            RedoStack.Push(new List<UndoRedoMethodCapsule>(last?.Reverse<UndoRedoMethodCapsule>()?.Select(z => new UndoRedoMethodCapsule(z.Method, z.Victim, ExecuteMethod(z), z.CallerObject)))); 
        //        }
        //        UndoRedoStackChanged?.Invoke(this, new EventArgs());
        //    }

        //}

        ///// <summary>
        ///// Use to redo the function done through the <see cref="Do"/> method
        ///// <para>It pulls the function call from the <see cref="RedoStack"/> and calls the method in the function call</para>
        ///// </summary>
        //public void Redo()
        //{
        //    if (RedoStack.Count > 0)
        //    {
        //        var last = RedoStack.Pop();
        //        if (last.Count > 0)
        //        {
        //            //pulls the last element in the redo stack and if it has a list of oerations to exectu, re-does them in reverse order
        //            UndoStack.Push(new List<UndoRedoMethodCapsule>(last?.Reverse<UndoRedoMethodCapsule>()?.Select(z => new UndoRedoMethodCapsule(z.Method, z.Victim ,ExecuteMethod(z), z.CallerObject))));
        //        }
        //        UndoRedoStackChanged?.Invoke(this, new EventArgs());
        //    }
        //}

        //public void ClearRedo()
        //{
        //    RedoStack.Clear();
        //}

        ///// <summary>
        ///// When called will execute the methode with the <paramref name="args"/> and Invoke it from the <paramref name="caller"/> object.
        ///// <para>The method needs to have the same return as the argument it takes, and the return will be saved as the <paramref name="args"/> in the <see cref="UndoStack"/></para>
        ///// </summary>
        ///// <param name="method">The <see cref="UndoRedoDelegate"/> method that will be called and executed.</param>
        ///// <param name="victim">The object whose value is being edited.</param>
        ///// <param name="args">The arguments passed through to the function.</param>
        ///// <param name="caller"></param>
        //public void Do(UndoRedoMethodCapsule.UndoRedoDelegate method, object victim, object args, object caller = null)
        //{
        //    try
        //    {
        //        var list = new List<UndoRedoMethodCapsule>();
        //        list.Add(new UndoRedoMethodCapsule(method, victim, ExecuteMethod(method, victim, args, caller), caller));
        //        UndoStack.Push(list);
        //        if (RedoStack.Count > 0)
        //        {
        //            ClearRedo();
        //        }
        //        UndoRedoStackChanged?.Invoke(this, new EventArgs());
        //    }
        //    catch (Exception e)
        //    {

        //    }
        //}

        ///// <summary>
        ///// When called will execute the methode in <paramref name= "work" /> and Invoke it from the caller also saved in <paramref name="work"/> object.
        ///// </summary>
        ///// <param name="work"></param>
        //public void Do(UndoRedoMethodCapsule work)
        //{
        //    try
        //    {
        //        work.Args = ExecuteMethod(work);
        //        var list = new List<UndoRedoMethodCapsule>();
        //        list.Add(work);
        //        UndoStack.Push(list);
        //        if (RedoStack.Count > 0)
        //        {
        //            ClearRedo();
        //        }
        //        UndoRedoStackChanged?.Invoke(this, new EventArgs());
        //    }
        //    catch (Exception e)
        //    {

        //    }
        //}


        ///// <summary>
        ///// Executes a list of <see cref="UndoRedoMethodCapsule"/> operations and saves it in the <see cref="UndoStack"/>
        ///// </summary>
        ///// <param name="worklist"></param>
        //public void DoMany(List<UndoRedoMethodCapsule> worklist)
        //{
        //    try
        //    {
        //        if (worklist != null)
        //        {
        //            UndoStack.Push(worklist.Select(a => new UndoRedoMethodCapsule(a.Method, a.Victim, ExecuteMethod(a), a.CallerObject)).ToList());
        //            UndoRedoStackChanged?.Invoke(this, new EventArgs());
        //        }
        //    }
        //    catch (Exception e)
        //    {

        //    }
        //}

        ///// <summary>
        ///// Support for the <see cref="Do"/> method.
        ///// </summary>
        ///// <param name="method"></param>
        ///// <param name="victim"></param>
        ///// <param name="args"></param>
        ///// <param name="caller"></param>
        ///// <returns></returns>
        //private object ExecuteMethod(UndoRedoMethodCapsule.UndoRedoDelegate method, object victim, object args, object caller = null)
        //{
        //    return ((UndoRedoMethodCapsule.UndoRedoDelegate)Delegate.CreateDelegate(typeof(UndoRedoMethodCapsule.UndoRedoDelegate), caller, method.Method)).Invoke(victim, args);
        //}

        ///// <summary>
        ///// Support for the <see cref="Do"/> method.
        ///// </summary>
        ///// <param name="method"></param>
        ///// <param name="victim"></param>
        ///// <param name="args"></param>
        ///// <param name="caller"></param>
        ///// <returns></returns>
        //private object ExecuteMethod(UndoRedoMethodCapsule item)
        //{
        //    return ExecuteMethod(item.Method, item.Victim, item.Args, item.CallerObject);
        //}

    }
}
