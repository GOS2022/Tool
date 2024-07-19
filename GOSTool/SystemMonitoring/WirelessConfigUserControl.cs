using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GOSTool.SystemMonitoring
{
    public partial class WirelessConfigUserControl : UserControl
    {
        public string Ip { get; set; } = "";
        public int Port { get; set; } = int.MinValue;
        public WirelessConfigUserControl()
        {
            InitializeComponent();

            Ip = Wireless.Ip;
            Port = Wireless.Port;

            IpTextBox.Text = Ip;
            PortTextBox.Text = Port.ToString();

            Wireless.StatusUpdateEvent += (object sender, string status) =>
            {
                Helper.SetLabelText_ThreadSafe(this, statusLabel, status);
            };

            Wireless.ProgressUpdateEvent += (object sender, int value) =>
            {
                Helper.SetProgressBar_ThreadSafe(this, progressBar1, value);
            };
        }

        private void IpTextBox_TextChanged(object sender, EventArgs e)
        {
            Ip = IpTextBox.Text;
            Wireless.Ip = Ip;
        }

        private void PortTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Port = int.Parse(PortTextBox.Text);
                Wireless.Port = Port;
            }
            catch
            {
                PortTextBox.Text = "";
            }
        }
    }
}
