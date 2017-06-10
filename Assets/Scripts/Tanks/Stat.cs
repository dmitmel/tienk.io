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
using UnityEngine.Events;

namespace Tienkio.Tanks {
    public class Stat : MonoBehaviour {
        public ScoreCounter scoreCounter;
        public int level;
        public int maxLevel = 7;
        public float baseValue, holderLevelBonus, statLevelBonus;

        public UnityEvent onValueChange;

        public float Value { get; private set; }

#if UNITY_EDITOR
        void OnValidate() {
            level = Mathf.Clamp(level, 0, maxLevel);
            Value = ComputeValue();
            onValueChange.Invoke();
        }
#endif

        void Start() {
            Value = ComputeValue();
        }

        float ComputeValue() {
            return baseValue + holderLevelBonus * scoreCounter.currentLevel.index + statLevelBonus * level;
        }

        public void Upgrade() {
            if (level < maxLevel && scoreCounter.upgradePoints > 0) {
                level += 1;
                scoreCounter.upgradePoints -= 1;
                Value = ComputeValue();
                onValueChange.Invoke();
            }
        }

        public void OnScoreChange() {
            Value = ComputeValue();
            onValueChange.Invoke();
        }

        public void OnRespawn() {
            level = 0;
            Value = ComputeValue();
            onValueChange.Invoke();
        }
    }
}
