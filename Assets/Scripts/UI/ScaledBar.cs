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

namespace Tienkio.UI {
    public class ScaledBar : Bar {
        [SerializeField, Space]
        protected Transform _bar;
        [SerializeField]
        protected Vector3 _barPosition, _barScale;

        public Transform bar {
            get { return _bar; }
            set {
                if (_bar != value) {
                    _bar = value;
                    Resize();
                }
            }
        }

        public Vector3 barPosition {
            get { return _barPosition; }
            set {
                if (_barPosition != value) {
                    _barPosition = value;
                    Resize();
                }
            }
        }

        public Vector3 barScale {
            get { return _barScale; }
            set {
                if (_barScale != value) {
                    _barScale = value;
                    Resize();
                }
            }
        }

        public override void Resize() {
            Vector3 position = _barPosition;
            Vector3 scale = _barScale;

            switch (_direction) {
                case Direction.LeftToRight: {
                        float size = fill * _barScale.x;
                        scale.x = size;
                        position.x = size / 2f - _barScale.x / 2f + _barPosition.x;
                    }
                    break;
                case Direction.RightToLeft: {
                        float size = fill * _barScale.x;
                        scale.x = size;
                        position.x = _barScale.x / 2f - size / 2f + _barPosition.x;
                    }
                    break;
                case Direction.BottomToTop: {
                        float size = fill * _barScale.y;
                        scale.y = size;
                        position.y = size / 2f - _barScale.y / 2f + _barPosition.y;
                    }
                    break;
                case Direction.TopToBottom: {
                        float size = fill * _barScale.y;
                        scale.y = size;
                        position.y = _barScale.y / 2f - size / 2f + _barPosition.y;
                    }
                    break;
                case Direction.BackToForward: {
                        float size = fill * _barScale.z;
                        scale.z = size;
                        position.z = size / 2f - _barScale.z / 2f + _barPosition.z;
                    }
                    break;
                case Direction.ForwardToBack: {
                        float size = fill * _barScale.z;
                        scale.z = size;
                        position.z = _barScale.z / 2f - size / 2f + _barPosition.z;
                    }
                    break;
            }

            _bar.localPosition = position;
            _bar.localScale = scale;
        }
    }
}
