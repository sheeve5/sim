using Assets.Scripts;
using System.Linq;

public class Message
{
    public string content;
    public int senderId;
    public int originId;
    public int ttl;
    public string messageId;
    public int destinationId;

    public Message(string content, int sensorId, int originId, int ttl)
    {
        this.content = content;
        this.senderId= sensorId;
        this.originId = originId;
        this.ttl = ttl;
    }

    public Message(string content, int sensorId, int originId, int ttl, int myId) : this(content, sensorId, originId, ttl)
    {
        this.messageId = originId.ToString("D3")+myId.ToString("D7");
    }

    public Message(string content, int sensorId, int originId, int ttl, string messageId) : this(content, sensorId, originId, ttl)
    {
        this.messageId = messageId;
    }

    public string getReqId()
    {
        if (GlobalSettings.patrolRequestCodes.Contains(this.content.Substring(0, 2)))
        {
            return this.content.Substring(2, 8);
        }
        else return "";
    }

    internal bool isPropagated()
    {
        if (content[0] == '1')
        {
            return true;
        }
        return false;
    }

    internal bool isDataRequest()
    {
        if (content[1] == '2')
        {
            return true;
        }
        return false;
    }

    internal bool isDataResponse()
    {
        if (content[1] == '3')
        {
            return true;
        }
        return false;
    }
}