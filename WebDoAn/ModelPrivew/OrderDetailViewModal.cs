namespace WebDoAn.ModelPrivew
{
    public class OrderDetailViewModal
    {
        public int Id { get; set; }
        public DateTime CreateTime { get; set; }
        public int Quantity { get; set; }
        public int Total { get; set; }
        public int Sum { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string Image {  get; set; }
        public string Name {  get; set; }
    }
}
