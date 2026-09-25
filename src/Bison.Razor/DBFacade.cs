using System.Data;
using Microsoft.Data.Sqlite;
public class DBFacade
{
    public static List<ObservationViewModel> getObs()
    {
        //move all sqlite into DBFacade.cs later
        var sqlDBFilePath = "../../data/sqlite/tmp/bison.db";
        var sqlQuery = @"SELECT u.username, o.text, o.pub_date
                        FROM observation AS o 
                        JOIN user AS u ON o.user_id = u.user_id 
                        ORDER by o.pub_date desc";
        List<ObservationViewModel> list = new List<ObservationViewModel>();
        using (var connection = new SqliteConnection($"Data Source={sqlDBFilePath}"))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;


            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var dataRecord = (IDataRecord)reader;
                for (int i = 0; i < dataRecord.FieldCount; i++)
                {
                    Console.WriteLine("Data record 1: " + dataRecord[i]);


                }
            }
            return list;
        }
    }

    private static string UnixTimeStampToDateTimeString(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp);
        return dateTime.ToString("MM/dd/yy H:mm:ss");
    }
}