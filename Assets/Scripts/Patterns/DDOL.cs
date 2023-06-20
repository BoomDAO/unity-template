using  ItsJackAnton.Patterns;
public class DDOL : Singleton<DDOL>
{
    //#pragma warning disable
    //#pragma warning restore

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(this);
    }
}