using System;
using UnityEditor;

namespace Database
{
    public class OnHitEffect
    {
        public string GeneratedID { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public bool IsStackable { get; set; } = false;
        public virtual string Description() => string.Empty;
        public Action<DamageArgs> OnHitOnOwner { get; set; } //ownerStats - тут статы того кто стрелял
        public Action<DamageArgs> OnHitOnEnemy { get; set; } //onwerStats - тут статы в кого попали
    }
}
