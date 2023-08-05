using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserService.Core.Models.UserModel
{
    public sealed class User : AggregateRoot
    {
        public Handle Handle { get; }
        
        public Email Email { get; }

        public Name Name { get; }
        
        public DateOfBirth DateOfBirth { get; }

        public CreationDate CreationDate { get; }

        public User(string handle, string email, string firstName, string lastName, DateOnly dateOfBirth) : base()
        {
            Handle = handle;
            Email = new(email);
            Name = new(firstName, lastName);
            DateOfBirth = dateOfBirth;

            //TODO: CreationDate is shown with IST time, check why
            CreationDate = DateTimeOffset.Now;
        }

        public User(Snapshot snapshot) : base(snapshot.Id)
        {
            Handle = snapshot.Handle;
            Email = new(snapshot.Email, snapshot.EmailIsVerified);
            Name = new(snapshot.FirstName, snapshot.LastName);
            DateOfBirth = snapshot.DateOfBirth;
            CreationDate = snapshot.CreationDate;
        }

        public override bool Equals(object? obj)
        {
            if (obj is not User)
                return false;

            User user = (User)obj;

            return
                Id.Equals(user.Id) &&
                Handle.Equals(user.Handle) &&
                Email.Equals(user.Email) &&
                Name.Equals(user.Name) &&
                DateOfBirth.Equals(user.DateOfBirth) &&
                CreationDate.Equals(user.CreationDate);
        }

        public override int GetHashCode()
        {
            HashCode hashCode = new();
            hashCode.Add(Id);
            hashCode.Add(Handle);
            hashCode.Add(Email);
            hashCode.Add(Email);
            hashCode.Add(Name);
            hashCode.Add(DateOfBirth);
            hashCode.Add(CreationDate);
            return hashCode.ToHashCode();
        }

        public Snapshot GetSnapshot()
        {
            return new Snapshot(
                Id,
                Handle,
                Email.Address,
                Email.IsVerified,
                Name.FirstName,
                Name.LastName,
                DateOfBirth,
                CreationDate);
        }

        public sealed class Snapshot
        {
            public Guid Id { get; }
            public string Handle { get; }
            public string Email { get; }
            public bool EmailIsVerified { get; }
            public string FirstName { get; }
            public string LastName { get; }
            public DateOnly DateOfBirth { get; }
            public DateTimeOffset CreationDate { get; }

            public Snapshot(Guid id, string handle, string email, bool emailIsVerified, string firstName, string lastName, 
                DateOnly dateOfBirth, DateTimeOffset creationDate)
            {
                Id = id;
                Handle = handle;
                Email = email;
                EmailIsVerified = emailIsVerified;
                FirstName = firstName;
                LastName = lastName;
                DateOfBirth = dateOfBirth;
                CreationDate = creationDate;
            }
        }
    }
}
