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
using UnityEngine.UI;

namespace Tienkio.UI {
    public abstract class Bar : MonoBehaviour {
        public enum Direction { LeftToRight, RightToLeft, BottomToTop, TopToBottom, BackToForward, ForwardToBack }

        [SerializeField]
        protected Direction _direction;
        [SerializeField, Range(0, 1)]
        protected float _offset;
        [SerializeField, Range(0, 1)]
        protected float _value;

        protected float fill { get { return _offset + _value * (1 - _offset); } }

        public Direction direction {
            get { return _direction; }
            set {
                if (_direction != value) {
                    _direction = value;
                    Resize();
                }
            }
        }

        public float offset {
            get { return _offset; }
            set {
                float clampedValue = Mathf.Clamp01(value);
                if (_offset != clampedValue) {
                    _offset = clampedValue;
                    Resize();
                }
            }
        }

        public float value {
            get { return _value; }
            set {
                float clampedValue = Mathf.Clamp01(value);
                if (_value != clampedValue) {
                    _value = clampedValue;
                    Resize();
                }
            }
        }

#if UNITY_EDITOR
        protected virtual void OnValidate() {
            Resize();
        }
#endif

        public abstract void Resize();
    }
}
