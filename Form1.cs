using System;
using System.Data;
using System.Windows.Forms;

namespace sv
{
    public partial class Form1 : Form
    {
        private DataTable studentTable;
        private DataGridView dataGridView;
        private Button btnAdd, btnEdit, btnDelete, btnSearch;

        public Form1()
        {
            InitializeComponent();
            SetupForm();
        }

        private void SetupForm()
        {
            // Tạo DataTable cho danh sách sinh viên
            studentTable = new DataTable();
            studentTable.Columns.Add("Mã sinh viên", typeof(string));
            studentTable.Columns.Add("Tên", typeof(string));
            studentTable.Columns.Add("Lớp", typeof(string));

            // Tạo DataGridView
            dataGridView = new DataGridView
            {
                DataSource = studentTable,
                Dock = DockStyle.Top,
                Height = 250
            };
            Controls.Add(dataGridView);

            // Tạo nút Thêm
            btnAdd = new Button
            {
                Text = "Thêm",
                Top = dataGridView.Bottom + 10,
                Left = 10
            };
            btnAdd.Click += BtnAdd_Click;
            Controls.Add(btnAdd);

            // Tạo nút Sửa
            btnEdit = new Button
            {
                Text = "Sửa",
                Top = dataGridView.Bottom + 10,
                Left = btnAdd.Right + 10
            };
            btnEdit.Click += BtnEdit_Click;
            Controls.Add(btnEdit);

            // Tạo nút Xóa
            btnDelete = new Button
            {
                Text = "Xóa",
                Top = dataGridView.Bottom + 10,
                Left = btnEdit.Right + 10
            };
            btnDelete.Click += BtnDelete_Click;
            Controls.Add(btnDelete);

            // Tạo nút Tra cứu
            btnSearch = new Button
            {
                Text = "Tra cứu",
                Top = dataGridView.Bottom + 10,
                Left = btnDelete.Right + 10
            };
            btnSearch.Click += BtnSearch_Click;
            Controls.Add(btnSearch);
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            string studentId = Prompt.ShowDialog("Nhập mã sinh viên:");
            string name = Prompt.ShowDialog("Nhập tên:");
            string className = Prompt.ShowDialog("Nhập lớp:");

            if (!string.IsNullOrWhiteSpace(studentId) && !string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(className))
            {
                studentTable.Rows.Add(studentId, name, className);
            }
            else
            {
                MessageBox.Show("Thông tin không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                var row = dataGridView.SelectedRows[0].DataBoundItem as DataRowView;

                if (row != null)
                {
                    string studentId = Prompt.ShowDialog("Nhập mã sinh viên:", row.Row["Mã sinh viên"].ToString());
                    string name = Prompt.ShowDialog("Nhập tên:", row.Row["Tên"].ToString());
                    string className = Prompt.ShowDialog("Nhập lớp:", row.Row["Lớp"].ToString());

                    if (!string.IsNullOrWhiteSpace(studentId) && !string.IsNullOrWhiteSpace(name) && !string.IsNullOrWhiteSpace(className))
                    {
                        row.Row["Mã sinh viên"] = studentId;
                        row.Row["Tên"] = name;
                        row.Row["Lớp"] = className;
                    }
                    else
                    {
                        MessageBox.Show("Thông tin không được để trống!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Hãy chọn sinh viên cần sửa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count > 0)
            {
                var rowView = dataGridView.SelectedRows[0].DataBoundItem as DataRowView;
                if (rowView != null)
                {
                    studentTable.Rows.Remove(rowView.Row);
                }
            }
            else
            {
                MessageBox.Show("Hãy chọn sinh viên cần xóa!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string studentId = Prompt.ShowDialog("Nhập mã sinh viên:");

            if (!string.IsNullOrWhiteSpace(studentId))
            {
                foreach (DataRow row in studentTable.Rows)
                {
                    if (row["Mã sinh viên"].ToString().Equals(studentId, StringComparison.OrdinalIgnoreCase))
                    {
                        MessageBox.Show($"Thông tin sinh viên:\nMã sinh viên: {row["Mã sinh viên"]}\nTên: {row["Tên"]}\nLớp: {row["Lớp"]}",
                            "Thông tin sinh viên", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                MessageBox.Show("Không tìm thấy sinh viên với mã đã nhập!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("Vui lòng nhập mã sinh viên!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    public static class Prompt
    {
        public static string ShowDialog(string text, string defaultValue = "")
        {
            Form prompt = new Form
            {
                Width = 300,
                Height = 150,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = text,
                StartPosition = FormStartPosition.CenterScreen
            };

            Label textLabel = new Label { Left = 10, Top = 20, Text = text };
            TextBox inputBox = new TextBox { Left = 10, Top = 50, Width = 260, Text = defaultValue };
            Button confirmation = new Button { Text = "OK", Left = 200, Width = 70, Top = 80, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };

            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(inputBox);
            prompt.Controls.Add(confirmation);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? inputBox.Text : "";
        }
    }
}