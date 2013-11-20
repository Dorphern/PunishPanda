using UnityEngine;
using System.Collections.Generic;

namespace InAudio.ExtensionMethods
{
    public static class NodeExtensions
    {
        public static void AssignParent<T>(this T node, T newParent) where T : Object, ITreeNode<T>
        {
            if (node != null && newParent != null)
            {
                newParent.GetChildren.Add(node);
                node.GetParent = newParent;
            }
        }
    }

    public static class AudioNodeExtensions
    {
        public static AudioBankLink GetBank(this AudioNode node) 
        {
            if (node.IsRoot)
                return node.BankLink;
            if (node.Type == AudioNodeType.Folder && node.OverrideParentBank)
                return node.BankLink;

            return GetBank(node.Parent);
        }
    }
}