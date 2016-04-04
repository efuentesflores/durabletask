﻿namespace TaskHubStressTest
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Diagnostics;

    class OrchestrationConsoleTraceListener : ConsoleTraceListener
    {
        public override void TraceEvent(TraceEventCache eventCache, string source, TraceEventType eventType, int id, string message)
        {
            try
            {
                Dictionary<string, string> dict = message.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(part => part.Split('='))
                    .ToDictionary(split => split[0], split => split.Length > 1 ? split[1] : string.Empty);
                string iid;
                string msg;
                if (dict.TryGetValue("iid", out iid) && dict.TryGetValue("msg", out msg))
                {
                    string toWrite = $"[{DateTime.Now} {iid}] {msg}";
                    Console.WriteLine(toWrite);
                    Debug.WriteLine(toWrite);
                }
                else if (dict.TryGetValue("msg", out msg))
                {
                    string toWrite = $"[{DateTime.Now}] {msg}";
                    Console.WriteLine(toWrite);
                    Debug.WriteLine(toWrite);
                }
                else
                {
                    string toWrite = $"[{DateTime.Now}] {message}";
                    Console.WriteLine(toWrite);
                    Debug.WriteLine(toWrite);
                }
            }
            catch (Exception exception)
            {
                string toWrite = $"Exception while parsing trace:  {exception.Message}\n\t{exception.StackTrace}";
                Console.WriteLine(toWrite);
                Debug.WriteLine(toWrite);
            }
        }
    }
}