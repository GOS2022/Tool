using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GOSTool
{
    public partial class SoftwareDownloadWindow : Form
    {
        public bool wireless = false;
        private bool forceQuit = false;
        private List<SdhBinaryDescriptorMessage> binaryDescriptors = new List<SdhBinaryDescriptorMessage>();

        public SoftwareDownloadWindow()
        {
            InitializeComponent();
            usbConfigUserControl1.Show();
            wirelessConfigUserControl1.Hide();
            downloadButton.Enabled = false;
            resetButton.Enabled = false;

            // TODO: test
            //binaryPathTb.Text = "C:\\Users\\Gabor\\STM32CubeIDE\\workspace_1.5.1\\GOS2022_iplTest\\Debug\\GOS2022_iplTest.bin";
            binaryAddrTb.Text = "8020000";

            SvlSdh.BinaryDownloadProgressEvent += (sender, param) =>
            {
                TraceProgressChanging_ThreadSafe("Downloading progress: [" + param.Item1 + " / " + param.Item2 + "] chunks");
                SetProgressBar_ThreadSafe(100 * param.Item1 / param.Item2);
            };
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

        private async void connectButton_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
            binaryTree.Nodes.Clear();        
            selectedBinaryCB.Items.Clear();
            //selectedBinaryCB.SelectedText = "";
            selectedBinaryCB.SelectedValue = "";
            binaryDescriptors.Clear();

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
                        TraceProgressNew_ThreadSafe("Reading software info on " + usbConfigUserControl1.Port + "...");
                    }
                    else
                    {
                        TraceProgressNew_ThreadSafe("Reading software info on " + wirelessConfigUserControl1.Ip + ":" + wirelessConfigUserControl1.Port + "...");
                    }
                    
                    int binaryNum = 0;

                    if (!wireless)
                    {
                        binaryNum = SvlSdh.GetBinaryNum();
                    }
                    else
                    {
                        binaryNum = Wireless.GetBinaryNum();
                    }

                    TraceProgressNew_ThreadSafe("Number of binaries: " + binaryNum);
                    
                    for (int i = 0; i < binaryNum && !forceQuit; i++)
                    {
                        TraceProgressNew_ThreadSafe("Reading binary info [" + (i + 1) + "/" + binaryNum + "] ...");
                        SdhBinaryDescriptorMessage binaryDesc = new SdhBinaryDescriptorMessage();
                        
                        if (!wireless)
                        {
                            binaryDesc = SvlSdh.GetBinaryInfo(i);
                        }
                        else
                        {
                            binaryDesc = Wireless.GetBinaryInfo(i);
                            System.Threading.Thread.Sleep(100);
                        }

                        // TODO: check error handling.
                        if (binaryDesc.Size == 0)
                            return;

                        binaryDescriptors.Add(binaryDesc);
                        
                        TreeNode node = new TreeNode(binaryDesc.Name);
                        node.Nodes.Add("Install date: " + binaryDesc.InstallDate.ToString("yyyy-MM-dd HH:mm:ss"));
                        node.Nodes.Add("Location: 0x" + binaryDesc.Location.ToString("X"));
                        node.Nodes.Add("Start address: 0x" + binaryDesc.StartAddress.ToString("X"));
                        node.Nodes.Add("Size: " + binaryDesc.Size + " bytes");
                        node.Nodes.Add("Crc: 0x" + binaryDesc.Crc.ToString("X"));

                        ComboBoxAddItem_ThreadSafe(binaryDesc.Name);
                        TreeViewAddNode_ThreadSafe(node);
                    }

                    TraceProgressNew_ThreadSafe("Done.");

                    EnableButton_ThreadSafe(downloadButton, true);
                    EnableButton_ThreadSafe(installButton, true);
                    EnableButton_ThreadSafe(eraseButton, true);
                    EnableButton_ThreadSafe(resetButton, true);
                });
            }
        }

        private async void downloadButton_Click(object sender, EventArgs e)
        {
            if (binaryPathTb.Text == "" || binaryAddrTb.Text == "" || binaryNameTb.Text == "")
            {
                MessageBox.Show("Binary path, address, and name fields are mandatory.");
            }
            else
            {
                selectedBinaryCB.Items.Clear();
                selectedBinaryCB.SelectedValue = "";
                binaryDescriptors.Clear();

                TraceProgressNew_ThreadSafe("Reading binary file...");
                SetProgressBar_ThreadSafe(0);

                try
                {
                    List<byte> memoryContent = new List<byte>();
                    memoryContent.AddRange(File.ReadAllBytes(binaryPathTb.Text));

                    SdhBinaryDescriptorMessage testDesc = new SdhBinaryDescriptorMessage()
                    {
                        Name = binaryNameTb.Text,
                        InstallDate = DateTime.Now,
                        StartAddress = UInt32.Parse(binaryAddrTb.Text, System.Globalization.NumberStyles.HexNumber),
                        Size = (UInt32)memoryContent.Count,
                        Crc = Crc.GetCrc32_new(memoryContent.ToArray())
                    };

                    TraceProgressNew_ThreadSafe("Sending download request...");

                    await Task.Run(() =>
                    {
                        if ((!wireless && SvlSdh.SendBinaryDownloadRequest(testDesc) == SdhBinaryDownloadRequestResult.OK) ||
                            (wireless && Wireless.SendBinaryDownloadRequest(testDesc) == SdhBinaryDownloadRequestResult.OK))
                        {
                            TraceProgressNew_ThreadSafe("Starting download...");

                            if ((!wireless && SvlSdh.SendBinary(memoryContent) == true) ||
                                (wireless && Wireless.SendBinary(memoryContent) == true))
                            {
                                TraceProgressNew_ThreadSafe("Download successful.");

                                TraceProgressNew_ThreadSafe("Updating data...");

                                int binaryNum = 0;                              

                                if (!wireless)
                                {
                                    binaryNum = SvlSdh.GetBinaryNum();                                    
                                }
                                else
                                {
                                    binaryNum = Wireless.GetBinaryNum();
                                }

                                TreeViewClear_ThreadSafe();

                                for (int i = 0; i < binaryNum && !forceQuit; i++)
                                {
                                    SdhBinaryDescriptorMessage binaryDesc = new SdhBinaryDescriptorMessage();
                                    
                                    if (!wireless)
                                    {
                                        binaryDesc = SvlSdh.GetBinaryInfo(i);
                                    }
                                    else
                                    {
                                        binaryDesc = Wireless.GetBinaryInfo(i);
                                    }
                                    
                                    binaryDescriptors.Add(binaryDesc);

                                    TreeNode node = new TreeNode(binaryDesc.Name);
                                    node.Nodes.Add("Install date: " + binaryDesc.InstallDate.ToString("yyyy-MM-dd HH:mm:ss"));
                                    node.Nodes.Add("Location: 0x" + binaryDesc.Location.ToString("X"));
                                    node.Nodes.Add("Start address: 0x" + binaryDesc.StartAddress.ToString("X"));
                                    node.Nodes.Add("Size: " + binaryDesc.Size + " bytes");
                                    node.Nodes.Add("Crc: 0x" + binaryDesc.Crc.ToString("X"));

                                    ComboBoxAddItem_ThreadSafe(binaryDesc.Name);
                                    TreeViewAddNode_ThreadSafe(node);
                                }

                                TraceProgressNew_ThreadSafe("Done.");
                            }
                            else
                            {
                                TraceProgressNew_ThreadSafe("Download failed.");
                            }
                        }
                        else
                        {
                            TraceProgressNew_ThreadSafe("Download request failed.");
                        }
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error preparing binary. Message: " + ex.Message);
                }
            }
        }

        private delegate void ComboBoxAddItemDelegate(string item);

        private void ComboBoxAddItem(string item)
        {
            if (item != null)
            {
                selectedBinaryCB.Items.Add(item);
            }
        }

        private void ComboBoxAddItem_ThreadSafe(string item)
        {
            if (selectedBinaryCB.InvokeRequired)
            {
                ComboBoxAddItemDelegate d = new ComboBoxAddItemDelegate(ComboBoxAddItem);
                selectedBinaryCB.Invoke(d, item);
            }
            else
            {
                ComboBoxAddItem(item);
            }
        }

        private delegate void TreeViewClearDelegate();

        private void TreeViewClear()
        {
            binaryTree.Nodes.Clear();
        }

        private void TreeViewClear_ThreadSafe()
        {
            if (binaryTree.InvokeRequired)
            {
                TreeViewClearDelegate d = new TreeViewClearDelegate(TreeViewClear);
                binaryTree.Invoke(d);
            }
            else
            {
                TreeViewClear();
            }
        }

        private delegate void TreeViewAddNodeDelegate(TreeNode node);        

        private void TreeViewAddNode(TreeNode node)
        {
            binaryTree.Nodes.Add(node);
        }

        private void TreeViewAddNode_ThreadSafe(TreeNode node)
        {
            if (binaryTree.InvokeRequired)
            {
                TreeViewAddNodeDelegate d = new TreeViewAddNodeDelegate(TreeViewAddNode);
                binaryTree.Invoke(d, node);
            }
            else
            {
                TreeViewAddNode(node);
            }
        }

        private delegate void TraceProgressChangingDelegate(string message);

        private void TraceProgressChanging(string message)
        {
            richTextBox1.Lines = richTextBox1.Lines.Take(richTextBox1.Lines.Length - 2).ToArray();
            richTextBox1.Text += "\r\n[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "]\t" + message + "\r\n";
        }

        private void TraceProgressChanging_ThreadSafe(string message)
        {
            if (richTextBox1.InvokeRequired)
            {
                TraceProgressChangingDelegate d = new TraceProgressChangingDelegate(TraceProgressChanging);
                richTextBox1.Invoke(d, message);
            }
            else
            {
                TraceProgressChanging(message);
            }
        }

        private delegate void TraceProgressNewDelegate(string message);

        private void TraceProgressNew(string message)
        {
            richTextBox1.Text += "[" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "]\t" + message + "\r\n";
        }

        private void TraceProgressNew_ThreadSafe(string message)
        {
            if (richTextBox1.InvokeRequired)
            {
                TraceProgressNewDelegate d = new TraceProgressNewDelegate(TraceProgressNew);
                richTextBox1.Invoke(d, message);
            }
            else
            {
                TraceProgressNew(message);
            }
        }

        private delegate void EnableButtonDelegate(Button button, bool enabled);

        private void EnableButton(Button button, bool enabled)
        {
            button.Enabled = enabled;
        }

        private void EnableButton_ThreadSafe(Button button, bool enabled)
        {
            if (button.InvokeRequired)
            {
                EnableButtonDelegate d = new EnableButtonDelegate(EnableButton);
                button.Invoke(d, button, enabled);
            }
            else
            {
                EnableButton(button, enabled);
            }
        }

        private delegate void SetProgressBarDelegate(int percentage);

        private void SetProgressBar(int percentage)
        {
            progressBar1.Value = percentage;
            progressLabel.Text = percentage + "%";
        }

        private void SetProgressBar_ThreadSafe(int percentage)
        {
            if (progressBar1.InvokeRequired)
            {
                SetProgressBarDelegate d = new SetProgressBarDelegate(SetProgressBar);
                progressBar1.Invoke(d, percentage);
            }
            else
            {
                SetProgressBar(percentage);
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            // set the current caret position to the end
            richTextBox1.SelectionStart = richTextBox1.Text.Length;
            // scroll it automatically
            richTextBox1.ScrollToCaret();
        }

        private async void resetButton_Click(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                if (!wireless)
                {
                    SysmonFunctions.SendResetRequest();
                }
                else
                {
                    Wireless.SendResetRequest();
                }
            });
        }

        private async void installButton_Click(object sender, EventArgs e)
        {
            if (selectedBinaryCB.SelectedItem == null)
            {
                MessageBox.Show("First select a binary to install.");
            }
            else
            {
                int index = selectedBinaryCB.SelectedIndex;
                string fileName = selectedBinaryCB.SelectedItem.ToString();

                await Task.Run(() =>
                {
                    if (!wireless)
                    {
                        SvlSdh.SendInstallRequest(index);
                        TraceProgressNew_ThreadSafe("Install request sent for " + fileName + ".");
                        SysmonFunctions.SendResetRequest();
                    }
                    else
                    {
                        Wireless.SendInstallRequest(index);
                        TraceProgressNew_ThreadSafe("Install request sent for " + fileName + ".");
                        Wireless.SendResetRequest();
                    }
                });
            }
        }

        private void browseButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                binaryPathTb.Text = openFileDialog.FileName;
            }
        }

        private async void eraseButton_Click(object sender, EventArgs e)
        {
            if (selectedBinaryCB.SelectedItem == null)
            {
                MessageBox.Show("First select a binary to erase.");
            }
            else
            {
                int index = selectedBinaryCB.SelectedIndex;
                string binaryName = selectedBinaryCB.SelectedItem.ToString();
                //binaryTree.Nodes.Clear();
                selectedBinaryCB.Items.Clear();
                selectedBinaryCB.SelectedValue = "";
                //selectedBinaryCB.SelectedText = "";
                binaryDescriptors.Clear();

                await Task.Run(() =>
                {
                    TraceProgressNew_ThreadSafe("Erase request sent for " + binaryName + ". This might take a while...");

                    if (!wireless)
                    {
                        SvlSdh.SendEraseRequest(index);
                    }
                    else
                    {
                        Wireless.SendEraseRequest(index);
                    }

                    TraceProgressNew_ThreadSafe("Updating data...");

                    int binaryNum = 0;

                    if (!wireless)
                    {
                        binaryNum = SvlSdh.GetBinaryNum();
                    }
                    else
                    {
                        binaryNum = Wireless.GetBinaryNum();
                    }

                    TreeViewClear_ThreadSafe();

                    for (int i = 0; i < binaryNum && !forceQuit; i++)
                    {
                        SdhBinaryDescriptorMessage binaryDesc = new SdhBinaryDescriptorMessage();
                        
                        if (!wireless)
                        {
                            binaryDesc = SvlSdh.GetBinaryInfo(i);
                        }
                        else
                        {
                            binaryDesc = Wireless.GetBinaryInfo(i);
                        }
                                              
                        binaryDescriptors.Add(binaryDesc);

                        TreeNode node = new TreeNode(binaryDesc.Name);
                        node.Nodes.Add("Location: 0x" + binaryDesc.Location.ToString("X"));
                        node.Nodes.Add("Start address: 0x" + binaryDesc.StartAddress.ToString("X"));
                        node.Nodes.Add("Size: " + binaryDesc.Size + " bytes");
                        node.Nodes.Add("Crc: 0x" + binaryDesc.Crc.ToString("X"));

                        ComboBoxAddItem_ThreadSafe(binaryDesc.Name);
                        TreeViewAddNode_ThreadSafe(node);
                    }

                    TraceProgressNew_ThreadSafe("Done.");
                });
            }
        }

        private void SoftwareDownloadWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            forceQuit = true;
        }
    }
}
