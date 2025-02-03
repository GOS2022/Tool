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
    public partial class EventsWindow : Form
    {
        public EventsWindow()
        {
            InitializeComponent();
        }

        private bool wireless = false;
        private bool isMonitoringOn = false;
        private async void readButton_Click(object sender, EventArgs e)
        {
            isMonitoringOn = !isMonitoringOn;

            if (isMonitoringOn)
            {
                readButton.Text = "Stop";
                readButton.BackColor = Color.Red;
                pollPeriodNUD.Enabled = false;
            }
            else
            {
                readButton.Text = "Read";
                readButton.BackColor = Color.Green;
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
                        List<ListViewItem> ersItems = new List<ListViewItem>();
                        List<ErsEvent> ersEvents = new List<ErsEvent>();

                        // Get monitoring data.
                        if (wireless)
                        {
                            //ersEvents = Wireless.GetEvents();
                        }
                        else
                        {
                            ersEvents = SysmonFunctions.GetEvents();
                        }

                        if (ersEvents.Count > 0)
                        {
                            foreach (var ersEvent in ersEvents)
                            {
                                ersItems.Add(new ListViewItem(new string[] { ersEvent.Description, ersEvent.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss.fff"), ersEvent.Trigger.ToString(), BitConverter.ToString(ersEvent.EventData.ToArray()) }));
                            }

                            Helper.UpdateListViewWithItems_ThreadSafe(this, ersListView, ersItems);
                            //Helper.ResizeListView_ThreadSafe(this, ersListView);
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

        private async void clearButton_Click(object sender, EventArgs e)
        {
            bool clearSuccess = false;

            await Task.Run(() =>
            {
                // Try to connect on the given port configuration.
                if (!wireless)
                {
                    Uart.Init(usbConfigUserControl1.Port, usbConfigUserControl1.Baud);
                }

                if (wireless)
                {
                    //clearSuccess = Wireless.ClearEvents();
                }
                else
                {
                    clearSuccess = SysmonFunctions.ClearEvents();
                }

                Helper.UpdateListViewWithItems_ThreadSafe(this, ersListView, new List<ListViewItem>());
            });

            if (!clearSuccess)
                MessageBox.Show("Event clear failed.");
        }

        private void EventsWindow_Load(object sender, EventArgs e)
        {
            wirelessConfigUserControl1.Hide();
        }

        private void EventsWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            isMonitoringOn = false;
        }
    }
}
