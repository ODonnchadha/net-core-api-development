namespace HotelListing.Entities
{
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// An "Item:" Something that is borrowed or lended.
    /// </summary>
    public class Item
    {
        [Key()]
        public int Id { get; set; }

    }
}
