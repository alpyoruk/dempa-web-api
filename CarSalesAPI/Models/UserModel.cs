namespace CarSalesAPI.Models
{
    public class UserModel
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Mail { get; set; }
        public byte[] Password { get; set; }
        public bool isDeleted { get; set; }

        public string StringPassword { get; set; }
        public string FullName;
    }
}