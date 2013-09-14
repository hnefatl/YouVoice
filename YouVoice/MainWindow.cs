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

        protected delegate void VoidDelegate();

        #endregion

        public MainWindow()
        {
            
            // Initialise the functions
            CommandReceiver = new Listener();
            CommandReceiver.OnPlay += new ListenerEvent(Play);
            CommandReceiver.OnPause += new ListenerEvent(Pause);
            CommandReceiver.OnNext += new ListenerEvent(Next);
            CommandReceiver.OnPrevious += new ListenerEvent(Previous);
            CommandReceiver.OnExit += new ListenerEvent(Exit);
            CommandReceiver.OnStartListening += new ListenerEvent(StartListening);
            CommandReceiver.OnStopListening += new ListenerEvent(StopListening);

            CommandReceiver.Initialise();

            InitializeComponent();

            CommandReceiver.Start();
        }

        #region Command Handlers
        protected void Play()
        {

        }
        protected void Pause()
        {

        }
        protected void Next()
        {

        }
        protected void Previous()
        {

        }
        protected void Exit()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new VoidDelegate(Exit));
            }
            else
            {
                this.Close();
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
            }
        }
        #endregion
    }
}
