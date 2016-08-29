using UnityEngine;
using System;
using System.Collections.Generic;

//Corresponds to data at one pixel
public class Behavior {
    public float x; //unit coords driving this behavior (-1 to 1, centered). This should be scaled accordingly to determine movement target position
    public float y;
    public float moveSpeedFactor; //Red
    public float travelDistFactor; //Green
    public float groupingFactor; //Blue, 0 = independent, 1 = group
    public float factorsWeight; //alpha, 0 = [above] factors have no influence on current behavior, 1 = factors override current behavior

    //tweak factors
    private const float travelDistWeight = 1.0f;
    private const float groupCohesionCutoff = 0.4f;

    public override string ToString() {
        return "x=" + x + ", y=" + y + ", mvSpd=" + moveSpeedFactor + ", distFact=" + travelDistFactor + ", grpFact=" + groupingFactor + ", factWgt=" + factorsWeight;
    }

    //Given a current position and a behavior for the next step, apply the behavior and determine where the target of the next step will be
    //MaxStrideDist is a multiplier for position points, which default to unit circle
    //Returns (x,y) as target pos in local coords and z as speed to get there (speed is scaled to local already)
    public static Vector3 calcNext(BehaviorMap bMap, Vector2 curPos, Vector2 nearestGroupPos, int nearestGroupSize, float maxStrideDist) {
        Vector2 randPt = Util.nextApproxGaussUnitRandom();
        Behavior nextStepBehavior = bMap.getBehavior(randPt);

        Vector3 nextPoint = new Vector3(nextStepBehavior.x, nextStepBehavior.y);
        nextPoint *= maxStrideDist;

        //behaviors are rolled up into a factor applied to the movement
        float factor = nextStepBehavior.travelDistFactor * travelDistWeight;

        //For groupfactor 1, the next position will only move if it is headed toward the group avg pos.  groupFactor 0 is no restrictions on movement.
        //This is achieved by taking a vector from cur pos toward groupPos and projecting that on the direction we would be moving (input A)
        //Then we interp between point A and nextPoint with the groupFactor as the parameter
        Vector2 towardsGroup = nearestGroupPos;

        //As the agent gets further from the group, give the group factor more influence
        float groupDist = towardsGroup.magnitude;
        float expDistFunc = (float)Math.Pow(groupDist, 3) / 70; //values greater than 4 or 5 units will give results here
        if (expDistFunc < groupCohesionCutoff) {
            expDistFunc = 1;
        }
        float groupPull = nextStepBehavior.groupingFactor * Mathf.Max(1, expDistFunc);
        nextPoint = Vector2.Lerp(nextPoint, towardsGroup, Mathf.Clamp(groupPull,0,1));

        nextPoint.z = nextStepBehavior.moveSpeedFactor * Mathf.Clamp(nextStepBehavior.factorsWeight+ (groupPull / 100), 0, 1);

        //TODO global factor weight using alpha channel

        return nextPoint;
    }

}
