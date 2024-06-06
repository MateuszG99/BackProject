namespace Domain.DataModels
{
    public class Shop
    {
        public int ShopId { get; set; }

        public string ShopName { get; set; }

        public string ShopAddress { get; set; }

        public int OwnerId { get; set; }
        public Owner? Owner { get; set; }

        public int CategoryId { get; set; }
        public Category? Category { get; set; }
    }
}
