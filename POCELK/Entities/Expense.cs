namespace POCELK.Entities
{
    public class Expense
    {
        public Expense()
        {

        }

        public Expense(int id, decimal value, DateTime? lastUpdateDate)
        {
            Id = id;

            Value = value;

            LastUpdateDate = lastUpdateDate;
        }

        public int Id { get; set; }

        public decimal Value { get; set; }

        public DateTime? LastUpdateDate { get; set; }
    }
}
