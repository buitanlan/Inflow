using Inflow.Modules.Customers.Core.Domain.ValueObjects;
using Inflow.Modules.Customers.Core.Exceptions;
using Inflow.Shared.Abstractions.Kernel.ValueObjects;

namespace Inflow.Modules.Customers.Core.Domain.Entities;

internal class Customer
{
    public Guid Id { get; private set; }
    public string Email { get; set; }
    public string FullName { get; set; }
    public Name Name { get; private set; }
    public string Address { get; set; }
    public string Nationality { get; set; }
    public string Identity { get; set; }
    public string Notes { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public DateTime? VerifiedAt { get; set; }

    private Customer()
    {
    }
    
    public Customer(Guid id, string email, DateTime createdAt)
    {
        Id = id;
        Email = email ?? throw new InvalidCustomerEmailException(Id);
        IsActive = true;
        CreatedAt = createdAt;
    }
    
    public void Complete(Name name, FullName fullName, Address address, Nationality nationality, Identity identity,
        DateTime completedAt)
    {
        if (!IsActive)
        {
            throw new CustomerNotActiveException(Id);
        }
            
        if (CompletedAt.HasValue)
        {
            throw new CannotCompleteCustomerException(Id);
        }

        Name = name ?? throw new InvalidCustomerNameException(Id);
        FullName = fullName;
        Address = address;
        Nationality = nationality;
        Identity = identity;
        CompletedAt = completedAt;
    }

    public void Verify(DateTime verifiedAt)
    {
        if (!IsActive)
        {
            throw new CustomerNotActiveException(Id);
        }
            
        if (!CompletedAt.HasValue || VerifiedAt.HasValue)
        {
            throw new CannotVerifyCustomerException(Id);
        }

        VerifiedAt = verifiedAt;
    }

    public void Lock(string notes = null)
    {
        IsActive = false;
        Notes = notes?.Trim();
    }
        
    public void Unlock(string notes = null)
    {
        IsActive = true;
        Notes = notes?.Trim();
    }    
}