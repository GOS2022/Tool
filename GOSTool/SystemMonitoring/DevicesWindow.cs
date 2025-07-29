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
            driverDiagTree.Nodes.Clear();
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
                    int devNum = Sysmon.SvlDhs_GetDeviceNum(wireless);

                    for (int i = 0; i < devNum && !forceQuit; i++)
                    {
                        DhsDeviceDescriptor deviceDescriptor = Sysmon.SvlDhs_GetDeviceInfo(i, wireless);

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

                        TreeViewAddNode_ThreadSafe(deviceTree, node);
                    }

                    DrvDiag driverDiag = Sysmon.SvlDhs_GetDriverDiag(wireless);
                    TreeNode diagNode = new TreeNode("Uart diagnostics");                    
                    diagNode.Nodes.Add(new TreeNode("Global error flags: " + driverDiag.UartDiag.GlobalErrorFlags) { NodeFont = fontRegular });
                    TreeNode subNode = new TreeNode("Uart instance error flags");
                    for (int i = 0; i < driverDiag.UartDiag.InstanceErrorFlags.Count; i++)
                    {
                        subNode.Nodes.Add(new TreeNode("Instance [" + i + "] error flags: " + driverDiag.UartDiag.InstanceErrorFlags[i]) { NodeFont = fontRegular });
                    }
                    diagNode.Nodes.Add(subNode);
                    
                    subNode = new TreeNode("Uart initialized flags");
                    for (int i = 0; i < driverDiag.UartDiag.InstanceInitialized.Count; i++)
                    {
                        subNode.Nodes.Add(new TreeNode("Instance [" + i + "] initialized: " + driverDiag.UartDiag.InstanceInitialized[i]) { NodeFont = fontRegular });
                    }
                    diagNode.Nodes.Add(subNode);

                    subNode = new TreeNode("Uart framing error counters");
                    for (int i = 0; i < driverDiag.UartDiag.FramingErrorCntr.Count; i++)
                    {
                        subNode.Nodes.Add(new TreeNode("Instance [" + i + "] framing error counter: " + driverDiag.UartDiag.FramingErrorCntr[i]) { NodeFont = fontRegular });
                    }
                    diagNode.Nodes.Add(subNode);

                    subNode = new TreeNode("Uart overrun error counters");
                    for (int i = 0; i < driverDiag.UartDiag.OverrunErrorCntr.Count; i++)
                    {
                        subNode.Nodes.Add(new TreeNode("Instance [" + i + "] overrun error counter: " + driverDiag.UartDiag.OverrunErrorCntr[i]) { NodeFont = fontRegular });
                    }
                    diagNode.Nodes.Add(subNode);

                    subNode = new TreeNode("Uart noise error counters");
                    for (int i = 0; i < driverDiag.UartDiag.NoiseErrorCntr.Count; i++)
                    {
                        subNode.Nodes.Add(new TreeNode("Instance [" + i + "] noise error counter: " + driverDiag.UartDiag.NoiseErrorCntr[i]) { NodeFont = fontRegular });
                    }
                    diagNode.Nodes.Add(subNode);

                    subNode = new TreeNode("Uart parity error counters");
                    for (int i = 0; i < driverDiag.UartDiag.ParityErrorCntr.Count; i++)
                    {
                        subNode.Nodes.Add(new TreeNode("Instance [" + i + "] parity error counter: " + driverDiag.UartDiag.ParityErrorCntr[i]) { NodeFont = fontRegular });
                    }
                    diagNode.Nodes.Add(subNode);
                    diagNode.Expand();

                    TreeViewAddNode_ThreadSafe(driverDiagTree, diagNode);
                    
                    diagNode = new TreeNode("Timer diagnostics");
                    diagNode.Nodes.Add(new TreeNode("Global error flags: " + driverDiag.TmrDiag.GlobalErrorFlags) { NodeFont = fontRegular });
                    subNode = new TreeNode("Timer instance error flags");
                    for (int i = 0; i < driverDiag.TmrDiag.InstanceErrorFlags.Count; i++)
                    {
                        subNode.Nodes.Add(new TreeNode("Instance [" + i + "] error flags: " + driverDiag.TmrDiag.InstanceErrorFlags[i]) { NodeFont = fontRegular });
                    }
                    diagNode.Nodes.Add(subNode);

                    subNode = new TreeNode("Timer initialized flags");
                    for (int i = 0; i < driverDiag.TmrDiag.InstanceInitialized.Count; i++)
                    {
                        subNode.Nodes.Add(new TreeNode("Instance [" + i + "] initialized: " + driverDiag.TmrDiag.InstanceInitialized[i]) { NodeFont = fontRegular });
                    }
                    diagNode.Nodes.Add(subNode);
                    diagNode.Expand();

                    TreeViewAddNode_ThreadSafe(driverDiagTree, diagNode);

                    diagNode = new TreeNode("SPI diagnostics");
                    diagNode.Nodes.Add(new TreeNode("Global error flags: " + driverDiag.SpiDiag.GlobalErrorFlags) { NodeFont = fontRegular });
                    subNode = new TreeNode("SPI instance error flags");
                    for (int i = 0; i < driverDiag.SpiDiag.InstanceErrorFlags.Count; i++)
                    {
                        subNode.Nodes.Add(new TreeNode("Instance [" + i + "] error flags: " + driverDiag.SpiDiag.InstanceErrorFlags[i]) { NodeFont = fontRegular });
                    }
                    diagNode.Nodes.Add(subNode);

                    subNode = new TreeNode("SPI initialized flags");
                    for (int i = 0; i < driverDiag.SpiDiag.InstanceInitialized.Count; i++)
                    {
                        subNode.Nodes.Add(new TreeNode("Instance [" + i + "] initialized: " + driverDiag.SpiDiag.InstanceInitialized[i]) { NodeFont = fontRegular });
                    }
                    diagNode.Nodes.Add(subNode);
                    diagNode.Expand();

                    TreeViewAddNode_ThreadSafe(driverDiagTree, diagNode);

                    diagNode = new TreeNode("I2C diagnostics");
                    diagNode.Nodes.Add(new TreeNode("Global error flags: " + driverDiag.I2cDiag.GlobalErrorFlags) { NodeFont = fontRegular });
                    subNode = new TreeNode("I2C instance error flags");
                    for (int i = 0; i < driverDiag.I2cDiag.InstanceErrorFlags.Count; i++)
                    {
                        subNode.Nodes.Add(new TreeNode("Instance [" + i + "] error flags: " + driverDiag.I2cDiag.InstanceErrorFlags[i]) { NodeFont = fontRegular });
                    }
                    diagNode.Nodes.Add(subNode);

                    subNode = new TreeNode("I2C initialized flags");
                    for (int i = 0; i < driverDiag.I2cDiag.InstanceInitialized.Count; i++)
                    {
                        subNode.Nodes.Add(new TreeNode("Instance [" + i + "] initialized: " + driverDiag.I2cDiag.InstanceInitialized[i]) { NodeFont = fontRegular });
                    }
                    diagNode.Nodes.Add(subNode);
                    diagNode.Expand();

                    TreeViewAddNode_ThreadSafe(driverDiagTree, diagNode);
                });
            }
        }

        private delegate void TreeViewAddNodeDelegate(TreeView treeView, TreeNode node);

        private void TreeViewAddNode(TreeView treeView, TreeNode node)
        {
            treeView.Nodes.Add(node);
        }

        private void TreeViewAddNode_ThreadSafe(TreeView treeView, TreeNode node)
        {
            if (treeView.InvokeRequired)
            {
                TreeViewAddNodeDelegate d = new TreeViewAddNodeDelegate(TreeViewAddNode);
                treeView.Invoke(d, treeView, node);
            }
            else
            {
                TreeViewAddNode(treeView, node);
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
