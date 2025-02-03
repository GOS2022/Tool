using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GOSTool.SystemMonitoring
{
    public partial class MonitoringDataWindow : Form
    {
        public MonitoringDataWindow()
        {
            InitializeComponent();
        }

        private bool wireless = false;
        private bool isMonitoringOn = false;

        private async void connectButton_Click(object sender, EventArgs e)
        {
            isMonitoringOn = !isMonitoringOn;

            if (isMonitoringOn)
            {
                connectButton.Text = "Stop";
                connectButton.BackColor = Color.Red;
                pollPeriodNUD.Enabled = false;
            }
            else
            {
                connectButton.Text = "Monitor";
                connectButton.BackColor = Color.Green;
                pollPeriodNUD.Enabled = true;
            }

            // Check for invalid configuration.
            if (usbComRadioButton.Checked && (usbConfigUserControl1.Port == null || usbConfigUserControl1.Baud < 0))
            {
                MessageBox.Show("Please select a port and a baud rate first!", "USB not configured", MessageBoxButtons.OK);
            }
            else if (wirelessComRadioButton.Checked && (wirelessConfigUserControl1.Ip == "" || wirelessConfigUserControl1.Port < 0))
            {
                MessageBox.Show("Please provide an IP address and a port first!", "Wireless not configured", MessageBoxButtons.OK);
            }
            else
            {
                await Task.Run(() =>
                {
                    // Try to connect on the given port configuration.
                    if (!wireless)
                    {
                        Uart.Init(usbConfigUserControl1.Port, usbConfigUserControl1.Baud);
                    }

                    while (isMonitoringOn)
                    {
                        List<ListViewItem> mdiItems = new List<ListViewItem>();
                        List<MdiVariable> mdiVariables = new List<MdiVariable>();

                        // Get monitoring data.
                        if (wireless)
                        {
                            //mdiVariables = Wireless.GetMonitoringData();
                        }
                        else
                        {
                            mdiVariables = SysmonFunctions.GetMonitoringData();
                        }

                        if (mdiVariables.Count > 0)
                        {
                            foreach (var mdiVar in mdiVariables)
                            {
                                mdiItems.Add(new ListViewItem(new string[] { mdiVar.Name, mdiVar.Type.ToString(), mdiVar.Value }));
                            }

                            Helper.UpdateListViewWithItems_ThreadSafe(this, mdiListView, mdiItems);
                            //Helper.ResizeListView_ThreadSafe(this, mdiListView);
                        }

                        Thread.Sleep((int)pollPeriodNUD.Value);
                    }
                });
            }
        }

        private void usbComRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            wireless = !usbComRadioButton.Checked;

            if (wireless)
            {
                wirelessConfigUserControl1.Show();
                usbConfigUserControl1.Hide();
            }
            else
            {
                wirelessConfigUserControl1.Hide();
                usbConfigUserControl1.Show();
            }
        }

        private void wirelessComRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            wireless = wirelessComRadioButton.Checked;

            if (wireless)
            {
                wirelessConfigUserControl1.Show();
                usbConfigUserControl1.Hide();
            }
            else
            {
                wirelessConfigUserControl1.Hide();
                usbConfigUserControl1.Show();
            }
        }

        private void MonitoringDataWindow_Load(object sender, EventArgs e)
        {
            wirelessConfigUserControl1.Hide();
        }

        private void MonitoringDataWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            isMonitoringOn = false;
        }
    }
}
