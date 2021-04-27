using Opyum.StandardPlayback.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave;
using Opyum.StandardPlayback;
using Opyum.Structures;
using Opyum.Structures.Playlist;

namespace TestPlayer
{
    public partial class Form1 : Form
    {
        private static Type[] possiblePlayers;
        private StandardPlayer player;
        IWavePlayer pp;

        public Form1()
        {
            InitializeComponent();

            this.MinimumSize = this.Size;
            if (possiblePlayers == null)
            {
                possiblePlayers = GetAllPlayers(); 
            }
            #region WaveOutEvent

            //for (int i = 0; i < WaveOut.DeviceCount; i++)
            //{
            //    var caps = WaveOut.GetCapabilities(i);
            //    richTextBox1.AppendText($"{caps.ProductName} \t\t {caps.ProductGuid}\n");
            //}

            //pp = new WaveOutEvent() { DeviceNumber = 0 };
            #endregion

            //pp = new AsioOut(AsioOut.GetDriverNames()[0]);
            pp = new WaveOut();
        }

        private void openButton_Click(object sender, EventArgs e)
        {
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                try
                {
                    player.Dispose();
                    GC.Collect();
                }
                catch (Exception)
                {

                }
            }

            else
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "AudioFile | *.mp3; *.mp2; *.wav; *.aif; *.wma; *.mp4; *.aac";
                if (ofd.ShowDialog() != DialogResult.OK) return;

                if (player != null)
                {
                    player.Dispose(); 
                    GC.Collect();
                }
                if (player == null)
                {
                    player = (Opyum.StandardPlayback.StandardPlayer)GetInstance(GetTheRightPlayer(ofd.FileName));
                    if (pp != null)
                    {
                        richTextBox1.AppendText("Instantiated\n");
                        //player = new StandardPlayer(pp);
                        //var type = player.GetType();
                        player.InstantiatePlayerOutput(pp);

                    }
                    //else
                    //{
                    //    //player = new StandardPlayer(); 
                    //}
                }
                
                player?.StopStream();

                player.Dispose();
                player.StartStream(ofd.FileName);
                pauseButton.Enabled = true; 
            }

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Dispose();
            GC.Collect();
        }

        private void pauseButton_Click(object sender, EventArgs e)
        {
            if (ModifierKeys.HasFlag(Keys.Control))
            {
                try
                {
                    player.sourceStream.Position = 0;
                }
                catch (Exception)
                {

                }
            }
            else
            {
                if (player != null)
                {
                    player.PausePlay();
                }
                player.PlayPauseStream(); 
            }
        }


        private static Type[] GetAllPlayers()
        {
            var temp = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            List<Assembly> allAssemblies = new List<Assembly>();
            foreach (var dll in Directory.GetFiles(temp, "*.dll"))
            {
                allAssemblies.Add(Assembly.LoadFile(dll));
            }

            var t = allAssemblies.SelectMany((a) =>
                a.GetTypes().
                Where((x) =>
                    x.GetCustomAttributes(typeof(Opyum.StandardPlayback.Attributes.AudioPlayerAttribute), true).Length > 0));
                        

            return t.ToArray();
        }

        private static Type GetTheRightPlayer(string file)
        {
            var t2 = $"*{Path.GetExtension(file)}";

            var players = possiblePlayers.ToList().Where((p) => ((AudioPlayerAttribute)p.GetCustomAttribute(typeof(AudioPlayerAttribute))).SupportedFormats.Contains($"*{Path.GetExtension(file)}"));
            return players.FirstOrDefault();
        }

        private static object GetInstance(Type t) => Activator.CreateInstance(t);
    }
}
