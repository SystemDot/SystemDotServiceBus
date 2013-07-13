namespace Messages
{
    public class TestMessage
    {
        public string Text { get; set; }

        public TestMessage() { }
        
        public TestMessage(string text)
        {
            this.Text = text;
        }
    }
}
