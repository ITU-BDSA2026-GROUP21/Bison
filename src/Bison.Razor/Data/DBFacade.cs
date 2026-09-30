using System.Data;
using System.Globalization;
using Microsoft.Data.Sqlite;

public interface IDBFacade
{
    public List<ObservationViewModel> GetObservations(int page);
    public List<ObservationViewModel> GetObservationsByAuthor(string Author, int page);

}

public class DBFacade : IDBFacade
{

    private readonly string dbPath;

    public DBFacade(string dbPath)
    {
        this.dbPath = $"Data Source={dbPath}";
    }

    public List<ObservationViewModel> GetObservations(int page)
    {
        int offset = page == 1 ? 0 : (page-1) * 32; 
        int limit = 32;
        var sqlQuery = @"SELECT u.username, o.text, o.pub_date
                        FROM observation AS o 
                        JOIN user AS u ON o.author_id = u.user_id 
                        ORDER by o.pub_date desc
                        LIMIT @limit OFFSET @offset";
        return getList(sqlQuery, ("@limit", limit), ("@offset", offset));
    }

    public List<ObservationViewModel> GetObservationsByAuthor(string author, int page)
    {
        int offset = page == 1 ? 0 : (page-1) * 32; 
        int limit = 32;
        var sqlQuery = @"SELECT u.username, o.text, o.pub_date
                        FROM observation AS o 
                        JOIN user AS u ON o.author_id = u.user_id 
                        WHERE u.username = @author
                        ORDER by o.pub_date desc
                        LIMIT @limit OFFSET @offset";
        return getList(sqlQuery, ("@author", author), ("@limit", limit), ("@offset", offset));

    }

     public List<ObservationViewModel> getList(string sqlQuery, params (string name, object value)[] parameters)
    {
        var list = new List<ObservationViewModel>();

        using (var connection = new SqliteConnection(dbPath))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;
            foreach(var (name, value) in parameters)
            {
                command.Parameters.AddWithValue(name, value);
            }


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