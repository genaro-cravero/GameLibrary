namespace GameLibrary;

public class Game
{
    public Game(string name, GameGenre genre, int releaseYear, bool isCompleted)
    {
        Name = name;
        Genre = genre;
        ReleaseYear = releaseYear;
        IsCompleted = isCompleted;
    }
    public string Name { get; } = string.Empty;
    public GameGenre Genre { get; }
    public int ReleaseYear { get; }
    public bool IsCompleted { get; }

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
