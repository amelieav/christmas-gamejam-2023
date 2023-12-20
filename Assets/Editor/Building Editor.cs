using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Building))]
public class BuildingEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Connect all"))
        {
            Building building = (Building)target;

            foreach (AnchoredJoint2D joint in building.gameObject.GetComponentsInChildren(typeof(AnchoredJoint2D), true))
            {
                joint.enabled = true;
                joint.connectedBody = null;
            }

            for (int i = 0; i < building.transform.childCount - 1; i++)
            {
                Transform wallA = building.transform.GetChild(i);
                for (int j = i + 1; j < building.transform.childCount; j++)
                {
                    Transform wallB = building.transform.GetChild(j);
                    foreach (AnchoredJoint2D jointA in wallA.GetComponents<AnchoredJoint2D>())
                    {
                        foreach (AnchoredJoint2D jointB in wallB.GetComponents<AnchoredJoint2D>())
                        {
                            //Debug.Log($"{i},{j},{jointA.anchor} {jointB.anchor} {Vector3.Distance(wallA.TransformPoint(jointA.anchor), wallB.TransformPoint(jointB.anchor))}");
                            if (Vector3.Distance(wallA.TransformPoint(jointA.anchor), wallB.TransformPoint(jointB.anchor)) < building.connectRadius)
                            {
                                Debug.Log("connect");
                                jointA.connectedBody = wallB.GetComponent<Rigidbody2D>();
                                jointA.breakForce = building.glueStrength;
                                if (jointA as HingeJoint2D != null)
                                {
                                    jointA.breakForce = building.hingedStrength;
                                }
                                jointB.connectedBody = wallA.GetComponent<Rigidbody2D>();
                                jointB.breakForce = building.glueStrength;
                                if (jointB as HingeJoint2D != null)
                                {
                                    jointB.breakForce = building.hingedStrength;
                                }
                                Debug.Log(jointA.connectedBody);
                                Debug.Log(jointB.connectedBody);
                            }
                        }
                        
                    }
                }
            }
            foreach (AnchoredJoint2D joint in building.gameObject.GetComponentsInChildren(typeof(AnchoredJoint2D), true))
            {
                Debug.Log(joint.connectedBody);
                if (joint.connectedBody == null)
                {
                    joint.enabled = false;
                }
            }
        }
        else if (GUILayout.Button("Set Breakforce"))
        {
            Building building = (Building)target;
            foreach (AnchoredJoint2D joint in building.gameObject.GetComponentsInChildren(typeof(AnchoredJoint2D), true))
            {
                joint.enabled = true;
                joint.connectedBody = null;
                joint.breakForce = building.glueStrength;
                if (joint as HingeJoint2D != null)
                {
                    joint.breakForce = building.hingedStrength;
                }
            }
        }
    }
}
