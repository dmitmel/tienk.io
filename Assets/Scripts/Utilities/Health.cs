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

        [SerializeField]
        protected float _health, _maxHealth;
        public float healthRegen, extraRegenTimeout, extraRegen;

        public float health {
            get { return _health; }
            set {
                float clampedValue = Mathf.Min(value, _maxHealth);
                if (_health != clampedValue) {
                    _health = clampedValue;
                    healthBarController.value = _health / _maxHealth;
                    bool isExtraRegenEnabled = extraRegenTimeout > 0 && extraRegen > 0;
                    if (isExtraRegenEnabled) nextExtraRegen = Time.time + extraRegenTimeout;
                }
            }
        }

        public float maxHealth {
            get { return _maxHealth; }
            set {
                float clampedValue = Mathf.Max(value, 0);
                if (_maxHealth != clampedValue) {
                    _maxHealth = clampedValue;
                    health = Mathf.Clamp(_health, 0, _maxHealth);
                }
            }
        }

        float nextExtraRegen;

#if UNITY_EDITOR
        void OnValidate() {
            _maxHealth = Mathf.Max(_maxHealth, 0);
            _health = Mathf.Min(_health, _maxHealth);

            if (healthBarController == null) {
                healthBarController = healthBar.GetComponent<Bar>();
                if (healthBarController == null) Debug.LogError("Health - healthBar must have a Bar", this);
            } else {
                if (_maxHealth > 0) healthBarController.value = _health / _maxHealth;
                else healthBarController.value = 1;
            }
        }
#endif

        void Awake() {
            healthBarController = healthBar.GetComponent<Bar>();
            if (healthBarController == null) Debug.LogError("Health - healthBar must have a Bar", this);
        }

        protected virtual void Start() {
            healthBarController.value = _health / _maxHealth;
        }

        void FixedUpdate() {
            bool isRegenEnabled = healthRegen > 0;
            bool isExtraRegenEnabled = extraRegenTimeout > 0 && extraRegen > 0;

            if (isRegenEnabled || isExtraRegenEnabled) {
                float now = Time.time;
                bool isExtraRegen = isExtraRegenEnabled && now >= nextExtraRegen;

                if (health < _maxHealth) {
                    float healthPerSecond = isExtraRegen ? extraRegen : healthRegen;
                    float regen = Mathf.Min(_maxHealth * healthPerSecond * Time.deltaTime, _maxHealth - health);
                    _health += regen;
                    healthBarController.value = health / _maxHealth;
                } else if (isExtraRegenEnabled) {
                    nextExtraRegen = Time.time + extraRegenTimeout;
                }
            }
        }
    }
}
