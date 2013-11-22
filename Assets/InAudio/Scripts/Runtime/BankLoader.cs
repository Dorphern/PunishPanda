using InAudio;
using UnityEngine;

public static class BankLoader{

    public static AudioBank Load(AudioBankLink bankLink)
    {
        if (bankLink == null)
            return null;
        var bank = SaveAndLoad.LoadAudioBank(bankLink.ID);

        if (Application.isPlaying && HDRInstanceFinder.DataManager != null)
        {
            HDRInstanceFinder.DataManager.BankIsLoaded(bank); 
            for (int i = 0; i < bank.Clips.Count; i++)
            {
                (bank.Clips[i].Node.NodeData as AudioData).Clip = bank.Clips[i].Clip;
            }
        }
        return bank;
    }

    public static void Unload(AudioBankLink bankLink)
    {
        AudioBank bank = HDRInstanceFinder.DataManager.GetLoadedBank(bankLink);
        if (bank != null)
        {
            for (int i = 0; i < bank.Clips.Count; i++)
            {
                (bank.Clips[i].Node.NodeData as AudioData).Clip = null;
            }
            Resources.UnloadUnusedAssets();
        }
    }

    public static void LoadAutoLoadedBanks()
    {
        LoadAuto(HDRInstanceFinder.DataManager.BankLinkTree);
    }

    private static void LoadAuto(AudioBankLink bankLink)
    {
        if (bankLink == null)
            return;
        if (bankLink.AutoLoad)
            Load(bankLink);

        for (int i = 0; i < bankLink.Children.Count; ++i)
        {
            LoadAuto(bankLink.Children[i]);
        }
    }
}