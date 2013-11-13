using UnityEngine;
using System.Collections;

public class ElectricityTrap : TrapBase {

    [SerializeField] GameObject electricity;
    [SerializeField] GameObject electricityBox0;
    [SerializeField] GameObject electricityBox1;

    [SerializeField] int electricTextureCount = 5;
    protected int electricIndex = 0;
    protected float electricityTextureTileWidth = 0.166f;

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
            electricIndex++;
            if (electricIndex == electricTextureCount) electricIndex = 0;
            yield return new WaitForSeconds(0.07f);
        }
    }

    override protected bool PandaAttemptKill (PandaAI pandaAI, bool isPerfect)
    {
        return pandaAI.AttemptDeathTrapKill(this, isPerfect);
    }

}
