using System.Threading.Tasks;

public abstract class BJGameManagerFactory
{
    public abstract Task<(BJGameManager, BJPlayer)> CreateManagerAsync(BJGameLoadData data);
}