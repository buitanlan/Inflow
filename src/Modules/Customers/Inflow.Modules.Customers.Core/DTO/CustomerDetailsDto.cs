namespace Inflow.Modules.Customers.Core.DTO;

internal class CustomerDetailsDto : CustomerDto
{
    public string Notes { get;  set; }
    public string Address { get;  set; }
    public IdentityDto Identity { get; set; }

}

