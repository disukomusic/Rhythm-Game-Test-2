using HDyar.OSUImporter;
using UnityEngine;

public class PlayerHitData
{
    public int pressTime;
    public int releaseTime;
    public int lane;
    public ActiveHitObject hitObject;

    public float GetScoreForPress()
    {
        
        //todo: score for hold and non-holds
        if (hitObject == null)
        {
            //if > startTime && <songEndTime
            return Mathf.Abs(releaseTime - pressTime) * 2;
        }
        return Mathf.Abs(releaseTime - pressTime);
    }
}
