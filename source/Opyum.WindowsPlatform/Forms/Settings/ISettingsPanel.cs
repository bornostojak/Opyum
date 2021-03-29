using System.Windows.Forms;
using Opyum.WindowsPlatform.Settings;
using Opyum.Structures.Global;

namespace Opyum.WindowsPlatform.Forms.Settings
{
    public interface ISettingsPanel<T> : IUndoRedoable
    {
        object LoadElements(T data);
    }

    public interface ISettingsPanel : IUndoRedoable
    {
        object LoadElements();

        void KeyPressResolve(object sender, KeyEventArgs e);
    }


}
