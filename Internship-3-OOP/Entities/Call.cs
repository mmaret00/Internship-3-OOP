using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Internship_3_OOP.Entities;

namespace Internship_3_OOP.Entities
{
    public enum CallStatus
    {
        in_process,
        missed,
        complete
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

        static public List<(DateTime, CallStatus)> CreateSortedListOfCalls(Call[] call)
        {
            var sortedCalls = new List<(DateTime, CallStatus)>();

            for (int i = 0; i < call.Length; i++)
            {
                sortedCalls.Add((call[i].CallSetupTime, call[i]._callStatus));
            }
            sortedCalls.Sort((a, b) => b.Item1.CompareTo(a.Item1));

            return sortedCalls;
        }
    }
}
