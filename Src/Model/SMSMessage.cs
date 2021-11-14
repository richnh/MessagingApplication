namespace EmailServerService.Model
{
    public class SMSMessage : Message
    {
        public SMSMessage(string content)
        {
            Content = content;
        }
    }
}
