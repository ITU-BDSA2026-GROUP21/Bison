using System.Runtime.InteropServices;

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

        int beforeAdding = observationDatabase.Read().Count();

        Assert.Equal(beforeAdding, observationDatabase.Read().Count()); // Precondition check

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
        Assert.Equal(beforeAdding + 1, observationDatabase.Read().Count()); // Postcondition check
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

    //Fuzzer from fuzzingbook.org changed into c#
    static string Fuzzer(int maxLength = 100, int charStart = 32, int charRange = 32)
    {
        int stringLength = Random.Shared.Next(0, maxLength + 1);
        char[] output = new char[stringLength];

        for (int i = 0; i < stringLength; i++)
        {
            output[i] = (char)Random.Shared.Next(
                charStart,
                charStart + charRange);
        }

        return new string(output);
    }


    [Fact]
    public void FuzzingObsComsProps()
    {
        // Arrange
        CSVDatabase<ObservationRecord> observationDatabase = CSVDatabase<ObservationRecord>.getInstance(observationPath);
        CSVDatabase<CommentRecord> commentDatabase = CSVDatabase<CommentRecord>.getInstance(commentPath);
        CSVDatabase<ProposalRecord> proposalDatabase = CSVDatabase<ProposalRecord>.getInstance(proposalPath);

        Oracle oracle = new Oracle();

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

        var random = new Random();

        var failedComms = 0;
        var failedProps = 0;

        // Act

        for (int i = 0; i < 100; i++)
        {
            // Generate either observation/comment/proposal
            var generatedType = random.Next(0, 3);
            switch (generatedType)
            {
                case 0: //OBSERVATIONS
                    var obsAuthor = Fuzzer();
                    var observation = Fuzzer();
                    var Location = Fuzzer();
                    var id = ++observationDatabase.Read().Last().ID;
                    validObservationIDs.Add(id);

                    ObservationRecord o = new ObservationRecord
                    {
                        Author = obsAuthor,
                        Observation = observation,
                        Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                        ID = id,
                        Location = Location
                    };
                    oracle.observations.Add(id);
                    observationDatabase.Store(o);

                    break;

                case 1: //COMMENTS
                    var comAuthor = Fuzzer();
                    var comment = Fuzzer();
                    int obsID;

                    if (random.NextDouble() < 0.8) //this makes 80% of the fuzzing succeed (that's good)
                    {
                        obsID = random.Next(0, observationDatabase.Read().Count());
                        //the ones oracle accepts
                        oracle.comments.Add(comment);
                    }
                    else
                    {
                        obsID = random.Next(1000, validObservationIDs.Count + 1000);
                        //the ones oracle declines
                        failedComms++;
                    }

                    CommentRecord c = new CommentRecord
                    {
                        Author = comAuthor,
                        Comment = comment,
                        Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                        ObservationID = obsID
                    };
                    commentDatabase.Store(c);

                    break;

                case 2: //PROPOSALS
                    string taxonID;

                    if (random.NextDouble() < 0.8) //this makes 80% of the fuzzing succeed (that's good)
                    {

                        taxonID = validTaxonIDs[random.Next(0, 6)];
                        //the ones oracle accepts
                        oracle.proposals.Add(taxonID);
                    }
                    else
                    {
                        taxonID = Fuzzer();
                        //the ones oracle declines
                        failedProps++;
                    }

                    ProposalRecord p = new ProposalRecord
                    {
                        Author = "",
                        TaxonID = taxonID,
                        Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                        ObservationID = random.Next(0, observationDatabase.Read().Count())
                    };
                    proposalDatabase.Store(p);

                    break;

                default:
                    break;
            }
        }


        // Assert

        Assert.Equal(observationDatabase.Read().Count(), oracle.observations.Count() + 1);
        Assert.Equal(commentDatabase.Read().Count() - failedComms, oracle.comments.Count() + 2);
        Assert.Equal(proposalDatabase.Read().Count() - failedProps, oracle.proposals.Count() + 1);

        foreach (var idCheck in oracle.observations)
        {
            Assert.Equal(oracle.observations.Contains(idCheck), validObservationIDs.Contains(idCheck));
        }
    }
}

public class Oracle
{
    public List<int> observations;
    public List<String> comments;
    public List<String> proposals;

    public Oracle()
    {
        observations = new List<int>();
        comments = new List<String>();
        proposals = new List<String>();
    }

}