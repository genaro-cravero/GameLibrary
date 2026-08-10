namespace GameLibrary;

public class Game
{
    public Game() { }
    public Game(string name, GameGenre genre, int releaseYear, bool isCompleted)
    {
        Name = name;
        Genre = genre;
        ReleaseYear = releaseYear;
        IsCompleted = isCompleted;
    }
    public Game(Game instanceToCopy)
    {
        Name = instanceToCopy.Name;
        Genre = instanceToCopy.Genre;
        ReleaseYear = instanceToCopy.ReleaseYear;
        IsCompleted = instanceToCopy.IsCompleted;
    }

    public string Name { get; set; } = string.Empty;
    public GameGenre Genre { get; set; }
    public int ReleaseYear { get; set; }
    public bool IsCompleted { get; set; }

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
