using System.Net.Sockets;

public class BJGameLoadData
{
    public NetworkStream DataStream;
    public User LocalUser;
    public User ExternalUser;

    public BJPlayer BJLocalUser;
    public BJPlayer BJExternalUser;

    public BJCardManager BJCardManager;

    public BJGameManagerFactory Factory;

    public BJGameLoadData(NetworkStream dataStream, User localUser, User externalUser, BJGameManagerFactory factory)
    {
        DataStream = dataStream;
        LocalUser = localUser;
        ExternalUser = externalUser;
        Factory = factory;
    }
}