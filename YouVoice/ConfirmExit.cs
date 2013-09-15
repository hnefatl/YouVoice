using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace YouVoice
{
    public partial class ConfirmExit : Form
    {
        public delegate void VoidDelegate();

        public bool Exit { get; private set; }

        protected Listener CommandReceiver;

        public ConfirmExit()
        {
            InitializeComponent();

            this.CommandReceiver = new Listener();
            this.CommandReceiver.AddCommand(new Command("Yes", Yes, new List<string>() { "Yes", "Ok", "Close", "Quit" }));
            this.CommandReceiver.AddCommand(new Command("No", No, new List<string>() { "No", "Cancel", "Stop" }));

            this.CommandReceiver.Start();

            Exit = false;
        }

        protected void Yes()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new VoidDelegate(Yes));
            }
            else
            {
                Exit = true;
                this.Close();
            }
        }
        protected void No()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new VoidDelegate(No));
            }
            else
            {
                Exit = false;
                this.Close();
            }
        }

        private void ButtonYes_Click(object sender, EventArgs e)
        {
            Yes();
        }
        private void ButtonNo_Click(object sender, EventArgs e)
        {
            No();
        }
    }
}
