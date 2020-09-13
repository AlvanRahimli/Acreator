using System;

namespace Acreator.Models
{
    public class Message
    {
        public int Id { get; set; }
        public string ClientName { get; set; }
        public string ClientEmail { get; set; }
        public string ClientPhone { get; set; }
        public string Content { get; set; }
        public DateTime Date { get; set; }
        public MessageStatus Status { get; set; }
    }
}