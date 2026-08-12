namespace GameLibrary;

public class Game
{
    public int Id { get; set; }
    public string Name { get; init; } = string.Empty;
    public GameGenre Genre { get; init; }
    public int ReleaseYear { get; init; }
    public bool IsCompleted { get; init; }
    public Game() { }
    public Game(string name, GameGenre genre, int releaseYear, bool isCompleted)
    {
        Name = name;
        Genre = genre;
        ReleaseYear = releaseYear;
        IsCompleted = isCompleted;
    }

    public override bool Equals(object? obj)
    {
        if (obj is Game other)
        {
            return string.CompareOrdinal(this.Name, other.Name) == 0;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }

}
