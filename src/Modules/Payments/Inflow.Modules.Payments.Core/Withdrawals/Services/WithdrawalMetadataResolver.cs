using Inflow.Shared.Infrastructure.Serialization;
using Microsoft.Extensions.Logging;

namespace Inflow.Modules.Payments.Core.Withdrawals.Services;

internal sealed class WithdrawalMetadataResolver(
    IJsonSerializer jsonSerializer,
    ILogger<WithdrawalMetadataResolver> logger) : IWithdrawalMetadataResolver
{
    public Guid? TryResolveWithdrawalId(string metadata)
    {
        if (string.IsNullOrWhiteSpace(metadata))
        {
            return null;
        }

        try
        {
            return jsonSerializer.Deserialize<Metadata>(metadata).WithdrawalId;
        }
        catch (Exception e)
        {
            logger.LogError(e, e.Message);
            logger.LogError($"Couldn't resolve withdrawal metadata for value{Environment.NewLine}{metadata}");
                
            return null;
        }
    }
        
    private class Metadata
    {
        public Guid WithdrawalId { get; set; }
    }
}
