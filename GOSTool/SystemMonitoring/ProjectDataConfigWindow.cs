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
    public partial class ProjectDataConfigWindow : Form
    {
        private bool wireless = false;

        public ProjectDataConfigWindow()
        {
            InitializeComponent();
        }

        private async void readButton_Click(object sender, EventArgs e)
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
                    if (!wireless)
                    {
                        Uart.Init(usbConfigUserControl1.Port, usbConfigUserControl1.Baud);
                    }

                    List<string[]> swInfoItems = new List<string[]>();
                    SoftwareInfo softwareInfo = new SoftwareInfo();

                    // Get software info.
                    if (wireless)
                    {
                        softwareInfo = Wireless.GetSoftwareInfo();
                    }
                    else
                    {
                        softwareInfo = SysmonFunctions.GetSoftwareInfo();
                    }

                    if (!(softwareInfo.AppSwVerInfo.Name is null) && softwareInfo.AppSwVerInfo.Name != "")
                    {
                        swInfoItems.Add((new string[] { "Application driver lib name", softwareInfo.AppLibVerInfo.Name }));
                        swInfoItems.Add((new string[] { "Application driver lib version", softwareInfo.AppLibVerInfo.Major.ToString("D2") + "." + softwareInfo.AppLibVerInfo.Minor.ToString("D2") + "." + softwareInfo.AppLibVerInfo.Build.ToString("D2") }));
                        swInfoItems.Add((new string[] { "Application driver lib author", softwareInfo.AppLibVerInfo.Author }));
                        swInfoItems.Add((new string[] { "Application driver lib description", softwareInfo.AppLibVerInfo.Description }));
                        swInfoItems.Add((new string[] { "Application driver lib date", softwareInfo.AppLibVerInfo.Date.ToString("yyyy-MM-dd") }));

                        swInfoItems.Add((new string[] { "Application name", softwareInfo.AppSwVerInfo.Name }));
                        swInfoItems.Add((new string[] { "Application version", softwareInfo.AppSwVerInfo.Major.ToString("D2") + "." + softwareInfo.AppSwVerInfo.Minor.ToString("D2") + "." + softwareInfo.AppSwVerInfo.Build.ToString("D2") }));
                        swInfoItems.Add((new string[] { "Application author", softwareInfo.AppSwVerInfo.Author }));
                        swInfoItems.Add((new string[] { "Application description", softwareInfo.AppSwVerInfo.Description }));
                        swInfoItems.Add((new string[] { "Application date", softwareInfo.AppSwVerInfo.Date.ToString("yyyy-MM-dd") }));

                        swInfoItems.Add((new string[] { "Application OS version", softwareInfo.AppOsInfo.Major.ToString("D2") + "." + softwareInfo.AppOsInfo.Minor.ToString("D2") }));

                        swInfoItems.Add((new string[] { "Application address", "0x" + softwareInfo.AppBinaryInfo.StartAddress.ToString("X8") }));
                        swInfoItems.Add((new string[] { "Application size", softwareInfo.AppBinaryInfo.Size.ToString() }));
                        swInfoItems.Add((new string[] { "Application CRC", "0x" + softwareInfo.AppBinaryInfo.Crc.ToString("X8") }));
                    }

                    if (softwareInfo.BldSwVerInfo.Name != "")
                    {
                        swInfoItems.Add((new string[] { "Bootloader driver lib name", softwareInfo.BldLibVerInfo.Name }));
                        swInfoItems.Add((new string[] { "Bootloader driver lib version", softwareInfo.BldLibVerInfo.Major.ToString("D2") + "." + softwareInfo.BldLibVerInfo.Minor.ToString("D2") + "." + softwareInfo.BldLibVerInfo.Build.ToString("D2") }));
                        swInfoItems.Add((new string[] { "Bootloader driver lib author", softwareInfo.BldLibVerInfo.Author }));
                        swInfoItems.Add((new string[] { "Bootloader driver lib description", softwareInfo.BldLibVerInfo.Description }));
                        swInfoItems.Add((new string[] { "Bootloader driver lib date", softwareInfo.BldLibVerInfo.Date.ToString("yyyy-MM-dd") }));

                        swInfoItems.Add((new string[] { "Bootloader name", softwareInfo.BldSwVerInfo.Name }));
                        swInfoItems.Add((new string[] { "Bootloader version", softwareInfo.BldSwVerInfo.Major.ToString("D2") + "." + softwareInfo.BldSwVerInfo.Minor.ToString("D2") + "." + softwareInfo.BldSwVerInfo.Build.ToString("D2") }));
                        swInfoItems.Add((new string[] { "Bootloader author", softwareInfo.BldSwVerInfo.Author }));
                        swInfoItems.Add((new string[] { "Bootloader description", softwareInfo.BldSwVerInfo.Description }));
                        swInfoItems.Add(new string[] { "Bootloader date", softwareInfo.BldSwVerInfo.Date.ToString("yyyy-MM-dd") });

                        swInfoItems.Add((new string[] { "Bootloader OS version", softwareInfo.BldOsInfo.Major.ToString("D2") + "." + softwareInfo.BldOsInfo.Minor.ToString("D2") }));

                        swInfoItems.Add((new string[] { "Bootloader address", "0x" + softwareInfo.BldBinaryInfo.StartAddress.ToString("X8") }));
                        swInfoItems.Add((new string[] { "Bootloader size", softwareInfo.BldBinaryInfo.Size.ToString() }));
                        swInfoItems.Add((new string[] { "Bootloader CRC", "0x" + softwareInfo.BldBinaryInfo.Crc.ToString("X8") }));
                    }

                    Helper.UpdateDataGridViewWithItems_ThreadSafe(this, swInfoGridView, swInfoItems);

                    Thread.Sleep(100);

                    List<string[]> hwInfoItems = new List<string[]>();
                    HardwareInfo hardwareInfo = new HardwareInfo();

                    // Get software info.
                    if (wireless)
                    {
                        hardwareInfo = Wireless.GetHardwareInfo();
                    }
                    else
                    {
                        hardwareInfo = SysmonFunctions.GetHardwareInfo();
                    }

                    if (hardwareInfo.BoardName != null)
                    {
                        hwInfoItems.Add((new string[] { "Board name", hardwareInfo.BoardName }));
                        hwInfoItems.Add((new string[] { "Revision", hardwareInfo.Revision }));
                        hwInfoItems.Add((new string[] { "Author", hardwareInfo.Author }));
                        hwInfoItems.Add((new string[] { "Description", hardwareInfo.Description }));
                        hwInfoItems.Add((new string[] { "Date", hardwareInfo.Date.ToString("yyyy-MM-dd") }));
                        hwInfoItems.Add((new string[] { "Serial number", hardwareInfo.SerialNumber }));

                        Helper.UpdateDataGridViewWithItems_ThreadSafe(this, hwInfoGridView, hwInfoItems);

                        Thread.Sleep(100);
                    }

                    List<string[]> wifiCfgItems = new List<string[]>();
                    WifiConfig wifiCfg = new WifiConfig();

                    // Get software info.
                    if (wireless)
                    {
                        //wifiCfg = Wireless.GetWifiConfig();
                    }
                    else
                    {
                        wifiCfg = SysmonFunctions.GetWifiConfig();
                    }

                    if (wifiCfg.Ssid != null)
                    {
                        wifiCfgItems.Add((new string[] { "SSID", wifiCfg.Ssid }));
                        wifiCfgItems.Add((new string[] { "Password", wifiCfg.Pwd }));
                        wifiCfgItems.Add((new string[] { "IP address", wifiCfg.IpAddress }));
                        wifiCfgItems.Add((new string[] { "Gateway", wifiCfg.GateWay }));
                        wifiCfgItems.Add((new string[] { "Subnet", wifiCfg.Subnet }));
                        wifiCfgItems.Add((new string[] { "Port", wifiCfg.Port.ToString() }));

                        Helper.UpdateDataGridViewWithItems_ThreadSafe(this, wifiCfgGridView, wifiCfgItems);

                        Thread.Sleep(100);
                    }

                    List<string[]> bldConfigItems = new List<string[]>();
                    BootloaderConfig bldConfig = new BootloaderConfig();

                    // Get software info.
                    if (wireless)
                    {
                        //bldConfig = Wireless.GetBootloaderConfig();
                    }
                    else
                    {
                        bldConfig = SysmonFunctions.GetBootloaderConfig();
                    }

                    bldConfigItems.Add((new string[] { "Install requested", bldConfig.InstallRequested.ToString() }));
                    bldConfigItems.Add((new string[] { "Reserved", bldConfig.Reserved.ToString() }));
                    bldConfigItems.Add((new string[] { "Bianry index", bldConfig.BinaryIndex.ToString() }));
                    bldConfigItems.Add((new string[] { "Update mode", bldConfig.UpdateMode.ToString() }));
                    //bldConfigItems.Add((new string[] { "Wireless update", bldConfig.WirelessUpdate.ToString() }));
                    //bldConfigItems.Add((new string[] { "Wait for connection on startup", bldConfig.WaitForConnectionOnStartup.ToString() }));
                    bldConfigItems.Add((new string[] { "Startup counter", bldConfig.StartupCounter.ToString() }));
                    //bldConfigItems.Add((new string[] { "Connection timeout", bldConfig.ConnectionTimeout.ToString() }));
                    bldConfigItems.Add((new string[] { "Request timeout", bldConfig.RequestTimeout.ToString() }));
                    bldConfigItems.Add((new string[] { "Install timeout", bldConfig.InstallTimeout.ToString() }));

                    Helper.UpdateDataGridViewWithItems_ThreadSafe(this, bldCfgGridView, bldConfigItems);

                    Thread.Sleep(100);
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

        private void ProjectDataConfigWindow_Load(object sender, EventArgs e)
        {
            wirelessConfigUserControl1.Hide();
        }

        private async void writeButton_Click(object sender, EventArgs e)
        {
            int activeTab = tabControl1.SelectedIndex;

            switch (activeTab)
            {
                case 0:
                    {
                        // SW info.
                        SoftwareInfo swInfoToSet = new SoftwareInfo();
                        int idx = 0;
                        if (swInfoGridView.Rows[0].Cells[0].Value.ToString() == "Application driver lib name")
                        {
                            swInfoToSet.AppLibVerInfo.Name = swInfoGridView.Rows[idx++].Cells[1].Value.ToString();
                            swInfoToSet.AppLibVerInfo.Major = UInt16.Parse(swInfoGridView.Rows[idx].Cells[1].Value.ToString().Split('.')[0]);
                            swInfoToSet.AppLibVerInfo.Minor = UInt16.Parse(swInfoGridView.Rows[idx].Cells[1].Value.ToString().Split('.')[1]);
                            swInfoToSet.AppLibVerInfo.Build = UInt16.Parse(swInfoGridView.Rows[idx++].Cells[1].Value.ToString().Split('.')[2]);
                            swInfoToSet.AppLibVerInfo.Author = swInfoGridView.Rows[idx++].Cells[1].Value.ToString();
                            swInfoToSet.AppLibVerInfo.Description = swInfoGridView.Rows[idx++].Cells[1].Value.ToString();
                            swInfoToSet.AppLibVerInfo.Date = DateTime.Parse(swInfoGridView.Rows[idx++].Cells[1].Value.ToString());
                            swInfoToSet.AppSwVerInfo.Name = swInfoGridView.Rows[idx++].Cells[1].Value.ToString();
                            swInfoToSet.AppSwVerInfo.Major = UInt16.Parse(swInfoGridView.Rows[idx].Cells[1].Value.ToString().Split('.')[0]);
                            swInfoToSet.AppSwVerInfo.Minor = UInt16.Parse(swInfoGridView.Rows[idx].Cells[1].Value.ToString().Split('.')[1]);
                            swInfoToSet.AppSwVerInfo.Build = UInt16.Parse(swInfoGridView.Rows[idx++].Cells[1].Value.ToString().Split('.')[2]);
                            swInfoToSet.AppSwVerInfo.Author = swInfoGridView.Rows[idx++].Cells[1].Value.ToString();
                            swInfoToSet.AppSwVerInfo.Description = swInfoGridView.Rows[idx++].Cells[1].Value.ToString();
                            swInfoToSet.AppSwVerInfo.Date = DateTime.Parse(swInfoGridView.Rows[idx++].Cells[1].Value.ToString());
                            swInfoToSet.AppOsInfo.Major = UInt16.Parse(swInfoGridView.Rows[idx].Cells[1].Value.ToString().Split('.')[0]);
                            swInfoToSet.AppOsInfo.Minor = UInt16.Parse(swInfoGridView.Rows[idx++].Cells[1].Value.ToString().Split('.')[1]);
                            swInfoToSet.AppBinaryInfo.StartAddress = UInt32.Parse(swInfoGridView.Rows[idx++].Cells[1].Value.ToString().Substring(2), System.Globalization.NumberStyles.HexNumber);
                            swInfoToSet.AppBinaryInfo.Size = UInt32.Parse(swInfoGridView.Rows[idx++].Cells[1].Value.ToString());
                            swInfoToSet.AppBinaryInfo.Crc = UInt32.Parse(swInfoGridView.Rows[idx++].Cells[1].Value.ToString().Substring(2), System.Globalization.NumberStyles.HexNumber);
                        }

                        if (swInfoGridView.Rows[idx] != null)
                        {
                            swInfoToSet.BldLibVerInfo.Name = swInfoGridView.Rows[idx++].Cells[1].Value.ToString();
                            swInfoToSet.BldLibVerInfo.Major = UInt16.Parse(swInfoGridView.Rows[idx].Cells[1].Value.ToString().Split('.')[0]);
                            swInfoToSet.BldLibVerInfo.Minor = UInt16.Parse(swInfoGridView.Rows[idx].Cells[1].Value.ToString().Split('.')[1]);
                            swInfoToSet.BldLibVerInfo.Build = UInt16.Parse(swInfoGridView.Rows[idx++].Cells[1].Value.ToString().Split('.')[2]);
                            swInfoToSet.BldLibVerInfo.Author = swInfoGridView.Rows[idx++].Cells[1].Value.ToString();
                            swInfoToSet.BldLibVerInfo.Description = swInfoGridView.Rows[idx++].Cells[1].Value.ToString();
                            swInfoToSet.BldLibVerInfo.Date = DateTime.Parse(swInfoGridView.Rows[idx++].Cells[1].Value.ToString());
                            swInfoToSet.BldSwVerInfo.Name = swInfoGridView.Rows[idx++].Cells[1].Value.ToString();
                            swInfoToSet.BldSwVerInfo.Major = UInt16.Parse(swInfoGridView.Rows[idx].Cells[1].Value.ToString().Split('.')[0]);
                            swInfoToSet.BldSwVerInfo.Minor = UInt16.Parse(swInfoGridView.Rows[idx].Cells[1].Value.ToString().Split('.')[1]);
                            swInfoToSet.BldSwVerInfo.Build = UInt16.Parse(swInfoGridView.Rows[idx++].Cells[1].Value.ToString().Split('.')[2]);
                            swInfoToSet.BldSwVerInfo.Author = swInfoGridView.Rows[idx++].Cells[1].Value.ToString();
                            swInfoToSet.BldSwVerInfo.Description = swInfoGridView.Rows[idx++].Cells[1].Value.ToString();
                            swInfoToSet.BldSwVerInfo.Date = DateTime.Parse(swInfoGridView.Rows[idx++].Cells[1].Value.ToString());
                            swInfoToSet.BldOsInfo.Major = UInt16.Parse(swInfoGridView.Rows[idx].Cells[1].Value.ToString().Split('.')[0]);
                            swInfoToSet.BldOsInfo.Minor = UInt16.Parse(swInfoGridView.Rows[idx++].Cells[1].Value.ToString().Split('.')[1]);
                            swInfoToSet.BldBinaryInfo.StartAddress = UInt32.Parse(swInfoGridView.Rows[idx++].Cells[1].Value.ToString().Substring(2), System.Globalization.NumberStyles.HexNumber);
                            swInfoToSet.BldBinaryInfo.Size = UInt32.Parse(swInfoGridView.Rows[idx++].Cells[1].Value.ToString());
                            swInfoToSet.BldBinaryInfo.Crc = UInt32.Parse(swInfoGridView.Rows[idx++].Cells[1].Value.ToString().Substring(2), System.Globalization.NumberStyles.HexNumber);
                        }

                        await Task.Run(() =>
                        {
                            List<string[]> swInfoItems = new List<string[]>();
                            SoftwareInfo softwareInfo = new SoftwareInfo();

                            // Get software info.
                            if (wireless)
                            {
                                //softwareInfo = Wireless.SetSoftwareInfo(swInfoToSet);
                            }
                            else
                            {
                                softwareInfo = SysmonFunctions.SetSoftwareInfo(swInfoToSet);
                            }

                            if (!(softwareInfo.AppSwVerInfo.Name is null) && softwareInfo.AppSwVerInfo.Name != "")
                            {
                                swInfoItems.Add((new string[] { "Application driver lib name", softwareInfo.AppLibVerInfo.Name }));
                                swInfoItems.Add((new string[] { "Application driver lib version", softwareInfo.AppLibVerInfo.Major.ToString("D2") + "." + softwareInfo.AppLibVerInfo.Minor.ToString("D2") + "." + softwareInfo.AppLibVerInfo.Build.ToString("D2") }));
                                swInfoItems.Add((new string[] { "Application driver lib author", softwareInfo.AppLibVerInfo.Author }));
                                swInfoItems.Add((new string[] { "Application driver lib description", softwareInfo.AppLibVerInfo.Description }));
                                swInfoItems.Add((new string[] { "Application driver lib date", softwareInfo.AppLibVerInfo.Date.ToString("yyyy-MM-dd") }));

                                swInfoItems.Add((new string[] { "Application name", softwareInfo.AppSwVerInfo.Name }));
                                swInfoItems.Add((new string[] { "Application version", softwareInfo.AppSwVerInfo.Major.ToString("D2") + "." + softwareInfo.AppSwVerInfo.Minor.ToString("D2") + "." + softwareInfo.AppSwVerInfo.Build.ToString("D2") }));
                                swInfoItems.Add((new string[] { "Application author", softwareInfo.AppSwVerInfo.Author }));
                                swInfoItems.Add((new string[] { "Application description", softwareInfo.AppSwVerInfo.Description }));
                                swInfoItems.Add((new string[] { "Application date", softwareInfo.AppSwVerInfo.Date.ToString("yyyy-MM-dd") }));

                                swInfoItems.Add((new string[] { "Application OS version", softwareInfo.AppOsInfo.Major.ToString("D2") + "." + softwareInfo.AppOsInfo.Minor.ToString("D2") }));

                                swInfoItems.Add((new string[] { "Application address", "0x" + softwareInfo.AppBinaryInfo.StartAddress.ToString("X8") }));
                                swInfoItems.Add((new string[] { "Application size", softwareInfo.AppBinaryInfo.Size.ToString() }));
                                swInfoItems.Add((new string[] { "Application CRC", "0x" + softwareInfo.AppBinaryInfo.Crc.ToString("X8") }));
                            }

                            if (softwareInfo.BldSwVerInfo.Name != "")
                            {
                                swInfoItems.Add((new string[] { "Bootloader driver lib name", softwareInfo.BldLibVerInfo.Name }));
                                swInfoItems.Add((new string[] { "Bootloader driver lib version", softwareInfo.BldLibVerInfo.Major.ToString("D2") + "." + softwareInfo.BldLibVerInfo.Minor.ToString("D2") + "." + softwareInfo.BldLibVerInfo.Build.ToString("D2") }));
                                swInfoItems.Add((new string[] { "Bootloader driver lib author", softwareInfo.BldLibVerInfo.Author }));
                                swInfoItems.Add((new string[] { "Bootloader driver lib description", softwareInfo.BldLibVerInfo.Description }));
                                swInfoItems.Add((new string[] { "Bootloader driver lib date", softwareInfo.BldLibVerInfo.Date.ToString("yyyy-MM-dd") }));

                                swInfoItems.Add((new string[] { "Bootloader name", softwareInfo.BldSwVerInfo.Name }));
                                swInfoItems.Add((new string[] { "Bootloader version", softwareInfo.BldSwVerInfo.Major.ToString("D2") + "." + softwareInfo.BldSwVerInfo.Minor.ToString("D2") + "." + softwareInfo.BldSwVerInfo.Build.ToString("D2") }));
                                swInfoItems.Add((new string[] { "Bootloader author", softwareInfo.BldSwVerInfo.Author }));
                                swInfoItems.Add((new string[] { "Bootloader description", softwareInfo.BldSwVerInfo.Description }));
                                swInfoItems.Add(new string[] { "Bootloader date", softwareInfo.BldSwVerInfo.Date.ToString("yyyy-MM-dd") });

                                swInfoItems.Add((new string[] { "Bootloader OS version", softwareInfo.BldOsInfo.Major.ToString("D2") + "." + softwareInfo.BldOsInfo.Minor.ToString("D2") }));

                                swInfoItems.Add((new string[] { "Bootloader address", "0x" + softwareInfo.BldBinaryInfo.StartAddress.ToString("X8") }));
                                swInfoItems.Add((new string[] { "Bootloader size", softwareInfo.BldBinaryInfo.Size.ToString() }));
                                swInfoItems.Add((new string[] { "Bootloader CRC", "0x" + softwareInfo.BldBinaryInfo.Crc.ToString("X8") }));
                            }

                            Helper.UpdateDataGridViewWithItems_ThreadSafe(this, swInfoGridView, swInfoItems);

                            Thread.Sleep(100);
                        });
                        break;
                    }
                case 1:
                    {
                        // HW info.
                        HardwareInfo hwInfoToSet = new HardwareInfo();
                        hwInfoToSet.BoardName = hwInfoGridView.Rows[0].Cells[1].Value.ToString();
                        hwInfoToSet.Revision = hwInfoGridView.Rows[1].Cells[1].Value.ToString();
                        hwInfoToSet.Author = hwInfoGridView.Rows[2].Cells[1].Value.ToString();
                        hwInfoToSet.Description = hwInfoGridView.Rows[3].Cells[1].Value.ToString();
                        hwInfoToSet.Date = DateTime.Parse(hwInfoGridView.Rows[4].Cells[1].Value.ToString());
                        hwInfoToSet.SerialNumber = hwInfoGridView.Rows[5].Cells[1].Value.ToString();

                        await Task.Run(() =>
                        {
                            List<string[]> hwInfoItems = new List<string[]>();
                            HardwareInfo hardwareInfo = new HardwareInfo();

                            if (wireless)
                            {
                                //hardwareInfo = Wireless.SetHardwareInfo(hwInfoToSet);
                            }
                            else
                            {
                                hardwareInfo = SysmonFunctions.SetHardwareInfo(hwInfoToSet);
                            }

                            if (hardwareInfo.BoardName != null)
                            {
                                hwInfoItems.Add((new string[] { "Board name", hardwareInfo.BoardName }));
                                hwInfoItems.Add((new string[] { "Revision", hardwareInfo.Revision }));
                                hwInfoItems.Add((new string[] { "Author", hardwareInfo.Author }));
                                hwInfoItems.Add((new string[] { "Description", hardwareInfo.Description }));
                                hwInfoItems.Add((new string[] { "Date", hardwareInfo.Date.ToString("yyyy-MM-dd") }));
                                hwInfoItems.Add((new string[] { "Serial number", hardwareInfo.SerialNumber }));

                                Helper.UpdateDataGridViewWithItems_ThreadSafe(this, hwInfoGridView, hwInfoItems);

                                Thread.Sleep(100);
                            }
                        });
                        break;
                    }
                case 2:
                    {
                        // BLD config.
                        BootloaderConfig bldConfigToSet = new BootloaderConfig();
                        bldConfigToSet.InstallRequested = bool.Parse(bldCfgGridView.Rows[0].Cells[1].Value.ToString());
                        bldConfigToSet.Reserved = byte.Parse(bldCfgGridView.Rows[1].Cells[1].Value.ToString());
                        bldConfigToSet.BinaryIndex = UInt16.Parse(bldCfgGridView.Rows[2].Cells[1].Value.ToString());
                        bldConfigToSet.UpdateMode = bool.Parse(bldCfgGridView.Rows[3].Cells[1].Value.ToString());
                        //bldConfigToSet.WirelessUpdate = bool.Parse(bldCfgGridView.Rows[4].Cells[1].Value.ToString());
                        //bldConfigToSet.WaitForConnectionOnStartup = bool.Parse(bldCfgGridView.Rows[5].Cells[1].Value.ToString());
                        bldConfigToSet.StartupCounter = byte.Parse(bldCfgGridView.Rows[4].Cells[1].Value.ToString());
                        //bldConfigToSet.ConnectionTimeout = UInt32.Parse(bldCfgGridView.Rows[7].Cells[1].Value.ToString());
                        bldConfigToSet.RequestTimeout = UInt32.Parse(bldCfgGridView.Rows[5].Cells[1].Value.ToString());
                        bldConfigToSet.InstallTimeout = UInt32.Parse(bldCfgGridView.Rows[6].Cells[1].Value.ToString());

                        await Task.Run(() =>
                        {
                            List<string[]> bldConfigItems = new List<string[]>();
                            BootloaderConfig bldConfig = new BootloaderConfig();

                            if (wireless)
                            {
                                //bldConfig = Wireless.SetBootloaderConfig(bldConfigToSet);
                            }
                            else
                            {
                                bldConfig = SysmonFunctions.SetBootloaderConfig(bldConfigToSet);
                            }

                            if (bldConfig != null)
                            {
                                bldConfigItems.Add((new string[] { "Install requested", bldConfig.InstallRequested.ToString() }));
                                bldConfigItems.Add((new string[] { "Reserved", bldConfig.Reserved.ToString() }));
                                bldConfigItems.Add((new string[] { "Bianry index", bldConfig.BinaryIndex.ToString() }));
                                bldConfigItems.Add((new string[] { "Update mode", bldConfig.UpdateMode.ToString() }));
                                //bldConfigItems.Add((new string[] { "Wireless update", bldConfig.WirelessUpdate.ToString() }));
                                //bldConfigItems.Add((new string[] { "Wait for connection on startup", bldConfig.WaitForConnectionOnStartup.ToString() }));
                                bldConfigItems.Add((new string[] { "Startup counter", bldConfig.StartupCounter.ToString() }));
                                //bldConfigItems.Add((new string[] { "Connection timeout", bldConfig.ConnectionTimeout.ToString() }));
                                bldConfigItems.Add((new string[] { "Request timeout", bldConfig.RequestTimeout.ToString() }));
                                bldConfigItems.Add((new string[] { "Install timeout", bldConfig.InstallTimeout.ToString() }));

                                Helper.UpdateDataGridViewWithItems_ThreadSafe(this, bldCfgGridView, bldConfigItems);
                            }
                        });
                        break;
                    }
                case 3:
                    {
                        // WiFi config.
                        WifiConfig wifiConfigToSet = new WifiConfig();
                        wifiConfigToSet.Ssid = wifiCfgGridView.Rows[0].Cells[1].Value.ToString();
                        wifiConfigToSet.Pwd = wifiCfgGridView.Rows[1].Cells[1].Value.ToString();
                        wifiConfigToSet.IpAddress = wifiCfgGridView.Rows[2].Cells[1].Value.ToString();
                        wifiConfigToSet.GateWay = wifiCfgGridView.Rows[3].Cells[1].Value.ToString();
                        wifiConfigToSet.Subnet = wifiCfgGridView.Rows[4].Cells[1].Value.ToString();
                        wifiConfigToSet.Port = UInt16.Parse(wifiCfgGridView.Rows[5].Cells[1].Value.ToString());

                        await Task.Run(() =>
                        {
                            List<string[]> wifiCfgItems = new List<string[]>();
                            WifiConfig wifiCfg = new WifiConfig();

                            if (wireless)
                            {
                                //wifiConfig = Wireless.SetBootloaderConfig(bldConfigToSet);
                            }
                            else
                            {
                                wifiCfg = SysmonFunctions.SetWifiConfig(wifiConfigToSet);
                            }

                            if (wifiCfg != null)
                            {
                                wifiCfgItems.Add((new string[] { "SSID", wifiCfg.Ssid }));
                                wifiCfgItems.Add((new string[] { "Password", wifiCfg.Pwd }));
                                wifiCfgItems.Add((new string[] { "IP address", wifiCfg.IpAddress }));
                                wifiCfgItems.Add((new string[] { "Gateway", wifiCfg.GateWay }));
                                wifiCfgItems.Add((new string[] { "Subnet", wifiCfg.Subnet }));
                                wifiCfgItems.Add((new string[] { "Port", wifiCfg.Port.ToString() }));

                                Helper.UpdateDataGridViewWithItems_ThreadSafe(this, wifiCfgGridView, wifiCfgItems);

                                Thread.Sleep(100);
                            }
                        });
                        break;
                    }
                default:break;
            }
        }
    }
}
