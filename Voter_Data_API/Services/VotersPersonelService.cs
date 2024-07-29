using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Voter_Data_API.MODELS;

namespace Voter_Data_API.Services
{
    public class VotersPersonelService : IVotersPersonelService
    {
        private readonly IMongoCollection<VoterPersonelInfo> _votercollection; // <1>
        private readonly IOptions<Mongo_DataSettings> _dbsetting; // <2>

        public VotersPersonelService(IOptions<Mongo_DataSettings> dbsetting) // <2>)
        {
            _dbsetting = dbsetting;

            var client = new MongoClient(_dbsetting.Value.ConnectionString); // <3>
            var monogdatabase = client.GetDatabase(_dbsetting.Value.DatabaseName); // <4>

            _votercollection = monogdatabase.GetCollection<VoterPersonelInfo>(_dbsetting.Value.VotersCollectionName); // <5>

        }


        public async Task<IEnumerable<VoterPersonelInfo>>  GetAllVotersAsync() =>
            await _votercollection.Find(_ => true).ToListAsync();

        public async Task<VoterPersonelInfo> GetVoterAsync(string id) =>
            await _votercollection.Find(a => a.Id==id).FirstOrDefaultAsync();

        public async Task CreateVoterAsync(VoterPersonelInfo person) =>
            await _votercollection.InsertOneAsync(person);


        public async Task UpdateVoterAsync(string id, VoterPersonelInfo person) =>
            await _votercollection.ReplaceOneAsync(a => a.Id == id, person);

        public async Task DeleteVoterAsync(string id) =>
            await _votercollection.DeleteOneAsync(a => a.Id == id);

    }
}
