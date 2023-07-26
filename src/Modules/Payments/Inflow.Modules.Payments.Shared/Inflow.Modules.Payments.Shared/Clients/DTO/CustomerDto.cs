namespace Inflow.Modules.Payments.Shared.Clients.DTO;

internal class CustomerDto
{
    public Guid CustomerId { get; set; }
    public string State { get; set; }
    public bool IsActive { get; set; }
}
