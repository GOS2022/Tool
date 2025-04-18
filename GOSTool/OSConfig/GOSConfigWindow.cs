﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GOSTool
{
    public partial class GOSConfigWindow : Form
    {
        private ProjectData _projectData;
        public GOSConfigWindow()
        {
            InitializeComponent();
            _projectData = ProjectHandler.GetProjectData();
        }

        private List<string> osVersions = new List<string>();
        private List<(string, string)> configParameters = new List<(string, string)>();
        private string selectedOsPath = "";

        private async void GOSConfigWindow_Load(object sender, EventArgs e)
        {
            LoadWindow loadWindow = new LoadWindow();
            loadWindow.TopMost = true;
            loadWindow.Show();

            await Task.Run(GitHubHelper.UpdateOsList);

            GetOsVersions();
            osVersionComboBox.SelectedItem = _projectData.OsConfig.Version;

            FillDataGridViewFromConfig();

            dataGridView1.CellValueChanged += (s, args) =>
            {
                configParameters[args.RowIndex] = (configParameters[args.RowIndex].Item1, dataGridView1.Rows[args.RowIndex].Cells[args.ColumnIndex].Value.ToString());
            };

            loadWindow.Close();
        }
        private void FillDataGridViewFromConfig()
        {
            dataGridView1.Rows.Clear();

            List<(string, string)> cParamsCopy = new List<(string, string)>(configParameters);

            foreach ((string, string) param in cParamsCopy)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = param.Item1.Trim(' ');
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = param.Item2.Trim(' ');
            }
        }

        private void GetOsVersions()
        {
            osVersions.Clear();
            string[] directories = Directory.GetDirectories(ProgramData.OSPath);
            foreach (string dir in directories)
            {
                osVersions.Add(Path.GetFileName(dir));
            }
            osVersionComboBox.Items.Clear();
            osVersionComboBox.Items.AddRange(osVersions.ToArray());
        }

        private void GetConfigParameters()
        {
            string[] _files = Directory.GetFiles(selectedOsPath, "gos_config.h", SearchOption.AllDirectories);
            configParameters.Clear();

            if (_files.Length == 1)
            {
                using(StreamReader sr = File.OpenText(_files[0]))
                {
                    while (!sr.EndOfStream)
                    {
                        var line = sr.ReadLine();

                        if (line.Contains("#define"))
                        {
                            string[] splittedLine = line.Split(new string[] { "#define", "( ", " )" }, StringSplitOptions.RemoveEmptyEntries);

                            if (splittedLine.Length == 2)
                            {
                                configParameters.Add((splittedLine[0], splittedLine[1]));
                            }
                        }
                    }
                }
            }

            dataGridView1.Rows.Clear();

            List<(string, string)> cParamCopy = new List<(string, string)>(configParameters);
            foreach((string, string) param in cParamCopy)
            {
                dataGridView1.Rows.Add();
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = param.Item1.Trim(' ');
                dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = param.Item2.Trim(' ');
            }
        }

        private void osVersionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedOsPath = ProgramData.OSPath + "\\" + osVersionComboBox.SelectedItem;
            dataGridView1.Rows.Clear();

            GetConfigParameters();
            FillDataGridViewFromConfig();         
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            _projectData.OsConfig.Version = osVersionComboBox.SelectedItem.ToString();
            _projectData.OsConfig.Configuration = configParameters;
            ProjectHandler.SaveProjectData(_projectData);

            if (Directory.Exists(ProjectHandler.WorkspacePath.Value + "\\" + ProjectHandler.ProjectName.Value + "\\Build\\GOS2022"))
                Directory.Delete(ProjectHandler.WorkspacePath.Value + "\\" + ProjectHandler.ProjectName.Value + "\\Build\\GOS2022", true);
            Helper.CopyFilesRecursively(ProgramData.OSPath + "\\" + _projectData.OsConfig.Version, ProjectHandler.WorkspacePath.Value + "\\" + ProjectHandler.ProjectName.Value + "\\Build\\GOS2022");
            OverwriteConfig(ProjectHandler.WorkspacePath.Value + "\\" + ProjectHandler.ProjectName.Value + "\\Build\\GOS2022", "gos_config.h");
            OverwriteConfig(ProgramData.OSPath + "\\" + _projectData.OsConfig.Version, "gos_config.h");
            Close();
        }

        private void OverwriteConfig(string root, string configFile)
        {
            string[] _files = Directory.GetFiles(root, configFile, SearchOption.AllDirectories);

            if (_files.Length == 1)
            {
                string configToReplace = "";
                using (StreamReader sr = File.OpenText(_files[0]))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        string modifiedLine = line;

                        foreach ((string, string) configParam in configParameters)
                        {
                            if (line.Contains(configParam.Item1))
                            {
                                modifiedLine = line.Replace(line.Substring(line.IndexOf("("), line.IndexOf(")") - line.IndexOf("(") + 1), "( " + configParam.Item2 + " )");
                                break;
                            }
                        }

                        configToReplace += modifiedLine + "\r\n";
                    }
                }

                using (StreamWriter sw = File.CreateText(_files[0]))
                {
                    sw.Write(configToReplace);
                }
            }
        }
    }
}
