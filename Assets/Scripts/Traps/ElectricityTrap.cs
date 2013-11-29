using UnityEngine;
using System.Collections;

public class ElectricityTrap : TrapBase {

    [SerializeField] GameObject electricity;
    [SerializeField] GameObject electricityBox0;
    [SerializeField] GameObject electricityBox1;
    [SerializeField] int electricTextureCount = 5;

    protected float averageFrameRate = 10f;
    protected float frameRateRange = 0.1f;

    protected int electricIndex = 0;
    protected float electricityTextureTileWidth;

    void Awake ()
    {
        electricityTextureTileWidth = 1f / electricTextureCount;
    }

    override public TrapType GetTrapType ()
    {
        return TrapType.Electicity;
    }

    public override void ActivateTrap ()
    {
        base.ActivateTrap();
        electricity.SetActive(true);
        StartCoroutine(PlayTextureChange());
    }

    public override void DeactivateTrap ()
    {
        base.DeactivateTrap();
        electricity.SetActive(false);
    }

    IEnumerator PlayTextureChange ()
    {
        while (IsActive())
        {
            electricity.renderer.material.mainTextureOffset = new Vector2(electricityTextureTileWidth * electricIndex, 0);
            //electricIndex++;
            int newI = Random.Range(0, electricTextureCount);
            if (electricIndex == newI) newI++;
            electricIndex = newI;
            if (electricIndex == electricTextureCount) electricIndex = 0;
            yield return new WaitForSeconds(1f / averageFrameRate);
        }
    }

    override protected bool PandaAttemptKill (PandaAI pandaAI, bool isPerfect)
    {
        return pandaAI.AttemptDeathTrapKill(this, isPerfect, GetComponentInChildren<Collider>());
    }

}
