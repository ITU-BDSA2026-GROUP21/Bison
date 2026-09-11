using SimpleDB;
class Program
{
    static void Main(string[] args)
    {
        IDatabaseRepository<ObservationRecord> observationDatabase = CSVDatabase<ObservationRecord>.getInstance("../../data/bison_observe_cli_db.csv");
        IDatabaseRepository<CommentRecord> commentDatabase = CSVDatabase<CommentRecord>.getInstance("../../data/bison_comment_cli_db.csv");

        UserInterface.run(args, observationDatabase, commentDatabase);
    }
}

