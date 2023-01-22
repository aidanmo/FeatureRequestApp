namespace FeatureRequestAppLibrary.DataAccess;

public interface IFeatureRequestData
{
    Task CreateFeatureRequest(FeatureRequestModel feature);
    Task<List<FeatureRequestModel>> GetAllApprovedFeatureRequests();
    Task<List<FeatureRequestModel>> GetAllFeatureRequests();
    Task<List<FeatureRequestModel>> GetAllFeatureRequestsForApproval();
    Task<FeatureRequestModel> GetFeatureRequest(string id);
    Task UpdateFeatureRequest(FeatureRequestModel feature);
    Task UpvoteFeature(string featureId, string userId);
}