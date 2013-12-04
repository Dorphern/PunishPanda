using System.Collections.Generic;
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

    [SerializeField]
    [EventHookAttribute("On Trap Awake")]
    private List<AudioEvent> onTrapAwake = new List<AudioEvent>();

    [SerializeField]
    [EventHookAttribute("On Trap Disable")]
    private List<AudioEvent> onTrapDisable = new List<AudioEvent>();

    void Awake ()
    {
        electricityTextureTileWidth = 1f / electricTextureCount;
    }

    override public TrapType GetTrapType ()
    {
        return TrapType.Electicity;
    }

    public override void ActivateTrap (bool playAnimation = true)
    {
        base.ActivateTrap();
        HDRSystem.PostEvents(gameObject, onTrapAwake);
        electricity.SetActive(true);
        StartCoroutine(PlayTextureChange());
    }

    public override void DeactivateTrap (bool playAnimation = true)
    {
        base.DeactivateTrap();
        HDRSystem.PostEvents(gameObject, onTrapDisable);
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
        return pandaAI.AttemptDeathTrapKill(this, isPerfect);
    }

}
