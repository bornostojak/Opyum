using System.Windows.Forms;
using Opyum.WindowsPlatform.Settings;
using Opyum.Structures.Global;

namespace Opyum.WindowsPlatform.Settings
{
    public interface ISettingsPanel<T> : IUndoRedoable
    {
        object LoadElements(T data);
        event SettingsChangedEventHandler SettingsChanged;
    }

    public interface ISettingsPanel : IUndoRedoable
    {
        object LoadElements();
        event SettingsChangedEventHandler SettingsChanged;

        void KeyPressResolve(object sender, KeyEventArgs e);
    }


}
