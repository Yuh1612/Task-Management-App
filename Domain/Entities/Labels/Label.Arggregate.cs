using Domain.Base;

namespace Domain.Entities.Labels
{
    public partial class Label : IAggregateRoot
    {
        public Label(string title, string color)
        {
            Title = title;
            Color = color;
        }

        public void Update(string? title, string? color)
        {
            Title = title ?? Title;
            Color = color ?? Color;
        }
    }
}