using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Text;
using System.Windows.Forms;

using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Google.Apis.YouTube.v3.Data;

namespace YouVoice
{
    public partial class MainWindow : Form
    {
        #region Members

        protected Listener CommandReceiver;
        protected Listener ListenListener;

        protected delegate void VoidDelegate();

        protected string VideoURL = "http://www.youtube.com/watch?v=2-_g8NZr1tA";

        #endregion

        public MainWindow()
        {
            // Initialise the functions
            CommandReceiver = new Listener();
            ListenListener = new Listener();
            InitialiseCommandReceivers();

            InitializeComponent();

            StartListening();
            CommandReceiver.Start();
        }

        public void InitialiseCommandReceivers()
        {
            // Add the commands
            CommandReceiver.AddCommand(new Command("Play", Play, new List<string>() { "Play", "Start", "Begin" }));
            CommandReceiver.AddCommand(new Command("Pause", Pause, new List<string>() { "Stop", "Halt", "Pause" }));
            CommandReceiver.AddCommand(new Command("Next", Next, new List<string>() { "Next", "Skip", "Forward", "Skip Forward" }));
            CommandReceiver.AddCommand(new Command("Previous", Previous, new List<string>() { "Previous", "Back", "Skip Back" }));
            CommandReceiver.AddCommand(new Command("Mute", Mute, new List<string>() { "Mute", "Volume off", "Quiet" }));
            CommandReceiver.AddCommand(new Command("Unmute", Unmute, new List<string>() { "Unmute", "Volume on", "Loud" }));
            CommandReceiver.AddCommand(new Command("Fullscreen", Fullscreen, new List<string>() { "Fullscreen", "Big", "Large" }));
            CommandReceiver.AddCommand(new Command("Windowed", Windowed, new List<string>() { "Windowed", "Small" }));
            CommandReceiver.AddCommand(new Command("Browse", Browse, new List<string>() { "Browse", "Search", "Find" }));
            CommandReceiver.AddCommand(new Command("Exit", Exit, new List<string>() { "Exit", "Quit", "Close" }));
            CommandReceiver.AddCommand(new Command("StopListening", StopListening, new List<string>() { "Stop Listening" }));

            ListenListener.AddCommand(new Command("StartListening", StartListening, new List<string>() { "Start Listening" }));
        }

        #region Command Handlers
        protected void Play()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new VoidDelegate(Play));
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(VideoURL))
                {
                    if (VideoURL != Player.Movie)
                    {
                        VideoURL += "&autoplay=1";
                        Player.LoadMovie(0, VideoURL);
                    }
                }
            }
        }
        protected void Pause()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new VoidDelegate(Pause));
            }
            else
            {
                Player.StopPlay();
            }
        }
        protected void Next()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new VoidDelegate(Next));
            }
            else
            {

            }
        }
        protected void Previous()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new VoidDelegate(Previous));
            }
            else
            {

            }
        }
        protected void Mute()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new VoidDelegate(Mute));
            }
            else
            {

            }
        }
        protected void Unmute()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new VoidDelegate(Unmute));
            }
            else
            {

            }
        }
        protected void Fullscreen()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new VoidDelegate(Fullscreen));
            }
            else
            {

            }
        }
        protected void Windowed()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new VoidDelegate(Windowed));
            }
            else
            {

            }
        }
        protected void Browse()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new VoidDelegate(Browse));
            }
            else
            {
                List<Command> RecogCommands = CommandReceiver.Commands;

                CommandReceiver.Disable();

                BrowseWindow Browser = new BrowseWindow();
                Browser.ShowDialog(this);

                CommandReceiver.Enable();
            }
        }

        protected void Exit()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new VoidDelegate(Exit));
            }
            else
            {
                // Ask for confirmation on the exit
                CommandReceiver.Disable();

                ConfirmExit ExitWindow=new ConfirmExit();
                ExitWindow.ShowDialog(this);

                if (ExitWindow.Exit)
                {
                    // Close the application
                    CommandReceiver.Stop();
                    this.Close();
                }
                else
                {
                    // Reset the commands
                    CommandReceiver.Enable();
                }
            }
        }

        protected void StartListening()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new VoidDelegate(StartListening));
            }
            else
            {
                StatusLabel.Text = "Listening";
                ListenListener.Disable();
                CommandReceiver.Enable();
            }
        }
        protected void StopListening()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new VoidDelegate(StopListening));
            }
            else
            {
                StatusLabel.Text = "Not listening";
                CommandReceiver.Disable();
                ListenListener.Enable();
            }
        }
        #endregion

        protected override void OnClosing(CancelEventArgs e)
        {
            CommandReceiver.Stop();

            base.OnClosing(e);
        }
    }
}
