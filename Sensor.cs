using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sensor : Node
{
    private int cd = 20 + UnityEngine.Random.Range(0, 5);
    private List<Tile> monitoredTiles = new List<Tile>();
    private List<Record> records = new List<Record>();
    private List<MobileNode> localDrones = new List<MobileNode>();
    public List<PatrolRequest> myRequests = new List<PatrolRequest>();
    private int requestNo = 1;


    public Sensor(int x, int y, GameObject sensor, int no)
    {
        nodeObject = GameObject.Instantiate(sensor);
        nodeObject.transform.position = new Vector3(x, y, 0);
        this.nodeId = no;
    }


    internal void initiate()
    {
        //throw new NotImplementedException();
    }






    public override void sendMessage(Message message)
    {
        Record record = archivedMessages.Find(r => r.MsgId == message.messageId);
        if (record == null)
        {//i've never received this message
            receivedMessages.Add(message);
            archivedMessages.Add(new Record(message));
        }
        else //i've already received this message
        {
            record.addSender(message.senderId);
        }
    }

    public override void analyzeMessages()
    {
        foreach (Message m in receivedMessages)
        {
            bool removeMessage = false;
            if (m.content[0] == '0')
            {
                switch (m.content[1])
                {
                    case '2':
                        sendEnvironmentData(m);
                        break;
                    case '3':
                        receiveEnvironmentData(m);
                        break;
                    default:
                        break;
                }

                removeMessage = true;//non propagated messages will not be sent forward
            }
            else
            {
                switch (m.content[1])
                {
                    case '4':
                        if (ongoingRequests.Exists(r => r.RequestId == m.getReqId()) || solvedRequests.Exists(r => r.RequestId == m.getReqId()))
                        {//if the request is already being negotiated or it has been fulfilled
                            removeMessage = true; //do not propagate the initial request
                        }
                        break;
                    case '5':
                        if (solvedRequests.Exists(r => r.RequestId == m.getReqId())) //i have to check if this request wasn't cancelled
                        {
                            removeMessage = true;
                        }
                        else
                        {//this has not been solved yet. have there already been offers of solving this?
                            OngoingPatrolRequest existingRequest = ongoingRequests.Find(r => r.RequestId == m.getReqId());

                            if (existingRequest == null)//i have not received any ofers yet, this is the best offer by default
                            {
                                ongoingRequests.Add(new OngoingPatrolRequest(m));
                            }
                            else //i have already received offers
                            {
                                int distance = Convert.ToInt32(m.content.Substring(8, 4));
                                if (existingRequest.Distance < distance) //this is the best offer
                                {
                                    existingRequest.bid(m);
                                }
                                else //better offer already exists
                                {
                                    removeMessage = true;
                                }
                            }
                            
                        }
                        if (Convert.ToInt32(m.content.Substring(2, 4)) == nodeId)
                        {
                            solvedRequests.Add(new PatrolRequest(m));
                            Debug.Log(m.content + "   resolved by " + m.originId);
                            myRequests.RemoveAll(r=> r.RequestId== m.getReqId());
                            
                            Message newMessage = new Message("16" + m.getReqId() + m.originId.ToString("D4"), nodeId, nodeId, GlobalSettings.defaultTtl, msgId);
                            msgId++;
                            processedMessages.Add(newMessage);
                            archivedMessages.Add(new Record(newMessage));

                            
                        }
                        break;
                    case '6':
                        if (solvedRequests.Exists(r => r.RequestId == m.getReqId()))
                        {//i have already cancelled this request
                            removeMessage = true;
                        }
                        else
                        {
                            solvedRequests.Add(new PatrolRequest(m));
                        }
                        break;
                    case '7':
                        break;
                    default:
                        break;
                }
            }

            if (!removeMessage && m.ttl > 0)
            {
                processedMessages.Add(m);
            }
        }
        receivedMessages.Clear();
    }

    internal void askForDrone()
    {
        string content = "14";
        content += nodeId.ToString("D4");
        content += requestNo.ToString("D4");
        content += Convert.ToInt32(nodeObject.transform.position.x).ToString("D3");
        content += Convert.ToInt32(nodeObject.transform.position.y).ToString("D3");
        Message m = new Message(content, nodeId, nodeId, GlobalSettings.defaultTtl, msgId);
        processedMessages.Add(m);
        myRequests.Add(new PatrolRequest(m));
        archivedMessages.Add(new Record(m));
        msgId++;

    }

    private void receiveEnvironmentData(Message m)
    {
        Debug.Log("received data " + m.content + " from " + m.senderId);
    }

    private void sendEnvironmentData(Message m)
    {
        Message newMessage = new Message("03" + "00000000", nodeId, nodeId, 1, msgId);
        newMessage.destinationId = m.originId;
        msgId++;
        processedMessages.Add(newMessage);
    }

    public void askForData()
    {
        foreach(Node n in neighbors)
        {
            if (n.GetType() == this.GetType())
            {
                Message newMessage = new Message("02", nodeId, nodeId, 1, msgId);
                newMessage.destinationId = n.nodeId;
                msgId++;
                processedMessages.Add(newMessage);
            }

        }

    }
}
