using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Enums;

using Bogus;

namespace Ambev.DeveloperEvaluation.Integration.TestData;

public static class UserTestData
{
    private static readonly Faker<User> _userFaker;
    private static readonly object _lock = new();

    static UserTestData()
    {
        lock (_lock)
        {
            _userFaker = new Faker<User>()
                .RuleFor(u => u.Id, f => Guid.NewGuid())
                .RuleFor(u => u.Username, f => f.Internet.UserName())
                .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.Username))
                .RuleFor(u => u.Phone, f => f.Phone.PhoneNumber("(##) #####-####"))
                .RuleFor(u => u.Password, f => f.Internet.Password(12, false, "\\w", "!@#$%^&*"))
                .RuleFor(u => u.Role, f => f.PickRandom<UserRole>())
                .RuleFor(u => u.Status, f => f.PickRandom<UserStatus>())
                .RuleFor(u => u.CreatedAt, f => DateTime.UtcNow)
                .RuleFor(u => u.UpdatedAt, f => null);
        }
    }

    public static User GenerateValidUser(
        string? email = null,
        UserRole? role = null,
        UserStatus? status = null)
    {
        var user = _userFaker.Generate();

        if (!string.IsNullOrWhiteSpace(email))
            user.Email = email;

        if (role.HasValue)
            user.Role = role.Value;

        if (status.HasValue)
            user.Status = status.Value;

        user.Phone = new Faker().Phone.PhoneNumber("(##) #####-####");
        user.Password = new Faker().Internet.Password(
            length: 12,
            prefix: "!Aa1");

        return user;
    }
}
