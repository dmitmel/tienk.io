//
//  Copyright (c) 2017  FederationOfCoders.org
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using UnityEngine;
using Tienkio.UI;

namespace Tienkio.Utilities {
    public class Health : MonoBehaviour {
        public GameObject healthBar;
        Bar healthBarController;

        public float health;
        protected float lastHealth;
        public float maxHealth, healthRegen, extraRegenTimeout, extraRegen;

        float nextExtraRegen;

#if UNITY_EDITOR
        void OnValidate() {
            healthBarController = healthBar.GetComponent<Bar>();
            if (healthBarController == null) Debug.LogError("Health - healthBar must have a Bar", this);
        }
#endif

        protected virtual void Start() {
            healthBarController = healthBar.GetComponent<Bar>();
            if (healthBarController == null) Debug.LogError("Health - healthBar must have a Bar", this);
        }

        void FixedUpdate() {
            bool isRegenEnabled = healthRegen > 0;
            bool isExtraRegenEnabled = extraRegenTimeout > 0 && extraRegen > 0;

            if (lastHealth != health) {
                healthBarController.value = health / maxHealth;
                if (isExtraRegenEnabled) nextExtraRegen = Time.time + extraRegenTimeout;
                lastHealth = health;
            }

            if (isRegenEnabled || isExtraRegenEnabled) {
                float now = Time.time;
                bool isExtraRegen = isExtraRegenEnabled && now >= nextExtraRegen;

                if (health < maxHealth) {
                    float healthPerSecond = isExtraRegen ? extraRegen : healthRegen;
                    float regen = Mathf.Min(maxHealth * healthPerSecond * Time.deltaTime, maxHealth - health);
                    health = lastHealth += regen;
                    healthBarController.value = health / maxHealth;
                } else if (isExtraRegenEnabled) {
                    nextExtraRegen = Time.time + extraRegenTimeout;
                }
            }
        }
    }
}
