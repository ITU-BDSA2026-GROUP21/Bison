using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
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
}