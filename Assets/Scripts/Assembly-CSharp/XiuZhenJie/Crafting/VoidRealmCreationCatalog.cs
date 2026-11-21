using System;
using UnityEngine;

namespace XiuZhenJie
{
    [Serializable]
    internal sealed class VoidRealmCreationStage
    {
        public string name;
        public string[] options;
    }

    [Serializable]
    internal sealed class VoidRealmCreationBrief
    {
        public string name, hint;
        public int[] target;
        public int polarity, element, qi;
    }

    [Serializable]
    internal sealed class VoidRealmCreationHall
    {
        public string id, product, mechanic, unit, traits, materialNote;
        public VoidRealmCreationStage[] stages;
        public VoidRealmCreationBrief[] briefs;
    }

    internal static class VoidRealmCreationCatalog
    {
        [Serializable]
        private sealed class Book
        {
            public VoidRealmCreationHall[] halls;
            public string[] polarities, elements, qi;
        }

        private static Book loaded;
        private static Book Rules
        {
            get
            {
                if (loaded == null)
                {
                    var file = ReleasedResources.Load<TextAsset>("VoidRealm/creation");
                    loaded = file == null ? new Book() : JsonUtility.FromJson<Book>(file.text);
                }
                return loaded;
            }
        }

        internal static VoidRealmCreationHall Find(string hallId) =>
            Array.Find(Rules.halls ?? Array.Empty<VoidRealmCreationHall>(), hall => hall.id == hallId);
        internal static string[] Polarities => Rules.polarities ?? Array.Empty<string>();
        internal static string[] Elements => Rules.elements ?? Array.Empty<string>();
        internal static string[] Qi => Rules.qi ?? Array.Empty<string>();
    }
}
