using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Forms;
using BookWinForms.Models;
using BookWinForms.Services;

namespace BookWinForms
{
    public partial class Form1 : Form
    {
        private readonly ApiService _apiService;

        public Form1()
        {
            InitializeComponent();
            _apiService = new ApiService();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await LoadBooks();
        }

        private async Task LoadBooks(string query = "")
        {
            List<Book> books;
            if (string.IsNullOrEmpty(query))
            {
                books = await _apiService.GetBooksAsync();
            }
            else
            {
                books = await _apiService.SearchBooksAsync(query);
            }

            // Download images for each book
            foreach (var book in books)
            {
                if (!string.IsNullOrEmpty(book.Image))
                {
                    book.ImageFile = await _apiService.DownloadImageAsync(book.Image);
                }
            }

            dgvBooks.DataSource = null; // Reset
            dgvBooks.DataSource = books;

            // Setup DataGridView
            dgvBooks.RowTemplate.Height = 80;
            if (dgvBooks.Columns["Image"] != null) dgvBooks.Columns["Image"].Visible = false;
            if (dgvBooks.Columns["CategoryId"] != null) dgvBooks.Columns["CategoryId"].Visible = false;
            if (dgvBooks.Columns["Category"] != null) dgvBooks.Columns["Category"].Visible = false;
            if (dgvBooks.Columns["ImageFile"] != null)
            {
                var imgCol = (DataGridViewImageColumn)dgvBooks.Columns["ImageFile"];
                imgCol.HeaderText = "Hình ảnh";
                imgCol.ImageLayout = DataGridViewImageCellLayout.Zoom;
                imgCol.Width = 100;
            }
        }

        private async void btnSearch_Click(object sender, EventArgs e)
        {
            await LoadBooks(txtSearch.Text);
        }

        private async void btnAddBook_Click(object sender, EventArgs e)
        {
            using (var addBookForm = new AddBookForm())
            {
                if (addBookForm.ShowDialog() == DialogResult.OK)
                {
                    var newBook = addBookForm.NewBook;
                    var (success, message) = await _apiService.AddBookAsync(newBook);
                    if (success)
                    {
                        MessageBox.Show("Thêm sách thành công!");
                        await LoadBooks();
                    }
                    else
                    {
                        MessageBox.Show("Thêm sách thất bại: " + message);
                    }
                }
            }
        }
    }
}
