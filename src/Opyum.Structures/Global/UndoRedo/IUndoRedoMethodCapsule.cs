namespace Opyum.Structures.Global
{
    public interface IUndoRedoMethodCapsule
    {
        object CallerObject { get; set; }

        void Invoke();
        void InvokeAction();
        void InvokeFunc();
    }
}