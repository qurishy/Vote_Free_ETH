namespace Voter_Data_API.MODELS
{
    public class Mongo_DataSettings
    {
        public string? ConnectionString { get; set; }
        public string? DatabaseName { get; set; }

        public string? VotersCollectionName { get; set; }
    }
}
