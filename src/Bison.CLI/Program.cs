using SimpleDB;


class Program
{
    static void Main(string[] args)
    {

        IDatabaseRepository<ObservationRecord> observationDatabase = new CSVDatabase<ObservationRecord>("../../data/bison_observe_cli_db.csv");
        IDatabaseRepository<CommentRecord> commentDatabase = new CSVDatabase<CommentRecord>("../../data/bison_comment_cli_db.csv");

        UserInterface.run(args, observationDatabase, commentDatabase);

    }
}

