using Newtonsoft.Json;

namespace BookWinForms.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public override string ToString()
        {
            return Name;
        }
    }

    public class Book
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? Image { get; set; }
        public int CategoryId { get; set; }
        public virtual Category? Category { get; set; }

        [JsonIgnore]
        public System.Drawing.Image? ImageFile { get; set; }
    }
}
