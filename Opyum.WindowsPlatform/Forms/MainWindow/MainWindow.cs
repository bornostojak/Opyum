using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Opyum.WindowsPlatform.Settings;
using Opyum.WindowsPlatform.Shortcuts;

namespace Opyum.WindowsPlatform
{
    [Opyum.Structures.Attributes.OpyumApplicationPlatform(Structures.Enums.ApplicationPlatform.Windows)]
    public partial class MainWindow : Form
    {
        public static MainWindow Window { get; protected set; }

        public MainWindow()
        {
            Window = this;
            InitializeComponent();
            WindowSetup();

            panel2.SizeChanged += (a, b) =>
            {
                button1.Width = panel2.Width / 4;  
                button2.Width = panel2.Width / 4;
                button3.Width = panel2.Width / 4;
            };

        }

        private void MainWindow_MaximizedBoundsChanged(object sender, EventArgs e)
        {

        }

        private async void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            IShortcutKeyBinding shortcut = null;
            //ShortcutResolver.ResolveShortcut(sender, e, SettingsManager.GlobalSettings.Shortcuts);
            await Task.Run(() =>
            {
                shortcut = ShortcutResolver.ResolveShortcut(sender, e);
            });
            shortcut?.Run(sender);
        }
    }
}
