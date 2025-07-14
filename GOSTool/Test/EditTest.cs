using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GOSTool.Test
{
    public partial class EditTest : Form
    {
        private Test _currentTest = new Test();
        private List<TestCase> _testCases = new List<TestCase>();

        public EditTest(Test test)
        {
            InitializeComponent();
            _currentTest = test;
            for (int i = 0; i < _currentTest.TestCases.Count; i++)
            {
                for (int j = 0; j < _currentTest.TestCases[i].TestSteps.Count; j++)
                {
                    TestSteps.UpdateTestStep(_currentTest.TestCases[i].TestSteps[j]);
                }

                _currentTest.TestCases[i].Print = Print;
                _currentTest.TestCases[i].PrintLine = PrintLine;
            }
            _currentTest.Print = Print;
            _currentTest.PrintLine = PrintLine;
            Text = "Edit test [" + test.Name + "]";
            RefreshTestCases();
        }

        private void RefreshTestCases()
        {
            _testCases = ProjectHandler.GetTestCases();

            for (int i = 0; i < _testCases.Count; i++)
            {
                for (int j = 0; j < _testCases[i].TestSteps.Count; j++)
                {
                    TestSteps.UpdateTestStep(_testCases[i].TestSteps[j]);
                }

                _testCases[i].Print = Print;
                _testCases[i].PrintLine = PrintLine;
            }

            testCaseCB.Items.Clear();
            testCaseCB.Items.AddRange(_testCases.Select(x => x.Name).ToArray());
            RefreshList();
        }

        private void RefreshList()
        {
            testCaseListView.Items.Clear();
            testCaseListView.Items.AddRange(_currentTest.TestCases.Select(x => new ListViewItem(x.Name)).ToArray());
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

        private void testCaseListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (testCaseListView.SelectedIndices.Count > 0)
            {
                testCasePropertyGrid.SelectedObject = _currentTest.TestCases[testCaseListView.SelectedIndices[0]];
            }
        }

        private void editTestCasesButton_Click(object sender, EventArgs e)
        {
            EditTestCases editor = new EditTestCases();
            editor.Show();
            editor.FormClosed += (s, a) =>
            {
                RefreshTestCases();
            };
        }

        private void addCaseButton_Click(object sender, EventArgs e)
        {
            if (!(testCasePropertyGrid.SelectedObject is null))
            {
                var testCase = (TestCase)testCasePropertyGrid.SelectedObject;

                if (!(testCase is null))
                {
                    _currentTest.TestCases.Add(testCase);
                    RefreshList();
                }
            }
            else
            {
                MessageBox.Show("First select a test step!");
            }
        }

        private void testCaseCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            testCasePropertyGrid.SelectedObject = _testCases[testCaseCB.SelectedIndex];
        }

        private async void runCaseButton_Click(object sender, EventArgs e)
        {
            if (testCaseListView.SelectedIndices.Count > 0)
            {
                int idx = testCaseListView.SelectedIndices[0];

                await Task.Run(() =>
                {
                    _testCases[idx].Execute();
                });
            }
        }

        private async void runTestButton_Click(object sender, EventArgs e)
        {
            await Task.Run(() => { _currentTest.Execute(); });            
        }

        private void saveTestButton_Click(object sender, EventArgs e)
        {
            ProjectHandler.SaveTest(_currentTest);
            MessageBox.Show("Test saved.");
        }
    }
}
