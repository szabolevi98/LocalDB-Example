using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LocalDB_Example
{
    public partial class Users : Form
    {
        public Users()
        {
            InitializeComponent();
        }

        private void usersBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            try
            {
                this.Validate();
                this.usersBindingSource.EndEdit();
                this.tableAdapterManager.UpdateAll(this.databaseDataSet);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void Users_Load(object sender, EventArgs e)
        {
            this.usersTableAdapter.Fill(this.databaseDataSet.Users);
            this.MinimumSize = this.Size;
        }

        private void EditCheck(object sender, EventArgs e)
        {
            if (bindingNavigatorPositionItem.Text == "0")
            {
                MessageBox.Show("First please add a new line with the \"+\" button!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                TextBox textbox = sender as TextBox;
                textbox.TextChanged -= EditCheck; //Remove the EditCheck to not trigger again this event
                textbox.Text = string.Empty; //When setting the textbox empty
                textbox.TextChanged += EditCheck; //Then re-enable this event
            }
        }

        private void searchButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(searchTextBox.Text) || searchTextBox.Text == "Search..." )
            {
                MessageBox.Show("The input is empty!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                searchTextBox.Text = "Search...";
            }
            else
            {
                string keres = searchTextBox.Text;
                usersDataGridView.ClearSelection();
                try
                {
                    List<int> matches = new List<int>();
                    bool match = false;
                    foreach (DataGridViewRow row in usersDataGridView.Rows)
                    {
                        for (int i = 0; i < row.Cells.Count; i++)
                        {
                            if (row.Cells[i].Value != null && row.Cells[i].Value.ToString().IndexOf(keres, StringComparison.OrdinalIgnoreCase) >= 0)
                            {
                                usersDataGridView.Rows[row.Index].Cells[i].Selected = true;
                                matches.Add(row.Index + 1);
                                match = true;
                                searchTextBox.Text = "Search...";
                            }
                        }
                    }
                    if (match)
                    {
                        List<int> matchesDistinct = matches.Distinct().ToList();
                        string matchString = String.Join(", ", matchesDistinct.ToArray());
                        if (matches.Count == 1)
                        {
                            MessageBox.Show($"Match in the following line: {matchString}.\nCell selected.", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show($"Match in the following line(s): {matchString}.\nCells selected.", "Search", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        usersDataGridView.FirstDisplayedScrollingRowIndex = usersDataGridView.SelectedCells[usersDataGridView.SelectedCells.Count - 1].RowIndex;
                    }
                    else
                    {
                        MessageBox.Show($"The text: {searchTextBox.Text} not found!", "Search", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void searchTextBox_Click(object sender, EventArgs e)
        {
            if (searchTextBox.Text == "Search...")
            {
                searchTextBox.Clear();
            }
        }

        private void searchTextBox_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(searchTextBox.Text))
            {
                searchTextBox.Text = "Search...";
            }
        }

        private void searchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                searchButton.PerformClick();
            }
        }

        private void searchButton_MouseHover(object sender, EventArgs e)
        {
            toolTip.SetToolTip(searchButton, "Search");
        }
    }
}
