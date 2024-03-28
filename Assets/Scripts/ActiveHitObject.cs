using HDyar.OSUImporter;

public class ActiveHitObject
{
    public OSUHitObject hitObject;
    public PlayerHitData press;
    public int Time => hitObject.Time;

    public ActiveHitObject(OSUHitObject o)
    {
        hitObject = o;
        press = null;
    }
}
