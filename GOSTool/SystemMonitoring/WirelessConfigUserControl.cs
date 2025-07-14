using GOSTool.SystemMonitoring.Com;
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
        public string Ip { get; set; } = "192.168.100.184";
        public int Port { get; set; } = int.MinValue;
        public WirelessConfigUserControl()
        {
            InitializeComponent();

            Ip = Sysmon.Ip;
            Port = Sysmon.WirelessPort;

            IpTextBox.Text = Ip;
            PortTextBox.Text = Port.ToString();

            Sysmon.StatusUpdateEvent += (object sender, SysmonStatusUpdateEventArgs args) =>
            {
                if (args.IsWireless)
                    Helper.SetLabelText_ThreadSafe(this, statusLabel, args.Status);
            };

            /*Wireless.ProgressUpdateEvent += (object sender, int value) =>
            {
                Helper.SetProgressBar_ThreadSafe(this, progressBar1, value);
            };*/
        }

        private void IpTextBox_TextChanged(object sender, EventArgs e)
        {
            Ip = IpTextBox.Text;
            Sysmon.Ip = Ip;
        }

        private void PortTextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Port = int.Parse(PortTextBox.Text);
                Sysmon.WirelessPort = Port;
            }
            catch
            {
                PortTextBox.Text = "";
            }
        }
    }
}
