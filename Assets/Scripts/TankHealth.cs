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

namespace Tienkio {
    public class TankHealth : ObjectWithHealth {
        [Space]
        public StatsHolder stats;
        public float statToExtraRegenMultiplier = 1;

        float lastHealthRegen, lastMaxHealth;

        protected override void Start() {
            base.Start();

            health = maxHealth = lastMaxHealth = stats.maxHealth.Value;
            healthRegen = lastHealthRegen = stats.healthRegen.Value;
            extraRegen = healthRegen * statToExtraRegenMultiplier;
        }

        protected override void FixedUpdate() {
            base.FixedUpdate();

            float newHealthRegen = stats.healthRegen.Value;
            if (newHealthRegen != lastHealthRegen) {
                lastHealthRegen = healthRegen = stats.healthRegen.Value;
                extraRegen = healthRegen * statToExtraRegenMultiplier;
            }

            float newMaxHealth = stats.maxHealth.Value;
            if (newMaxHealth != lastMaxHealth) {
                maxHealth = newMaxHealth;
                lastHealth = health = health * newMaxHealth / lastMaxHealth;
                lastMaxHealth = newMaxHealth;
            }
        }

        public void OnRespawn() {
            health = maxHealth = lastMaxHealth = stats.maxHealth.Value;
            healthRegen = lastHealthRegen = stats.healthRegen.Value;
            extraRegen = healthRegen * statToExtraRegenMultiplier;
        }
    }
}
