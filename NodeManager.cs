using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    List<Sensor> sensors = new List<Sensor>();
    public GameObject sensorObject;
    public GameObject droneObject;

    List<MobileNode> drones = new List<MobileNode>();

    // Start is called before the first frame update
    void Start()
    {
        int sensorRows = (int)GlobalSettings.height / GlobalSettings.sensorDensity;
        int sensorColumns = (int)GlobalSettings.width / GlobalSettings.sensorDensity;
        int no = 1;
        for (int i = 0; i < sensorRows; i++)
        {
            for (int j = 0; j < sensorColumns; j++)
            {
                sensors.Add(new Sensor((int)(i * GlobalSettings.sensorDensity + GlobalSettings.sensorDensity * .5), (int)(j * GlobalSettings.sensorDensity + GlobalSettings.sensorDensity * .5), sensorObject, no));
                no++;
            }
        }

        foreach (Sensor transmitter in sensors)
        {
            foreach (Sensor receiver in sensors)
            {
                if (Vector3.Distance(transmitter.getPosition(), receiver.getPosition()) < GlobalSettings.sensorRange && transmitter != receiver)
                {
                    transmitter.addNeighbor(receiver);
                }
            }
        }

        for (int i = 0; i<10; i++)
        {
            drones.Add(new MobileNode(Random.Range(2,GlobalSettings.width-1), Random.Range(2, GlobalSettings.height-1), droneObject, no));
            no++;
        }


    }
    int cd = 200;
    int no = 1;
    // Update is called once per frame
    void FixedUpdate()
    {
        if (cd % 100 == 0)
        {
            foreach (Sensor transmitter in sensors)
            {

                foreach(MobileNode d in drones)
                {
                    if (Vector3.Distance(transmitter.getPosition(), d.getPosition()) < GlobalSettings.sensorRange)
                    {
                        transmitter.addNeighbor(d);
                        d.addNeighbor(transmitter);
                    }
                    else
                    {
                        transmitter.removeNeighbor(d);
                        d.removeNeighbor(transmitter);
                    }
                }
            }

        }

        /*
        if (Random.Range(0, 100) > 90 && cd <1)
        {
            cd = 100;
            int source = Random.Range(0, sensors.Count);
            sensors[source].sendMessage(new Message(Random.Range(0, 5), source, source, 10, no));
            no++;
        }*/

        if (cd < 1)
        {
            cd = 100;
            //Message m = new Message("140001" + no.ToString("D4") + "000000", 0, 0, GlobalSettings.defaultTtl, no);
            //sensors[0].sendMessage(m);
            //sensors[0].myRequests.Add(new PatrolRequest(m));
            //no++;

            if (Random.Range(0, 5) >= 3)
            {
                sensors[Random.Range(0, sensors.Count)].askForDrone();
            }
            else
            {
                sensors[Random.Range(0, 100)].askForData();
            }
        }

        cd--;

        foreach (Sensor s in sensors)
        {
            s.analyzeMessages();
        }
       
        foreach(MobileNode d in drones)
        {
            d.analyzeMessages();
        }

        foreach (Sensor s in sensors)
        {
            s.tempPropagate();
        }
        foreach (MobileNode d in drones)
        {
            d.tempPropagate();
            d.move();
        }
    }
}
