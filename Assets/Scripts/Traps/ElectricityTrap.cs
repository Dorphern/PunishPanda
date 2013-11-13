using UnityEngine;
using System.Collections;

public class ElectricityTrap : TrapBase {

    [SerializeField] GameObject electricity;
    [SerializeField] GameObject electricityBox0;
    [SerializeField] GameObject electricityBox1;

    [SerializeField] Texture[] electricTextures;
    protected int electricIndex = 0;
    protected int baseRotation = 45;

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
            electricity.renderer.material.mainTexture = electricTextures[electricIndex];
            electricIndex++;
            if (electricIndex == electricTextures.Length) electricIndex = 0;
            electricity.transform.eulerAngles = 
                new Vector3(0, 0, Random.Range(0, 2) * 180 + baseRotation);
            //electricIndex = Random.Range(0, electricTextures.Length);
            yield return new WaitForSeconds(0.07f);
        }
    }

    override protected bool PandaAttemptKill (PandaAI pandaAI, bool isPerfect)
    {
        return pandaAI.AttemptDeathTrapKill(this, isPerfect);
    }

}
