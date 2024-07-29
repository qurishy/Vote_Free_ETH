using Voter_Data_API.MODELS;

namespace Voter_Data_API.Services
{
    public interface IVotersPersonelService
    {
         Task<IEnumerable<VoterPersonelInfo>> GetAllVotersAsync();

         Task<VoterPersonelInfo> GetVoterAsync(string id);


         Task CreateVoterAsync(VoterPersonelInfo person);


         Task UpdateVoterAsync( string id, VoterPersonelInfo person);

         Task DeleteVoterAsync(string id);

    }
           
}