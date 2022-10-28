using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Node
{
    public int nodeId;
    protected GameObject nodeObject;
    protected List<Message> receivedMessages = new List<Message>();
    protected List<Message> processedMessages = new List<Message>();
    protected List<Record> archivedMessages = new List<Record>();
    protected List<OngoingPatrolRequest> ongoingRequests = new List<OngoingPatrolRequest>();
    protected List<PatrolRequest> solvedRequests = new List<PatrolRequest>();
    protected List<Node> neighbors = new List<Node>();
    private int cd = 20;
    protected int msgId = 1;

    public abstract void sendMessage(Message m);

    public abstract void analyzeMessages();
    public Vector3 getPosition()
    {
        return nodeObject.transform.position;
    }
    internal void addNeighbor(Node receiver)
    {
        neighbors.Add(receiver);
    }

    internal void removeNeighbor(Node receiver)
    {
        neighbors.Remove(receiver);
    }
    

    public void tempPropagate()
    {
        if (processedMessages.Count > 0)
        {
            if(processedMessages[0].content[1]=='4')
            {
                nodeObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
            if (processedMessages[0].content[1] == '5')
            {
                nodeObject.GetComponent<SpriteRenderer>().color = Color.blue;
            }
            if (processedMessages[0].content[1] == '6')
            {
                nodeObject.GetComponent<SpriteRenderer>().color = Color.black;
            }
            if (processedMessages[0].content[1] == '3')
            {
                nodeObject.GetComponent<SpriteRenderer>().color = Color.red;
            }
        }
        else
        {
            nodeObject.GetComponent<SpriteRenderer>().color = Color.white;
        }

        if (cd == 0)
        {
            foreach (Message m in processedMessages)
            {
                if (m.isPropagated()) {
                    Record r = archivedMessages.Find(r => r.MsgId == m.messageId);
                    foreach (Node n in neighbors)
                    {
                        if (!r.hasSender(n.nodeId))//this node hasn't sent this to me.
                        {
                            n.sendMessage(new Message(m.content, nodeId, m.originId, m.ttl - 1, m.messageId));
                        }
                    }
                }
                else
                {
                    neighbors.Find(n => n.nodeId == m.destinationId).sendMessage(m);
        
                }

            }
            processedMessages.Clear();
            cd = 20 + UnityEngine.Random.Range(0, 5);
        }
        else
        {
            cd--;
        }
    }
}
