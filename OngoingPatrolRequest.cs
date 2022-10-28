using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class OngoingPatrolRequest : PatrolRequest
    {
        int winnerId;
        int distance;

        public OngoingPatrolRequest(Message m) : base(m){
            WinnerId = m.originId;
            distance = Convert.ToInt32(m.content.Substring(8, 4));
        }

        public int Distance { get => distance; set => distance = value; }
        public int WinnerId { get => winnerId; set => winnerId = value; }

        internal void bid(Message m)
        {
            WinnerId = m.originId;
            distance = Convert.ToInt32(m.content.Substring(10, 4));
        }


    }
}
