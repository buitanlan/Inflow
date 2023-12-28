using Inflow.Modules.Wallets.Core.Owners.Types;
using Inflow.Shared.Abstractions.Kernel.Types;

namespace Inflow.Modules.Wallets.Core.Wallets.Types;

internal class WalletId(Guid value) : TypeId(value)
{
    public WalletId() : this(Guid.NewGuid())
    {
    }

    public static implicit operator WalletId(Guid id) => new(id);
        
    public static implicit operator WalletId(OwnerId id) => id.Value;
        
    public override string ToString() => Value.ToString();
}