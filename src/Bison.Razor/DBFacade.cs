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
                        JOIN user AS u ON o.author_id = u.user_id 
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
                string name = reader.GetString(0);
                string text = reader.GetString(1);
                int pubDate = reader.GetInt32(2);

                list.Add(new ObservationViewModel(name, text, UnixTimeStampToDateTimeString(pubDate)));
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