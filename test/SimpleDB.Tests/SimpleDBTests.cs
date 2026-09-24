namespace SimpleDB.Tests;

public class SimpleDBTests
{

    string observationPath = Path.Combine(AppContext.BaseDirectory, "bison_observe_cli_db_test.csv");

    string commentPath = Path.Combine(AppContext.BaseDirectory, "bison_comment_cli_db_test.csv");

    string proposalPath = Path.Combine(AppContext.BaseDirectory, "bison_proposal_cli_db_test.csv");

    [Fact]
    public void StoringObservationInCSV()
    {
        // Arrange
        CSVDatabase<ObservationRecord> observationDatabase = CSVDatabase<ObservationRecord>.getInstance(observationPath);

        Assert.Single(observationDatabase.Read()); // Precondition check

        ObservationRecord o = new ObservationRecord
        {
            Author = "test",
            Observation = "This is a test",
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            ID = 1,
            Location = ""
        };

        // Act
        observationDatabase.Store(o);

        // Assert
        Assert.Equal(2, observationDatabase.Read().Count()); // Postcondition check
    }


    [Fact]
    public void StoringCommentInCSV()
    {
        // Arrange
        CSVDatabase<CommentRecord> commentDatabase = CSVDatabase<CommentRecord>.getInstance(commentPath);

        Assert.Single(commentDatabase.Read()); // Precondition check

        CommentRecord o = new CommentRecord
        {
            Author = "test",
            Comment = "This is a test",
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            ObservationID = 1
        };

        // Act
        commentDatabase.Store(o);

        // Assert
        Assert.Equal(2, commentDatabase.Read().Count()); // Postcondition check
    }

    [Fact]
    public void StoringProposalInCSV()
    {
        // Arrange
        CSVDatabase<ProposalRecord> proposalDatabase = CSVDatabase<ProposalRecord>.getInstance(proposalPath);

        Assert.Empty(proposalDatabase.Read()); // Precondition check

        ProposalRecord p = new ProposalRecord
        {
            Author = "test",
            TaxonID = "MSTSNM:Arter:3e4e67e4-f785-ea11-aa77-501ac539d1ea",
            Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
            ObservationID = 1
        };

        // Act
        proposalDatabase.Store(p);

        // Assert
        Assert.Single(proposalDatabase.Read());
    }

    public void FuzzingObsComsProps()
    {
        // Arrange
        CSVDatabase<ObservationRecord> observationDatabase = CSVDatabase<ObservationRecord>.getInstance(observationPath);
        CSVDatabase<CommentRecord> commentDatabase = CSVDatabase<CommentRecord>.getInstance(commentPath);
        CSVDatabase<ProposalRecord> proposalDatabase = CSVDatabase<ProposalRecord>.getInstance(proposalPath);

        var validObservationIDs = new List<int>();
        var validTaxonIDs = new List<string>();

        for (int i = 0; i < observationDatabase.Read().Count(); i++)
        {
            validObservationIDs.Add(i);
        }

        validTaxonIDs.Add("MSTSNM:Arter:3e4e67e4-f785-ea11-aa77-501ac539d1ea");
        validTaxonIDs.Add("MSTSNM:Arter:495067e4-f785-ea11-aa77-501ac539d1ea");
        validTaxonIDs.Add("MSTSNM:Arter:fab9f9f3-f785-ea11-aa77-501ac539d1ea");
        validTaxonIDs.Add("MSTSNM:Arter:33bff9f3-f785-ea11-aa77-501ac539d1ea");
        validTaxonIDs.Add("MSTSNM:Arter:ca4d8cf8-f785-ea11-aa77-501ac539d1ea");
        validTaxonIDs.Add("MSTSNM:Arter:f3fa2bf9-f785-ea11-aa77-501ac539d1ea");

        //var oracle = new TestOracle();
        //make an Oracle class? can also be used for other oracle tests later

        var random = new Random();

        for (int i = 0; i < 1000; i++)
        {
            // Generate either observation/comment/proposal
            var generatedType = random.Next(0, 2);
            if (generatedType == 0)
            {
                //make an observation
            }

            if (generatedType == 1)
            {
                //make a comment
                int obsID;

                if (random.NextDouble() < 0.8) //this makes 80% of the fuzzing succeed (that's good)
                {
                    obsID = validObservationIDs.IndexOf(random.Next(0, validObservationIDs.Count));
                    //have the oracle accept it
                }
                else
                {
                    obsID = validObservationIDs.IndexOf(random.Next(1000, validObservationIDs.Count + 1000));
                    //have the oracle decline it!
                }

                //make the rest of the comment, and add it
            }

            if (generatedType == 2)
            {
                //make a proposal
                int obsID;

                if (random.NextDouble() < 0.8) //this makes 80% of the fuzzing succeed (that's good)
                {
                    obsID = validObservationIDs.IndexOf(random.Next(0, validObservationIDs.Count));
                    //have the oracle accept it
                }
                else
                {
                    obsID = validObservationIDs.IndexOf(random.Next(1000, validObservationIDs.Count + 1000));
                    //have the oracle decline it!
                }

                //make the rest of the proposal, and add it
            }

            // Send request through API


        }


        // Act
        //insert the randomized obs/coms/props into their respective dbs
        //and make sure the Oracle validates the validity of each entry

        // Assert
        //easiest asserting case is by checking the size and contents of the databases compared to the Oracle
        //if that is not enough, we must check the complete outputs of each database, and then compare those with the Oracle
    }
}