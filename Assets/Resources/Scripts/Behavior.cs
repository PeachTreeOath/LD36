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

    public override string ToString() {
        return "x=" + x + ", y=" + y + ", mvSpd=" + moveSpeedFactor + ", distFact=" + travelDistFactor + ", grpFact=" + groupingFactor + ", factWgt=" + factorsWeight;
    }

    //Given a current position and a behavior for the next step, apply the behavior and determine where the target of the next step will be
    //curHeading is normalized direction of (last) travel
    //Move scale is general multiplier for position points, which default to image map size (when scale = 1)
    //Returns (x,y) as target pos and z as speed to get there
    public static Vector3 calcNext(Behavior nextStepBehavior, Vector2 curPos, Vector2 curHeading, Vector2 nearestGroupPos, float moveScale) {
        Vector3 nextPoint = new Vector3(nextStepBehavior.x, nextStepBehavior.y);

        //behaviors are rolled up into a factor applied to the movement
        float factor = nextStepBehavior.travelDistFactor;

        //For groupfactor 1, the next position will only move if it is headed toward the group avg pos.  groupFactor 0 is no restrictions on movement.
        //This is achieved by taking a vector from cur pos toward groupPos and projecting that on the direction we would be moving (input A)
        //Then we interp between point A and nextPoint with the groupFactor as the parameter
        Vector2 towardsGroup = nearestGroupPos - curPos;
        Vector2 nextDir = ((Vector2)nextPoint).normalized;
        float projectionForward = Mathf.Max(0, Vector2.Dot(towardsGroup, nextDir));
        nextPoint = Vector2.Lerp(nextPoint, projectionForward * nextDir, nextStepBehavior.groupingFactor);

        factor *= nextStepBehavior.factorsWeight; //global decision scaling
        
        nextPoint *= factor;  //magnitude determines how willing the agent is to move to this point

        nextPoint *= moveScale;

        //point next step relative to the forward direction the agent is travelling (e.g. local coords)
        float rot = Vector2.Angle(nextPoint, curHeading);
        nextPoint = Quaternion.AngleAxis(rot, Vector3.forward) * nextPoint;

        nextPoint = curPos + (Vector2)nextPoint; //local to world 

        //speed is only influenced by global scale 
        nextPoint.z = nextStepBehavior.moveSpeedFactor * nextStepBehavior.factorsWeight;

        return nextPoint;
    }
}
