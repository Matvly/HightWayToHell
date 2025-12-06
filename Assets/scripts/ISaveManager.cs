using System.Threading.Tasks;

public interface ISaveManager
{
    public void Save<T>(T data);
    public Task<T> Load<T>();
}

