public class NextSceneData
{
    private static NextSceneData nextSceneData;
    
    public string SceneName;
    public int SceneIndex;

    public static NextSceneData Init()
    {
        if (nextSceneData == null) 
            nextSceneData = new NextSceneData();

        return nextSceneData;
    }
}