using InAudio;
using UnityEngine;

public static class BankLoader{

    public static AudioBank Load(AudioBankLink bankLink)
    {
        if (bankLink == null)
            return null;
        var bank = SaveAndLoad.LoadAudioBank(bankLink.ID);

        if (Application.isPlaying && InAudioInstanceFinder.DataManager != null)
        {
            InAudioInstanceFinder.DataManager.BankIsLoaded(bank); 
            for (int i = 0; i < bank.Clips.Count; i++)
            {
                (bank.Clips[i].Node.NodeData as AudioData).Clip = bank.Clips[i].Clip;
            }
            bankLink.IsLoaded = true;
        }
        
        return bank;
    }

    public static void Unload(AudioBankLink bankLink)
    {
        AudioBank bank = InAudioInstanceFinder.DataManager.GetLoadedBank(bankLink);
        if (bank != null)
        {
            for (int i = 0; i < bank.Clips.Count; i++)
            {
                (bank.Clips[i].Node.NodeData as AudioData).Clip = null;
            }
            Resources.UnloadUnusedAssets();
            bankLink.IsLoaded = false;
        }
        
    }

    public static void LoadAutoLoadedBanks()
    {
        LoadAuto(InAudioInstanceFinder.DataManager.BankLinkTree);
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
