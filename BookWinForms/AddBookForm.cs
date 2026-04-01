using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using BookWinForms.Models;
using BookWinForms.Services;

namespace BookWinForms
{
    public partial class AddBookForm : Form
    {
        private readonly ApiService _apiService;
        public Book NewBook { get; private set; }

        public AddBookForm()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }

        private async void AddBookForm_Load(object sender, EventArgs e)
        {
            await LoadCategories();
        }

        private async Task LoadCategories()
        {
            var categories = await _apiService.GetCategoriesAsync();
            cmbCategory.DataSource = categories;
            cmbCategory.DisplayMember = "Name";
            cmbCategory.ValueMember = "Id";
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtImage.Text = openFileDialog1.FileName;
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTitle.Text) || string.IsNullOrEmpty(txtAuthor.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin.");
                return;
            }

            string imageFileName = "";
            if (!string.IsNullOrEmpty(txtImage.Text) && System.IO.File.Exists(txtImage.Text))
            {
                btnSave.Enabled = false;
                btnSave.Text = "Đang lưu...";
                imageFileName = await _apiService.UploadImageAsync(txtImage.Text);
                if (string.IsNullOrEmpty(imageFileName))
                {
                    MessageBox.Show("Không thể tải lên hình ảnh.");
                    btnSave.Enabled = true;
                    btnSave.Text = "Lưu";
                    return;
                }
            }

            NewBook = new Book
            {
                Title = txtTitle.Text,
                Author = txtAuthor.Text,
                Price = decimal.TryParse(txtPrice.Text, out decimal price) ? price : 0,
                Image = imageFileName, // Store the filename returned by the server
                CategoryId = (int)cmbCategory.SelectedValue
            };

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
