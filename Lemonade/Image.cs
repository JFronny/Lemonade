using CC_Functions.Commandline.TUI;

namespace Lemonade
{
    public class Image : Control
    {
        public Pixel[,] Img;
        public Image(Pixel[,] img) => Img = img;

        public override Pixel[,] Render() => Img;

        public override bool Selectable { get; } = false;
    }
}