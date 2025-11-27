using System.Linq;
using UnityEngine;

namespace Mimi.EffectMaker.Core
{
    public class MonoSequenceEffectMaker : MonoEffectMaker
    {
        [SerializeField] private MonoEffectMaker[] effects;
        protected override BaseEffectMaker CreateEffectMaker()
        {
            var interfaceEffects = effects.Where(e => e != null).Cast<IEffectMaker>().ToArray();

            return new SequenceEffectMaker(interfaceEffects);
        }
    }
}