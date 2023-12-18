namespace CV.Models
{
    public class Chat
    {
        public int MID { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }
        public bool Read { get; set; }

        public int AID { get; set; }
    }
}
