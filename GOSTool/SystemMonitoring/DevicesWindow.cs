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

            Font font = new Font(deviceTree.Font, FontStyle.Bold);
            deviceTree.Font = font;
        }

        private async void connectButton_Click(object sender, EventArgs e)
        {
            deviceTree.Nodes.Clear();
            Font fontRegular = new Font(deviceTree.Font, FontStyle.Regular);

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
                        devNum = SvlDhs.GetDeviceNum();
                    }
                    else
                    {
                        devNum = Wireless.GetDeviceNum();
                    }

                    for (int i = 0; i < devNum && !forceQuit; i++)
                    {
                        DhsDeviceDescriptor deviceDescriptor = new DhsDeviceDescriptor();

                        if (!wireless)
                        {
                            deviceDescriptor = SvlDhs.GetDeviceInfo(i);
                        }
                        else
                        {
                            deviceDescriptor = Wireless.GetDeviceInfo(i);
                        }

                        TreeNode node = new TreeNode(deviceDescriptor.Name);
                        node.Nodes.Add(new TreeNode("Description: " + deviceDescriptor.Description) { NodeFont = fontRegular });
                        node.Nodes.Add(new TreeNode("Device ID: 0x" + deviceDescriptor.DeviceId.ToString("X")) { NodeFont = fontRegular });
                        node.Nodes.Add(new TreeNode("Device type: " + deviceDescriptor.DeviceType) { NodeFont = fontRegular });
                        node.Nodes.Add(new TreeNode("Enabled: " + deviceDescriptor.Enabled) { NodeFont = fontRegular });
                        node.Nodes.Add(new TreeNode("State: " + deviceDescriptor.DeviceState) { 
                            NodeFont = new Font(deviceTree.Font, FontStyle.Bold), 
                            ForeColor = deviceDescriptor.DeviceState == "Healthy" ? Color.Green : deviceDescriptor.DeviceState == "Uninitialized" ? Color.Gray : deviceDescriptor.DeviceState == "Warning" ? Color.Orange : Color.Red } );
                        node.Nodes.Add(new TreeNode("Error code: " + deviceDescriptor.ErrorCode){ NodeFont = fontRegular });
                        node.Nodes.Add(new TreeNode("Error counter: " + deviceDescriptor.ErrorCounter){ NodeFont = fontRegular });
                        node.Nodes.Add(new TreeNode("Error tolerance: " + deviceDescriptor.ErrorTolerance) { NodeFont = fontRegular });
                        node.Nodes.Add(new TreeNode("Read counter: " + deviceDescriptor.ReadCounter) { NodeFont = fontRegular });
                        node.Nodes.Add(new TreeNode("Write counter: " + deviceDescriptor.WriteCounter) { NodeFont = fontRegular });

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
