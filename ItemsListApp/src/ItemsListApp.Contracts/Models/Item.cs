using System;

namespace ItemsListApp.Contracts.Models
{
    public class Item
    {
        public Guid Id { get; set; }

        public string Text { get; set; }

        public DateTime CreationTime { get; set; }

        public DateTime LastUpdateTime { get; set; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Text)}: {Text}, {nameof(CreationTime)}: {CreationTime}, {nameof(LastUpdateTime)}: {LastUpdateTime}";
        }
    }
}