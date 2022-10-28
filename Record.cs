using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class Record
    {
        string msgId;
        List<int> senderId = new List<int>();

        public Record(Message message)
        {
            msgId = message.messageId;
            senderId.Append(message.senderId);
        }

        public string MsgId { get => msgId; set => msgId = value; }
        public Message Message { get; }

        public void addSender(int id)
        {
            senderId.Append(id);
        }

        public bool hasSender(int id)
        {
            return senderId.Contains(id);
        }
    }
}
