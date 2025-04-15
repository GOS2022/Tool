using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace GOSTool
{
    public partial class LoadGraphUserControl : UserControl
    {
        private int _numOfSamples;
        private ToolTip _toolTip = new ToolTip();
        private Point _mouseLocation = new Point(-100,-100);
        public int NumberOfSamples
        { 
            get
            {
                return _numOfSamples;
            }
            set
            {
                measurementSemaphore.Wait();
                _numOfSamples = value;

                // Adjust size.
                if (measurements.Count != NumberOfSamples)
                {
                    if (measurements.Count > NumberOfSamples)
                    {
                        measurements = measurements.GetRange(measurements.Count - NumberOfSamples, NumberOfSamples);
                    }
                    else
                    {
                        int size = measurements.Count;
                        for (int i = 0; i < NumberOfSamples - size; i++)
                        {
                            measurements.Insert(0, new MonitoringData(-1));
                        }
                    }
                }
                measurementSemaphore.Release();
            }
        }

        private SemaphoreSlim measurementSemaphore = new SemaphoreSlim(1, 1);
        private List<MonitoringData> measurements = new List<MonitoringData>();

        public LoadGraphUserControl()
        {
            InitializeComponent();

            NumberOfSamples = 200;

            for (int i = 0; i < NumberOfSamples; i++)
            {
                measurements.Add(new MonitoringData(-1));
            }
        }

        public void AddNewMeasurement(float value)
        {
            if (value >= 0 && value <= 100)
            {
                measurementSemaphore.Wait();

                MonitoringData newValue = new MonitoringData(value);
                Logger.LogNewMeasurement(newValue);

                List<MonitoringData> shifterMeasurements = new List<MonitoringData>(measurements.GetRange(1, measurements.Count - 1));
                shifterMeasurements.Add(newValue);
                measurements = shifterMeasurements;
                Invalidate();

                measurementSemaphore.Release();
            }
        }

        public float GetAvg()
        {
            float sum = 0;
            int count = 0;

            for (int i = 0; i < measurements.Count; i++)
            {
                if (measurements[i].CpuLoad != -1)
                {
                    sum += measurements[i].CpuLoad;
                    count++;
                }
            }

            return sum / count;
        }

        private void LoadGraphUserControl_Paint(object sender, PaintEventArgs e)
        {
            DrawGrid(e);
            DrawMeasurements(e);
            DrawCursor(e);
        }

        private void LoadGraphUserControl_Resize(object sender, EventArgs e)
        {
            Invalidate();
        }

        private void LoadGraphUserControl_MouseMove(object sender, MouseEventArgs e)
        {
            _mouseLocation.X = e.X;
            _mouseLocation.Y = e.Y;
            Invalidate();
        }

        private void DrawGrid(PaintEventArgs e)
        {
            measurementSemaphore.Wait();

            Graphics g = e.Graphics;
            g.Clear(this.BackColor);
            Pen pen = new Pen(Color.Green);
            Pen linePen = new Pen(Color.Yellow, 2);

            int height = this.ClientSize.Height;
            int width = this.ClientSize.Width;

            float x1, x2, y1, y2;

            for (int i = 10; i < 100; i += 10)
            {
                x1 = 0;
                x2 = width;
                y1 = (float)i / 100f * (float)height;
                y2 = (float)i / 100f * (float)height;

                g.DrawLine(pen, x1, y1, x2, y2);
            }

            for (int i = 5; i < 100; i += 5)
            {
                x1 = (float)i / 100f * (float)width;
                x2 = (float)i / 100f * (float)width;
                y1 = 0;
                y2 = height;

                g.DrawLine(pen, x1, y1, x2, y2);
            }

            measurementSemaphore.Release();
        }

        private void DrawMeasurements(PaintEventArgs e)
        {
            measurementSemaphore.Wait();

            int height = this.ClientSize.Height;
            int width = this.ClientSize.Width;
            float x1, x2, y1, y2;
            Pen linePen = new Pen(Color.Yellow, 2);
            Graphics g = e.Graphics;

            // Paint measurements
            float step = (float)width / (float)(NumberOfSamples - 1);
            int j = 1;

            for (float i = step; (i < width) && (j < NumberOfSamples); i += step)
            {
                x1 = i - step;
                x2 = i;

                y1 = 0.1f * height + 0.8f * height * (100f - measurements[j - 1].CpuLoad) / 100f;

                if (measurements[j - 1].CpuLoad == 0)
                {
                    y1 = 0.9f * height;
                }

                y2 = 0.1f * height + 0.8f * height * (100f - measurements[j].CpuLoad) / 100f;

                if (measurements[j].CpuLoad == 0)
                {
                    y2 = 0.9f * height;
                }

                if (measurements[j - 1].CpuLoad >= 0)
                {
                    g.DrawLine(linePen, x1, y1, x2, y2);
                }
                else
                {
                    x1 = 0;
                    y1 = 0;
                }

                measurements[j - 1].Location = new Point((int)x1, (int)y1);
                j++;
            }

            measurementSemaphore.Release();
        }

        private bool DrawCursor(PaintEventArgs e)
        {
            measurementSemaphore.Wait();

            Graphics g = e.Graphics;
            float[] dashValues = { 5, 2, 15, 4 };
            Pen pen = new Pen(Color.Red, 2);
            bool found = false;
            int height = ClientSize.Height;
            int width = ClientSize.Width;

            for (int i = 0; i < measurements.Count; i++)
            {
                if (measurements[i].CpuLoad < 0)
                {
                    //break;
                }
                else
                {
                    if (Math.Abs(measurements[i].Location.X - _mouseLocation.X) < 3 /*&&
                        Math.Abs(measurements[i].Location.Y - _mouseLocation.Y) < 10*/)
                    {
                        g.DrawLine(pen, measurements[i].Location.X, measurements[i].Location.Y, measurements[i].Location.X, height);
                        g.DrawLine(pen, measurements[i].Location.X, measurements[i].Location.Y, 0, measurements[i].Location.Y);

                        if ((width - _mouseLocation.X) > 180)
                        {
                            g.DrawString("Value: " + measurements[i].CpuLoad + "%\r\nTime: " + measurements[i].TimeStamp.ToString(), new Font("Consolas", 8), new SolidBrush(Color.White),
                                new Point(_mouseLocation.X + 10, _mouseLocation.Y - 20));
                        }
                        else
                        {
                            g.DrawString("Value: " + measurements[i].CpuLoad + "%\r\nTime: " + measurements[i].TimeStamp.ToString(), new Font("Consolas", 8), new SolidBrush(Color.White),
                                new Point(_mouseLocation.X - 170, _mouseLocation.Y - 20));
                        }

                        found = true;
                        break;
                    }
                }
            }

            measurementSemaphore.Release();

            return found;
        }

        private void LoadGraphUserControl_MouseLeave(object sender, EventArgs e)
        {
            _mouseLocation.X = -100;
            _mouseLocation.Y = -100;
            Invalidate();
        }
    }
}
