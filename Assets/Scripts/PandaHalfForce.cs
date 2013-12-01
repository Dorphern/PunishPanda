using UnityEngine;
using System.Collections;

public class PandaHalfForce : MonoBehaviour
{

    public Rigidbody LeftLeg;
    public Rigidbody RightLeg;

    public float MinLegForce = 0;
    public float MaxLegForce = 5;


    public float MinSpinForce = 30;
    public float MaxSpinForce = 50;

    public Rigidbody LeftHead;
    public Rigidbody RightHead;

    public float MinHeadForce = 0;
    public float MaxHeadForce = 5;

    public float Delay;

    private Vector3 trapPos;
    private PandaDirection pandaWalkDir;

    public void ThrowingStarSplit(PandaAI panda, Vector3 trapPos)
    {
        this.trapPos = trapPos;
        pandaWalkDir = panda.PandaDirection;

        StartCoroutine(SplitPandaThrowing());
    }

    private IEnumerator SplitPandaThrowing()
    {
        yield return new WaitForSeconds(Delay);

        Vector3 leftForce = new Vector3(0, 0, Random.Range(MinHeadForce, MaxHeadForce));
        Vector3 rightForce = new Vector3(0, 0, Random.Range(-MinHeadForce, -MaxHeadForce));
        LeftHead.AddForce(leftForce, ForceMode.Impulse);
        RightHead.AddForce(rightForce, ForceMode.Impulse);
    }

    public void SawSplit(PandaAI panda, Vector3 trapPos, BladeDirection spinDirection = BladeDirection.None)
    {
        pandaWalkDir = panda.PandaDirection;
        this.trapPos = trapPos;
        StartCoroutine(SplitPandaSaw(spinDirection));
    }

    private IEnumerator SplitPandaSaw(BladeDirection spinDirection)
    {
        yield return new WaitForSeconds(Delay);

        Vector3 leftForce = new Vector3(0, 0, Random.Range(-MinLegForce, -MaxLegForce));
        Vector3 rightForce = new Vector3(0, 0, Random.Range(MinLegForce, MaxLegForce));


        if (pandaWalkDir == PandaDirection.Right)
        {
            LeftLeg.AddForce(leftForce, ForceMode.Impulse);
            RightLeg.AddForce(rightForce, ForceMode.Impulse);
        }
        else
        {
            LeftLeg.AddForce(-leftForce, ForceMode.Impulse);
            RightLeg.AddForce(-rightForce, ForceMode.Impulse);
        }

        Vector3 currentPos = transform.position;
        trapPos.z = currentPos.z;
        if (spinDirection == BladeDirection.Clockwise)
            transform.RotateAround(trapPos, new Vector3(0, 0, 1), -Random.Range(20, 40));
        else if(spinDirection == BladeDirection.CounterClockwise)
            transform.RotateAround(trapPos, new Vector3(0, 0, 1), Random.Range(20, 40));

        Vector3 newPos = transform.position;
        transform.position = currentPos;
        
        Vector3 rotationDir = newPos - currentPos;

        LeftLeg.AddForce(rotationDir * Random.Range(MinSpinForce, MaxSpinForce), ForceMode.Impulse);
        RightLeg.AddForce(rotationDir * Random.Range(MinSpinForce, MaxSpinForce), ForceMode.Impulse);
    }
}
