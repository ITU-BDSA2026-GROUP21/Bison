using System.Data;
using System.Globalization;
using Microsoft.Data.Sqlite;

public interface IDBFacade
{
    public ObservationViewModel GetObservationByID(int id);
    public List<ObservationViewModel> GetObservations(int page);
    public List<ObservationViewModel> GetObservationsByAuthor(string Author, int page);
    public List<CommentViewModel> GetCommentsByObservationID(int id, int page);
    public List<ProposalViewModel> GetProposalsByObservationID(int id, int page);
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
        int offset = page == 1 ? 0 : (page - 1) * 32;
        int limit = 32;
        var sqlQuery = @"SELECT u.username, o.text, o.pub_date
                        FROM observation AS o 
                        JOIN user AS u ON o.author_id = u.user_id 
                        ORDER by o.pub_date desc
                        LIMIT @limit OFFSET @offset";
        return getObservationList(sqlQuery, ("@limit", limit), ("@offset", offset));
    }

    public List<ObservationViewModel> GetObservationsByAuthor(string author, int page)
    {
        int offset = page == 1 ? 0 : (page - 1) * 32;
        int limit = 32;
        var sqlQuery = @"SELECT u.username, o.text, o.pub_date
                        FROM observation AS o 
                        JOIN user AS u ON o.author_id = u.user_id 
                        WHERE u.username = @author
                        ORDER by o.pub_date desc
                        LIMIT @limit OFFSET @offset";
        return getObservationList(sqlQuery, ("@author", author), ("@limit", limit), ("@offset", offset));
    }

    public ObservationViewModel GetObservationByID(int id)
    {
        var sqlQuery = @"SELECT u.username, o.text, o.pub_date
                        FROM observation AS o 
                        JOIN user AS u ON o.author_id = u.user_id 
                        WHERE o.observation_id = @id";
        return GetObservation(sqlQuery, "@id", id);
    }
    public List<CommentViewModel> GetCommentsByObservationID(int id, int page)
    {
        int offset = page == 1 ? 0 : (page - 1) * 32;
        int limit = 32;
        var sqlQuery = @"SELECT u.username, c.text, c.pub_date
                        FROM comment AS c
                        JOIN user AS u ON c.author_id = u.user_id
                        WHERE c.observation_id = @id
                        ORDER by c.pub_date desc
                        LIMIT @limit OFFSET @offset";
        return getCommentList(sqlQuery, ("@id", id), ("@limit", limit), ("@offset", offset));
    }

    public List<ProposalViewModel> GetProposalsByObservationID(int id, int page)
    {
        int offset = page == 1 ? 0 : (page - 1) * 32;
        int limit = 32;
        var sqlQuery = @"SELECT u.username, p.text, p.pub_date
                        FROM proposal as p
                        JOIN user AS u ON p.author_id = u.user_id
                        WHERE p.observation_id = @id
                        ORDER by p.pub_date desc
                        LIMIT @limit OFFSET @offset";
        return getProposalList(sqlQuery, ("@id", id), ("@limit", limit), ("@offset", offset));
    }

    public List<ObservationViewModel> getObservationList(string sqlQuery, params (string name, object value)[] parameters)
    {
        var list = new List<ObservationViewModel>();

        using (var connection = new SqliteConnection(dbPath))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;
            foreach (var (name, value) in parameters)
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

    public ObservationViewModel GetObservation(string sqlQuery, string name, object value)
    {
        using (var connection = new SqliteConnection(dbPath))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;
            command.Parameters.AddWithValue(name, value);

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var dataRecord = (IDataRecord)reader;
                string obsName = reader.GetString(0);
                string text = reader.GetString(1);
                int pubDate = reader.GetInt32(2);

                return new ObservationViewModel(obsName, text, UnixTimeStampToDateTimeString(pubDate));
            }

            return null;
        }
    }

    public List<CommentViewModel> getCommentList(string sqlQuery, params (string name, object value)[] parameters)
    {
        var list = new List<CommentViewModel>();

        using (var connection = new SqliteConnection(dbPath))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;
            foreach (var (name, value) in parameters)
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

                list.Add(new CommentViewModel(name, text, UnixTimeStampToDateTimeString(pubDate)));
            }
            return list;
        }
    }

    public List<ProposalViewModel> getProposalList(string sqlQuery, params (string name, object value)[] parameters)
    {
        var list = new List<ProposalViewModel>();

        using (var connection = new SqliteConnection(dbPath))
        {
            connection.Open();

            var command = connection.CreateCommand();
            command.CommandText = sqlQuery;
            foreach (var (name, value) in parameters)
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

                list.Add(new ProposalViewModel(name, text, UnixTimeStampToDateTimeString(pubDate)));
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