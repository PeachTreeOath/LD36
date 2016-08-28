using UnityEngine;
using System;
using System.Collections.Generic;

//Corresponds to data at one pixel
public class Behavior {
    public int x;
    public int y;
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
    //curHeading is normalized direction of (last) travel
    //Move scale is general multiplier for position points, which default to image map size (when scale = 1)
    //Returns (x,y) as target pos in local coords and z as speed to get there (speed is scaled to local already)
    public static Vector3 calcNext(Behavior nextStepBehavior, Vector2 curPos, Vector2 nearestGroupPos) {
        dbgInfo db = new dbgInfo();
        db.curPos = curPos;

        Vector3 nextPoint = new Vector3(nextStepBehavior.x, nextStepBehavior.y);
        db.newTarget = nextPoint;

        //behaviors are rolled up into a factor applied to the movement
        float factor = nextStepBehavior.travelDistFactor;
        db.moveDist = factor;

        //For groupfactor 1, the next position will only move if it is headed toward the group avg pos.  groupFactor 0 is no restrictions on movement.
        //This is achieved by taking a vector from cur pos toward groupPos and projecting that on the direction we would be moving (input A)
        //Then we interp between point A and nextPoint with the groupFactor as the parameter
        Vector2 towardsGroup = nearestGroupPos - curPos;
        db.groupDir = towardsGroup;
        //Debug.Log("towardsGroup vec " + towardsGroup);
        Vector2 nextDir = ((Vector2)nextPoint).normalized;
        db.nextDir = nextDir;
        //Debug.Log("NextDir pt " + nextPoint + ", vec " + nextDir);
        //float projectionForward = Mathf.Max(0, Vector2.Dot(towardsGroup, nextDir));
        float projectionForward = Vector2.Dot(towardsGroup, nextDir);
        db.groupWeight = projectionForward;

        //Debug.Log("grpFactor " + nextStepBehavior.groupingFactor);
        //Debug.Log("Proj amt " + projectionForward + ", projNext dir " + projectionForward * nextDir);
        nextPoint = Vector2.Lerp(nextPoint, projectionForward * nextDir, nextStepBehavior.groupingFactor);
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
