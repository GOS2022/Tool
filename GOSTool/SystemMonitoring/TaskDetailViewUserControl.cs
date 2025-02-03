using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GOSTool
{
    public partial class TaskDetailViewUserControl : UserControl
    {
        private TaskData _taskData = new TaskData();

        public bool isMonitoringOn = false;

        public int TaskIndex { get; set; }

        public TaskDetailViewUserControl()
        {
            InitializeComponent();
        }

        public void SetTaskData(TaskData taskData)
        {
            _taskData = taskData;
        }

        public async void Activate(MonitoringWindow window)
        {
            isMonitoringOn = true;

            await Task.Run(() =>
            {
                while (isMonitoringOn)
                {
                    TaskVariableData taskData = null;

                    if (window.wireless)
                    {
                        taskData = Wireless.GetTaskVariableData(TaskIndex);
                    }
                    else
                    {
                        taskData = SysmonFunctions.GetTaskVariableData(TaskIndex);
                    }

                    if (!(taskData is null))
                    {
                        var stackUtil = 100f * (float)taskData.TaskStackMaxUsage / _taskData.TaskStackSize;
                        var percentage = taskData.TaskCpuUsage / 100f;
                        stackLoadGraph.AddNewMeasurement(stackUtil);
                        cpuLoadGraph.AddNewMeasurement(percentage);

                        Helper.SetLabelText_ThreadSafe(this, stackUtilLabel, String.Format("{0:0.##}", stackUtil) + "%");
                        Helper.SetLabelText_ThreadSafe(this, cpuUtilLabel, String.Format("{0:0.##}", percentage) + "%");
                        Helper.SetLabelText_ThreadSafe(this, avgCpuUtil, String.Format("{0:0.##}", cpuLoadGraph.GetAvg()) + "%");

                        List<ListViewItem> listViewItems = new List<ListViewItem>();

                        listViewItems.Add(new ListViewItem(new string[] { "Task priority", taskData.TaskPriority.ToString() }));
                        //listViewItems.Add(new ListViewItem(new string[] { "Task original priority", taskDatas[taskIndex].TaskOriginalPriority.ToString() }));
                        listViewItems.Add(new ListViewItem(new string[] { "Task state", Helper.ConvertTaskState(taskData.TaskState) }));
                        listViewItems.Add(new ListViewItem(new string[] { "Task runtime", Helper.ConvertTaskRuntime(taskData.TaskRuntime) }));
                        listViewItems.Add(new ListViewItem(new string[] { "Task CS counter", taskData.TaskCsCounter.ToString() }));
                        listViewItems.Add(new ListViewItem(new string[] { "Task CPU usage", (taskData.TaskCpuUsage / 100f).ToString() + "%" }));
                        listViewItems.Add(new ListViewItem(new string[] { "Task max CPU usage", (taskData.TaskCpuUsageMax / 100f).ToString() + "%" }));
                        listViewItems.Add(new ListViewItem(new string[] { "Task max stack usage", string.Format("0x{0:X4}", taskData.TaskStackMaxUsage) }));

                        Helper.UpdateListViewWithItems_ThreadSafe(this, listView1, listViewItems);
                        Helper.ResizeListView_ThreadSafe(this, listView1);
                    }

                    Thread.Sleep(500);
                }
            });
        }

        public void Deactivate()
        {
            isMonitoringOn = false;
        }

        private void TaskDetailViewUserControl_Load(object sender, EventArgs e)
        {
            splitContainer2.SplitterDistance = splitContainer2.Height / 2;
        }
    }
}
