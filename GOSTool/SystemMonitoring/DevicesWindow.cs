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
    public partial class DevicesWindow : Form
    {
        public bool wireless = false;
        private bool forceQuit = false;

        public DevicesWindow()
        {
            InitializeComponent();
            usbConfigUserControl1.Show();
            wirelessConfigUserControl1.Hide();
        }

        private async void connectButton_Click(object sender, EventArgs e)
        {
            deviceTree.Nodes.Clear();             

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
                    if (!wireless)
                    {
                        Uart.Init(usbConfigUserControl1.Port, usbConfigUserControl1.Baud);
                    }

                    int devNum = 0;

                    if (!wireless)
                    {
                        devNum = SysmonFunctions.GetDeviceNum();
                    }
                    else
                    {
                        //binaryNum = Wireless.GetBinaryNum();
                    }

                    for (int i = 0; i < devNum && !forceQuit; i++)
                    {
                        DeviceDescriptor deviceDescriptor = new DeviceDescriptor();

                        if (!wireless)
                        {
                            deviceDescriptor = SysmonFunctions.GetDeviceInfo(i);
                        }

                        TreeNode node = new TreeNode(deviceDescriptor.Name);
                        node.Nodes.Add("Description: " + deviceDescriptor.Description);
                        node.Nodes.Add("Device ID: 0x" + deviceDescriptor.DeviceId.ToString("X"));
                        node.Nodes.Add("Device type: " + deviceDescriptor.DeviceType);
                        node.Nodes.Add("Enabled: " + deviceDescriptor.Enabled);
                        node.Nodes.Add("State: " + deviceDescriptor.DeviceState);
                        node.Nodes.Add("Error code: " + deviceDescriptor.ErrorCode);
                        node.Nodes.Add("Error counter: " + deviceDescriptor.ErrorCounter);
                        node.Nodes.Add("Error tolerance: " + deviceDescriptor.ErrorTolerance);
                        node.Nodes.Add("Read counter: " + deviceDescriptor.ReadCounter);
                        node.Nodes.Add("Write counter: " + deviceDescriptor.WriteCounter);

                        TreeViewAddNode_ThreadSafe(node);
                    }
                });
            }
        }

        private delegate void TreeViewAddNodeDelegate(TreeNode node);

        private void TreeViewAddNode(TreeNode node)
        {
            deviceTree.Nodes.Add(node);
        }

        private void TreeViewAddNode_ThreadSafe(TreeNode node)
        {
            if (deviceTree.InvokeRequired)
            {
                TreeViewAddNodeDelegate d = new TreeViewAddNodeDelegate(TreeViewAddNode);
                deviceTree.Invoke(d, node);
            }
            else
            {
                TreeViewAddNode(node);
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
    }
}
