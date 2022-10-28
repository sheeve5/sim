using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class PatrolRequest
    {
        string requestId;
        
        public PatrolRequest(Message m)
        {
            requestId = m.content.Substring(2,8);
        }

        public string RequestId { get => requestId; set => requestId = value; }
    }
}
