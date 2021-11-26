using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using Internship_3_OOP.Entities;


namespace Internship_3_OOP.Entities
{
    public enum CallStatus
    {
        missed,
        complete,
        in_process
    }

    public class Call
    {
        public DateTime CallSetupTime { get; set; }
        public CallStatus _callStatus;

        public Call(CallStatus cs)
        {
            CallSetupTime = DateTime.Now;
            _callStatus = cs;
        }

        public Call(DateTime dt, CallStatus cs)
        {
            CallSetupTime = dt;
            _callStatus = cs;
        }
    }
}
