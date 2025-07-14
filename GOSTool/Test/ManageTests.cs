using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GOSTool.Test
{
    public partial class ManageTests : Form
    {
        private List<Test> _tests = new List<Test>();

        public ManageTests()
        {
            InitializeComponent();
            RefreshTests();
        }

        private void RefreshTests()
        {
            _tests = ProjectHandler.GetTests();

            for (int i = 0; i < _tests.Count; i++)
            {
                for (int j = 0; j < _tests[i].TestCases.Count; j++)
                {
                    for (int k = 0; k < _tests[i].TestCases[j].TestSteps.Count; k++)
                    {
                        TestSteps.UpdateTestStep(_tests[i].TestCases[j].TestSteps[k]);
                        _tests[i].TestCases[j].TestSteps[k].Print = Print;
                        _tests[i].TestCases[j].TestSteps[k].PrintLine = PrintLine;
                    }

                    _tests[i].TestCases[j].Print = Print;
                    _tests[i].TestCases[j].PrintLine = PrintLine;
                }
                _tests[i].Print = Print;
                _tests[i].PrintLine = PrintLine;
            }

            RefreshList();
        }

        private void RefreshList()
        {
            testsListView.Items.Clear();
            testsListView.Items.AddRange(_tests.Select(x => new ListViewItem(x.Name)).ToArray());
        }

        private object Print(string msg)
        {
            Helper.SetRichTextBoxText_ThreadSafe(this, logTB, msg, true);
            return null;
        }

        private object PrintLine(string msg)
        {
            Helper.SetRichTextBoxText_ThreadSafe(this, logTB, msg + Environment.NewLine, true);
            return null;
        }

        private void newTestButton_Click(object sender, EventArgs e)
        {
            if (testNameTB.Text != "")
            {                
                _tests.Add(new Test() { Name = testNameTB.Text });
                ProjectHandler.SaveTest(new Test() { Name = testNameTB.Text });
                RefreshTests();
            }
            else
            {
                MessageBox.Show("Test name cannot be empty!");
            }
        }

        private void testsListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (testsListView.SelectedIndices.Count > 0)
            {
                testPropertyGrid.SelectedObject = _tests[testsListView.SelectedIndices[0]];
            }
        }

        private void editTestButton_Click(object sender, EventArgs e)
        {
            if (testsListView.SelectedIndices.Count > 0)
            {
                EditTest editor = new EditTest(_tests[testsListView.SelectedIndices[0]]);
                editor.Show();
            }
            else
            {
                MessageBox.Show("First select a test to edit!");
            }
        }

        private async void runTestButton_Click(object sender, EventArgs e)
        {
            if (testsListView.SelectedIndices.Count > 0)
            {
                int idx = testsListView.SelectedIndices[0];
                await Task.Run(() => { _tests[idx].Execute(); });
            }
            else
            {
                MessageBox.Show("First select a test to edit!");
            }


        }
    }
}
