using System.Net;

public class BJGameLoadData
{
    public IPEndPoint Address;
    public User LocalUser;
    public User ExternalUser;

    public BJPlayer BJLocalUser;
    public BJPlayer BJExternalUser;

    public BJCardManager BJCardManager;

    public BJGameManagerFactory Factory;

    public BJGameLoadData(IPEndPoint addres, User localUser, User externalUser, BJGameManagerFactory factory)
    {
        Address = addres;
        LocalUser = localUser;
        ExternalUser = externalUser;
        Factory = factory;
    }
}