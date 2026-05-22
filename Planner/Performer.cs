using System.Text.RegularExpressions;

namespace Planner;

public class Performer
{
    public Guid Id { get; }
    public string Name { get; private set; }
    public string? Email { get; private set; }

    public Performer(string name, string? email = null)
    {
        Id = Guid.NewGuid();
        SetName(name);
        SetEmail(email);
    }

    public void SetName(string newName)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(newName);
        Name = newName;
    }

    public void SetEmail(string? newEmail)
    {
        if (newEmail is not null && !Regex.IsMatch(newEmail, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase))
            throw new ArgumentException("Invalid email format");

        Email = newEmail;
    }
}