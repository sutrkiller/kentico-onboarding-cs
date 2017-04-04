using System;

namespace ItemsListApp.Api.Models
{
    public class Item
    {
        public Guid Id { get; set; }
        public string Text { get; set; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Text)}: {Text}";
        }
    }
}