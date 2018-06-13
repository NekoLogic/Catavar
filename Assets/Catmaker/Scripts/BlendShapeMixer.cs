using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlendShapeMixer : MonoBehaviour
{
    public List<BlendTarget> targets = new List<BlendTarget> ();
    public float balance = 0.5f;
    public SkinnedMeshRenderer mesh;

    public void SetValue (float value)
    {
        if (targets.Count == 0) return;
        if (targets.Count > 2) Debug.LogWarning ("Blend shape mixer will not use more than 2 shape targets");

        int shapeIndexA = GetShapeKeyIndex (0);
        if (shapeIndexA == -1)
        {
            Debug.LogWarning ("Could not find shape key for " + targets[0].ToString ());
            return;
        }
        if (targets.Count == 1) mesh.SetBlendShapeWeight ((int) shapeIndexA, value * 100f);
        else
        {
            int shapeIndexB = GetShapeKeyIndex (1);
            if (shapeIndexB == -1)
            {
                Debug.LogWarning ("Could not find shape key for " + targets[1].ToString ());
                return;
            }
            float valueA = Mathf.Clamp (balance - value, 0, balance) / balance;
            float valueB = Mathf.Clamp (value - balance, 0, 1 - balance) / (1 - balance);

            mesh.SetBlendShapeWeight ((int) shapeIndexA, valueA * 100f);
            mesh.SetBlendShapeWeight ((int) shapeIndexB, valueB * 100f);
        }
    }

    public float GetSliderValueForTargets ()
    {
        if (targets.Count == 0) return 0;
        int shapeIndexA = GetShapeKeyIndex (0);
        if (shapeIndexA == -1)
        {
            Debug.LogWarning ("Could not find shape key for " + targets[0].ToString ());
            return 0;
        }
        float valueA = mesh.GetBlendShapeWeight (shapeIndexA) / 100f;
        if (targets.Count == 1) return valueA;
        return balance - (valueA * balance);
    }

    private int GetShapeKeyIndex (int index)
    {
        BlendTarget bTarget = targets[index];
        return mesh.sharedMesh.GetBlendShapeIndex (bTarget.ToString ());
    }
}

[System.Serializable]
public class BlendTarget
{
    public BlendSide side;
    public BlendLocation location;
    public BlendProperty property;

    override public string ToString ()
    {
        return "" + side + location + property;
    }
}

public enum BlendSide
{
    None,
    Left,
    Right,
    Both
}

public enum BlendLocation
{
    Ear,
    Eye,
    Nose,
    Muzzle,
    Jaw,
    Skull
}

public enum BlendProperty
{
    Long,
    Short,
    Wide,
    Narrow,
    Deep,
    Shallow,
    Tall,
    Fat,
    Clockwise,
    Anticlockwise,
    Curl,
    In,
    Out,
    Up,
    Down,
    Backwards,
    Forwards,
    Flat,
    Thin
}