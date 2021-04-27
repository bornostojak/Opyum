using System;

namespace Opyum.Structures.Global
{
    public interface IUndoRedoable
    {
        void Undo();
        void Redo();
    }
}
