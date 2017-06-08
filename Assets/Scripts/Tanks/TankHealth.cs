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
using Tienkio.Utilities;

namespace Tienkio.Tanks {
    public class TankHealth : Health {
        [Space]
        public StatsHolder stats;
        public float statToExtraRegenMultiplier = 1;

        float lastMaxHealth;

        protected override void Start() {
            base.Start();
            healthRegen = stats.healthRegen.Value;
            extraRegen = healthRegen * statToExtraRegenMultiplier;

            maxHealth = stats.maxHealth.Value;
            lastHealth = lastMaxHealth = health = maxHealth;
        }

        public void OnHealthRegenUpgrade() {
            healthRegen = stats.healthRegen.Value;
            extraRegen = healthRegen * statToExtraRegenMultiplier;
        }

        public void OnMaxHealthUpgrade() {
            maxHealth = stats.maxHealth.Value;
            lastHealth = health = health * (maxHealth / lastMaxHealth);
            lastMaxHealth = maxHealth;
        }

        public void OnRespawn() {
            health = maxHealth = lastMaxHealth = stats.maxHealth.Value;
            healthRegen = stats.healthRegen.Value;
            extraRegen = healthRegen * statToExtraRegenMultiplier;
        }
    }
}
