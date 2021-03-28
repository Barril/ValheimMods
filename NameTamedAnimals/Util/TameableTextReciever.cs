using NameTamedAnimals.Patches;

namespace NameTamedAnimals.Utils
{
    internal class TameableTextReciever : TextReceiver
    {
        private Tameable instance;
        private string name;

        public TameableTextReciever(ref Tameable instance, string name)
        {
            this.instance = instance;
            this.name = name;
        }

        public string GetText() => this.name;
        public void SetText(string text) {
            this.name = text;
            Tameable_Patch.SetName(ref this.instance, text, true);
        }
    }
}