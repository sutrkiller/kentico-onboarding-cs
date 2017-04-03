using System;

namespace ItemsListApp.Api.Models
{
    public class Item
    {
        public Guid Id { get; set; }
        public string Text { get; set; }

        protected bool Equals(Item other)
        {
            return Id.Equals(other.Id);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Item) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}