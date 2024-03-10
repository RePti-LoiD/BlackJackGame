using System.Net;

public class BJGameLoadData
{
    public User LocalUser;
    public User ExternalUser;

    public BJPlayer BJLocalUser;
    public BJPlayer BJExternalUser;

    public BJCardManager BJCardManager;

    public BJGameManagerFactory Factory;
    public IPEndPoint EndPoint;

    public BJGameLoadData(IPEndPoint endPoint, User localUser, User externalUser, BJGameManagerFactory factory)
    {
        EndPoint = endPoint;
        LocalUser = localUser;
        ExternalUser = externalUser;
        Factory = factory;
    }
}