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

using System;
using UnityEngine;

namespace Deepio {
    public abstract class Stat {
        protected StatsHolder holder;

        [SerializeField, Range(0, 7)]
        protected int _statLevel;
        int lastStatLevel = -1;
        int statLevel {
            get { return _statLevel; }
            set {
                if (value < 0 || value > 7)
                    throw new ArgumentOutOfRangeException("value", value, "value must be >= 0 and <= 7");
                _statLevel = value;
            }
        }

        float lastStatValue;
        int lastHolderLevel;
        public float statValue {
            get {
                if (_statLevel != lastStatLevel || holder.level != lastHolderLevel) {
                    lastStatValue = ComputeValue();
                    lastStatLevel = _statLevel;
                    lastHolderLevel = holder.level;
                }
                return lastStatValue;
            }
        }

        public void AttachTo(StatsHolder holder) {
            this.holder = holder;
        }

        protected abstract float ComputeValue();
    }

    [Serializable]
    public class SimpleComputedStat : Stat {
        public float baseValue, holderLevelBonus, statLevelBonus;

        protected override float ComputeValue() {
            return baseValue + holderLevelBonus * holder.level + statLevelBonus * _statLevel;
        }
    }

    public class StatsHolder : MonoBehaviour {
        public SimpleComputedStat healthRegen;
        public SimpleComputedStat maxHealth;
        public SimpleComputedStat bodyDamage;
        public SimpleComputedStat bulletSpeed;
        public SimpleComputedStat bulletPenetration;
        public SimpleComputedStat bulletDamage;
        public SimpleComputedStat reload;
        public SimpleComputedStat movementSpeed;

        [Space]
        public int level;

        void Start() {
            healthRegen.AttachTo(this);
            maxHealth.AttachTo(this);
            bodyDamage.AttachTo(this);
            bulletSpeed.AttachTo(this);
            bulletPenetration.AttachTo(this);
            bulletDamage.AttachTo(this);
            reload.AttachTo(this);
            movementSpeed.AttachTo(this);
        }
    }
}