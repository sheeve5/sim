using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    class MobileNode:Node
    {
        private Vector3 velocity;
        private bool onPatrol = false;

        public MobileNode(int x, int y, GameObject drone, int no)
        {
            nodeObject = GameObject.Instantiate(drone);
            nodeObject.transform.position = new Vector3(x, y, 0);
            this.nodeId = no;
            velocity = new Vector3(UnityEngine.Random.value/10, UnityEngine.Random.value/10);
        }

        public override void sendMessage(Message message)
        {
            Record record = archivedMessages.Find(r => r.MsgId == message.messageId);
            if (record == null)
            {//i've never received this message
                receivedMessages.Add(message);
                archivedMessages.Add(new Record(message));
            }
        }
        
        public override void analyzeMessages()
        {
            foreach (Message m in receivedMessages)
            {
                switch (m.content[1])
                {
                    case '4':
                        Debug.Log("14 code msg received"); // i haven't received anything about this message yet
                        if (!ongoingRequests.Any(r => r.WinnerId == nodeId) && !onPatrol){//i have to see if im not preparing for patrol or if im not busy

                            if (!ongoingRequests.Exists(r => r.RequestId == m.getReqId() || solvedRequests.Exists(r => r.RequestId == m.getReqId())))
                            {
                                int distance = (int)Vector2.Distance(new Vector2(Convert.ToInt32(m.content.Substring(10, 3)), Convert.ToInt32(m.content.Substring(13, 3))), new Vector2(nodeObject.transform.position.x, nodeObject.transform.position.y));

                                Message newMessage = new Message("15" + m.getReqId() + distance.ToString("D4"), nodeId, nodeId, GlobalSettings.defaultTtl, msgId);
                                msgId++;
                                ongoingRequests.Add(new OngoingPatrolRequest(newMessage));//add my own response to offers
                                processedMessages.Add(newMessage);
                                archivedMessages.Add(new Record(newMessage));
                            }

                        }
                        break;
                    case '5':
                        Debug.Log("15 code msg received");
                        if (solvedRequests.Exists(r => r.RequestId == m.getReqId())) //i have to check if this request wasn't cancelled
                        {
                            
                        }
                        else if(!ongoingRequests.Any(r => r.WinnerId == nodeId) && !onPatrol)//i can only make an offer if im not busy
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
                                    
                                }
                            }

                        }
                        break;
                    case '6':
                        if (solvedRequests.Exists(r => r.RequestId == m.getReqId()))
                        {//i have already cancelled this request
                            Debug.Log("drone " + nodeId + " knows, that request " + m.getReqId() + " has been resolved");
                        }
                        else
                        {
                            solvedRequests.Add(new PatrolRequest(m));
                            Debug.Log("drone " + nodeId + " has learned, that request " + m.getReqId() + " has been resolved by drone " + m.content.Substring(10,4));
                            ongoingRequests.RemoveAll(r => r.RequestId == m.getReqId());
                            if(Convert.ToInt32(m.content.Substring(10, 4)) == nodeId)
                            {
                                onPatrol = true;
                            }
                        }
                        break;
                    case '7':
                        break;
                    default:
                        break;
                }

                processedMessages.Add(m);

            }
            receivedMessages.Clear();

            
        }


        public void move()
        {
            nodeObject.transform.position += velocity;
            if (nodeObject.transform.position.x < 0)
            {
                velocity.x = -velocity.x;
            }
            if (nodeObject.transform.position.x > GlobalSettings.width)
            {
                velocity.x = -velocity.x;
            }
            if (nodeObject.transform.position.y < 0)
            {
                velocity.y = -velocity.y;
            }
            if (nodeObject.transform.position.y > GlobalSettings.height)
            {
                velocity.y = -velocity.y;
            }

            if (onPatrol)
            {
                nodeObject.GetComponent<SpriteRenderer>().color = Color.black;
            }
        }


    }



}
