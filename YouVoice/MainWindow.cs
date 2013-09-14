using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Threading;

namespace YouVoice
{
    public partial class MainWindow : Form
    {
        #region Members

        protected Listener CommandReceiver;

        #endregion

        public MainWindow()
        {
            InitializeComponent();
        }
    }
}
