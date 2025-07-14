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
    public partial class EditTestCases : Form
    {
        private List<TestCase> _testCases = new List<TestCase>();

        public EditTestCases()
        {
            InitializeComponent();

            testStepsCB.Items.AddRange(TestSteps.GetTestSteps());

            TestSteps.Print = Print;
            TestSteps.PrintLine = PrintLine;

            _testCases = ProjectHandler.GetTestCases();
            testCaseNameCB.Items.AddRange(_testCases.Select(x => x.Name).ToArray());

            for (int i = 0; i < _testCases.Count; i++)
            {
                _testCases[i].Print = Print;
                _testCases[i].PrintLine = PrintLine;

                for (int j = 0; j < _testCases[i].TestSteps.Count; j++)
                {
                    TestSteps.UpdateTestStep(_testCases[i].TestSteps[j]);
                }
            }

            Text = "Edit Test Case []";
        }        

        private object Print (string msg)
        {
            Helper.SetRichTextBoxText_ThreadSafe(this, logTB, msg, true);
            return null;
        }

        private object PrintLine(string msg)
        {
            Helper.SetRichTextBoxText_ThreadSafe(this, logTB, msg + Environment.NewLine, true);
            return null;
        }

        private void saveCaseButton_Click(object sender, EventArgs e)
        {
            if (testCaseNameCB.Text == "")
            {
                MessageBox.Show("Test case name cannot be empty.");
            }
            else
            {
                if (_testCases.Where(x => x.Name == testCaseNameCB.Text).Count() > 0)
                {
                    ProjectHandler.SaveTestCase(_testCases.First(x => x.Name == testCaseNameCB.Text));
                    RefreshList();
                    Text = "Edit Test Case [" + _testCases.First(x => x.Name == testCaseNameCB.Text).Name + "]";
                }
                else
                {
                    MessageBox.Show("This test case does not exist yet. First, it must be created.");
                }
            }
        }

        private void testStepsCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            TestStep currentEditStep = TestSteps.GetTestStep(testStepsCB.SelectedItem.ToString());
            testStepPropertyGrid.SelectedObject = currentEditStep;
        }

        private void addStepButton_Click(object sender, EventArgs e)
        {
            if (!(testStepPropertyGrid.SelectedObject is null))
            {
                var testStep = (TestStep)testStepPropertyGrid.SelectedObject;
                var testCase = _testCases.FirstOrDefault(x => x.Name == testCaseNameCB.Text);

                if (!(testCase is null))
                {
                    if (testStep is SetupSerialTestStep)
                    {
                        testCase.TestSteps.Add(testStep as SetupSerialTestStep);
                    }
                    else if (testStep is SetupWirelessTestStep)
                    {
                        testCase.TestSteps.Add(testStep as SetupWirelessTestStep);
                    }
                    else if (testStep is PingTestStep)
                    {
                        testCase.TestSteps.Add(testStep as PingTestStep);
                    }
                    else if (testStep is WaitTestStep)
                    {
                        testCase.TestSteps.Add(testStep as WaitTestStep);
                    }
                    else if (testStep is TaskDataTestStep)
                    {
                        testCase.TestSteps.Add(testStep as TaskDataTestStep);
                    }
                    else
                    {
                        testCase.TestSteps.Add(new TestStep(testStep));
                    }
                    RefreshList();
                }
            }
            else
            {
                MessageBox.Show("First select a test step!");
            }
        }

        private void RefreshList()
        {
            int idx = 1;
            testStepListView.Items.Clear();
            var testCase = _testCases.First(x => x.Name == testCaseNameCB.Text);
            foreach (var step in _testCases.First(x => x.Name == testCaseNameCB.Text).TestSteps)
            {
                testStepListView.Items.Add("[" + idx++ + "] " + step.Name);
            }
        }

        private void testStepListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (testStepListView.SelectedIndices.Count > 0)
            {
                testStepPropertyGrid.SelectedObject = _testCases.First(x => x.Name == testCaseNameCB.Text).TestSteps[testStepListView.SelectedIndices[0]];
            }
        }

        private async void runCaseButton_Click(object sender, EventArgs e)
        {
            logTB.Text = "";
            var testCase = testCaseNameCB.Text;
            await Task.Run(() =>
            {
                _testCases.First(x=>x.Name == testCase).Execute();
            });
        }

        private void logTB_TextChanged(object sender, EventArgs e)
        {
            // set the current caret position to the end
            logTB.SelectionStart = logTB.Text.Length;
            // scroll it automatically
            logTB.ScrollToCaret();
        }

        private void testStepPropertyGrid_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            RefreshList();
        }

        private void deleteCaseButton_Click(object sender, EventArgs e)
        {
            // TODO
        }

        private void deleteStepButton_Click(object sender, EventArgs e)
        {
            if (testStepListView.SelectedIndices.Count > 0)
            {
                _testCases.First(x => x.Name == testCaseNameCB.Text).TestSteps.RemoveAt(testStepListView.SelectedIndices[0]);
                ProjectHandler.SaveTestCase(_testCases.First(x => x.Name == testCaseNameCB.Text));
                RefreshList();
            }
        }

        private async void runStepButton_Click(object sender, EventArgs e)
        {
            var step = testCaseNameCB.Text;
            int idx = testStepListView.SelectedIndices[0];

            if (testStepListView.SelectedIndices.Count > 0)
            {
                await Task.Run(() =>
                {
                    _testCases.First(x => x.Name == step).TestSteps[idx].Execute();
                });
            }
        }

        private void moveUpButton_Click(object sender, EventArgs e)
        {
            if (testStepListView.SelectedIndices.Count > 0 && _testCases.First(x => x.Name == testCaseNameCB.Text).TestSteps.Count > 1)
            {
                if (testStepListView.SelectedIndices[0] > 0)
                {
                    int idx = testStepListView.SelectedIndices[0];
                    var tempItems = new TestStep[] { _testCases.First(x => x.Name == testCaseNameCB.Text).TestSteps[idx], _testCases.First(x => x.Name == testCaseNameCB.Text).TestSteps[idx-1] };
                    _testCases.First(x => x.Name == testCaseNameCB.Text).TestSteps.RemoveRange(idx-1, 2);
                    _testCases.First(x => x.Name == testCaseNameCB.Text).TestSteps.InsertRange(idx-1, tempItems);
                    ProjectHandler.SaveTestCase(_testCases.First(x => x.Name == testCaseNameCB.Text));
                    RefreshList();
                }                
            }
        }

        private void moveDownButton_Click(object sender, EventArgs e)
        {
            if (testStepListView.SelectedIndices.Count > 0 && _testCases.First(x => x.Name == testCaseNameCB.Text).TestSteps.Count > 1)
            {
                if (testStepListView.SelectedIndices[0] < _testCases.First(x => x.Name == testCaseNameCB.Text).TestSteps.Count - 1)
                {
                    int idx = testStepListView.SelectedIndices[0];
                    var tempItems = new TestStep[] { _testCases.First(x => x.Name == testCaseNameCB.Text).TestSteps[idx+1], _testCases.First(x => x.Name == testCaseNameCB.Text).TestSteps[idx] };
                    _testCases.First(x => x.Name == testCaseNameCB.Text).TestSteps.RemoveRange(idx, 2);
                    _testCases.First(x => x.Name == testCaseNameCB.Text).TestSteps.InsertRange(idx, tempItems);
                    ProjectHandler.SaveTestCase(_testCases.First(x => x.Name == testCaseNameCB.Text));
                    RefreshList();
                }
            }
        }

        private void testCaseNameCB_SelectedIndexChanged(object sender, EventArgs e)
        {
            Text = "Edit Test Case [" + _testCases.First(x => x.Name == testCaseNameCB.Text).Name + "]";
            RefreshList();
        }

        private void createCaseButton_Click(object sender, EventArgs e)
        {
            if (testCaseNameCB.Text == "")
            {
                MessageBox.Show("Test case name cannot be empty.");
            }
            else
            {
                if (_testCases.Where(x => x.Name == testCaseNameCB.Text).Count() > 0)
                {
                    MessageBox.Show("A test case with this name already exists.");
                }
                else
                {
                    _testCases.Add(new TestCase() { Name = testCaseNameCB.Text });
                    ProjectHandler.SaveTestCase(_testCases.First(x => x.Name == testCaseNameCB.Text));
                    Text = "Edit Test Case [" + _testCases.First(x => x.Name == testCaseNameCB.Text).Name + "]";
                    RefreshList();
                }
            }
        }
    }
}
