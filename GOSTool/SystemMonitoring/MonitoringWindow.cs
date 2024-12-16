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

namespace GOSTool
{
    public partial class MonitoringWindow : Form
    {
        private bool isMonitoringOn = false;

        private System.Windows.Forms.Timer monitoringTimer;

        private DateTime monitoringTime = new DateTime();

        private int focusedIdx = 0;

        private ContextMenu taskCm = new ContextMenu();

        private List<TaskData> taskData = new List<TaskData>();

        public bool wireless = false;

        public MonitoringWindow()
        {
            InitializeComponent();
        }

        private void MonitoringWindow_Load(object sender, EventArgs e)
        {
            Text = ProgramData.Name + " " + ProgramData.Version + " - System monitoring";

            wirelessConfigUserControl1.Hide();

            taskCm.MenuItems.Add("Show details", new EventHandler(ShowTaskDetails));
            taskCm.MenuItems.Add("Suspend", new EventHandler(SuspendTask));
            taskCm.MenuItems.Add("Resume", new EventHandler(ResumeTask));
            taskCm.MenuItems.Add("Delete", new EventHandler(DeleteTask));
            taskCm.MenuItems.Add("Block", new EventHandler(BlockTask));
            taskCm.MenuItems.Add("Unblock", new EventHandler(UnblockTask));
            taskCm.MenuItems.Add("Wake up", new EventHandler(WakeupTask));

            int prevSelectedTabIndex = 0;

            tabControl1.SelectedIndexChanged += (tsender, te) =>
            {
                if (prevSelectedTabIndex > 0)
                {
                    // Deactivate previous tab.
                    (tabControl1.TabPages[prevSelectedTabIndex].Controls[0] as TaskDetailViewUserControl).Deactivate();
                }

                if (tabControl1.SelectedIndex > 0)
                {
                    // Activate new tab.
                    (tabControl1.TabPages[tabControl1.SelectedIndex].Controls[0] as TaskDetailViewUserControl).Activate(this);
                    prevSelectedTabIndex = tabControl1.SelectedIndex;
                }
            };
        }

        private async void connectButton_Click(object sender, EventArgs e)
        {
            if (/*usbComRadioButton.Checked*/true)
            {
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
                        Uart.Init(usbConfigUserControl1.Port, usbConfigUserControl1.Baud);

                        if ((!wireless && SysmonFunctions.PingDevice() == SysmonFunctions.PingResult.OK) ||
                            (wireless && Wireless.PingDevice() == SysmonFunctions.PingResult.OK))
                        {
                            Helper.SetCheckBoxParameters_ThreadSafe(this, linkActiveCheckBox, "Link active", Color.Green, true);
                            List<TaskData> taskDatas = new List<TaskData>();

                            // Get all task data.
                            if (wireless)
                            {
                                taskDatas = Wireless.GetAllTaskData();
                            }
                            else
                            {
                                taskDatas = SysmonFunctions.GetAllTaskData();
                            }

                            List <ListViewItem> listViewItems = new List<ListViewItem>();
                            taskData = taskDatas;

                            foreach (var taskData in taskDatas)
                            {
                                string[] row = {
                                    string.Format("0x{0:X4}", taskData.TaskId),
                                    taskData.TaskName,
                                    string.Format("0x{0:X4}", taskData.TaskStackSize),
                                    taskData.TaskPriority.ToString(),
                                    ((float)taskData.TaskCpuUsageLimit / 100f).ToString() + "%",
                                    Convert.ToString(taskData.TaskPrivileges, 2).PadLeft(16, '0'),
                                };

                                var listViewItem = new ListViewItem(row);
                                listViewItems.Add(listViewItem);
                            }

                            Helper.UpdateListViewWithItems_ThreadSafe(this, taskListView, listViewItems);
                            Helper.ResizeListView_ThreadSafe(this, taskListView);

                            Thread.Sleep(100);

                            // Get software info.
                            BootloaderData softwareInfo = new BootloaderData();
                            if (wireless)
                            {
                                softwareInfo = Wireless.GetSoftwareInfo();
                            }
                            else
                            {
                                softwareInfo = SysmonFunctions.GetSoftwareInfo();
                            }
                             
                            List<ListViewItem> swInfoItems = new List<ListViewItem>();

                            swInfoItems.Add(new ListViewItem(new string[] { "Bootloader driver lib name", softwareInfo.BootloaderDriverInfo.Name }));
                            swInfoItems.Add(new ListViewItem(new string[] { "Bootloader driver lib version", softwareInfo.BootloaderDriverInfo.Major.ToString("D2") + "." + softwareInfo.BootloaderDriverInfo.Minor.ToString("D2") + "." + softwareInfo.BootloaderDriverInfo.Build.ToString("D2") }));
                            swInfoItems.Add(new ListViewItem(new string[] { "Bootloader driver lib author", softwareInfo.BootloaderDriverInfo.Author }));
                            swInfoItems.Add(new ListViewItem(new string[] { "Bootloader driver lib description", softwareInfo.BootloaderDriverInfo.Description }));
                            swInfoItems.Add(new ListViewItem(new string[] { "Bootloader driver lib date", softwareInfo.BootloaderDriverInfo.Date.ToString("yyyy-MM-dd") }));

                            swInfoItems.Add(new ListViewItem(new string[] { "Bootloader name", softwareInfo.BootloaderInfo.Name }));
                            swInfoItems.Add(new ListViewItem(new string[] { "Bootloader version", softwareInfo.BootloaderInfo.Major.ToString("D2") + "." + softwareInfo.BootloaderInfo.Minor.ToString("D2") + "." + softwareInfo.BootloaderInfo.Build.ToString("D2") }));
                            swInfoItems.Add(new ListViewItem(new string[] { "Bootloader author", softwareInfo.BootloaderInfo.Author }));
                            swInfoItems.Add(new ListViewItem(new string[] { "Bootloader description", softwareInfo.BootloaderInfo.Description }));
                            swInfoItems.Add(new ListViewItem(new string[] { "Bootloader date", softwareInfo.BootloaderInfo.Date.ToString("yyyy-MM-dd") }));
                            swInfoItems.Add(new ListViewItem(new string[] { "Bootloader address", "0x" + softwareInfo.StartAddress.ToString("X8") }));
                            swInfoItems.Add(new ListViewItem(new string[] { "Bootloader size", softwareInfo.Size.ToString() }));
                            swInfoItems.Add(new ListViewItem(new string[] { "Bootloader CRC", "0x" + softwareInfo.Crc.ToString("X8") }));
                            swInfoItems.Add(new ListViewItem(new string[] { "Bootloader update mode", softwareInfo.BootUpdateMode.ToString() }));

                            swInfoItems.Add(new ListViewItem(new string[] { "Application driver lib name", softwareInfo.ApplicationData.DriverVersion.Name }));
                            swInfoItems.Add(new ListViewItem(new string[] { "Application driver lib version", softwareInfo.ApplicationData.DriverVersion.Major.ToString("D2") + "." + softwareInfo.ApplicationData.DriverVersion.Minor.ToString("D2") + "." + softwareInfo.ApplicationData.DriverVersion.Build.ToString("D2") }));
                            swInfoItems.Add(new ListViewItem(new string[] { "Application driver lib author", softwareInfo.ApplicationData.DriverVersion.Author }));
                            swInfoItems.Add(new ListViewItem(new string[] { "Application driver lib description", softwareInfo.ApplicationData.DriverVersion.Description }));
                            swInfoItems.Add(new ListViewItem(new string[] { "Application driver lib date", softwareInfo.ApplicationData.DriverVersion.Date.ToString("yyyy-MM-dd") }));

                            swInfoItems.Add(new ListViewItem(new string[] { "Application name", softwareInfo.ApplicationData.AppVersion.Name }));
                            swInfoItems.Add(new ListViewItem(new string[] { "Application version", softwareInfo.ApplicationData.AppVersion.Major.ToString("D2") + "." + softwareInfo.ApplicationData.AppVersion.Minor.ToString("D2") + "." + softwareInfo.ApplicationData.AppVersion.Build.ToString("D2") }));
                            swInfoItems.Add(new ListViewItem(new string[] { "Application author", softwareInfo.ApplicationData.AppVersion.Author }));
                            swInfoItems.Add(new ListViewItem(new string[] { "Application description", softwareInfo.ApplicationData.AppVersion.Description }));
                            swInfoItems.Add(new ListViewItem(new string[] { "Application date", softwareInfo.ApplicationData.AppVersion.Date.ToString("yyyy-MM-dd") }));
                            swInfoItems.Add(new ListViewItem(new string[] { "Application address", "0x" + softwareInfo.ApplicationData.StartAddress.ToString("X8") }));
                            swInfoItems.Add(new ListViewItem(new string[] { "Application size", softwareInfo.ApplicationData.Size.ToString() }));
                            swInfoItems.Add(new ListViewItem(new string[] { "Application CRC", "0x" + softwareInfo.ApplicationData.Crc.ToString("X8") }));

                            Helper.UpdateListViewWithItems_ThreadSafe(this, swInfoListView, swInfoItems);
                            Helper.ResizeListView_ThreadSafe(this, swInfoListView);

                            Thread.Sleep(100);
                        }
                        else
                        {
                            Uart.DeInit();
                            Helper.SetCheckBoxParameters_ThreadSafe(this, linkActiveCheckBox, "Link inactive", Color.Gray, false);
                        }
                    });   
                }
            }
            else
            {
                // To be implemented later.
            }
        }

        private async void resetRequestButton_Click(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                if (wireless)
                {
                    Wireless.SendResetRequest();
                }
                else
                {
                    SysmonFunctions.SendResetRequest();
                }
            });
        }

        private async void monitoringButton_Click(object sender, EventArgs e)
        {
            isMonitoringOn = !isMonitoringOn;

            if (isMonitoringOn)
            {
                monitoringButton.Text = "Stop";
                monitoringButton.BackColor = Color.Red;

                // Start monitoring.
                monitoringTime = DateTime.Now;
                monitoringTimer = new System.Windows.Forms.Timer();
                monitoringTimer.Interval = 100;
                monitoringTimer.Tick += MonitoringTimer_Tick;
                monitoringTimer.Start();
            }
            else
            {
                monitoringButton.Text = "Monitoring";
                monitoringButton.BackColor = Color.Green;
                monitoringTimer.Stop();
            }

            if (isMonitoringOn)
            {
                await Task.Run(() =>
                {
                    while (isMonitoringOn)
                    {
                        float percentage = -1;

                        if (wireless)
                        {
                            percentage = Wireless.GetCpuLoad();
                        }
                        else
                        {
                            percentage = SysmonFunctions.GetCpuLoad();
                        }

                        cpuLoadGraph.AddNewMeasurement(percentage);

                        Helper.SetLabelText_ThreadSafe(this, currentCpuUtil, String.Format("{0:0.##}", percentage) + "%");
                        Helper.SetLabelText_ThreadSafe(this, avgCpuUtil, String.Format("{0:0.##}", cpuLoadGraph.GetAvg()) + "%");
                        
                        Thread.Sleep(250);

                        // Refresh system runtime.
                        if (wireless)
                        {
                            Helper.SetLabelText_ThreadSafe(this, sysRuntimeLabel, Wireless.GetSystemRuntime());
                        }
                        else
                        {
                            Helper.SetLabelText_ThreadSafe(this, sysRuntimeLabel, SysmonFunctions.GetSystemRuntime());
                        }

                        Thread.Sleep(250);
                    }
                });
            }
        }

        private void MonitoringTimer_Tick(object sender, EventArgs e)
        {
            TimeSpan elapsedTime = (DateTime.Now - monitoringTime);
            monitoringTimeLabel.Text = elapsedTime.Days.ToString("D2") + ":" +
                elapsedTime.Hours.ToString("D2") + ":" + elapsedTime.Minutes.ToString("D2") + ":" +
                elapsedTime.Seconds.ToString("D2") + ":" + elapsedTime.Milliseconds.ToString("D3");
        }

        private void sampleNUD_ValueChanged(object sender, EventArgs e)
        {
            cpuLoadGraph.NumberOfSamples = (int)sampleNUD.Value;
        }

        private void MonitoringWindow_FormClosing(object sender, FormClosingEventArgs e)
        {            
            isMonitoringOn = false;
            Uart.DeInit();
        }

        private void taskListView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                try
                {
                    var focusedItem = taskListView.FocusedItem;
                    focusedIdx = taskListView.FocusedItem.Index;
                    if (focusedItem != null && focusedItem.Bounds.Contains(e.Location))
                    {
                        taskCm.Show(taskListView, focusedItem.Position);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void ShowTaskDetails(object sender, EventArgs e)
        {
            TabPage taskDataTabPage = new TabPage(taskData[focusedIdx].TaskName);
            TaskDetailViewUserControl taskDataView = new TaskDetailViewUserControl() { Dock = DockStyle.Fill };
            taskDataView.SetTaskData(taskData[focusedIdx]);
            taskDataView.TaskIndex = focusedIdx;
            taskDataTabPage.Controls.Add(taskDataView);

            if (tabControl1.TabPages.IndexOf(taskDataTabPage) == -1)
            {
                tabControl1.TabPages.Add(taskDataTabPage);
            }
        }

        private async void SuspendTask(object sender, EventArgs e)
        {
            try
            {
                var itemIndex = focusedIdx;

                await Task.Run(() =>
                {
                    if (wireless)
                    {
                        Wireless.ModifyTask(itemIndex, IplTaskModificationType.IPL_TASK_MODIFY_SUSPEND);
                    }
                    else
                    {
                        SysmonFunctions.ModifyTask(itemIndex, SysmonTaskModifyType.GOS_SYSMON_TASK_MOD_TYPE_SUSPEND);
                    }
                });
            }
            catch
            {

            }
        }

        private async void ResumeTask(object sender, EventArgs e)
        {
            try
            {
                var itemIndex = focusedIdx;

                await Task.Run(() =>
                {
                    if (wireless)
                    {
                        Wireless.ModifyTask(itemIndex, IplTaskModificationType.IPL_TASK_MODIFY_RESUME);
                    }
                    else
                    {
                        SysmonFunctions.ModifyTask(itemIndex, SysmonTaskModifyType.GOS_SYSMON_TASK_MOD_TYPE_RESUME);
                    }
                });
            }
            catch
            {

            }
        }

        private async void DeleteTask(object sender, EventArgs e)
        {
            try
            {
                var itemIndex = focusedIdx;

                await Task.Run(() =>
                {
                    if (wireless)
                    {
                        Wireless.ModifyTask(itemIndex, IplTaskModificationType.IPL_TASK_MODIFY_DELETE);
                    }
                    else
                    {
                        SysmonFunctions.ModifyTask(itemIndex, SysmonTaskModifyType.GOS_SYSMON_TASK_MOD_TYPE_DELETE);
                    }
                });
            }
            catch
            {

            }
        }

        private async void BlockTask(object sender, EventArgs e)
        {
            try
            {
                var itemIndex = focusedIdx;

                await Task.Run(() =>
                {
                    if (wireless)
                    {
                        Wireless.ModifyTask(itemIndex, IplTaskModificationType.IPL_TASK_MODIFY_BLOCK);
                    }
                    else
                    {
                        SysmonFunctions.ModifyTask(itemIndex, SysmonTaskModifyType.GOS_SYSMON_TASK_MOD_TYPE_BLOCK);
                    }
                });
            }
            catch
            {

            }
        }

        private async void UnblockTask(object sender, EventArgs e)
        {
            try
            {
                var itemIndex = focusedIdx;

                await Task.Run(() =>
                {
                    if (wireless)
                    {
                        Wireless.ModifyTask(itemIndex, IplTaskModificationType.IPL_TASK_MODIFY_UNBLOCK);
                    }
                    else
                    {
                        SysmonFunctions.ModifyTask(itemIndex, SysmonTaskModifyType.GOS_SYSMON_TASK_MOD_TYPE_UNBLOCK);
                    }
                });
            }
            catch
            {

            }
        }

        private async void WakeupTask(object sender, EventArgs e)
        {
            try
            {
                var itemIndex = focusedIdx;

                await Task.Run(() =>
                {
                    if (wireless)
                    {
                        Wireless.ModifyTask(itemIndex, IplTaskModificationType.IPL_TASK_MODIFY_WAKEUP);
                    }
                    else
                    {
                        SysmonFunctions.ModifyTask(itemIndex, SysmonTaskModifyType.GOS_SYSMON_TASK_MOD_TYPE_WAKEUP);
                    }
                });
            }
            catch
            {

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
    }
}
