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

    //prob want to remove this before release
    public class dbgInfo {
        public Vector2 lastHeading;
        public Vector2 curPos;
        public Vector2 newTarget;
        public Vector2 groupDir;
        public Vector2 nextDir;
        public Vector2 nextStepPreScale;
        public Vector2 nextStepAfterScaleLocal;
        public float moveSpeed;
        public float moveDist;
        public float groupWeight;
        public float globalWeight;
        public override string ToString() {
            return "dbgInfo=[" +
        "lastHeading=" + lastHeading +
        "curPos=" + curPos +
        "newTarget=" + newTarget +
        "groupDir=" + groupDir +
        "nextDir=" + nextDir +
        "nextStepPreScale=" + nextStepPreScale +
        "nextStepAfterScaleLocal=" + nextStepAfterScaleLocal +
        "moveSpeed=" + moveSpeed +
        "moveDist=" + moveDist +
        "groupWeight=" + groupWeight +
        "globalWeight=" + globalWeight + "]"; 
        }
    }
    public static dbgInfo lastDbgInfo;

    public override string ToString() {
        return "x=" + x + ", y=" + y + ", mvSpd=" + moveSpeedFactor + ", distFact=" + travelDistFactor + ", grpFact=" + groupingFactor + ", factWgt=" + factorsWeight;
    }

    //Given a current position and a behavior for the next step, apply the behavior and determine where the target of the next step will be
    //MaxStrideDist is a multiplier for position points, which default to unit circle
    //Returns (x,y) as target pos in local coords and z as speed to get there (speed is scaled to local already)
    public static Vector3 calcNext(Behavior nextStepBehavior, Vector2 curPos, Vector2 nearestGroupPos, float maxStrideDist) {
        dbgInfo db = new dbgInfo();
        db.curPos = curPos;

        Vector3 nextPoint = new Vector3(nextStepBehavior.x, nextStepBehavior.y);
        nextPoint *= maxStrideDist;
        db.newTarget = nextPoint;

        //behaviors are rolled up into a factor applied to the movement
        float factor = nextStepBehavior.travelDistFactor * 1.5f;
        db.moveDist = factor;

        //For groupfactor 1, the next position will only move if it is headed toward the group avg pos.  groupFactor 0 is no restrictions on movement.
        //This is achieved by taking a vector from cur pos toward groupPos and projecting that on the direction we would be moving (input A)
        //Then we interp between point A and nextPoint with the groupFactor as the parameter
        Vector2 towardsGroup = nearestGroupPos - curPos;
        db.groupDir = towardsGroup;
        //Debug.Log("towardsGroup vec " + towardsGroup);
        Vector2 nextDir = ((Vector2)nextPoint).normalized;
        db.nextDir = nextDir;

        ////Debug.Log("towards group: " + towardsGroup + ", nextDir: " + nextDir);
        float projectionForward = Vector2.Dot(towardsGroup, nextDir);
        ////Debug.Log("Proj result = " + projectionForward);
        db.groupWeight = projectionForward;

        //Debug.Log("grpFactor " + nextStepBehavior.groupingFactor);
        //Debug.Log("Proj amt " + projectionForward + ", projNext dir " + projectionForward * nextDir);
        //nextPoint = Vector2.Lerp(nextPoint, projectionForward * nextDir, nextStepBehavior.groupingFactor);
        //As the agent gets further from the group, give the group factor more influence
        float expDistFunc = (float)Math.Pow(towardsGroup.magnitude,3) / 100 - 0.5f; //
        float groupPull = nextStepBehavior.groupingFactor * Mathf.Max(1, expDistFunc);
        nextPoint = Vector2.Lerp(projectionForward * nextDir, towardsGroup, groupPull);
        db.nextStepPreScale = nextPoint;

        factor *= nextStepBehavior.factorsWeight; //global decision scaling
        db.globalWeight = factor;

        nextPoint *= factor;  //magnitude determines how willing the agent is to move to this point
        db.nextStepAfterScaleLocal = nextPoint;

        //speed is only influenced by global scale 
        nextPoint.z = nextStepBehavior.moveSpeedFactor * nextStepBehavior.factorsWeight;
        db.moveSpeed = nextPoint.z;
        lastDbgInfo = db;

        return nextPoint;
    }

}
