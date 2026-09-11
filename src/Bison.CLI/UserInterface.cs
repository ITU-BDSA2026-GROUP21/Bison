using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using SimpleDB;
public static class UserInterface
{
    public static void PrintObservations(IEnumerable<ObservationRecord> obs)
    {
        foreach(ObservationRecord o in obs)
        {
            DateTimeOffset date = DateTimeOffset.FromUnixTimeSeconds((long)Convert.ToDouble(o.Timestamp));
            Console.WriteLine(o.Author + " @ " +  date.ToString("MM/dd/yy HH:mm:ss")  + ": " + o.Observation);
        }
    }

    public static void PrintComments(IEnumerable<CommentRecord> com, int ID)
    {
        foreach(CommentRecord r in com)
        {
            if(r.ObservationID == ID)
            {
                DateTimeOffset date = DateTimeOffset.FromUnixTimeSeconds((long)Convert.ToDouble(r.Timestamp));
                Console.WriteLine(r.Author + " @ " +  date.ToString("MM/dd/yy HH:mm:ss")  + ": " + r.Comment);
            }
        }
    }
}