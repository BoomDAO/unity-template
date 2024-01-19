
public class DDOL : Singleton<DDOL>
{

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
}