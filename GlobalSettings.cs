using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public static class GlobalSettings
    {
        public static float speed = 0.003f;
        public static int width = 52;
        public static int height = 52;
        public static int sensorDensity = 4;
        public static int sensorRange = 5;
        public static int defaultTtl = 20;

        public static string[] patrolRequestCodes = { "14","15","16" };

    }

    /*--------------------message codes----------------------------

       AB(CCCC,DDDD) - msg format
       a=0 - non propagated
       a=1 - propagated
       c - data1
       d - data2

       02 - environment data requested
       03TEMPHUM - environment info sent
       14REQIDCOORD - patrol request    [AB|CCCC|DDDD|XXXYYY - AB=code, CCCC=senderID, DDDD=requestID, XY = coord]
       15REQIDDISTANCE - patrol request received [AB|CCCC|DDDD|EEEE - AB=code, CCCC=senderID, DDDD=requestID, EEEE = distance]
       16REQIDWINID - assistance received [AB|CCCC|DDDD|EEEE - AB=code, CCCC=senderID, DDDD=requestID, EEEE = winnerID]
       17COORD - fire detected




   ---------------------------------------------------------------*/



}
