namespace Wedding.Data
{
    public class ImageLocation
    {
        public int Id { get; set; }
        public string ImageSource { get; set; } = null!;
        public int Height { get; set; }
        public int Width { get; set; }
        public bool IsPortrait => Width <= Height;
    }
}