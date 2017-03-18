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

namespace Deepio {
    public class PlayerHealth : ObjectWithHealth {
        public StatsHolder stats;

        float lastHealthRegen, lastMaxHealth;

        public override float health {
            get { return base.health; }
            set {
                base.health = value;
                if (base.health <= 0) Destroy(gameObject);
            }
        }

        protected override void Start() {
            base.Start();

            health = maxHealth = lastMaxHealth = stats.maxHealth.statValue;
            healthRegen = lastHealthRegen = stats.healthRegen.statValue;
        }

        protected override void Update() {
            base.Update();

            float newHealthRegen = stats.healthRegen.statValue;
            if (newHealthRegen != lastHealthRegen)
                healthRegen = lastHealthRegen = stats.healthRegen.statValue;

            float newMaxHealth = stats.maxHealth.statValue;
            if (newMaxHealth != lastMaxHealth) {
                maxHealth = newMaxHealth;
                health = health * newMaxHealth / lastMaxHealth;
                lastMaxHealth = newMaxHealth;
            }
        }
    }
}
