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
    private TrapBase trapBase;

    public void ThrowingStarSplit(PandaAI panda, TrapBase trap)
    {
        this.trapPos = trap.transform.position;
        pandaWalkDir = panda.PandaDirection;
        trapBase = trap;

        StartCoroutine(SplitPandaThrowing(panda.transform.position));
    }

    private IEnumerator SplitPandaThrowing(Vector3 pandaPos)
    {
        yield return new WaitForSeconds(Delay);

        var facingForce = Random.Range(10f, 15f);
        Util.Draw.Sphere(trapBase.transform, Color.red, 10000f);
        var forceDir = (pandaPos - trapBase.transform.position).normalized;
        forceDir *= facingForce;
        //if (Random.Range(0, 2) == 0)
          //  facingForce = -facingForce;

        Vector3 leftForce = new Vector3(0, 0, Random.Range(MinHeadForce, MaxHeadForce)) + forceDir;
        Vector3 rightForce = new Vector3(0, 0, Random.Range(-MinHeadForce, -MaxHeadForce)) + forceDir;
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
