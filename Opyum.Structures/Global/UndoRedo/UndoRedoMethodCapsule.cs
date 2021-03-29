namespace Opyum.Structures.Global
{
    public class UndoRedoMethodCapsule
    {
        /// <summary>
        /// Create a <see cref="UndoRedoMethodCapsule"/> with the info needed to execute the operation.
        /// </summary>
        /// <param name="method">The method to execute a certain operation.</param>
        /// <param name="victim">The object being edited.</param>
        /// <param name="args">The the new value of the <paramref name="victim"/>.</param>
        /// <param name="caller">The object used to Invoke the method.</param>
        public UndoRedoMethodCapsule(UndoRedoDelegate method, object victim, object args, object caller)
        {
            Method = method;
            Victim = victim;
            Args = args;
            CallerObject = caller;
        }

        public delegate object UndoRedoDelegate(object obj, object args);
        public object Args { get; set; }
        public object Victim { get; set; }
        public UndoRedoDelegate Method { get; set; }

        public object CallerObject { get; set; }
    }


}
